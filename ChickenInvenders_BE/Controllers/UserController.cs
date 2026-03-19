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

        // ===== PER-LEVEL HIGHSCORE =====

        [HttpGet("highscore/{level:int}")]
        public async Task<IActionResult> GetHighScoreForLevel(int level)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "User not found in token" });
                }

                var userId = int.Parse(userIdClaim.Value);

                var entry = await _context.UserLevelScores
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.Level == level);

                var response = new LevelHighScoreResponse
                {
                    Level = level,
                    HighScore = entry?.HighScore ?? 0
                };

                return Ok(response);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while fetching level high score" });
            }
        }

        [HttpPut("highscore/{level:int}")]
        public async Task<IActionResult> UpsertHighScoreForLevel(int level, [FromBody] UpdateLevelHighScoreRequest request)
        {
            try
            {
                if (request.HighScore < 0 || request.ChickenLegs < 0)
                {
                    return BadRequest(new { message = "Values must be >= 0" });
                }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(new { message = "User not found in token" });
                }

                var userId = int.Parse(userIdClaim.Value);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                var entry = await _context.UserLevelScores
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.Level == level);

                int currentBestLegs = entry?.BestChickenLegs ?? 0;
                int newBestLegs = Math.Max(currentBestLegs, request.ChickenLegs);
                int deltaLegs = Math.Max(0, request.ChickenLegs - currentBestLegs);

                if (entry == null)
                {
                    entry = new UserLevelScore
                    {
                        UserId = userId,
                        Level = level,
                        HighScore = request.HighScore,
                        BestChickenLegs = request.ChickenLegs,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    _context.UserLevelScores.Add(entry);
                }
                else
                {
                    if (request.HighScore > entry.HighScore)
                    {
                        entry.HighScore = request.HighScore;
                    }

                    if (newBestLegs > entry.BestChickenLegs)
                    {
                        entry.BestChickenLegs = newBestLegs;
                    }

                    entry.UpdatedAt = DateTime.UtcNow;
                }

                if (deltaLegs > 0)
                {
                    user.TotalChickenLegs += deltaLegs;
                    user.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();

                var response = new LevelHighScoreResponse
                {
                    Level = level,
                    HighScore = entry.HighScore
                };

                return Ok(response);
            }
            catch
            {
                return StatusCode(500, new { message = "An error occurred while updating level high score" });
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

        // Tổng chicken legs
        [HttpGet("chicken-legs")]
        public async Task<IActionResult> GetTotalChickenLegs()
        {
            try
            {
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

                return Ok(new ChickenLegsResponse
                {
                    TotalChickenLegs = user.TotalChickenLegs
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching chicken legs", error = ex.Message });
            }
        }

        // ===== SKINS =====

        [HttpGet("skins")]
        public async Task<IActionResult> GetUserSkins()
        {
            try
            {
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

                var owned = await _context.UserOwnedSkins
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .Select(x => x.SkinIndex)
                    .ToListAsync();

                if (!owned.Contains(0))
                {
                    owned.Add(0); // default skin always available
                }

                owned.Sort();

                return Ok(new UserSkinsResponse
                {
                    EquippedSkinIndex = user.EquippedSkinIndex,
                    OwnedSkinIndexes = owned
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching skins", error = ex.Message });
            }
        }

        [HttpPost("skins/buy")]
        public async Task<IActionResult> BuySkin([FromBody] SkinActionRequest request)
        {
            try
            {
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

                if (request.SkinIndex == 0)
                {
                    return Ok(new { message = "Default skin is already owned" });
                }

                var existing = await _context.UserOwnedSkins
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.SkinIndex == request.SkinIndex);

                if (existing == null)
                {
                    _context.UserOwnedSkins.Add(new UserOwnedSkin
                    {
                        UserId = userId,
                        SkinIndex = request.SkinIndex,
                        CreatedAt = DateTime.UtcNow
                    });
                    await _context.SaveChangesAsync();
                }

                return Ok(new { message = "Skin owned" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while buying skin", error = ex.Message });
            }
        }

        [HttpPut("skins/equip")]
        public async Task<IActionResult> EquipSkin([FromBody] SkinActionRequest request)
        {
            try
            {
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

                bool owned = request.SkinIndex == 0 || await _context.UserOwnedSkins
                    .AnyAsync(x => x.UserId == userId && x.SkinIndex == request.SkinIndex);

                if (!owned)
                {
                    return BadRequest(new { message = "Skin is not owned by user" });
                }

                user.EquippedSkinIndex = request.SkinIndex;
                user.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                return Ok(new { equippedSkinIndex = user.EquippedSkinIndex });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while equipping skin", error = ex.Message });
            }
        }
    }
}
