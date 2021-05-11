using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkAppReactAPI.Migrations
{
    public partial class add_column_update_tbl_feeback_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Feelbacks",
                newName: "PointRating");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PointRating",
                table: "Feelbacks",
                newName: "Rating");
        }
    }
}
