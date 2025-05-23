using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ModelTracKer.Migrations
{
    /// <inheritdoc />
    public partial class Added2ForeignKeyRelationsShiptomaintable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "GenAiAdoptation",
                table: "tracker",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "GenAiTool_Id",
                table: "tracker",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReasonForNoGenAiAdoptation_Id",
                table: "tracker",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GenAiTool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    isDisabled = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenAiTool", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReasonForNoGenAiAdoptation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    isDisabled = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReasonForNoGenAiAdoptation", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tracker_GenAiTool_Id",
                table: "tracker",
                column: "GenAiTool_Id");

            migrationBuilder.CreateIndex(
                name: "IX_tracker_ReasonForNoGenAiAdoptation_Id",
                table: "tracker",
                column: "ReasonForNoGenAiAdoptation_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_tracker_GenAiTool_GenAiTool_Id",
                table: "tracker",
                column: "GenAiTool_Id",
                principalTable: "GenAiTool",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tracker_ReasonForNoGenAiAdoptation_ReasonForNoGenAiAdoptation_Id",
                table: "tracker",
                column: "ReasonForNoGenAiAdoptation_Id",
                principalTable: "ReasonForNoGenAiAdoptation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tracker_GenAiTool_GenAiTool_Id",
                table: "tracker");

            migrationBuilder.DropForeignKey(
                name: "FK_tracker_ReasonForNoGenAiAdoptation_ReasonForNoGenAiAdoptation_Id",
                table: "tracker");

            migrationBuilder.DropTable(
                name: "GenAiTool");

            migrationBuilder.DropTable(
                name: "ReasonForNoGenAiAdoptation");

            migrationBuilder.DropIndex(
                name: "IX_tracker_GenAiTool_Id",
                table: "tracker");

            migrationBuilder.DropIndex(
                name: "IX_tracker_ReasonForNoGenAiAdoptation_Id",
                table: "tracker");

            migrationBuilder.DropColumn(
                name: "GenAiAdoptation",
                table: "tracker");

            migrationBuilder.DropColumn(
                name: "GenAiTool_Id",
                table: "tracker");

            migrationBuilder.DropColumn(
                name: "ReasonForNoGenAiAdoptation_Id",
                table: "tracker");
        }
    }
}
