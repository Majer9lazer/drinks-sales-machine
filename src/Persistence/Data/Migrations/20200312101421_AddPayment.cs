using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Data.Migrations
{
    public partial class AddPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PaymentId",
                table: "MachineCoins",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityUserId = table.Column<string>(nullable: true),
                    MachineId = table.Column<int>(nullable: true),
                    PaymentDateUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_Payments_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
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
                name: "IX_Payments_IdentityUserId",
                table: "Payments",
                column: "IdentityUserId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
