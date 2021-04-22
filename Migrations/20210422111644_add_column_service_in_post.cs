using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkAppReactAPI.Migrations
{
    public partial class add_column_service_in_post : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ServiceId",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ServiceId",
                table: "Posts",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Services_ServiceId",
                table: "Posts",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Services_ServiceId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ServiceId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Posts");
        }
    }
}
