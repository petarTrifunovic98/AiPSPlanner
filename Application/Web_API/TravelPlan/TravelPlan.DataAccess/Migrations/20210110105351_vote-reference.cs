using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelPlan.DataAccess.Migrations
{
    public partial class votereference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccommodationPictures_Votable_AccommodationId",
                table: "AccommodationPictures");

            migrationBuilder.DropForeignKey(
                name: "FK_Votable_Trips_TripId",
                table: "Votable");

            migrationBuilder.DropForeignKey(
                name: "FK_Votable_Votable_LocationId",
                table: "Votable");

            migrationBuilder.DropIndex(
                name: "IX_Votable_LocationId",
                table: "Votable");

            migrationBuilder.DropIndex(
                name: "IX_Votable_TripId",
                table: "Votable");

            migrationBuilder.DropColumn(
                name: "Accommodation_Description",
                table: "Votable");

            migrationBuilder.DropColumn(
                name: "Accommodation_From",
                table: "Votable");

            migrationBuilder.DropColumn(
                name: "Accommodation_Name",
                table: "Votable");

            migrationBuilder.DropColumn(
                name: "Accommodation_To",
                table: "Votable");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Votable");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Votable");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Votable");

            migrationBuilder.DropColumn(
                name: "From",
                table: "Votable");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Votable");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Votable");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Votable");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Votable");

            migrationBuilder.DropColumn(
                name: "To",
                table: "Votable");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "Votable");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Votable");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TripId = table.Column<int>(type: "int", nullable: false),
                    VotableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationId);
                    table.ForeignKey(
                        name: "FK_Locations_Trips_TripId",
                        column: x => x.TripId,
                        principalTable: "Trips",
                        principalColumn: "TripId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Locations_Votable_VotableId",
                        column: x => x.VotableId,
                        principalTable: "Votable",
                        principalColumn: "VotableId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accommodations",
                columns: table => new
                {
                    AccommodationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    From = table.Column<DateTime>(type: "datetime2", nullable: false),
                    To = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    VotableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accommodations", x => x.AccommodationId);
                    table.ForeignKey(
                        name: "FK_Accommodations_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "LocationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Accommodations_Votable_VotableId",
                        column: x => x.VotableId,
                        principalTable: "Votable",
                        principalColumn: "VotableId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_LocationId",
                table: "Accommodations",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Accommodations_VotableId",
                table: "Accommodations",
                column: "VotableId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_TripId",
                table: "Locations",
                column: "TripId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_VotableId",
                table: "Locations",
                column: "VotableId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccommodationPictures_Accommodations_AccommodationId",
                table: "AccommodationPictures",
                column: "AccommodationId",
                principalTable: "Accommodations",
                principalColumn: "AccommodationId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccommodationPictures_Accommodations_AccommodationId",
                table: "AccommodationPictures");

            migrationBuilder.DropTable(
                name: "Accommodations");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.AddColumn<string>(
                name: "Accommodation_Description",
                table: "Votable",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Accommodation_From",
                table: "Votable",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Accommodation_Name",
                table: "Votable",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Accommodation_To",
                table: "Votable",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Votable",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Votable",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Votable",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "From",
                table: "Votable",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Votable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Votable",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Votable",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Votable",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "To",
                table: "Votable",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TripId",
                table: "Votable",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Votable",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votable_LocationId",
                table: "Votable",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Votable_TripId",
                table: "Votable",
                column: "TripId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccommodationPictures_Votable_AccommodationId",
                table: "AccommodationPictures",
                column: "AccommodationId",
                principalTable: "Votable",
                principalColumn: "VotableId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votable_Trips_TripId",
                table: "Votable",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "TripId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votable_Votable_LocationId",
                table: "Votable",
                column: "LocationId",
                principalTable: "Votable",
                principalColumn: "VotableId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
