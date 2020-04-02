using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class RemovePaymentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MachineCoins_Payments_PaymentId",
                table: "MachineCoins");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_MachineCoins_PaymentId",
                table: "MachineCoins");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "MachineCoins");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PaymentId",
                table: "MachineCoins",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MachineDrinkId = table.Column<long>(type: "bigint", nullable: true),
                    MachineId = table.Column<int>(type: "int", nullable: true),
                    PaymentDateUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    ShortChange = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_MachineDrinks_MachineDrinkId",
                        column: x => x.MachineDrinkId,
                        principalTable: "MachineDrinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MachineCoins_PaymentId",
                table: "MachineCoins",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ApplicationUserId",
                table: "Payments",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MachineDrinkId",
                table: "Payments",
                column: "MachineDrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MachineId",
                table: "Payments",
                column: "MachineId");

            migrationBuilder.AddForeignKey(
                name: "FK_MachineCoins_Payments_PaymentId",
                table: "MachineCoins",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "PaymentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
