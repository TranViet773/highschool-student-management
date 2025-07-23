using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class Add_Attendance_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    Student_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentsId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Attendance_Id);
                    table.ForeignKey(
                        name: "FK_Attendances_AspNetUsers_StudentsId",
                        column: x => x.StudentsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_StudentsId",
                table: "Attendances",
                column: "StudentsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");
        }
    }
}
