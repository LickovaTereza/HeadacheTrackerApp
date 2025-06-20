using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeadacheTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddIsActiveOnTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Triggers",
                newName: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Triggers",
                newName: "IsDeleted");
        }
    }
}
