using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class Add_Entity_Reevaluation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "OralScore1",
                table: "Student_Score",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OralScore2",
                table: "Student_Score",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OralScore3",
                table: "Student_Score",
                type: "float",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Reevaluation",
                columns: table => new
                {
                    Reevaluation_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reevaluation_SubjectId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reevaluation_StudentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reevaluation_EditorId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reevaluation_ApporverId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reevaluation_ScoreColumn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reevaluation_PreScore = table.Column<double>(type: "float", nullable: false),
                    Reevaluation_AfterScore = table.Column<double>(type: "float", nullable: true),
                    Reevaluation_Semester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reevaluation_Year = table.Column<int>(type: "int", nullable: false),
                    Reevaluation_Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reevaluation_Status = table.Column<int>(type: "int", nullable: false),
                    Reevaluation_TimestampApprove = table.Column<DateOnly>(type: "date", nullable: true),
                    Reevaluation_TimestampEdit = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reevaluation_TimestampRequest = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StudentsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubjectsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reevaluation", x => x.Reevaluation_Id);
                    table.ForeignKey(
                        name: "FK_Reevaluation_AspNetUsers_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reevaluation_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reevaluation_Subjects_SubjectsId",
                        column: x => x.SubjectsId,
                        principalTable: "Subjects",
                        principalColumn: "Subject_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reevaluation_StudentsId",
                table: "Reevaluation",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Reevaluation_SubjectsId",
                table: "Reevaluation",
                column: "SubjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_Reevaluation_TeacherId",
                table: "Reevaluation",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reevaluation");

            migrationBuilder.DropColumn(
                name: "OralScore1",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "OralScore2",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "OralScore3",
                table: "Student_Score");
        }
    }
}
