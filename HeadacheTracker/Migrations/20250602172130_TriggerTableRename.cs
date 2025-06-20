using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeadacheTracker.Migrations
{
    /// <inheritdoc />
    public partial class TriggerTableRename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Trigger",
                table: "Trigger");

            migrationBuilder.RenameTable(
                name: "Trigger",
                newName: "Triggers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Triggers",
                table: "Triggers",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Triggers",
                table: "Triggers");

            migrationBuilder.RenameTable(
                name: "Triggers",
                newName: "Trigger");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trigger",
                table: "Trigger",
                column: "Id");
        }
    }
}
