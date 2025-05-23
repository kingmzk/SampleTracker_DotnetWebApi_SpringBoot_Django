using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModelTracKer.Migrations
{
    /// <inheritdoc />
    public partial class mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_opp_competition_competition_competion_id",
                table: "opp_competition");

            migrationBuilder.DropIndex(
                name: "IX_opp_competition_competion_id",
                table: "opp_competition");

            migrationBuilder.DropColumn(
                name: "competion_id",
                table: "opp_competition");

            migrationBuilder.CreateIndex(
                name: "IX_opp_competition_competition_id",
                table: "opp_competition",
                column: "competition_id");

            migrationBuilder.AddForeignKey(
                name: "FK_opp_competition_competition_competition_id",
                table: "opp_competition",
                column: "competition_id",
                principalTable: "competition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_opp_competition_competition_competition_id",
                table: "opp_competition");

            migrationBuilder.DropIndex(
                name: "IX_opp_competition_competition_id",
                table: "opp_competition");

            migrationBuilder.AddColumn<int>(
                name: "competion_id",
                table: "opp_competition",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_opp_competition_competion_id",
                table: "opp_competition",
                column: "competion_id");

            migrationBuilder.AddForeignKey(
                name: "FK_opp_competition_competition_competion_id",
                table: "opp_competition",
                column: "competion_id",
                principalTable: "competition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
