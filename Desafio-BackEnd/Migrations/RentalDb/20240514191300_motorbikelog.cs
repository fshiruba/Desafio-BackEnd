using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Desafio_Backend.Migrations.RentalDb
{
    /// <inheritdoc />
    public partial class motorbikelog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "Motorbikes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "MotorbikeLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TimeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MotorbikeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MotorbikeLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MotorbikeLogs_Motorbikes_MotorbikeId",
                        column: x => x.MotorbikeId,
                        principalTable: "Motorbikes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Motorbikes",
                columns: new[] { "Id", "AdminId", "Model", "Plate", "ProductionYear" },
                values: new object[,]
                {
                    { 1, "SYSTEM", "YAMAHA", "ABC-1234", 2000 },
                    { 2, "SYSTEM", "HONDA", "DEF-5678", 2010 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MotorbikeLogs_MotorbikeId",
                table: "MotorbikeLogs",
                column: "MotorbikeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MotorbikeLogs");

            migrationBuilder.DeleteData(
                table: "Motorbikes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Motorbikes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "Motorbikes",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
