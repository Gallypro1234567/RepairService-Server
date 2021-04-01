using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkAppReactAPI.Migrations
{
    public partial class Initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerOfServices_Services_CategoryId",
                table: "WorkerOfServices");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "WorkerOfServices",
                newName: "ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerOfServices_CategoryId",
                table: "WorkerOfServices",
                newName: "IX_WorkerOfServices_ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerOfServices_Services_ServiceId",
                table: "WorkerOfServices",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerOfServices_Services_ServiceId",
                table: "WorkerOfServices");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "WorkerOfServices",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkerOfServices_ServiceId",
                table: "WorkerOfServices",
                newName: "IX_WorkerOfServices_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerOfServices_Services_CategoryId",
                table: "WorkerOfServices",
                column: "CategoryId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
