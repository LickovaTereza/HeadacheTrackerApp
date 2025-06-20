using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeadacheTracker.Migrations
{
    /// <inheritdoc />
    public partial class NewTab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeadacheEntryTreatment");

            migrationBuilder.DropTable(
                name: "HeadacheEntryTrigger");

            migrationBuilder.AddColumn<int>(
                name: "HeadacheEntryId",
                table: "Triggers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HeadacheEntryId",
                table: "Treatments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Medications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medications", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_HeadacheEntryId",
                table: "Triggers",
                column: "HeadacheEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_HeadacheEntryId",
                table: "Treatments",
                column: "HeadacheEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_HeadacheEntries_HeadacheEntryId",
                table: "Treatments",
                column: "HeadacheEntryId",
                principalTable: "HeadacheEntries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Triggers_HeadacheEntries_HeadacheEntryId",
                table: "Triggers",
                column: "HeadacheEntryId",
                principalTable: "HeadacheEntries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_HeadacheEntries_HeadacheEntryId",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_Triggers_HeadacheEntries_HeadacheEntryId",
                table: "Triggers");

            migrationBuilder.DropTable(
                name: "Medications");

            migrationBuilder.DropIndex(
                name: "IX_Triggers_HeadacheEntryId",
                table: "Triggers");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_HeadacheEntryId",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "HeadacheEntryId",
                table: "Triggers");

            migrationBuilder.DropColumn(
                name: "HeadacheEntryId",
                table: "Treatments");

            migrationBuilder.CreateTable(
                name: "HeadacheEntryTreatment",
                columns: table => new
                {
                    HeadacheEntriesId = table.Column<int>(type: "int", nullable: false),
                    TreatmentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeadacheEntryTreatment", x => new { x.HeadacheEntriesId, x.TreatmentsId });
                    table.ForeignKey(
                        name: "FK_HeadacheEntryTreatment_HeadacheEntries_HeadacheEntriesId",
                        column: x => x.HeadacheEntriesId,
                        principalTable: "HeadacheEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeadacheEntryTreatment_Treatments_TreatmentsId",
                        column: x => x.TreatmentsId,
                        principalTable: "Treatments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HeadacheEntryTrigger",
                columns: table => new
                {
                    HeadacheEntriesId = table.Column<int>(type: "int", nullable: false),
                    TriggersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeadacheEntryTrigger", x => new { x.HeadacheEntriesId, x.TriggersId });
                    table.ForeignKey(
                        name: "FK_HeadacheEntryTrigger_HeadacheEntries_HeadacheEntriesId",
                        column: x => x.HeadacheEntriesId,
                        principalTable: "HeadacheEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HeadacheEntryTrigger_Triggers_TriggersId",
                        column: x => x.TriggersId,
                        principalTable: "Triggers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HeadacheEntryTreatment_TreatmentsId",
                table: "HeadacheEntryTreatment",
                column: "TreatmentsId");

            migrationBuilder.CreateIndex(
                name: "IX_HeadacheEntryTrigger_TriggersId",
                table: "HeadacheEntryTrigger",
                column: "TriggersId");
        }
    }
}
