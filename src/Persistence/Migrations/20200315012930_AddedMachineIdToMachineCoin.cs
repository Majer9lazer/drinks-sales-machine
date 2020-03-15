using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddedMachineIdToMachineCoin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MachineCoins_Coins_CoinId",
                table: "MachineCoins");

            migrationBuilder.DropForeignKey(
                name: "FK_MachineCoins_Machines_MachineId",
                table: "MachineCoins");

            migrationBuilder.AlterColumn<int>(
                name: "MachineId",
                table: "MachineCoins",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CoinId",
                table: "MachineCoins",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MachineCoins_Coins_CoinId",
                table: "MachineCoins",
                column: "CoinId",
                principalTable: "Coins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MachineCoins_Machines_MachineId",
                table: "MachineCoins",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MachineCoins_Coins_CoinId",
                table: "MachineCoins");

            migrationBuilder.DropForeignKey(
                name: "FK_MachineCoins_Machines_MachineId",
                table: "MachineCoins");

            migrationBuilder.AlterColumn<int>(
                name: "MachineId",
                table: "MachineCoins",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CoinId",
                table: "MachineCoins",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_MachineCoins_Coins_CoinId",
                table: "MachineCoins",
                column: "CoinId",
                principalTable: "Coins",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MachineCoins_Machines_MachineId",
                table: "MachineCoins",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
