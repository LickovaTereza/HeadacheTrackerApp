using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HeadacheTracker.Migrations
{
    /// <inheritdoc />
    public partial class HeadacheRecordTabNullableMedicationTreatmentTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeadacheRecords_Medications_MedicationId",
                table: "HeadacheRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_HeadacheRecords_Treatments_TreatmentId",
                table: "HeadacheRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_HeadacheRecords_Triggers_TriggerId",
                table: "HeadacheRecords");

            migrationBuilder.AlterColumn<int>(
                name: "TriggerId",
                table: "HeadacheRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TreatmentId",
                table: "HeadacheRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MedicationId",
                table: "HeadacheRecords",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_HeadacheRecords_Medications_MedicationId",
                table: "HeadacheRecords",
                column: "MedicationId",
                principalTable: "Medications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HeadacheRecords_Treatments_TreatmentId",
                table: "HeadacheRecords",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HeadacheRecords_Triggers_TriggerId",
                table: "HeadacheRecords",
                column: "TriggerId",
                principalTable: "Triggers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HeadacheRecords_Medications_MedicationId",
                table: "HeadacheRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_HeadacheRecords_Treatments_TreatmentId",
                table: "HeadacheRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_HeadacheRecords_Triggers_TriggerId",
                table: "HeadacheRecords");

            migrationBuilder.AlterColumn<int>(
                name: "TriggerId",
                table: "HeadacheRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TreatmentId",
                table: "HeadacheRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MedicationId",
                table: "HeadacheRecords",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HeadacheRecords_Medications_MedicationId",
                table: "HeadacheRecords",
                column: "MedicationId",
                principalTable: "Medications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HeadacheRecords_Treatments_TreatmentId",
                table: "HeadacheRecords",
                column: "TreatmentId",
                principalTable: "Treatments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HeadacheRecords_Triggers_TriggerId",
                table: "HeadacheRecords",
                column: "TriggerId",
                principalTable: "Triggers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
