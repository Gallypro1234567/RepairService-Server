using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkAppReactAPI.Migrations
{
    public partial class Initial11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "PreferentialOfServices");

            migrationBuilder.DropColumn(
                name: "Percents",
                table: "PreferentialOfServices");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "PreferentialOfServices");

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "Preferentials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Percents",
                table: "Preferentials",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "Preferentials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromDate",
                table: "Preferentials");

            migrationBuilder.DropColumn(
                name: "Percents",
                table: "Preferentials");

            migrationBuilder.DropColumn(
                name: "ToDate",
                table: "Preferentials");

            migrationBuilder.AddColumn<DateTime>(
                name: "FromDate",
                table: "PreferentialOfServices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Percents",
                table: "PreferentialOfServices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ToDate",
                table: "PreferentialOfServices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
