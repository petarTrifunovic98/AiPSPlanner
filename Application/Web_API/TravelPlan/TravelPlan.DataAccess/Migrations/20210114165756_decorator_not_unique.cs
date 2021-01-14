using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelPlan.DataAccess.Migrations
{
    public partial class decorator_not_unique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AddOns_DecoratorId",
                table: "AddOns");

            migrationBuilder.CreateIndex(
                name: "IX_AddOns_DecoratorId",
                table: "AddOns",
                column: "DecoratorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AddOns_DecoratorId",
                table: "AddOns");

            migrationBuilder.CreateIndex(
                name: "IX_AddOns_DecoratorId",
                table: "AddOns",
                column: "DecoratorId",
                unique: true,
                filter: "[DecoratorId] IS NOT NULL");
        }
    }
}
