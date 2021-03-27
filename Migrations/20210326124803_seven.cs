using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkAppReactAPI.Migrations
{
    public partial class seven : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_HistoryExChange_DetailsExchangeid",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "DetailsExchangeid",
                table: "Order",
                newName: "HistoryExChangeid");

            migrationBuilder.RenameIndex(
                name: "IX_Order_DetailsExchangeid",
                table: "Order",
                newName: "IX_Order_HistoryExChangeid");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_HistoryExChange_HistoryExChangeid",
                table: "Order",
                column: "HistoryExChangeid",
                principalTable: "HistoryExChange",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_HistoryExChange_HistoryExChangeid",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "HistoryExChangeid",
                table: "Order",
                newName: "DetailsExchangeid");

            migrationBuilder.RenameIndex(
                name: "IX_Order_HistoryExChangeid",
                table: "Order",
                newName: "IX_Order_DetailsExchangeid");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_HistoryExChange_DetailsExchangeid",
                table: "Order",
                column: "DetailsExchangeid",
                principalTable: "HistoryExChange",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
