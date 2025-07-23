using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class UpdateattScoringType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalEvaluated_Timestamp",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "FinalEvaluation",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "FinalEvaluation_Expiry",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "FinalExamScore_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "IsQualitativeAssessment",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "MidTermEvaluated_Timpstamp",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "MidTermEvaluation",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "MidTermEvaluation_Expiry",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "MidTermScore_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "OralScore1_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "OralScore2_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "OralScore3_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "OralScore_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "QuizScore1_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "QuizScore2_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "QuizScore3_Expire",
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

            migrationBuilder.AddColumn<bool>(
                name: "Subject_ScoringType",
                table: "Subjects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ScoringType",
                table: "Student_Score",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subject_ScoringType",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "ScoringType",
                table: "Student_Score");

            migrationBuilder.AddColumn<DateTime>(
                name: "FinalEvaluated_Timestamp",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "FinalEvaluation",
                table: "Student_Score",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinalEvaluation_Expiry",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinalExamScore_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsQualitativeAssessment",
                table: "Student_Score",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MidTermEvaluated_Timpstamp",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "MidTermEvaluation",
                table: "Student_Score",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MidTermEvaluation_Expiry",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MidTermScore_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OralScore1_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OralScore2_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OralScore3_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OralScore_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QuizScore1_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QuizScore2_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QuizScore3_Expire",
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
    }
}
