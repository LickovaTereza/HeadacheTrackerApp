using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeadacheTracker.Migrations
{
    /// <inheritdoc />
    public partial class DropHeadacheEntryTab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_HeadacheEntries_HeadacheEntryId",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_Triggers_HeadacheEntries_HeadacheEntryId",
                table: "Triggers");

            migrationBuilder.DropTable(
                name: "HeadacheEntries");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "HeadacheEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Intensity = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeadacheEntries", x => x.Id);
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
    }
}
