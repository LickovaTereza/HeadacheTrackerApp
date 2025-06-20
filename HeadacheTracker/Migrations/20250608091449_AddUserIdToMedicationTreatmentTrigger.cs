using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeadacheTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToMedicationTreatmentTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Triggers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Treatments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Medications",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Triggers_UserId",
                table: "Triggers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Treatments_UserId",
                table: "Treatments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Medications_UserId",
                table: "Medications",
                column: "UserId");

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

            migrationBuilder.DropIndex(
                name: "IX_Triggers_UserId",
                table: "Triggers");

            migrationBuilder.DropIndex(
                name: "IX_Treatments_UserId",
                table: "Treatments");

            migrationBuilder.DropIndex(
                name: "IX_Medications_UserId",
                table: "Medications");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Triggers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Treatments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Medications");
        }
    }
}
