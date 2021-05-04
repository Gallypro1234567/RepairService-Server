using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkAppReactAPI.Migrations
{
    public partial class update_column_tbl_ApplyPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkerOfService",
                table: "ApplyToPosts",
                newName: "WorkerOfServiceCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WorkerOfServiceCode",
                table: "ApplyToPosts",
                newName: "WorkerOfService");
        }
    }
}
