using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class TempMigration : Migration
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

            migrationBuilder.AlterColumn<string>(
                name: "strPassword",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    Attendance_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsExcused = table.Column<bool>(type: "bit", nullable: false),
                    IsUnexcused = table.Column<bool>(type: "bit", nullable: false),
                    Attendance_Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attendance_Semester = table.Column<int>(type: "int", nullable: false),
                    Attendance_Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attendance_Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Attendance_MarkedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Attendance_SubjectName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Attendance_SubjectId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Attendance_Id);
                    table.ForeignKey(
                        name: "FK_Attendances_AspNetUsers_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attendances_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reevaluations",
                columns: table => new
                {
                    Reevaluation_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reevaluation_ApporverId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reevaluation_ScoreColumn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reevaluation_PreScore = table.Column<double>(type: "float", nullable: false),
                    Reevaluation_AfterScore = table.Column<double>(type: "float", nullable: true),
                    Reevaluation_Semester = table.Column<int>(type: "int", nullable: false),
                    Reevaluation_Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reevaluation_Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reevaluation_Status = table.Column<int>(type: "int", nullable: false),
                    Reevaluation_TimestampApprove = table.Column<DateOnly>(type: "date", nullable: true),
                    Reevaluation_TimestampEdit = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reevaluation_TimestampRequest = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StudentsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SubjectsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reevaluations", x => x.Reevaluation_Id);
                    table.ForeignKey(
                        name: "FK_Reevaluations_AspNetUsers_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reevaluations_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reevaluations_Subjects_SubjectsId",
                        column: x => x.SubjectsId,
                        principalTable: "Subjects",
                        principalColumn: "Subject_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentsId",
                table: "Attendances",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_TeacherId",
                table: "Attendances",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Reevaluations_StudentsId",
                table: "Reevaluations",
                column: "StudentsId");

            migrationBuilder.CreateIndex(
                name: "IX_Reevaluations_SubjectsId",
                table: "Reevaluations",
                column: "SubjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_Reevaluations_TeacherId",
                table: "Reevaluations",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Reevaluations");

            migrationBuilder.DropColumn(
                name: "OralScore1",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "OralScore2",
                table: "Student_Score");

            migrationBuilder.DropColumn(
                name: "OralScore3",
                table: "Student_Score");

            migrationBuilder.AlterColumn<string>(
                name: "strPassword",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
