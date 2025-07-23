using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class Updatemigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reevaluation_AspNetUsers_StudentsId",
                table: "Reevaluation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reevaluation_AspNetUsers_TeacherId",
                table: "Reevaluation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reevaluation_Subjects_SubjectsId",
                table: "Reevaluation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reevaluation",
                table: "Reevaluation");

            migrationBuilder.RenameTable(
                name: "Reevaluation",
                newName: "Reevaluations");

            migrationBuilder.RenameIndex(
                name: "IX_Reevaluation_TeacherId",
                table: "Reevaluations",
                newName: "IX_Reevaluations_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Reevaluation_SubjectsId",
                table: "Reevaluations",
                newName: "IX_Reevaluations_SubjectsId");

            migrationBuilder.RenameIndex(
                name: "IX_Reevaluation_StudentsId",
                table: "Reevaluations",
                newName: "IX_Reevaluations_StudentsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reevaluations",
                table: "Reevaluations",
                column: "Reevaluation_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reevaluations_AspNetUsers_StudentsId",
                table: "Reevaluations",
                column: "StudentsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reevaluations_AspNetUsers_TeacherId",
                table: "Reevaluations",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reevaluations_Subjects_SubjectsId",
                table: "Reevaluations",
                column: "SubjectsId",
                principalTable: "Subjects",
                principalColumn: "Subject_Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reevaluations_AspNetUsers_StudentsId",
                table: "Reevaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reevaluations_AspNetUsers_TeacherId",
                table: "Reevaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reevaluations_Subjects_SubjectsId",
                table: "Reevaluations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reevaluations",
                table: "Reevaluations");

            migrationBuilder.RenameTable(
                name: "Reevaluations",
                newName: "Reevaluation");

            migrationBuilder.RenameIndex(
                name: "IX_Reevaluations_TeacherId",
                table: "Reevaluation",
                newName: "IX_Reevaluation_TeacherId");

            migrationBuilder.RenameIndex(
                name: "IX_Reevaluations_SubjectsId",
                table: "Reevaluation",
                newName: "IX_Reevaluation_SubjectsId");

            migrationBuilder.RenameIndex(
                name: "IX_Reevaluations_StudentsId",
                table: "Reevaluation",
                newName: "IX_Reevaluation_StudentsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reevaluation",
                table: "Reevaluation",
                column: "Reevaluation_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reevaluation_AspNetUsers_StudentsId",
                table: "Reevaluation",
                column: "StudentsId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reevaluation_AspNetUsers_TeacherId",
                table: "Reevaluation",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reevaluation_Subjects_SubjectsId",
                table: "Reevaluation",
                column: "SubjectsId",
                principalTable: "Subjects",
                principalColumn: "Subject_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
