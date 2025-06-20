using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeadacheTracker.Migrations
{
    /// <inheritdoc />
    public partial class SetNullOnDeleteForHeadacheRecordRelations2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeadacheRecords_Medications_MedicationId",
                table: "HeadacheRecords");

            migrationBuilder.AddForeignKey(
                name: "FK_HeadacheRecords_Medications_MedicationId",
                table: "HeadacheRecords",
                column: "MedicationId",
                principalTable: "Medications",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeadacheRecords_Medications_MedicationId",
                table: "HeadacheRecords");

            migrationBuilder.AddForeignKey(
                name: "FK_HeadacheRecords_Medications_MedicationId",
                table: "HeadacheRecords",
                column: "MedicationId",
                principalTable: "Medications",
                principalColumn: "Id");
        }
    }
}
