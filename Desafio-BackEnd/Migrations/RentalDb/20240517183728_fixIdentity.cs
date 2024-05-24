using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Desafio_Backend.Migrations.RentalDb
{
    /// <inheritdoc />
    public partial class fixIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_DeliveryPeople_IdentityUser_IdentityUserId",
            //    table: "DeliveryPeople");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_IdentityUser",
            //    table: "IdentityUser");


            //migrationBuilder.RenameTable(
            //    name: "IdentityUser",
            //    newName: "AspNetUsers");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_AspNetUsers",
            //    table: "AspNetUsers",
            //    column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryPeople_AspNetUsers_IdentityUserId",
                table: "DeliveryPeople",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_DeliveryPeople_AspNetUsers_IdentityUserId",
            //    table: "DeliveryPeople");

            //migrationBuilder.DropPrimaryKey(
            //    name: "PK_AspNetUsers",
            //    table: "AspNetUsers");

            //migrationBuilder.RenameTable(
            //    name: "AspNetUsers",
            //    newName: "IdentityUser");

            //migrationBuilder.AddPrimaryKey(
            //    name: "PK_IdentityUser",
            //    table: "IdentityUser",
            //    column: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_DeliveryPeople_IdentityUser_IdentityUserId",
            //    table: "DeliveryPeople",cls
            //    column: "IdentityUserId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id");
        }
    }
}
