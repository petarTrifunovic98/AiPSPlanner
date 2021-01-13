using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelPlan.DataAccess.Migrations
{
    public partial class add_on_votable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_TripTypes_TripTypeId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_TripTypeId",
                table: "Trips");

            migrationBuilder.AddColumn<int>(
                name: "TripId",
                table: "TripTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VotableId",
                table: "AddOns",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TripTypes_TripId",
                table: "TripTypes",
                column: "TripId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddOns_VotableId",
                table: "AddOns",
                column: "VotableId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AddOns_Votables_VotableId",
                table: "AddOns",
                column: "VotableId",
                principalTable: "Votables",
                principalColumn: "VotableId");

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
                name: "FK_AddOns_Votables_VotableId",
                table: "AddOns");

            migrationBuilder.DropForeignKey(
                name: "FK_TripTypes_Trips_TripId",
                table: "TripTypes");

            migrationBuilder.DropIndex(
                name: "IX_TripTypes_TripId",
                table: "TripTypes");

            migrationBuilder.DropIndex(
                name: "IX_AddOns_VotableId",
                table: "AddOns");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "TripTypes");

            migrationBuilder.DropColumn(
                name: "VotableId",
                table: "AddOns");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TripTypeId",
                table: "Trips",
                column: "TripTypeId");

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
