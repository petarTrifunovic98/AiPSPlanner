using Microsoft.EntityFrameworkCore.Migrations;

namespace TravelPlan.DataAccess.Migrations
{
    public partial class abstract_factory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Votable_VotableId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Votable_VotableId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Users_UserId",
                table: "Vote");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Votable_VotableId",
                table: "Vote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vote",
                table: "Vote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Votable",
                table: "Votable");

            migrationBuilder.RenameTable(
                name: "Vote",
                newName: "Votes");

            migrationBuilder.RenameTable(
                name: "Votable",
                newName: "Votables");

            migrationBuilder.RenameIndex(
                name: "IX_Vote_VotableId",
                table: "Votes",
                newName: "IX_Votes_VotableId");

            migrationBuilder.RenameIndex(
                name: "IX_Vote_UserId",
                table: "Votes",
                newName: "IX_Votes_UserId");

            migrationBuilder.AddColumn<int>(
                name: "AddOnId",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TripTypeId",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Votes",
                table: "Votes",
                column: "VoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Votables",
                table: "Votables",
                column: "VotableId");

            migrationBuilder.CreateTable(
                name: "AddOns",
                columns: table => new
                {
                    AddOnId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOns", x => x.AddOnId);
                });

            migrationBuilder.CreateTable(
                name: "TripTypes",
                columns: table => new
                {
                    TripTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StandardList = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TripTypes", x => x.TripTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_AddOnId",
                table: "Trips",
                column: "AddOnId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_TripTypeId",
                table: "Trips",
                column: "TripTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Votables_VotableId",
                table: "Accommodations",
                column: "VotableId",
                principalTable: "Votables",
                principalColumn: "VotableId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Votables_VotableId",
                table: "Locations",
                column: "VotableId",
                principalTable: "Votables",
                principalColumn: "VotableId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_AddOns_AddOnId",
                table: "Trips",
                column: "AddOnId",
                principalTable: "AddOns",
                principalColumn: "AddOnId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_TripTypes_TripTypeId",
                table: "Trips",
                column: "TripTypeId",
                principalTable: "TripTypes",
                principalColumn: "TripTypeId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Users_UserId",
                table: "Votes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Votables_VotableId",
                table: "Votes",
                column: "VotableId",
                principalTable: "Votables",
                principalColumn: "VotableId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accommodations_Votables_VotableId",
                table: "Accommodations");

            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Votables_VotableId",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_AddOns_AddOnId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_TripTypes_TripTypeId",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Users_UserId",
                table: "Votes");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Votables_VotableId",
                table: "Votes");

            migrationBuilder.DropTable(
                name: "AddOns");

            migrationBuilder.DropTable(
                name: "TripTypes");

            migrationBuilder.DropIndex(
                name: "IX_Trips_AddOnId",
                table: "Trips");

            migrationBuilder.DropIndex(
                name: "IX_Trips_TripTypeId",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Votes",
                table: "Votes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Votables",
                table: "Votables");

            migrationBuilder.DropColumn(
                name: "AddOnId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "TripTypeId",
                table: "Trips");

            migrationBuilder.RenameTable(
                name: "Votes",
                newName: "Vote");

            migrationBuilder.RenameTable(
                name: "Votables",
                newName: "Votable");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_VotableId",
                table: "Vote",
                newName: "IX_Vote_VotableId");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_UserId",
                table: "Vote",
                newName: "IX_Vote_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vote",
                table: "Vote",
                column: "VoteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Votable",
                table: "Votable",
                column: "VotableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accommodations_Votable_VotableId",
                table: "Accommodations",
                column: "VotableId",
                principalTable: "Votable",
                principalColumn: "VotableId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Votable_VotableId",
                table: "Locations",
                column: "VotableId",
                principalTable: "Votable",
                principalColumn: "VotableId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Users_UserId",
                table: "Vote",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Votable_VotableId",
                table: "Vote",
                column: "VotableId",
                principalTable: "Votable",
                principalColumn: "VotableId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
