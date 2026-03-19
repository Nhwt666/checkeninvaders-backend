using System.ComponentModel.DataAnnotations;

namespace ChickenInvenders_BE.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;
        
        public int Score { get; set; }
        
        public int HighScore { get; set; } = 0;

        // Tổng số chicken legs đã kiếm được (tính theo best mỗi màn)
        public int TotalChickenLegs { get; set; } = 0;

        // Skin đang trang bị (index trong danh sách skin của game)
        public int EquippedSkinIndex { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }
}
