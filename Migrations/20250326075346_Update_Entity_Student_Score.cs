using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class Update_Entity_Student_Score : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Student_Score",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Comment_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Comment_UpdateAt",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinalExamScore_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "MidTearmScore",
                table: "Student_Score",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MidTermScore_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MidTermScore_Timestamp",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OralScore_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QuizScore_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TestScore_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateTime_Expire",
                table: "Evaluations",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "Comment_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "Comment_UpdateAt",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "FinalExamScore_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "MidTearmScore",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "MidTermScore_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "MidTermScore_Timestamp",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "OralScore_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "QuizScore_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "TestScore_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "UpdateTime_Expire",
                table: "Evaluations");
        }
    }
}
