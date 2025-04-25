using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class Update_att_Person_Class : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Classes_Quantity",
                table: "Classes",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "latestLogIn",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "strPassword",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "latestLogIn",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "strPassword",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Classes_Quantity",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
