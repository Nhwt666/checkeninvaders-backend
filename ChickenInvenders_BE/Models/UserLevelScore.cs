using System.ComponentModel.DataAnnotations;

namespace ChickenInvenders_BE.Models
{
    public class UserLevelScore
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Level identifier (typically buildIndex of Unity scene)
        /// </summary>
        [Required]
        public int Level { get; set; }

        [Range(0, int.MaxValue)]
        public int HighScore { get; set; }

        // Best chicken legs collected on this level
        [Range(0, int.MaxValue)]
        public int BestChickenLegs { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public User User { get; set; } = null!;
    }
}

