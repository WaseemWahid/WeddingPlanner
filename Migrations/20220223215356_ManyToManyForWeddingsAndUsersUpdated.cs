using Microsoft.EntityFrameworkCore.Migrations;

namespace WeddingPlanner.Migrations
{
    public partial class ManyToManyForWeddingsAndUsersUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlannerId",
                table: "Weddings");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Weddings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Weddings");

            migrationBuilder.AddColumn<int>(
                name: "PlannerId",
                table: "Weddings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
