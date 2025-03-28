using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class Update_Discriminator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_Class_AspNetUsers_Teacher_Id",
                table: "Teacher_Class");

            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_Class_Classes_Class_Id",
                table: "Teacher_Class");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teacher_Class",
                table: "Teacher_Class");

            migrationBuilder.RenameTable(
                name: "Teacher_Class",
                newName: "TeacherClasses");

            migrationBuilder.RenameIndex(
                name: "IX_Teacher_Class_Class_Id",
                table: "TeacherClasses",
                newName: "IX_TeacherClasses_Class_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherClasses",
                table: "TeacherClasses",
                columns: new[] { "Teacher_Id", "Class_Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherClasses_AspNetUsers_Teacher_Id",
                table: "TeacherClasses",
                column: "Teacher_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherClasses_Classes_Class_Id",
                table: "TeacherClasses",
                column: "Class_Id",
                principalTable: "Classes",
                principalColumn: "Classes_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeacherClasses_AspNetUsers_Teacher_Id",
                table: "TeacherClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherClasses_Classes_Class_Id",
                table: "TeacherClasses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherClasses",
                table: "TeacherClasses");

            migrationBuilder.RenameTable(
                name: "TeacherClasses",
                newName: "Teacher_Class");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherClasses_Class_Id",
                table: "Teacher_Class",
                newName: "IX_Teacher_Class_Class_Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teacher_Class",
                table: "Teacher_Class",
                columns: new[] { "Teacher_Id", "Class_Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_Class_AspNetUsers_Teacher_Id",
                table: "Teacher_Class",
                column: "Teacher_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_Class_Classes_Class_Id",
                table: "Teacher_Class",
                column: "Class_Id",
                principalTable: "Classes",
                principalColumn: "Classes_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
