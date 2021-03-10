using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelPlan.DataAccess.Migrations
{
    public partial class winter_spa_decorator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MealDecorator_Lvl2DependId",
                table: "AddOns",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkiEquipmentDecorator_Lvl1DependId",
                table: "AddOns",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpaDecorator_DecoratorId",
                table: "AddOns",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrainTourDecorator_Lvl1DependId",
                table: "AddOns",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WalkDecorator_Lvl1DependId",
                table: "AddOns",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WinterDecorator_DecoratorId",
                table: "AddOns",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddOns_SpaDecorator_DecoratorId",
                table: "AddOns",
                column: "SpaDecorator_DecoratorId");

            migrationBuilder.CreateIndex(
                name: "IX_AddOns_WinterDecorator_DecoratorId",
                table: "AddOns",
                column: "WinterDecorator_DecoratorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddOns_AddOns_SpaDecorator_DecoratorId",
                table: "AddOns",
                column: "SpaDecorator_DecoratorId",
                principalTable: "AddOns",
                principalColumn: "AddOnId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddOns_AddOns_WinterDecorator_DecoratorId",
                table: "AddOns",
                column: "WinterDecorator_DecoratorId",
                principalTable: "AddOns",
                principalColumn: "AddOnId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddOns_AddOns_SpaDecorator_DecoratorId",
                table: "AddOns");

            migrationBuilder.DropForeignKey(
                name: "FK_AddOns_AddOns_WinterDecorator_DecoratorId",
                table: "AddOns");

            migrationBuilder.DropIndex(
                name: "IX_AddOns_SpaDecorator_DecoratorId",
                table: "AddOns");

            migrationBuilder.DropIndex(
                name: "IX_AddOns_WinterDecorator_DecoratorId",
                table: "AddOns");

            migrationBuilder.DropColumn(
                name: "MealDecorator_Lvl2DependId",
                table: "AddOns");

            migrationBuilder.DropColumn(
                name: "SkiEquipmentDecorator_Lvl1DependId",
                table: "AddOns");

            migrationBuilder.DropColumn(
                name: "SpaDecorator_DecoratorId",
                table: "AddOns");

            migrationBuilder.DropColumn(
                name: "TrainTourDecorator_Lvl1DependId",
                table: "AddOns");

            migrationBuilder.DropColumn(
                name: "WalkDecorator_Lvl1DependId",
                table: "AddOns");

            migrationBuilder.DropColumn(
                name: "WinterDecorator_DecoratorId",
                table: "AddOns");
        }
    }
}
