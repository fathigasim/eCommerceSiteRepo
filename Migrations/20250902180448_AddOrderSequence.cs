using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace efcoreApi.Migrations
{
    public partial class AddOrderSequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "SequenceOrder");

            migrationBuilder.AlterColumn<int>(
                name: "OrdSeq",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR SequenceOrder",
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropSequence(
            //    name: "SequenceOrder");

            //migrationBuilder.AlterColumn<int>(
            //    name: "OrdSeq",
            //    table: "Orders",
            //    type: "int",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "int",
            //    oldDefaultValueSql: "NEXT VALUE FOR SequenceOrder");
        }
    }
}
