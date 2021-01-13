using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelPlan.DataAccess.Migrations
{
    public partial class sea_decorators : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DecoratorId",
                table: "AddOns",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddOns_DecoratorId",
                table: "AddOns",
                column: "DecoratorId",
                unique: true,
                filter: "[DecoratorId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AddOns_AddOns_DecoratorId",
                table: "AddOns",
                column: "DecoratorId",
                principalTable: "AddOns",
                principalColumn: "AddOnId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddOns_AddOns_DecoratorId",
                table: "AddOns");

            migrationBuilder.DropIndex(
                name: "IX_AddOns_DecoratorId",
                table: "AddOns");

            migrationBuilder.DropColumn(
                name: "DecoratorId",
                table: "AddOns");
        }
    }
}
