using FarolitoAPIs.DTOs;
using FarolitoAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestSharp;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace FarolitoAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public UsuarioController(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        //POST Login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "User not found with this email"
                });
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
            {
                return Unauthorized(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "Invalid Password"
                });
            }

            var token = GenerateToken(user);

            return Ok(new AuthResponseDTO
            {
                Token = token,
                IsSuccess = true,
                Message = "Login Success"
            });
        }

        private string GenerateToken(Usuario user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("JWTSetting").GetSection("securityKey").Value!);

            var roles = _userManager.GetRolesAsync(user).Result;

            List<Claim> claims = [
                new (JwtRegisteredClaimNames.Email, user.Email??""),
       new (JwtRegisteredClaimNames.Name, user.FullName??""),
       new (JwtRegisteredClaimNames.NameId, user.Id??""),
       new (JwtRegisteredClaimNames.Aud, _configuration.GetSection("JWTSetting").GetSection("ValidAudience").Value!),
       new (JwtRegisteredClaimNames.Iss, _configuration.GetSection("JWTSetting").GetSection("ValidIssuer").Value!)
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
                Roles = [.. await _userManager.GetRolesAsync(user)],
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                AccessFailedCount = user.AccessFailedCount
            });
        }

        //GET Usuarios
        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetailDTO>>> GetUsers()
        {
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
                    Roles = roles.ToArray()
                });
            }

            return Ok(userDetailDTOs);
        }

        //POST registrarCliente
        [AllowAnonymous]
        [HttpPost("registerClient")]
        public async Task<ActionResult<string>> Register(RegisterDTO registerDto)
        {
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
                return BadRequest(result.Errors);
            }

            if (registerDto.Roles is null)
            {
                await _userManager.AddToRoleAsync(user, "Cliente");
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


        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDTO.Email);
            if (user is null)
            {
                return Ok(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = "User does not exist with this email"
                });
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"http://localhost:4200/reset-password?email={user.Email}&token={WebUtility.UrlEncode(token)}";

            var client = new SendGridClient("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\r\n");
            var from = new EmailAddress("sergiocecyteg@gmail.com", "Farolito");
            var to = new EmailAddress(user.Email);
            var msg = MailHelper.CreateSingleEmail(from, to, "Password Reset", "", $"<p>Click <a href='{resetLink}'>here</a> to reset your password</p>");

            var response = await client.SendEmailAsync(msg);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(new AuthResponseDTO
                {
                    IsSuccess = true,
                    Message = "Email Sent with password reset link, please check your email"
                });
            }
            else
            {
                return BadRequest(new AuthResponseDTO
                {
                    IsSuccess = false,
                    Message = response.Body.ReadAsStringAsync().Result
                });
            }
        }

        [Authorize]
        [HttpPut("update")]
        public async Task<ActionResult<AuthResponseDTO>> UpdateUser(UpdateUserDTO updateUserDto)
        {
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
            }

            if (!string.IsNullOrEmpty(updateUserDto.FullName))
            {
                user.FullName = updateUserDto.FullName;
            }

            if (!string.IsNullOrEmpty(updateUserDto.PhoneNumber))
            {
                user.PhoneNumber = updateUserDto.PhoneNumber;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new AuthResponseDTO
            {
                IsSuccess = true,
                Message = "User updated successfully"
            });
        }


    }
}
