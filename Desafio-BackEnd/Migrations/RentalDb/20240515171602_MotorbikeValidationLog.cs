using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desafio_Backend.Migrations.RentalDb
{
    /// <inheritdoc />
    public partial class MotorbikeValidationLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Plate",
                table: "Motorbikes",
                type: "character varying(7)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.UpdateData(
                table: "Motorbikes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Plate",
                value: "ABC1234");

            migrationBuilder.UpdateData(
                table: "Motorbikes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Plate",
                value: "DEF5678");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Plate",
                table: "Motorbikes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(7)",
                oldMaxLength: 7);

            migrationBuilder.UpdateData(
                table: "Motorbikes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Plate",
                value: "ABC-1234");

            migrationBuilder.UpdateData(
                table: "Motorbikes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Plate",
                value: "DEF-5678");
        }
    }
}
