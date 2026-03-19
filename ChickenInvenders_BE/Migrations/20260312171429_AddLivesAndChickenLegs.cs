using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChickenInvenders_BE.Migrations
{
    /// <inheritdoc />
    public partial class AddLivesAndChickenLegs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "total_chicken_legs",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "best_chicken_legs",
                table: "user_level_scores",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Lives",
                table: "player_configs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "total_chicken_legs",
                table: "users");

            migrationBuilder.DropColumn(
                name: "best_chicken_legs",
                table: "user_level_scores");

            migrationBuilder.DropColumn(
                name: "Lives",
                table: "player_configs");
        }
    }
}
