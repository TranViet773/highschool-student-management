using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class Alter_ForgeinKey_Attendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AspNetUsers_StudentsId",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "Student_Id",
                table: "Attendances");

            migrationBuilder.AlterColumn<string>(
                name: "StudentsId",
                table: "Attendances",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AspNetUsers_StudentsId",
                table: "Attendances",
                column: "StudentsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AspNetUsers_StudentsId",
                table: "Attendances");

            migrationBuilder.AlterColumn<string>(
                name: "StudentsId",
                table: "Attendances",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Student_Id",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AspNetUsers_StudentsId",
                table: "Attendances",
                column: "StudentsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
