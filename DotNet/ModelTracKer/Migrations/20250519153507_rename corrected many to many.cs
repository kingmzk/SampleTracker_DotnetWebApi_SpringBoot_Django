using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModelTracKer.Migrations
{
    /// <inheritdoc />
    public partial class renamecorrectedmanytomany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_opp_competition_competition_competition_id",
                table: "opp_competition");

            migrationBuilder.DropForeignKey(
                name: "FK_opp_competition_tracker_tracker_id",
                table: "opp_competition");

            migrationBuilder.DropForeignKey(
                name: "FK_opp_microservice_microservice_microservice_id",
                table: "opp_microservice");

            migrationBuilder.DropForeignKey(
                name: "FK_opp_microservice_tracker_tracker_id",
                table: "opp_microservice");

            migrationBuilder.RenameColumn(
                name: "tracker_id",
                table: "opp_microservice",
                newName: "TrackerId");

            migrationBuilder.RenameColumn(
                name: "microservice_id",
                table: "opp_microservice",
                newName: "MicroserviceId");

            migrationBuilder.RenameIndex(
                name: "IX_opp_microservice_tracker_id",
                table: "opp_microservice",
                newName: "IX_opp_microservice_TrackerId");

            migrationBuilder.RenameIndex(
                name: "IX_opp_microservice_microservice_id",
                table: "opp_microservice",
                newName: "IX_opp_microservice_MicroserviceId");

            migrationBuilder.RenameColumn(
                name: "tracker_id",
                table: "opp_competition",
                newName: "TrackerId");

            migrationBuilder.RenameColumn(
                name: "competition_id",
                table: "opp_competition",
                newName: "CompetitionId");

            migrationBuilder.RenameIndex(
                name: "IX_opp_competition_tracker_id",
                table: "opp_competition",
                newName: "IX_opp_competition_TrackerId");

            migrationBuilder.RenameIndex(
                name: "IX_opp_competition_competition_id",
                table: "opp_competition",
                newName: "IX_opp_competition_CompetitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_opp_competition_competition_CompetitionId",
                table: "opp_competition",
                column: "CompetitionId",
                principalTable: "competition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_opp_competition_tracker_TrackerId",
                table: "opp_competition",
                column: "TrackerId",
                principalTable: "tracker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_opp_microservice_microservice_MicroserviceId",
                table: "opp_microservice",
                column: "MicroserviceId",
                principalTable: "microservice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_opp_microservice_tracker_TrackerId",
                table: "opp_microservice",
                column: "TrackerId",
                principalTable: "tracker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_opp_competition_competition_CompetitionId",
                table: "opp_competition");

            migrationBuilder.DropForeignKey(
                name: "FK_opp_competition_tracker_TrackerId",
                table: "opp_competition");

            migrationBuilder.DropForeignKey(
                name: "FK_opp_microservice_microservice_MicroserviceId",
                table: "opp_microservice");

            migrationBuilder.DropForeignKey(
                name: "FK_opp_microservice_tracker_TrackerId",
                table: "opp_microservice");

            migrationBuilder.RenameColumn(
                name: "TrackerId",
                table: "opp_microservice",
                newName: "tracker_id");

            migrationBuilder.RenameColumn(
                name: "MicroserviceId",
                table: "opp_microservice",
                newName: "microservice_id");

            migrationBuilder.RenameIndex(
                name: "IX_opp_microservice_TrackerId",
                table: "opp_microservice",
                newName: "IX_opp_microservice_tracker_id");

            migrationBuilder.RenameIndex(
                name: "IX_opp_microservice_MicroserviceId",
                table: "opp_microservice",
                newName: "IX_opp_microservice_microservice_id");

            migrationBuilder.RenameColumn(
                name: "TrackerId",
                table: "opp_competition",
                newName: "tracker_id");

            migrationBuilder.RenameColumn(
                name: "CompetitionId",
                table: "opp_competition",
                newName: "competition_id");

            migrationBuilder.RenameIndex(
                name: "IX_opp_competition_TrackerId",
                table: "opp_competition",
                newName: "IX_opp_competition_tracker_id");

            migrationBuilder.RenameIndex(
                name: "IX_opp_competition_CompetitionId",
                table: "opp_competition",
                newName: "IX_opp_competition_competition_id");

            migrationBuilder.AddForeignKey(
                name: "FK_opp_competition_competition_competition_id",
                table: "opp_competition",
                column: "competition_id",
                principalTable: "competition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_opp_competition_tracker_tracker_id",
                table: "opp_competition",
                column: "tracker_id",
                principalTable: "tracker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_opp_microservice_microservice_microservice_id",
                table: "opp_microservice",
                column: "microservice_id",
                principalTable: "microservice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_opp_microservice_tracker_tracker_id",
                table: "opp_microservice",
                column: "tracker_id",
                principalTable: "tracker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
