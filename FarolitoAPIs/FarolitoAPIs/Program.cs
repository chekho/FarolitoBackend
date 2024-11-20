using FarolitoAPIs.Data;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Microsoft.OpenApi.Models;
using Serilog;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
// Verificacion de existencia de archivo de logs
var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
//Configuracion para SQLServer 
var constrin = builder.Configuration.GetConnectionString("sqlString");
// Configuración de JWT
var JWTSettings = builder.Configuration.GetSection("JWTSetting");

if (!Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory);
}

// Configuracion de Logs
Log.Logger = new LoggerConfiguration()
    .WriteTo.File(Path.Combine(logDirectory, "log.txt"))
    .MinimumLevel.Verbose()
    .CreateLogger();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FarolitoCORS", app =>
    {
        app.WithOrigins("http://localhost:4200", "http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

builder.Services.AddDbContext<FarolitoDbContext>(options =>
    options.UseSqlServer(constrin, sqlServerOptions =>
        sqlServerOptions.CommandTimeout(600)
    )
);

//Agregamos la configuración para ASP -Net Core Identity
builder.Services.AddIdentity<Usuario, IdentityRole>().AddEntityFrameworkStores<FarolitoDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.SaveToken = true;
    opt.RequireHttpsMetadata = false;
    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = JWTSettings["ValidAudience"],
        ValidIssuer = JWTSettings["ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSettings.GetSection("securityKey").Value!))
    };
});

//Agregando la Definición de Seguridad
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = @"JWT Authorization Example : 'Bearer eyeleieieekeieieie",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
   {
       {
           new OpenApiSecurityScheme
           {
               Reference = new OpenApiReference
               {
                   Type = ReferenceType.SecurityScheme,
                   Id = "Bearer"
               },
               Scheme = "outh2",
               Name = "Bearer",
               In = ParameterLocation.Header,
           },
           new List<string>()
       }
   });
});

builder.Services.AddControllersWithViews();

var app = builder.Build();
//Prueba de registro de log
//Log.Information("Application started. Current directory: {Directory}", Environment.CurrentDirectory);

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<Usuario>>();
    SeedData.Initialize(services, userManager).Wait();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Manejo de excepciones global
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlePathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlePathFeature?.Error != null)
        {
            Log.Error(exceptionHandlePathFeature.Error, "Unhandled exception occured.");
        }
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An expected error occurred. Please try again later");
    });
});

app.UseCors("FarolitoCORS");

app.UseStaticFiles();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();