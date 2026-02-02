using System.ComponentModel.DataAnnotations;
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
}
