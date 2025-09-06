using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace efcoreApi.Migrations
{
    public partial class AddNaviationBasketItemsToGoodsId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_basketItems_GoodsId",
                table: "basketItems",
                column: "GoodsId");

            migrationBuilder.AddForeignKey(
                name: "FK_basketItems_goods_GoodsId",
                table: "basketItems",
                column: "GoodsId",
                principalTable: "goods",
                principalColumn: "GoodsId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_basketItems_goods_GoodsId",
                table: "basketItems");

            migrationBuilder.DropIndex(
                name: "IX_basketItems_GoodsId",
                table: "basketItems");
        }
    }
}
