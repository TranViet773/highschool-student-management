using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Att_In_Reevaluation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reevaluation_EditorId",
                table: "Reevaluations");

            migrationBuilder.DropColumn(
                name: "Reevaluation_StudentId",
                table: "Reevaluations");

            migrationBuilder.DropColumn(
                name: "Reevaluation_SubjectId",
                table: "Reevaluations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reevaluation_EditorId",
                table: "Reevaluations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reevaluation_StudentId",
                table: "Reevaluations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Reevaluation_SubjectId",
                table: "Reevaluations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
