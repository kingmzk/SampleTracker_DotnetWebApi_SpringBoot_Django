using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModelTracKer.Migrations
{
    /// <inheritdoc />
    public partial class AcceleratorMigrationManyToMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "acclerator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_acclerator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tracker",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tracker", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "opp_accelerator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackerId = table.Column<int>(type: "int", nullable: false),
                    AcceleratorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_opp_accelerator", x => x.Id);
                    table.ForeignKey(
                        name: "FK_opp_accelerator_acclerator_AcceleratorId",
                        column: x => x.AcceleratorId,
                        principalTable: "acclerator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_opp_accelerator_tracker_TrackerId",
                        column: x => x.TrackerId,
                        principalTable: "tracker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_opp_accelerator_AcceleratorId",
                table: "opp_accelerator",
                column: "AcceleratorId");

            migrationBuilder.CreateIndex(
                name: "IX_opp_accelerator_TrackerId",
                table: "opp_accelerator",
                column: "TrackerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "opp_accelerator");

            migrationBuilder.DropTable(
                name: "acclerator");

            migrationBuilder.DropTable(
                name: "tracker");
        }
    }
}
