using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class Alter_att_Reevaluation_and_Score : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "OralScore1_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OralScore1_Timestamp",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OralScore2_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OralScore2_Timestamp",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OralScore3_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OralScore3_Timestamp",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "QuizScore1",
                table: "Student_Score",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QuizScore1_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QuizScore1_Timestamp",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "QuizScore2",
                table: "Student_Score",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QuizScore2_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QuizScore2_Timestamp",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "QuizScore3",
                table: "Student_Score",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QuizScore3_Expire",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QuizScore3_Timestamp",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Reevaluation_TimestampApprove",
                table: "Reevaluations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClassId",
                table: "Reevaluations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Reevaluation_TimestampExpiry",
                table: "Reevaluations",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "OralScore1_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "OralScore1_Timestamp",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "OralScore2_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "OralScore2_Timestamp",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "OralScore3_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "OralScore3_Timestamp",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "QuizScore1",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "QuizScore1_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "QuizScore1_Timestamp",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "QuizScore2",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "QuizScore2_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "QuizScore2_Timestamp",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "QuizScore3",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "QuizScore3_Expire",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "QuizScore3_Timestamp",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Reevaluations");

            migrationBuilder.DropColumn(
                name: "Reevaluation_TimestampExpiry",
                table: "Reevaluations");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "Reevaluation_TimestampApprove",
                table: "Reevaluations",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
