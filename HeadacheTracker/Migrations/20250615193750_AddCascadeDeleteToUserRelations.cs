using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeadacheTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteToUserRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medications_AspNetUsers_UserId",
                table: "Medications");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_AspNetUsers_UserId",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_Triggers_AspNetUsers_UserId",
                table: "Triggers");

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_AspNetUsers_UserId",
                table: "Medications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_AspNetUsers_UserId",
                table: "Treatments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Triggers_AspNetUsers_UserId",
                table: "Triggers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medications_AspNetUsers_UserId",
                table: "Medications");

            migrationBuilder.DropForeignKey(
                name: "FK_Treatments_AspNetUsers_UserId",
                table: "Treatments");

            migrationBuilder.DropForeignKey(
                name: "FK_Triggers_AspNetUsers_UserId",
                table: "Triggers");

            migrationBuilder.AddForeignKey(
                name: "FK_Medications_AspNetUsers_UserId",
                table: "Medications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Treatments_AspNetUsers_UserId",
                table: "Treatments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Triggers_AspNetUsers_UserId",
                table: "Triggers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
