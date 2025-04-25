using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class Add_att_parent_toStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Student_Father_Phone",
                table: "AspNetUsers",
                newName: "Student_Father_Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Student_Father_Name",
                table: "AspNetUsers",
                newName: "Student_Father_Phone");
        }
    }
}
