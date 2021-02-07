using Microsoft.EntityFrameworkCore.Migrations;

namespace KhdoumWeb.Data.Migrations
{
    public partial class mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "QuantityDuration",
                table: "Items",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryService",
                table: "Cities",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityDuration",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DeliveryService",
                table: "Cities");
        }
    }
}
