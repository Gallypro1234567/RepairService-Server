using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkAppReactAPI.Migrations
{
    public partial class Initial1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Roles",
                newName: "FunctionCode");

            migrationBuilder.RenameColumn(
                name: "Percent",
                table: "PreferentialOfServices",
                newName: "Percents");

            migrationBuilder.AddColumn<bool>(
                name: "isDelete",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isExport",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isImport",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isInsert",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isSearch",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isUpdate",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDelete",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "isExport",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "isImport",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "isInsert",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "isSearch",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "isUpdate",
                table: "Roles");

            migrationBuilder.RenameColumn(
                name: "FunctionCode",
                table: "Roles",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "Percents",
                table: "PreferentialOfServices",
                newName: "Percent");
        }
    }
}
