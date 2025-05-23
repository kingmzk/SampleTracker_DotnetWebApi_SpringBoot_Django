using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModelTracKer.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyFieldaccmiccomp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "competition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_competition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "microservice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_microservice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "opp_competition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tracker_id = table.Column<int>(type: "int", nullable: false),
                    competition_id = table.Column<int>(type: "int", nullable: false),
                    competion_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_opp_competition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_opp_competition_competition_competion_id",
                        column: x => x.competion_id,
                        principalTable: "competition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_opp_competition_tracker_tracker_id",
                        column: x => x.tracker_id,
                        principalTable: "tracker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "opp_microservice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tracker_id = table.Column<int>(type: "int", nullable: false),
                    microservice_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_opp_microservice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_opp_microservice_microservice_microservice_id",
                        column: x => x.microservice_id,
                        principalTable: "microservice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_opp_microservice_tracker_tracker_id",
                        column: x => x.tracker_id,
                        principalTable: "tracker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_opp_competition_competion_id",
                table: "opp_competition",
                column: "competion_id");

            migrationBuilder.CreateIndex(
                name: "IX_opp_competition_tracker_id",
                table: "opp_competition",
                column: "tracker_id");

            migrationBuilder.CreateIndex(
                name: "IX_opp_microservice_microservice_id",
                table: "opp_microservice",
                column: "microservice_id");

            migrationBuilder.CreateIndex(
                name: "IX_opp_microservice_tracker_id",
                table: "opp_microservice",
                column: "tracker_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "opp_competition");

            migrationBuilder.DropTable(
                name: "opp_microservice");

            migrationBuilder.DropTable(
                name: "competition");

            migrationBuilder.DropTable(
                name: "microservice");
        }
    }
}
