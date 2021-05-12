using Microsoft.EntityFrameworkCore.Migrations;

namespace DelpinBooking.Migrations
{
    public partial class removePrice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PricePrDay",
                table: "Booking");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PricePrDay",
                table: "Booking",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
