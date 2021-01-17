using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelPlan.DataAccess.Migrations
{
    public partial class trip_trip_type_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_TripTypes_TripTypeId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_TripTypeId",
                table: "Trips");

            migrationBuilder.CreateIndex(
                name: "IX_TripTypes_TripId",
                table: "TripTypes",
                column: "TripId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TripTypes_Trips_TripId",
                table: "TripTypes",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "TripId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TripTypes_Trips_TripId",
                table: "TripTypes");

            migrationBuilder.DropIndex(
                name: "IX_TripTypes_TripId",
                table: "TripTypes");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TripTypeId",
                table: "Trips",
                column: "TripTypeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_TripTypes_TripTypeId",
                table: "Trips",
                column: "TripTypeId",
                principalTable: "TripTypes",
                principalColumn: "TripTypeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
