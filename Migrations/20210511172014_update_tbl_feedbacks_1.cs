using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkAppReactAPI.Migrations
{
    public partial class update_tbl_feedbacks_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "code",
                table: "Feedbacks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "code",
                table: "Feedbacks");
        }
    }
}
