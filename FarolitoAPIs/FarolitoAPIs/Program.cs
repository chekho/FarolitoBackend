using FarolitoAPIs.Controllers;
using FarolitoAPIs.Models;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var constrin = builder.Configuration.GetConnectionString("sqlString");
builder.Services.AddDbContext<FarolitoDbContext>(options => options.UseSqlServer(constrin));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
/*
 Scaffold-DbContext 'Server=DESKTOP-3K4LROA;Database=farolito_db;user id=sa;password=root;TrustServerCertificate=true' Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models


 
 */