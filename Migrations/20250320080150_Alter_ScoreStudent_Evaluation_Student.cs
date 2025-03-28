using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class Alter_ScoreStudent_Evaluation_Student : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluations_AspNetUsers_StudentId1",
                table: "Evaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_Evaluations_AspNetUsers_TeacherId",
                table: "Evaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Score_AspNetUsers_StudentId",
                table: "Student_Score");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Score_Subjects_SubjectId",
                table: "Student_Score");

            migrationBuilder.DropIndex(
                name: "IX_Evaluations_StudentId1",
                table: "Evaluations");

            migrationBuilder.DropIndex(
                name: "IX_Evaluations_TeacherId",
                table: "Evaluations");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "Evaluations");

            migrationBuilder.DropColumn(
                name: "TeacherId",
                table: "Evaluations");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubjectId",
                table: "Student_Score",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "Student_Score",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "Evaluations",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<string>(
                name: "Student_Parent_Career",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Student_Parent_Name",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Student_Parent_Phone",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_StudentId",
                table: "Evaluations",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluations_AspNetUsers_StudentId",
                table: "Evaluations",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Score_AspNetUsers_StudentId",
                table: "Student_Score",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Score_Subjects_SubjectId",
                table: "Student_Score",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Subject_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluations_AspNetUsers_StudentId",
                table: "Evaluations");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Score_AspNetUsers_StudentId",
                table: "Student_Score");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Score_Subjects_SubjectId",
                table: "Student_Score");

            migrationBuilder.DropIndex(
                name: "IX_Evaluations_StudentId",
                table: "Evaluations");

            migrationBuilder.DropColumn(
                name: "Student_Parent_Career",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Student_Parent_Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Student_Parent_Phone",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubjectId",
                table: "Student_Score",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "Student_Score",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "Evaluations",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "StudentId1",
                table: "Evaluations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeacherId",
                table: "Evaluations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_StudentId1",
                table: "Evaluations",
                column: "StudentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_TeacherId",
                table: "Evaluations",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluations_AspNetUsers_StudentId1",
                table: "Evaluations",
                column: "StudentId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluations_AspNetUsers_TeacherId",
                table: "Evaluations",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Score_AspNetUsers_StudentId",
                table: "Student_Score",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Score_Subjects_SubjectId",
                table: "Student_Score",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Subject_Id");
        }
    }
}
