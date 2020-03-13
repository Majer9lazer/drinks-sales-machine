using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddMachineCoinState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Coins");

            migrationBuilder.AddColumn<byte>(
                name: "CoinState",
                table: "MachineCoins",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoinState",
                table: "MachineCoins");

            migrationBuilder.AddColumn<byte>(
                name: "State",
                table: "Coins",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.UpdateData(
                table: "Coins",
                keyColumn: "Id",
                keyValue: 1,
                column: "State",
                value: (byte)1);

            migrationBuilder.UpdateData(
                table: "Coins",
                keyColumn: "Id",
                keyValue: 2,
                column: "State",
                value: (byte)1);

            migrationBuilder.UpdateData(
                table: "Coins",
                keyColumn: "Id",
                keyValue: 3,
                column: "State",
                value: (byte)1);

            migrationBuilder.UpdateData(
                table: "Coins",
                keyColumn: "Id",
                keyValue: 4,
                column: "State",
                value: (byte)1);
        }
    }
}
