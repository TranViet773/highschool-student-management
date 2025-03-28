using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class AlterAddressEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_PersonId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Wards_WardsWard_Id",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_PersonId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_WardsWard_Id",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "WardsWard_Id",
                table: "Addresses");

            migrationBuilder.AlterColumn<string>(
                name: "Person_Id",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Person_Id",
                table: "Addresses",
                column: "Person_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_Ward_Id",
                table: "Addresses",
                column: "Ward_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_Person_Id",
                table: "Addresses",
                column: "Person_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Wards_Ward_Id",
                table: "Addresses",
                column: "Ward_Id",
                principalTable: "Wards",
                principalColumn: "Ward_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_Person_Id",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Wards_Ward_Id",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_Person_Id",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_Ward_Id",
                table: "Addresses");

            migrationBuilder.AlterColumn<Guid>(
                name: "Person_Id",
                table: "Addresses",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "PersonId",
                table: "Addresses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WardsWard_Id",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PersonId",
                table: "Addresses",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_WardsWard_Id",
                table: "Addresses",
                column: "WardsWard_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_PersonId",
                table: "Addresses",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Wards_WardsWard_Id",
                table: "Addresses",
                column: "WardsWard_Id",
                principalTable: "Wards",
                principalColumn: "Ward_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
