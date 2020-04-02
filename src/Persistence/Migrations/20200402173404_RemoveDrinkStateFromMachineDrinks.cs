using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class RemoveDrinkStateFromMachineDrinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrinkState",
                table: "MachineDrinks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "DrinkState",
                table: "MachineDrinks",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }
    }
}
