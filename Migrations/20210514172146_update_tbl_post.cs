using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkAppReactAPI.Migrations
{
    public partial class update_tbl_post : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DistrictId",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "DistrictId",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
