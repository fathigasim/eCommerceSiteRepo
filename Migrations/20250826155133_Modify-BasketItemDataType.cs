using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace efcoreApi.Migrations
{
    public partial class ModifyBasketItemDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_basketItems_Register_RegisterId1",
                table: "basketItems");

            migrationBuilder.DropIndex(
                name: "IX_basketItems_RegisterId1",
                table: "basketItems");

            migrationBuilder.DropColumn(
                name: "RegisterId1",
                table: "basketItems");

            migrationBuilder.AlterColumn<string>(
                name: "RegisterId",
                table: "basketItems",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_basketItems_RegisterId",
                table: "basketItems",
                column: "RegisterId");

            migrationBuilder.AddForeignKey(
                name: "FK_basketItems_Register_RegisterId",
                table: "basketItems",
                column: "RegisterId",
                principalTable: "Register",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_basketItems_Register_RegisterId",
                table: "basketItems");

            migrationBuilder.DropIndex(
                name: "IX_basketItems_RegisterId",
                table: "basketItems");

            migrationBuilder.AlterColumn<int>(
                name: "RegisterId",
                table: "basketItems",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "RegisterId1",
                table: "basketItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_basketItems_RegisterId1",
                table: "basketItems",
                column: "RegisterId1");

            migrationBuilder.AddForeignKey(
                name: "FK_basketItems_Register_RegisterId1",
                table: "basketItems",
                column: "RegisterId1",
                principalTable: "Register",
                principalColumn: "Id");
        }
    }
}
