using Microsoft.EntityFrameworkCore.Migrations;

namespace DelpinBooking.Migrations
{
    public partial class foreignkeysMachines : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentStore",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "RentType",
                table: "Booking");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Machine",
                newName: "Id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Machine",
                newName: "ID");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentStore",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RentType",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
