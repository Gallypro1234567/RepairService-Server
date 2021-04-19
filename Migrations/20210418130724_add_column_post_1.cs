using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkAppReactAPI.Migrations
{
    public partial class add_column_post_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_WorkerOfServices_WorkerOfServiceId",
                table: "Posts");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkerOfServiceId",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_WorkerOfServices_WorkerOfServiceId",
                table: "Posts",
                column: "WorkerOfServiceId",
                principalTable: "WorkerOfServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_WorkerOfServices_WorkerOfServiceId",
                table: "Posts");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkerOfServiceId",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_WorkerOfServices_WorkerOfServiceId",
                table: "Posts",
                column: "WorkerOfServiceId",
                principalTable: "WorkerOfServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
