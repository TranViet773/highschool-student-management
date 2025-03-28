using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class Update_Schedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Class_Student_AspNetUsers_Student_Id",
                table: "Class_Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Class_Student_Classes_Class_Id",
                table: "Class_Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Class_Student_Schedules_Schedule_Id",
                table: "Class_Student");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Detail_Schedules_ScheduleId",
                table: "Schedule_Detail");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Detail_Subjects_SubjectId",
                table: "Schedule_Detail");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Score_AcademicTranscripts_ATId",
                table: "Student_Score");

            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_Subject_AspNetUsers_Teacher_Id",
                table: "Teacher_Subject");

            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_Subject_Classes_Class_Id",
                table: "Teacher_Subject");

            migrationBuilder.DropForeignKey(
                name: "FK_Teacher_Subject_Subjects_Subject_Id",
                table: "Teacher_Subject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teacher_Subject",
                table: "Teacher_Subject");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedule_Detail",
                table: "Schedule_Detail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Class_Student",
                table: "Class_Student");

            migrationBuilder.RenameTable(
                name: "Teacher_Subject",
                newName: "TeacherSubjects");

            migrationBuilder.RenameTable(
                name: "Schedule_Detail",
                newName: "Schedule_Details");

            migrationBuilder.RenameTable(
                name: "Class_Student",
                newName: "ClassStudents");

            migrationBuilder.RenameIndex(
                name: "IX_Teacher_Subject_Teacher_Id",
                table: "TeacherSubjects",
                newName: "IX_TeacherSubjects_Teacher_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Teacher_Subject_Class_Id",
                table: "TeacherSubjects",
                newName: "IX_TeacherSubjects_Class_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Schedule_Detail_ScheduleId",
                table: "Schedule_Details",
                newName: "IX_Schedule_Details_ScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_Class_Student_Schedule_Id",
                table: "ClassStudents",
                newName: "IX_ClassStudents_Schedule_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Class_Student_Class_Id",
                table: "ClassStudents",
                newName: "IX_ClassStudents_Class_Id");

            migrationBuilder.AlterColumn<Guid>(
                name: "Schedule_Id",
                table: "ClassStudents",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeacherSubjects",
                table: "TeacherSubjects",
                columns: new[] { "Subject_Id", "Teacher_Id", "Class_Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedule_Details",
                table: "Schedule_Details",
                columns: new[] { "SubjectId", "ScheduleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ClassStudents",
                table: "ClassStudents",
                columns: new[] { "Student_Id", "Class_Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudents_AspNetUsers_Student_Id",
                table: "ClassStudents",
                column: "Student_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudents_Classes_Class_Id",
                table: "ClassStudents",
                column: "Class_Id",
                principalTable: "Classes",
                principalColumn: "Classes_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClassStudents_Schedules_Schedule_Id",
                table: "ClassStudents",
                column: "Schedule_Id",
                principalTable: "Schedules",
                principalColumn: "Schedules_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Details_Schedules_ScheduleId",
                table: "Schedule_Details",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Schedules_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Details_Subjects_SubjectId",
                table: "Schedule_Details",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Subject_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Score_AcademicTranscripts_ATId",
                table: "Student_Score",
                column: "ATId",
                principalTable: "AcademicTranscripts",
                principalColumn: "AcademicTranscript_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubjects_AspNetUsers_Teacher_Id",
                table: "TeacherSubjects",
                column: "Teacher_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubjects_Classes_Class_Id",
                table: "TeacherSubjects",
                column: "Class_Id",
                principalTable: "Classes",
                principalColumn: "Classes_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeacherSubjects_Subjects_Subject_Id",
                table: "TeacherSubjects",
                column: "Subject_Id",
                principalTable: "Subjects",
                principalColumn: "Subject_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudents_AspNetUsers_Student_Id",
                table: "ClassStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudents_Classes_Class_Id",
                table: "ClassStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_ClassStudents_Schedules_Schedule_Id",
                table: "ClassStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Details_Schedules_ScheduleId",
                table: "Schedule_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Details_Subjects_SubjectId",
                table: "Schedule_Details");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Score_AcademicTranscripts_ATId",
                table: "Student_Score");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubjects_AspNetUsers_Teacher_Id",
                table: "TeacherSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubjects_Classes_Class_Id",
                table: "TeacherSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_TeacherSubjects_Subjects_Subject_Id",
                table: "TeacherSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeacherSubjects",
                table: "TeacherSubjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Schedule_Details",
                table: "Schedule_Details");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ClassStudents",
                table: "ClassStudents");

            migrationBuilder.RenameTable(
                name: "TeacherSubjects",
                newName: "Teacher_Subject");

            migrationBuilder.RenameTable(
                name: "Schedule_Details",
                newName: "Schedule_Detail");

            migrationBuilder.RenameTable(
                name: "ClassStudents",
                newName: "Class_Student");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherSubjects_Teacher_Id",
                table: "Teacher_Subject",
                newName: "IX_Teacher_Subject_Teacher_Id");

            migrationBuilder.RenameIndex(
                name: "IX_TeacherSubjects_Class_Id",
                table: "Teacher_Subject",
                newName: "IX_Teacher_Subject_Class_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Schedule_Details_ScheduleId",
                table: "Schedule_Detail",
                newName: "IX_Schedule_Detail_ScheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_ClassStudents_Schedule_Id",
                table: "Class_Student",
                newName: "IX_Class_Student_Schedule_Id");

            migrationBuilder.RenameIndex(
                name: "IX_ClassStudents_Class_Id",
                table: "Class_Student",
                newName: "IX_Class_Student_Class_Id");

            migrationBuilder.AlterColumn<Guid>(
                name: "Schedule_Id",
                table: "Class_Student",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teacher_Subject",
                table: "Teacher_Subject",
                columns: new[] { "Subject_Id", "Teacher_Id", "Class_Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Schedule_Detail",
                table: "Schedule_Detail",
                columns: new[] { "SubjectId", "ScheduleId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Class_Student",
                table: "Class_Student",
                columns: new[] { "Student_Id", "Class_Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_Class_Student_AspNetUsers_Student_Id",
                table: "Class_Student",
                column: "Student_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Class_Student_Classes_Class_Id",
                table: "Class_Student",
                column: "Class_Id",
                principalTable: "Classes",
                principalColumn: "Classes_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Class_Student_Schedules_Schedule_Id",
                table: "Class_Student",
                column: "Schedule_Id",
                principalTable: "Schedules",
                principalColumn: "Schedules_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Detail_Schedules_ScheduleId",
                table: "Schedule_Detail",
                column: "ScheduleId",
                principalTable: "Schedules",
                principalColumn: "Schedules_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Detail_Subjects_SubjectId",
                table: "Schedule_Detail",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Subject_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Score_AcademicTranscripts_ATId",
                table: "Student_Score",
                column: "ATId",
                principalTable: "AcademicTranscripts",
                principalColumn: "AcademicTranscript_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_Subject_AspNetUsers_Teacher_Id",
                table: "Teacher_Subject",
                column: "Teacher_Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_Subject_Classes_Class_Id",
                table: "Teacher_Subject",
                column: "Class_Id",
                principalTable: "Classes",
                principalColumn: "Classes_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Teacher_Subject_Subjects_Subject_Id",
                table: "Teacher_Subject",
                column: "Subject_Id",
                principalTable: "Subjects",
                principalColumn: "Subject_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
