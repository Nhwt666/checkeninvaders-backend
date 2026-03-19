using Microsoft.EntityFrameworkCore;
using ChickenInvenders_BE.Models;

namespace ChickenInvenders_BE.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        // High score per level
        public DbSet<UserLevelScore> UserLevelScores { get; set; }

        // Owned skins per user
        public DbSet<UserOwnedSkin> UserOwnedSkins { get; set; }

        // Game configuration tables
        public DbSet<PlayerConfig> PlayerConfigs { get; set; }
        public DbSet<EnemyConfig> EnemyConfigs { get; set; }
        public DbSet<GameplayConfig> GameplayConfigs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Username).HasColumnName("username").IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).HasColumnName("password_hash").IsRequired().HasMaxLength(255);
                entity.Property(e => e.Score).HasColumnName("score").HasDefaultValue(0);
                entity.Property(e => e.HighScore).HasColumnName("highscore").HasDefaultValue(0);
                entity.Property(e => e.TotalChickenLegs).HasColumnName("total_chicken_legs").HasDefaultValue(0);
                entity.Property(e => e.EquippedSkinIndex).HasColumnName("equipped_skin_index").HasDefaultValue(0);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // UserLevelScore mapping
            modelBuilder.Entity<UserLevelScore>(entity =>
            {
                entity.ToTable("user_level_scores");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Level).HasColumnName("level");
                entity.Property(e => e.HighScore).HasColumnName("high_score").HasDefaultValue(0);
                entity.Property(e => e.BestChickenLegs).HasColumnName("best_chicken_legs").HasDefaultValue(0);
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserId, e.Level }).IsUnique();
            });

            // UserOwnedSkin mapping
            modelBuilder.Entity<UserOwnedSkin>(entity =>
            {
                entity.ToTable("user_owned_skins");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.SkinIndex).HasColumnName("skin_index");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => new { e.UserId, e.SkinIndex }).IsUnique();
            });

            // PlayerConfig mapping
            modelBuilder.Entity<PlayerConfig>(entity =>
            {
                entity.ToTable("player_configs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Speed).HasColumnName("speed");
                entity.Property(e => e.ShieldTimeSeconds).HasColumnName("shield_time_seconds");
                entity.Property(e => e.ScorePerChickenLeg).HasColumnName("score_per_chicken_leg");
                entity.Property(e => e.DefaultBulletLevel).HasColumnName("default_bullet_level");
            });

            // EnemyConfig mapping
            modelBuilder.Entity<EnemyConfig>(entity =>
            {
                entity.ToTable("enemy_configs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Name).HasColumnName("name").IsRequired().HasMaxLength(50);
                entity.Property(e => e.MaxHealth).HasColumnName("max_health");
                entity.Property(e => e.ScoreOnKill).HasColumnName("score_on_kill");
                entity.Property(e => e.DamagePerBullet).HasColumnName("damage_per_bullet");
                entity.Property(e => e.EggSpawnIntervalMinSeconds).HasColumnName("egg_spawn_interval_min_seconds");
                entity.Property(e => e.EggSpawnIntervalMaxSeconds).HasColumnName("egg_spawn_interval_max_seconds");
                entity.Property(e => e.IsBoss).HasColumnName("is_boss");
            });

            // GameplayConfig mapping
            modelBuilder.Entity<GameplayConfig>(entity =>
            {
                entity.ToTable("gameplay_configs");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.GridSize).HasColumnName("grid_size");
                entity.Property(e => e.DestroyDistance).HasColumnName("destroy_distance");
            });
        }
    }
}
