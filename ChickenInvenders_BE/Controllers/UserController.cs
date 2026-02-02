using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChickenInvenders_BE.Data;
using ChickenInvenders_BE.Models;
using System.Security.Claims;

namespace ChickenInvenders_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("score")]
        public async Task<IActionResult> GetScore()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    // Debug: Check all claims
                    var allClaims = User.Claims.Select(c => $"{c.Type}: {c.Value}").ToList();
                    return Unauthorized(new { message = "User not found in token", claims = allClaims });
                }

                var userId = int.Parse(userIdClaim.Value);
                var user = await _context.Users.FindAsync(userId);
                
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(new { score = user.Score });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching score", error = ex.Message });
            }
        }

        [HttpPut("highscore")]
        public async Task<IActionResult> UpdateHighScore([FromBody] UpdateHighScoreRequest request)
        {
            try
            {
                if (request.HighScore < 0)
                {
                    return BadRequest(new { message = "HighScore must be >= 0" });
                }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "User not found in token" });
                }

                var userId = int.Parse(userIdClaim.Value);
                var user = await _context.Users.FindAsync(userId);
                
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                if (request.HighScore > user.HighScore)
                {
                    user.HighScore = request.HighScore;
                    user.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }

                return Ok(new { highScore = user.HighScore });
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while updating high score" });
            }
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    // Debug: Check all claims
                    var allClaims = User.Claims.Select(c => $"{c.Type}: {c.Value}").ToList();
                    return Unauthorized(new { message = "User not found in token", claims = allClaims });
                }

                var userId = int.Parse(userIdClaim.Value);
                var user = await _context.Users.FindAsync(userId);
                
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                var response = new UserResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    Score = user.Score,
                    HighScore = user.HighScore
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching profile", error = ex.Message });
            }
        }
    }
}
