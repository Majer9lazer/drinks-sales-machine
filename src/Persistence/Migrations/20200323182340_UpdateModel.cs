using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class UpdateModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUserIpAddress",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserIpAddress",
                table: "MachineCoins");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserIpAddress",
                table: "Payments",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserIpAddress",
                table: "MachineCoins",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
