using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using Serilog;

namespace FarolitoAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IConfiguration _configuration;

        // Diccionario para el inicio de sesión con tiempos
        private static ConcurrentDictionary<string, (int Attempts, DateTime BlockUntil)> _loginAttempts = new();
        private const int MaxAttempts = 3;
        private const int BlockDurationMinutes = 15;

        public UsuarioController(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        private string GetDeviceKey(string email, string userAgent)
        {
            return $"{email}:{userAgent}";
        }

        //POST Login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                Log.Warning("Login failed: Invalid model state: { ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            // Token recaptcha para validar 
            var recaptchaToken = loginDto.RecaptchaToken;
            var isValidRecaptcha = await VerifyRecaptcha(recaptchaToken);

            if (!isValidRecaptcha)
            {
                Log.Warning("Invalid reCAPTCHA");
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Invalid reCAPTCHA"
                });
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            var useragent = Request.Headers["User-Agent"].ToString();
            var deviceKey = GetDeviceKey(loginDto.Email, useragent);

            if (_loginAttempts.TryGetValue(deviceKey, out var attemptInfo))
            {
                if (DateTime.UtcNow < attemptInfo.BlockUntil)
                {
                    Log.Warning("Account locked for email: {Email} on device: {DeviceKey}", loginDto.Email, deviceKey);
                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Account locked. Please try again later."
                    });
                }
            }

            if (user == null)
            {
                IncrementLoginAttempts(deviceKey);
                Log.Warning("Login failed: User not found with email {Email}", loginDto.Email);
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "User not found with this email"
                });
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
            {
                IncrementLoginAttempts(deviceKey);
                Log.Warning("Login failed: Invalid password for user {Email}", loginDto.Email);
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Invalid Password"
                });
            }

            // Se ajusta la zona horaria a la usada en CDMX y se convierte el horario estándar al horario local
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("America/Mexico_City");
            DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);

            // Se asigna la hora local al campo de LastLogin y se actualiza el usuario
            user.LastLogin = localDateTime;
            await _userManager.UpdateAsync(user);
            _loginAttempts.TryRemove(deviceKey, out _);
            var token = GenerateToken(user);

            return Ok(new AuthResponseDTO
            {
                Token = token,
                IsSuccess = true,
                Message = "Login Success"
            });
        }

        // Método re-captcha para validar
        private async Task<bool> VerifyRecaptcha(string? token)
        {
            var secretKey = _configuration["GoogleReCAPTCHA:SecretKey"];
            using (var httpClient = new HttpClient())
            {
                var responseString = await httpClient.PostAsync(
                    $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={token}", null);
                var jsonResponse = await responseString.Content.ReadAsStringAsync();

                dynamic jsonData = JsonConvert.DeserializeObject(jsonResponse)!;

                return jsonData.success == true;
            }
        }

        // Método para incrementar la cantidad de inicios de sesión
        private void IncrementLoginAttempts(string deviceKey)
        {
            var now = DateTime.UtcNow;

            _loginAttempts.AddOrUpdate(deviceKey,
                _ => (1, now.AddMinutes(BlockDurationMinutes)),
                (_, oldValue) =>
                {
                    var newAttempts = oldValue.Attempts + 1;
                    return newAttempts >= MaxAttempts
                        ? (newAttempts, now.AddMinutes(BlockDurationMinutes))
                        : (newAttempts, oldValue.BlockUntil);
                });
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
            resetPasswordDTO.Token = WebUtility.UrlDecode(resetPasswordDTO.Token);
            if (user is null)
            {
                Log.Warning("Reset password failed: User not found with email {Email}", resetPasswordDTO.Email);
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "User does not exist with this mail"
                });
            }

            var result =
                await _userManager.ResetPasswordAsync(user, resetPasswordDTO.Token, resetPasswordDTO.NewPassword);

            if (!result.Succeeded)
            {
                //Log.Information("Password reset successfully for user {Email}", resetPasswordDTO.Email);
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = result.Errors.FirstOrDefault()!.Description
                });
            }

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "password reset successfully"
            });
        }

        [Authorize]
        [HttpPost("ChangePass")]
        public async Task<ActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            var user = await _userManager.FindByEmailAsync(changePasswordDTO.Email);
            if (user is null)
            {
                Log.Warning("Change password failed: User not found with email {Email}", changePasswordDTO.Email);
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "user does not exist with this mail"
                });
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDTO.CurrentPassword,
                changePasswordDTO.NewPassword);
            
            if (!result.Succeeded)
            {
                Log.Warning("Password change failed for user {Email}. Reason: {ErrorMessage}", changePasswordDTO.Email);
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = result.Errors.FirstOrDefault()!.Description
                });
            }

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "password change successfully"
            });
        }

        private string GenerateToken(Usuario user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWTSetting").GetSection("securityKey").Value!);

            var roles = _userManager.GetRolesAsync(user).Result;

            List<Claim> claims =
            [
                new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new(JwtRegisteredClaimNames.Name, user.FullName ?? ""),
                new(JwtRegisteredClaimNames.NameId, user.Id ?? ""),
                new(JwtRegisteredClaimNames.Aud,
                    _configuration.GetSection("JWTSetting").GetSection("ValidAudience").Value!),
                new(JwtRegisteredClaimNames.Iss,
                    _configuration.GetSection("JWTSetting").GetSection("ValidIssuer").Value!)
            ];

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        //GET Usuario 
        [Authorize]
        [HttpGet("detail")]
        public async Task<ActionResult<UserDetailDTO>> GetUserDetail()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(currentUserId!);

            if (user == null)
            {
                Log.Warning("User with ID {UserId} not found.", currentUserId);
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "User not found"
                });
            }

            return Ok(new UserDetailDTO
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Direccion = user.Direccion,
                UrlImage = user.urlImage,
                Tarjeta = user.Tarjeta,
                Roles = [.. await _userManager.GetRolesAsync(user)],
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                AccessFailedCount = user.AccessFailedCount
            });
        }

        //GET Usuarios
        //[Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetailDTO>>> GetUsers()
        {
            //Log.Information("Attempting to retrieve all users.");

            var users = await _userManager.Users.ToListAsync();

            var userDetailDTOs = new List<UserDetailDTO>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userDetailDTOs.Add(new UserDetailDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Direccion = user.Direccion,
                    UrlImage = user.urlImage,
                    Tarjeta = user.Tarjeta,
                    Roles = roles.ToArray(),
                });
            }

            return Ok(userDetailDTOs);
        }

        //POST registrarCliente
        [AllowAnonymous]
        [HttpPost("registerClient")]
        public async Task<ActionResult<string>> Register(RegisterDTO registerDto)
        {
            //Log.Information("Attempting to register a new client with email: {Email}", registerDto.Email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new Usuario
            {
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                Log.Warning("Failed to create user for email: {Email}. Errors: {Errors}",
                    registerDto.Email, string.Join(", ", result.Errors.Select(e => e.Description)));

                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, "Cliente");


            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Account Created Sucessfully!!!"
            });
        }

        //[Authorize(Roles = "Administrador")]
        [HttpPost("registerEmpl")]
        public async Task<ActionResult<string>> RegisterEmpl(RegisterDTO registerDto)
        {
            //Log.Information("Attempting to register a new employee with email: {Email}", registerDto.Email);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new Usuario
            {
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                Log.Warning("Failed to create user for email: {Email}. Errors: {Errors}",
                    registerDto.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
                return BadRequest(result.Errors);
            }

            if (registerDto.Roles is null)
            {
                await _userManager.AddToRoleAsync(user, "Cliente");
                //Log.Information("User with email {Email} assigned to default role 'Cliente'", registerDto.Email);
            }
            else
            {
                foreach (var role in registerDto.Roles)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Account Created Sucessfully!!!"
            });
        }


        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            //Log.Information("Received password reset request for email: {Email}", forgotPasswordDTO.Email);
            var user = await _userManager.FindByEmailAsync(forgotPasswordDTO.Email);
            if (user is null)
            {
                Log.Warning("User not found with email: {Email}", forgotPasswordDTO.Email);

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetLink =
                    $"http://localhost:4200/reset-password?email={user.Email}&token={WebUtility.UrlEncode(token)}";

                var apiKey = _configuration["MyVars:ApiUrl"];
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("sergiocecyteg@gmail.com", "Farolito");
                var subject = "Password Reset";
                var to = new EmailAddress(user.Email, "Cliente");
                var plainTextContent = "Click the link to reset your password: " + resetLink;
                var htmlContent = $"<p>Click <a href='{resetLink}'>here</a> to reset your password</p>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);

                if (response.StatusCode == HttpStatusCode.Accepted)
                {
                    return Ok(new AuthResponseDTO
                    {
                        IsSuccess = true,
                        Message = "Email Sent with password reset link, please check your email"
                    });
                }
                else
                {
                    var responseBody = await response.Body.ReadAsStringAsync();
                    Log.Error(
                        "Failed to send password reset email to {Email}. Error: {StatusCode}, Response: {ResponseBody}",
                        user.Email, response.StatusCode, responseBody);
                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = $"Error: {response.StatusCode}, {responseBody}"
                    });
                }
            }

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Operacion realizada con exito"
            });
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<AuthResponseDTO>> UpdateUser(UpdateUserDTO updateUserDto)
        {
            //Log.Information("Received request to update user with ID: {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "User not found"
                });
            }

            if (!string.IsNullOrEmpty(updateUserDto.Email))
            {
                user.Email = updateUserDto.Email;
                user.UserName = updateUserDto.Email;
                //Log.Information("User email updated to: {Email}", updateUserDto.Email);
            }

            if (!string.IsNullOrEmpty(updateUserDto.FullName))
            {
                user.FullName = updateUserDto.FullName;
                //Log.Information("User full name updated to: {FullName}", updateUserDto.FullName);
            }

            if (!string.IsNullOrEmpty(updateUserDto.PhoneNumber))
            {
                user.PhoneNumber = updateUserDto.PhoneNumber;
                //Log.Information("User phone number updated to: {PhoneNumber}", updateUserDto.PhoneNumber);
            }

            if (!string.IsNullOrEmpty(updateUserDto.Direccion))
            {
                user.Direccion = updateUserDto.Direccion;
                //Log.Information("User address updated to: {Direccion}", updateUserDto.Direccion);
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                Log.Error("Failed to update user: {UserId}. Errors: {Errors}", userId,
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                return BadRequest(result.Errors);
            }

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "User updated successfully"
            });
        }


        [Authorize]
        [HttpPatch("upload-image")]
        public async Task<ActionResult<AuthResponseDTO>> UploadUserImage([FromForm] UserImageDTO userImageDto)
        {
            //Log.Information("Received request to upload image for user with ID: {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Datos del modelo no válidos"
                });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no encontrado"
                });
            }

            if (userImageDto.Imagen != null)
            {
                //Log.Information("User {UserId} uploaded an image", userId);
                var extension = Path.GetExtension(userImageDto.Imagen.FileName).ToLower();
                var mimeType = userImageDto.Imagen.ContentType.ToLower();

                if (extension != ".webp" || mimeType != "image/webp")
                {
                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = "Solo se permiten imágenes en formato WebP"
                    });
                }

                var fileName = $"{userId}{extension}";
                var filePath = Path.Combine("wwwroot", "images", "usuario", fileName);

                var directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await userImageDto.Imagen.CopyToAsync(stream);
                }

                user.urlImage = $"/images/usuario/{fileName}";
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    Log.Error("Failed to update user {UserId} after image upload. Errors: {Errors}", userId,
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                    return BadRequest(new AuthResponseDTO
                    {
                        IsSuccess = false,
                        Message = "No se pudo actualizar la información del usuario"
                    });
                }
            }

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Imagen del usuario subida exitosamente"
            });
        }

        private bool IsValidCardNumber(string cardNumber)
        {
            int sum = 0;
            bool isSecond = false;

            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int digit = cardNumber[i] - '0';

                if (isSecond)
                {
                    digit *= 2;
                    if (digit > 9)
                    {
                        digit -= 9;
                    }
                }

                sum += digit;
                isSecond = !isSecond;
            }

            return sum % 10 == 0;
        }

        [Authorize]
        [HttpPatch("add-credit-card")]
        public async Task<ActionResult<AuthResponseDTO>> AddCreditCard([FromBody] CreditCardDTO creditCardDto)
        {
            //Log.Information("Received request to add credit card for user with ID: {UserId}", User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!IsValidCardNumber(creditCardDto.CardNumber))
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Número de tarjeta inválido."
                });
            }

            if (!creditCardDto.CardNumber.StartsWith("4"))
            {
                Log.Warning("Only Visa cards are accepted. Provided card number: {CardNumber}",
                    creditCardDto.CardNumber);

                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Solo se aceptan tarjetas Visa."
                });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Usuario no encontrado"
                });
            }

            user.Tarjeta = creditCardDto.CardNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                Log.Error("Failed to update user information for user ID: {UserId}. Errors: {Errors}", userId,
                    string.Join(", ", result.Errors.Select(e => e.Description)));

                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "No se pudo actualizar la información del usuario"
                });
            }

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "Tarjeta de crédito agregada exitosamente"
            });
        }
    }
}