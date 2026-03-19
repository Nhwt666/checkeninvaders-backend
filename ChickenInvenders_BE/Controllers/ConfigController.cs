using ChickenInvenders_BE.Data;
using ChickenInvenders_BE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChickenInvenders_BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // chỉ cho phép client đã đăng nhập lấy/sửa config
    public class ConfigController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConfigController(AppDbContext context)
        {
            _context = context;
        }

        // ===== PLAYER CONFIG =====

        [HttpGet("player")]
        public async Task<ActionResult<PlayerConfigDto>> GetPlayerConfig()
        {
            var config = await _context.PlayerConfigs.AsNoTracking().FirstOrDefaultAsync();
            if (config == null)
            {
                return NotFound(new { message = "Player config not found" });
            }

            return Ok(new PlayerConfigDto
            {
                Speed = config.Speed,
                ShieldTimeSeconds = config.ShieldTimeSeconds,
                ScorePerChickenLeg = config.ScorePerChickenLeg,
                DefaultBulletLevel = config.DefaultBulletLevel,
                Lives = config.Lives
            });
        }

        [HttpPut("player")]
        public async Task<IActionResult> UpsertPlayerConfig([FromBody] PlayerConfigDto dto)
        {
            var config = await _context.PlayerConfigs.FirstOrDefaultAsync();

            if (config == null)
            {
                config = new PlayerConfig();
                _context.PlayerConfigs.Add(config);
            }

            config.Speed = dto.Speed;
            config.ShieldTimeSeconds = dto.ShieldTimeSeconds;
            config.ScorePerChickenLeg = dto.ScorePerChickenLeg;
            config.DefaultBulletLevel = dto.DefaultBulletLevel;
            config.Lives = dto.Lives;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ===== ENEMY CONFIGS =====

        [HttpGet("enemies")]
        public async Task<ActionResult<IEnumerable<EnemyConfigDto>>> GetEnemyConfigs()
        {
            var configs = await _context.EnemyConfigs.AsNoTracking().ToListAsync();

            var result = configs.Select(e => new EnemyConfigDto
            {
                Name = e.Name,
                MaxHealth = e.MaxHealth,
                ScoreOnKill = e.ScoreOnKill,
                DamagePerBullet = e.DamagePerBullet,
                EggSpawnIntervalMinSeconds = e.EggSpawnIntervalMinSeconds,
                EggSpawnIntervalMaxSeconds = e.EggSpawnIntervalMaxSeconds,
                IsBoss = e.IsBoss
            });

            return Ok(result);
        }

        [HttpGet("enemies/{name}")]
        public async Task<ActionResult<EnemyConfigDto>> GetEnemyConfig(string name)
        {
            var config = await _context.EnemyConfigs.AsNoTracking()
                .FirstOrDefaultAsync(e => e.Name == name);

            if (config == null)
            {
                return NotFound(new { message = $"Enemy config '{name}' not found" });
            }

            return Ok(new EnemyConfigDto
            {
                Name = config.Name,
                MaxHealth = config.MaxHealth,
                ScoreOnKill = config.ScoreOnKill,
                DamagePerBullet = config.DamagePerBullet,
                EggSpawnIntervalMinSeconds = config.EggSpawnIntervalMinSeconds,
                EggSpawnIntervalMaxSeconds = config.EggSpawnIntervalMaxSeconds,
                IsBoss = config.IsBoss
            });
        }

        [HttpPut("enemies/{name}")]
        public async Task<IActionResult> UpsertEnemyConfig(string name, [FromBody] EnemyConfigDto dto)
        {
            var config = await _context.EnemyConfigs.FirstOrDefaultAsync(e => e.Name == name);

            if (config == null)
            {
                config = new EnemyConfig { Name = name };
                _context.EnemyConfigs.Add(config);
            }

            config.MaxHealth = dto.MaxHealth;
            config.ScoreOnKill = dto.ScoreOnKill;
            config.DamagePerBullet = dto.DamagePerBullet;
            config.EggSpawnIntervalMinSeconds = dto.EggSpawnIntervalMinSeconds;
            config.EggSpawnIntervalMaxSeconds = dto.EggSpawnIntervalMaxSeconds;
            config.IsBoss = dto.IsBoss;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ===== GAMEPLAY CONFIG =====

        [HttpGet("gameplay")]
        public async Task<ActionResult<GameplayConfigDto>> GetGameplayConfig()
        {
            var config = await _context.GameplayConfigs.AsNoTracking().FirstOrDefaultAsync();
            if (config == null)
            {
                return NotFound(new { message = "Gameplay config not found" });
            }

            return Ok(new GameplayConfigDto
            {
                GridSize = config.GridSize,
                DestroyDistance = config.DestroyDistance
            });
        }

        [HttpPut("gameplay")]
        public async Task<IActionResult> UpsertGameplayConfig([FromBody] GameplayConfigDto dto)
        {
            var config = await _context.GameplayConfigs.FirstOrDefaultAsync();

            if (config == null)
            {
                config = new GameplayConfig();
                _context.GameplayConfigs.Add(config);
            }

            config.GridSize = dto.GridSize;
            config.DestroyDistance = dto.DestroyDistance;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // ===== GAME BALANCE (flat object cho admin panel) =====

        [HttpGet("game-balance")]
        public async Task<ActionResult<GameBalanceConfigDto>> GetGameBalance()
        {
            // Enemy chính (gà thường)
            var enemy = await _context.EnemyConfigs.AsNoTracking()
                .FirstOrDefaultAsync(e => !e.IsBoss);

            // Boss
            var boss = await _context.EnemyConfigs.AsNoTracking()
                .FirstOrDefaultAsync(e => e.IsBoss);

            // Cấu hình player và gameplay
            var player = await _context.PlayerConfigs.AsNoTracking().FirstOrDefaultAsync();
            var gameplay = await _context.GameplayConfigs.AsNoTracking().FirstOrDefaultAsync();

            if (enemy == null || boss == null || player == null || gameplay == null)
            {
                return NotFound(new
                {
                    message = "Missing game config data (enemy, boss, player or gameplay)"
                });
            }

            // Tạm thời định nghĩa spawnRate = 1 / (avg egg spawn interval của gà thường)
            var avgInterval = (enemy.EggSpawnIntervalMinSeconds + enemy.EggSpawnIntervalMaxSeconds) / 2f;
            var spawnRate = avgInterval > 0 ? 1f / avgInterval : 0f;

            var dto = new GameBalanceConfigDto
            {
                EnemyHealth = enemy.MaxHealth,
                EnemyDamage = enemy.DamagePerBullet,
                BossHealth = boss.MaxHealth,
                BulletDamage = enemy.DamagePerBullet, // hiện game dùng cùng damage cho đạn
                SpawnRate = spawnRate,
                GridSize = gameplay.GridSize,
                DestroyDistance = gameplay.DestroyDistance,
                CoinReward = player.ScorePerChickenLeg,
                PlayerSpeed = player.Speed,
                ShieldTimeSeconds = player.ShieldTimeSeconds,
                DefaultBulletLevel = player.DefaultBulletLevel,
                Lives = player.Lives,
                EnemyEggMinSeconds = enemy.EggSpawnIntervalMinSeconds,
                EnemyEggMaxSeconds = enemy.EggSpawnIntervalMaxSeconds,
                BossEggMinSeconds = boss.EggSpawnIntervalMinSeconds,
                BossEggMaxSeconds = boss.EggSpawnIntervalMaxSeconds
            };

            return Ok(dto);
        }
    }
}

