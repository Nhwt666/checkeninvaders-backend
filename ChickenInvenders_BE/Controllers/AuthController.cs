using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChickenInvenders_BE.Data;
using ChickenInvenders_BE.Models;
using ChickenInvenders_BE.Utils;

namespace ChickenInvenders_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { message = "Request body is required" });
                }

                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { message = "Username and password are required" });
                }

                if (request.Password != request.ConfirmPassword)
                {
                    return BadRequest(new { message = "Password and confirm password do not match" });
                }

                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "Username already exists" });
                }

                var passwordHash = PasswordHasher.HashPassword(request.Password);
                
                var user = new User
                {
                    Username = request.Username,
                    PasswordHash = passwordHash,
                    Score = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var token = JwtTokenHelper.GenerateToken(
                    user.Id,
                    user.Username,
                    _configuration["Jwt:Key"]!,
                    _configuration["Jwt:Issuer"]!,
                    _configuration["Jwt:Audience"]!,
                    _configuration.GetValue<int>("Jwt:ExpireDays", 1)
                );

                var response = new AuthResponse
                {
                    Token = token,
                    Username = user.Username,
                    Score = user.Score,
                    HighScore = user.HighScore
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during registration", error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { message = "Request body is required" });
                }

                if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { message = "Username and password are required" });
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
                if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
                {
                    return Unauthorized(new { message = "Invalid username or password" });
                }

                var token = JwtTokenHelper.GenerateToken(
                    user.Id,
                    user.Username,
                    _configuration["Jwt:Key"]!,
                    _configuration["Jwt:Issuer"]!,
                    _configuration["Jwt:Audience"]!,
                    _configuration.GetValue<int>("Jwt:ExpireDays", 1)
                );

                var response = new AuthResponse
                {
                    Token = token,
                    Username = user.Username,
                    Score = user.Score,
                    HighScore = user.HighScore
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred during login", error = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}
