using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NL_THUD.Migrations
{
    /// <inheritdoc />
    public partial class Alter_RelationshipAddress_Ward : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addressses_AspNetUsers_PersonId",
                table: "Addressses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addressses_Provinces_ProvincesProvince_Id",
                table: "Addressses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addressses",
                table: "Addressses");

            migrationBuilder.RenameTable(
                name: "Addressses",
                newName: "Addresses");

            migrationBuilder.RenameColumn(
                name: "ProvincesProvince_Id",
                table: "Addresses",
                newName: "WardsWard_Id");

            migrationBuilder.RenameColumn(
                name: "Province_Id",
                table: "Addresses",
                newName: "Ward_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Addressses_ProvincesProvince_Id",
                table: "Addresses",
                newName: "IX_Addresses_WardsWard_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Addressses_PersonId",
                table: "Addresses",
                newName: "IX_Addresses_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Address_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AspNetUsers_PersonId",
                table: "Addresses",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Wards_WardsWard_Id",
                table: "Addresses",
                column: "WardsWard_Id",
                principalTable: "Wards",
                principalColumn: "Ward_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AspNetUsers_PersonId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Wards_WardsWard_Id",
                table: "Addresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Addressses");

            migrationBuilder.RenameColumn(
                name: "WardsWard_Id",
                table: "Addressses",
                newName: "ProvincesProvince_Id");

            migrationBuilder.RenameColumn(
                name: "Ward_Id",
                table: "Addressses",
                newName: "Province_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_WardsWard_Id",
                table: "Addressses",
                newName: "IX_Addressses_ProvincesProvince_Id");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_PersonId",
                table: "Addressses",
                newName: "IX_Addressses_PersonId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addressses",
                table: "Addressses",
                column: "Address_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addressses_AspNetUsers_PersonId",
                table: "Addressses",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addressses_Provinces_ProvincesProvince_Id",
                table: "Addressses",
                column: "ProvincesProvince_Id",
                principalTable: "Provinces",
                principalColumn: "Province_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
