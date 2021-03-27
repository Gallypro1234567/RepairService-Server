using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkAppReactAPI.Migrations
{
    public partial class six : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_Detailsid",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_HistoryExChange_DetailsExchangesid",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "DetailsExchangesid",
                table: "Order",
                newName: "DetailsExchangeid");

            migrationBuilder.RenameIndex(
                name: "IX_Order_DetailsExchangesid",
                table: "Order",
                newName: "IX_Order_DetailsExchangeid");

            migrationBuilder.RenameColumn(
                name: "Detailsid",
                table: "Customers",
                newName: "Userid");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_Detailsid",
                table: "Customers",
                newName: "IX_Customers_Userid");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_Userid",
                table: "Customers",
                column: "Userid",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_HistoryExChange_DetailsExchangeid",
                table: "Order",
                column: "DetailsExchangeid",
                principalTable: "HistoryExChange",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Users_Userid",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_HistoryExChange_DetailsExchangeid",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "DetailsExchangeid",
                table: "Order",
                newName: "DetailsExchangesid");

            migrationBuilder.RenameIndex(
                name: "IX_Order_DetailsExchangeid",
                table: "Order",
                newName: "IX_Order_DetailsExchangesid");

            migrationBuilder.RenameColumn(
                name: "Userid",
                table: "Customers",
                newName: "Detailsid");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_Userid",
                table: "Customers",
                newName: "IX_Customers_Detailsid");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Users_Detailsid",
                table: "Customers",
                column: "Detailsid",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_HistoryExChange_DetailsExchangesid",
                table: "Order",
                column: "DetailsExchangesid",
                principalTable: "HistoryExChange",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
