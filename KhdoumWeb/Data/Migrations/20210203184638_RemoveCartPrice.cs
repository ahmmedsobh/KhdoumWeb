using Microsoft.EntityFrameworkCore.Migrations;

namespace KhdoumWeb.Data.Migrations
{
    public partial class RemoveCartPrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Price",
            //    table: "Carts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<decimal>(
            //    name: "Price",
            //    table: "Carts",
            //    type: "decimal(18,2)",
            //    nullable: false,
            //    defaultValue: 0m);
        }
    }
}
