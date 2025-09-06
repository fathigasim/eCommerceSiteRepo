using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace efcoreApi.Migrations
{
    public partial class AddingGoodsDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoodsDetail",
                table: "goods",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoodsDetail",
                table: "goods");
        }
    }
}
