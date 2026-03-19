using ChickenInvenders_BE.Data;
using ChickenInvenders_BE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChickenInvenders_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaderboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LeaderboardController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Top players ordered by total score (sum of per-level highscores).
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeaderboardEntryDto>>> GetTop([FromQuery] int top = 50)
        {
            if (top <= 0) top = 50;

            var query = from u in _context.Users
                        join s in _context.UserLevelScores on u.Id equals s.UserId into scores
                        select new LeaderboardEntryDto
                        {
                            Username = u.Username,
                            TotalScore = scores.Sum(x => (int?)x.HighScore) ?? 0,
                            TotalChickenLegs = u.TotalChickenLegs
                        };

            var result = await query
                .OrderByDescending(x => x.TotalScore)
                .ThenBy(x => x.Username)
                .Take(top)
                .ToListAsync();

            return Ok(result);
        }
    }
}

