using System.ComponentModel.DataAnnotations;

namespace ChickenInvenders_BE.Models
{
    public class UserOwnedSkin
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int SkinIndex { get; set; }

        public DateTime CreatedAt { get; set; }

        public User User { get; set; } = null!;
    }
}

