using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class AddedIpAddressToIdentityUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_IdentityUserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_IdentityUserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "AdditionUserInfoInJson",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserIpAddress",
                table: "Payments",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserIpAddress",
                table: "MachineCoins",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IpAddress",
                table: "AspNetUsers",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ApplicationUserId",
                table: "Payments",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_ApplicationUserId",
                table: "Payments",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_ApplicationUserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_ApplicationUserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserIpAddress",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserIpAddress",
                table: "MachineCoins");

            migrationBuilder.DropColumn(
                name: "IpAddress",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "AdditionUserInfoInJson",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_IdentityUserId",
                table: "Payments",
                column: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_IdentityUserId",
                table: "Payments",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
