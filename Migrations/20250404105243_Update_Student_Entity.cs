using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class Update_Student_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Student_Parent_Name",
                table: "AspNetUsers",
                newName: "Student_Mother_Name");

            migrationBuilder.RenameColumn(
                name: "Student_Parent_Career",
                table: "AspNetUsers",
                newName: "Student_Mother_Career");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Student_Score",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Student_Father_Career",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Student_Father_Phone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "createAccountAt",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Student_Father_Career",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Student_Father_Phone",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "createAccountAt",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Student_Mother_Name",
                table: "AspNetUsers",
                newName: "Student_Parent_Name");

            migrationBuilder.RenameColumn(
                name: "Student_Mother_Career",
                table: "AspNetUsers",
                newName: "Student_Parent_Career");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "Student_Score",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
