using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkAppReactAPI.Migrations
{
    public partial class add_column_update_tbl_feeback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerOfServices_Feelbacks_FeelbackId",
                table: "WorkerOfServices");

            migrationBuilder.DropIndex(
                name: "IX_WorkerOfServices_FeelbackId",
                table: "WorkerOfServices");

            migrationBuilder.DropColumn(
                name: "FeelbackId",
                table: "WorkerOfServices");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Feelbacks");

            migrationBuilder.AddColumn<double>(
                name: "PointRating",
                table: "WorkerOfServices",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "Feelbacks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Feelbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostCode",
                table: "Feelbacks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Feelbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WorkerOfServiceCode",
                table: "Feelbacks",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PointRating",
                table: "WorkerOfServices");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "Feelbacks");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Feelbacks");

            migrationBuilder.DropColumn(
                name: "PostCode",
                table: "Feelbacks");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Feelbacks");

            migrationBuilder.DropColumn(
                name: "WorkerOfServiceCode",
                table: "Feelbacks");

            migrationBuilder.AddColumn<Guid>(
                name: "FeelbackId",
                table: "WorkerOfServices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Feelbacks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_WorkerOfServices_FeelbackId",
                table: "WorkerOfServices",
                column: "FeelbackId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerOfServices_Feelbacks_FeelbackId",
                table: "WorkerOfServices",
                column: "FeelbackId",
                principalTable: "Feelbacks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
