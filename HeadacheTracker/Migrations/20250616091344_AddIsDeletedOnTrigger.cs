using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeadacheTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedOnTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Triggers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Triggers");
        }
    }
}
