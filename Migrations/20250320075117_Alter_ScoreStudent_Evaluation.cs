using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class Alter_ScoreStudent_Evaluation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Score_AcademicTranscripts_ATId",
                table: "Student_Score");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Score_AspNetUsers_StudentId",
                table: "Student_Score");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Score_Scores_ScoreId",
                table: "Student_Score");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Score_Subjects_SubjectId",
                table: "Student_Score");

            migrationBuilder.DropTable(
                name: "AcademicTranscripts");

            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Student_Score",
                table: "Student_Score");

            migrationBuilder.DropIndex(
                name: "IX_Student_Score_ATId",
                table: "Student_Score");

            migrationBuilder.DropIndex(
                name: "IX_Student_Score_ScoreId",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "ScoreId",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "ATId",
                table: "Student_Score");

            migrationBuilder.AlterColumn<Guid>(
                name: "SubjectId",
                table: "Student_Score",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "Student_Score",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Student_Score",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<double>(
                name: "FinalExamScore",
                table: "Student_Score",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinalExamScore_Timestamp",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OralScore",
                table: "Student_Score",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OralScore_Timestamp",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "QuizScore",
                table: "Student_Score",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "QuizScore_Timestamp",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TestScore",
                table: "Student_Score",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TestScore_Timestamp",
                table: "Student_Score",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student_Score",
                table: "Student_Score",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Evaluations",
                columns: table => new
                {
                    Evaluation_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    semester = table.Column<int>(type: "int", nullable: false),
                    Evaluation_Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StudentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId1 = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evaluations", x => x.Evaluation_Id);
                    table.ForeignKey(
                        name: "FK_Evaluations_AspNetUsers_StudentId1",
                        column: x => x.StudentId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Evaluations_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_Score_StudentId",
                table: "Student_Score",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_StudentId1",
                table: "Evaluations",
                column: "StudentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Evaluations_TeacherId",
                table: "Evaluations",
                column: "TeacherId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Student_Score_AspNetUsers_StudentId",
                table: "Student_Score");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Score_Subjects_SubjectId",
                table: "Student_Score");

            migrationBuilder.DropTable(
                name: "Evaluations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Student_Score",
                table: "Student_Score");

            migrationBuilder.DropIndex(
                name: "IX_Student_Score_StudentId",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "FinalExamScore",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "FinalExamScore_Timestamp",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "OralScore",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "OralScore_Timestamp",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "QuizScore",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "QuizScore_Timestamp",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "TestScore",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "TestScore_Timestamp",
                table: "Student_Score");

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

            migrationBuilder.AddColumn<Guid>(
                name: "ScoreId",
                table: "Student_Score",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ATId",
                table: "Student_Score",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Student_Score",
                table: "Student_Score",
                columns: new[] { "StudentId", "ScoreId", "SubjectId", "ATId" });

            migrationBuilder.CreateTable(
                name: "AcademicTranscripts",
                columns: table => new
                {
                    AcademicTranscript_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AVG_1st = table.Column<float>(type: "real", nullable: false),
                    AVG_2st = table.Column<float>(type: "real", nullable: false),
                    AVG_Final = table.Column<float>(type: "real", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Conduct = table.Column<int>(type: "int", nullable: false),
                    Great = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Performance = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicTranscripts", x => x.AcademicTranscript_Id);
                    table.ForeignKey(
                        name: "FK_AcademicTranscripts_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    Score_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Score_1 = table.Column<float>(type: "real", nullable: false),
                    Score_2 = table.Column<float>(type: "real", nullable: false),
                    Score_3 = table.Column<float>(type: "real", nullable: false),
                    Score_final = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.Score_Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Student_Score_ATId",
                table: "Student_Score",
                column: "ATId");

            migrationBuilder.CreateIndex(
                name: "IX_Student_Score_ScoreId",
                table: "Student_Score",
                column: "ScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicTranscripts_TeacherId",
                table: "AcademicTranscripts",
                column: "TeacherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Score_AcademicTranscripts_ATId",
                table: "Student_Score",
                column: "ATId",
                principalTable: "AcademicTranscripts",
                principalColumn: "AcademicTranscript_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Score_AspNetUsers_StudentId",
                table: "Student_Score",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Score_Scores_ScoreId",
                table: "Student_Score",
                column: "ScoreId",
                principalTable: "Scores",
                principalColumn: "Score_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Score_Subjects_SubjectId",
                table: "Student_Score",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Subject_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
