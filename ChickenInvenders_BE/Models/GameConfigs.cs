using System.ComponentModel.DataAnnotations;

namespace ChickenInvenders_BE.Models
{
    // Cấu hình chung cho người chơi (tàu)
    public class PlayerConfig
    {
        public int Id { get; set; }

        // Tốc độ di chuyển của tàu
        [Range(0, float.MaxValue)]
        public float Speed { get; set; }

        // Thời gian tồn tại của khiên (giây)
        [Range(0, float.MaxValue)]
        public float ShieldTimeSeconds { get; set; }

        // Điểm cộng khi ăn 1 "chicken leg"
        [Range(0, int.MaxValue)]
        public int ScorePerChickenLeg { get; set; }

        // Cấp độ đạn mặc định (0: 1 tia, 1: 3 tia, ...)
        [Range(0, int.MaxValue)]
        public int DefaultBulletLevel { get; set; }

        // Số mạng (lives) tối đa trước khi Game Over
        [Range(1, int.MaxValue)]
        public int Lives { get; set; } = 5;
    }

    // Cấu hình cho từng loại enemy (gà thường, boss, ...)
    public class EnemyConfig
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty; // "Chicken", "Boss", ...

        // Máu tối đa
        [Range(0, int.MaxValue)]
        public int MaxHealth { get; set; }

        // Điểm cộng khi tiêu diệt
        [Range(0, int.MaxValue)]
        public int ScoreOnKill { get; set; }

        // Sát thương mỗi viên đạn gây lên enemy này
        [Range(0, int.MaxValue)]
        public int DamagePerBullet { get; set; }

        // Khoảng thời gian bắn trứng tối thiểu (giây)
        [Range(0, float.MaxValue)]
        public float EggSpawnIntervalMinSeconds { get; set; }

        // Khoảng thời gian bắn trứng tối đa (giây)
        [Range(0, float.MaxValue)]
        public float EggSpawnIntervalMaxSeconds { get; set; }

        // Đánh dấu có phải boss hay không
        public bool IsBoss { get; set; }
    }

    // Cấu hình gameplay chung (spawn, hủy object, ...)
    public class GameplayConfig
    {
        public int Id { get; set; }

        // Kích thước grid dùng để spawn gà
        [Range(0, float.MaxValue)]
        public float GridSize { get; set; }

        // Khoảng cách tối đa từ tâm màn hình để tự hủy object (DestroyIfReachDistances)
        [Range(0, float.MaxValue)]
        public float DestroyDistance { get; set; }
    }
}

