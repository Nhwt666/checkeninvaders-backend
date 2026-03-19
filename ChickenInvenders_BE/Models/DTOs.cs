using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ChickenInvenders_BE.Models
{
    public class RegisterRequest
    {
        [Required]
        [MaxLength(100)]
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [MinLength(6)]
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
        
        [Required]
        [JsonPropertyName("confirmPassword")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        [Required]
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }

    public class UpdateScoreRequest
    {
        [Required]
        [Range(0, int.MaxValue)]
        [JsonPropertyName("score")]
        public int Score { get; set; }
    }

    public class UpdateHighScoreRequest
    {
        [Required]
        [Range(0, int.MaxValue)]
        [JsonPropertyName("highScore")]
        public int HighScore { get; set; }
    }

    public class UpdateLevelHighScoreRequest
    {
        [Required]
        [Range(0, int.MaxValue)]
        [JsonPropertyName("highScore")]
        public int HighScore { get; set; }

        // Số chicken legs thu thập được trong lần chơi này
        [Required]
        [Range(0, int.MaxValue)]
        [JsonPropertyName("chickenLegs")]
        public int ChickenLegs { get; set; }
    }

    public class AuthResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; } = string.Empty;
        
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;
        
        [JsonPropertyName("score")]
        public int Score { get; set; }
        
        [JsonPropertyName("highScore")]
        public int HighScore { get; set; }
    }

    public class UserResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;
        
        [JsonPropertyName("score")]
        public int Score { get; set; }
        
        [JsonPropertyName("highScore")]
        public int HighScore { get; set; }
    }

    public class LevelHighScoreResponse
    {
        [JsonPropertyName("level")]
        public int Level { get; set; }

        [JsonPropertyName("highScore")]
        public int HighScore { get; set; }
    }

    public class ChickenLegsResponse
    {
        [JsonPropertyName("totalChickenLegs")]
        public int TotalChickenLegs { get; set; }
    }

    public class UserSkinsResponse
    {
        [JsonPropertyName("equippedSkinIndex")]
        public int EquippedSkinIndex { get; set; }

        [JsonPropertyName("ownedSkinIndexes")]
        public List<int> OwnedSkinIndexes { get; set; } = new();
    }

    public class SkinActionRequest
    {
        [Required]
        [Range(0, int.MaxValue)]
        [JsonPropertyName("skinIndex")]
        public int SkinIndex { get; set; }
    }

    public class LeaderboardEntryDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;

        // Tổng điểm = tổng high score của tất cả màn
        [JsonPropertyName("totalScore")]
        public int TotalScore { get; set; }

        // Tổng đùi gà (tính theo best mỗi màn)
        [JsonPropertyName("totalChickenLegs")]
        public int TotalChickenLegs { get; set; }
    }

    // ===== Game configuration DTOs =====

    public class PlayerConfigDto
    {
        [JsonPropertyName("speed")]
        public float Speed { get; set; }

        [JsonPropertyName("shieldTimeSeconds")]
        public float ShieldTimeSeconds { get; set; }

        [JsonPropertyName("scorePerChickenLeg")]
        public int ScorePerChickenLeg { get; set; }

        [JsonPropertyName("defaultBulletLevel")]
        public int DefaultBulletLevel { get; set; }

        // Số mạng tối đa (lives)
        [JsonPropertyName("lives")]
        public int Lives { get; set; }
    }

    public class EnemyConfigDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("maxHealth")]
        public int MaxHealth { get; set; }

        [JsonPropertyName("scoreOnKill")]
        public int ScoreOnKill { get; set; }

        [JsonPropertyName("damagePerBullet")]
        public int DamagePerBullet { get; set; }

        [JsonPropertyName("eggSpawnIntervalMinSeconds")]
        public float EggSpawnIntervalMinSeconds { get; set; }

        [JsonPropertyName("eggSpawnIntervalMaxSeconds")]
        public float EggSpawnIntervalMaxSeconds { get; set; }

        [JsonPropertyName("isBoss")]
        public bool IsBoss { get; set; }
    }

    public class GameplayConfigDto
    {
        [JsonPropertyName("gridSize")]
        public float GridSize { get; set; }

        [JsonPropertyName("destroyDistance")]
        public float DestroyDistance { get; set; }
    }

    // Gộp các thông số balance chính cho trang admin
    public class GameBalanceConfigDto
    {
        [JsonPropertyName("enemyHealth")]
        public int EnemyHealth { get; set; }

        [JsonPropertyName("enemyDamage")]
        public int EnemyDamage { get; set; }

        [JsonPropertyName("bossHealth")]
        public int BossHealth { get; set; }

        [JsonPropertyName("bulletDamage")]
        public int BulletDamage { get; set; }

        // Hệ số / tần suất spawn (ví dụ enemy per second)
        [JsonPropertyName("spawnRate")]
        public float SpawnRate { get; set; }

        // ===== Gameplay config (spawn & destroy) =====

        // Kích thước grid spawn gà
        [JsonPropertyName("gridSize")]
        public float GridSize { get; set; }

        // Khoảng cách tự hủy object
        [JsonPropertyName("destroyDistance")]
        public float DestroyDistance { get; set; }

        [JsonPropertyName("coinReward")]
        public int CoinReward { get; set; }

        // ===== Player config (máy bay) =====

        // Tốc độ di chuyển của máy bay
        [JsonPropertyName("playerSpeed")]
        public float PlayerSpeed { get; set; }

        // Thời gian khiên tồn tại (giây)
        [JsonPropertyName("shieldTimeSeconds")]
        public float ShieldTimeSeconds { get; set; }

        // Cấp đạn mặc định khi bắt đầu game
        [JsonPropertyName("defaultBulletLevel")]
        public int DefaultBulletLevel { get; set; }

        // Số mạng tối đa (lives)
        [JsonPropertyName("lives")]
        public int Lives { get; set; }

        // ===== Enemy egg spawn (gà thường & boss) =====

        [JsonPropertyName("enemyEggMinSeconds")]
        public float EnemyEggMinSeconds { get; set; }

        [JsonPropertyName("enemyEggMaxSeconds")]
        public float EnemyEggMaxSeconds { get; set; }

        [JsonPropertyName("bossEggMinSeconds")]
        public float BossEggMinSeconds { get; set; }

        [JsonPropertyName("bossEggMaxSeconds")]
        public float BossEggMaxSeconds { get; set; }
    }
}
