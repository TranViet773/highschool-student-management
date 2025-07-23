using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class Update_att_Evaluation_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Classification",
                table: "Evaluations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Conduct",
                table: "Evaluations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Performance",
                table: "Evaluations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "Evaluations",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Classification",
                table: "Evaluations");

            migrationBuilder.DropColumn(
                name: "Conduct",
                table: "Evaluations");

            migrationBuilder.DropColumn(
                name: "Performance",
                table: "Evaluations");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Evaluations");
        }
    }
}
