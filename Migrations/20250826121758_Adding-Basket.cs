using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace efcoreApi.Migrations
{
    public partial class AddingBasket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "basket",
                columns: table => new
                {
                    BasketId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_basket", x => x.BasketId);
                });

            migrationBuilder.CreateTable(
                name: "basketItems",
                columns: table => new
                {
                    BasketitemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BasketId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GoodsId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    RegisterId = table.Column<int>(type: "int", nullable: false),
                    RegisterId1 = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_basketItems", x => x.BasketitemId);
                    table.ForeignKey(
                        name: "FK_basketItems_basket_BasketId",
                        column: x => x.BasketId,
                        principalTable: "basket",
                        principalColumn: "BasketId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_basketItems_Register_RegisterId1",
                        column: x => x.RegisterId1,
                        principalTable: "Register",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_basketItems_BasketId",
                table: "basketItems",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_basketItems_RegisterId1",
                table: "basketItems",
                column: "RegisterId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "basketItems");

            migrationBuilder.DropTable(
                name: "basket");
        }
    }
}
