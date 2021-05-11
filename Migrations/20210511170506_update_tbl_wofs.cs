using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkAppReactAPI.Migrations
{
    public partial class update_tbl_wofs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PointRating",
                table: "WorkerOfServices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "PointRating",
                table: "WorkerOfServices",
                type: "float",
                nullable: true);
        }
    }
}
