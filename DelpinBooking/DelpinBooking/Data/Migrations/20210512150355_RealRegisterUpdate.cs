using Microsoft.EntityFrameworkCore.Migrations;

namespace DelpinBooking.Data.Migrations
{
    public partial class RealRegisterUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyForm",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyForm",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Usertype",
                table: "AspNetUsers");
        }
    }
}
