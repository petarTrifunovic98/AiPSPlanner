using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelPlan.DataAccess.Migrations
{
    public partial class pls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Locations_LocationId",
                table: "Accommodations");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Locations_LocationId",
                table: "Accommodations",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Locations_LocationId",
                table: "Accommodations");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Locations_LocationId",
                table: "Accommodations",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
