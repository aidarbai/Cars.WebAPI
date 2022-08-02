using Microsoft.EntityFrameworkCore.Migrations;

namespace Cars.DAL.Migrations
{
    public partial class ChangedUserPropBanned : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BannedOnDate",
                table: "AspNetUsers",
                newName: "BannedTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BannedTime",
                table: "AspNetUsers",
                newName: "BannedOnDate");
        }
    }
}
