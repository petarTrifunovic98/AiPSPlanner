using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelPlan.DataAccess.Migrations
{
    public partial class decorator_leveling : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LunchDecorator_Lvl2DependId",
                table: "AddOns",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Lvl1DependId",
                table: "AddOns",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Lvl2DependId",
                table: "AddOns",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LunchDecorator_Lvl2DependId",
                table: "AddOns");

            migrationBuilder.DropColumn(
                name: "Lvl1DependId",
                table: "AddOns");

            migrationBuilder.DropColumn(
                name: "Lvl2DependId",
                table: "AddOns");
        }
    }
}
