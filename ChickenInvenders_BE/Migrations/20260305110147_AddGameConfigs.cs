using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChickenInvenders_BE.Migrations
{
    /// <inheritdoc />
    public partial class AddGameConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "enemy_configs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    max_health = table.Column<int>(type: "integer", nullable: false),
                    score_on_kill = table.Column<int>(type: "integer", nullable: false),
                    damage_per_bullet = table.Column<int>(type: "integer", nullable: false),
                    egg_spawn_interval_min_seconds = table.Column<float>(type: "real", nullable: false),
                    egg_spawn_interval_max_seconds = table.Column<float>(type: "real", nullable: false),
                    is_boss = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enemy_configs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "gameplay_configs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    grid_size = table.Column<float>(type: "real", nullable: false),
                    destroy_distance = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gameplay_configs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "player_configs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    speed = table.Column<float>(type: "real", nullable: false),
                    shield_time_seconds = table.Column<float>(type: "real", nullable: false),
                    score_per_chicken_leg = table.Column<int>(type: "integer", nullable: false),
                    default_bullet_level = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_player_configs", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "enemy_configs");

            migrationBuilder.DropTable(
                name: "gameplay_configs");

            migrationBuilder.DropTable(
                name: "player_configs");
        }
    }
}
