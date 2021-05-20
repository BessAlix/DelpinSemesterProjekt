using Microsoft.EntityFrameworkCore.Migrations;

namespace DelpinBooking.Migrations
{
    public partial class maskiner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Machine_MachineId",
                table: "Booking");

            migrationBuilder.DropIndex(
                name: "IX_Booking_MachineId",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "MachineId",
                table: "Booking");

            migrationBuilder.AddColumn<int>(
                name: "BookingId",
                table: "Machine",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Machine_BookingId",
                table: "Machine",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machine_Booking_BookingId",
                table: "Machine",
                column: "BookingId",
                principalTable: "Booking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machine_Booking_BookingId",
                table: "Machine");

            migrationBuilder.DropIndex(
                name: "IX_Machine_BookingId",
                table: "Machine");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Machine");

            migrationBuilder.AddColumn<int>(
                name: "MachineId",
                table: "Booking",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_MachineId",
                table: "Booking",
                column: "MachineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Machine_MachineId",
                table: "Booking",
                column: "MachineId",
                principalTable: "Machine",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
