using Microsoft.EntityFrameworkCore.Migrations;

namespace Cars.DAL.Migrations
{
    public partial class RenamedModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotoUrls_CarsV2_CarV2Id",
                table: "PhotoUrls");

            migrationBuilder.RenameColumn(
                name: "CarV2Id",
                table: "PhotoUrls",
                newName: "CarId");

            migrationBuilder.RenameIndex(
                name: "IX_PhotoUrls_CarV2Id",
                table: "PhotoUrls",
                newName: "IX_PhotoUrls_CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoUrls_CarsV2_CarId",
                table: "PhotoUrls",
                column: "CarId",
                principalTable: "CarsV2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhotoUrls_CarsV2_CarId",
                table: "PhotoUrls");

            migrationBuilder.RenameColumn(
                name: "CarId",
                table: "PhotoUrls",
                newName: "CarV2Id");

            migrationBuilder.RenameIndex(
                name: "IX_PhotoUrls_CarId",
                table: "PhotoUrls",
                newName: "IX_PhotoUrls_CarV2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PhotoUrls_CarsV2_CarV2Id",
                table: "PhotoUrls",
                column: "CarV2Id",
                principalTable: "CarsV2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
