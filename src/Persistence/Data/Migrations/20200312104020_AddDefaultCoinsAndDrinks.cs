using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Data.Migrations
{
    public partial class AddDefaultCoinsAndDrinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Coins",
                columns: new[] { "Id", "ImageId", "MachineId", "Name", "State", "Value" },
                values: new object[,]
                {
                    { 1, null, null, "1.руб", (byte)1, 1 },
                    { 2, null, null, "2.руб", (byte)1, 2 },
                    { 3, null, null, "5.руб", (byte)1, 5 },
                    { 4, null, null, "10.руб", (byte)1, 10 }
                });

            migrationBuilder.InsertData(
                table: "Drinks",
                columns: new[] { "Id", "ImageId", "MachineId", "Name", "Price" },
                values: new object[,]
                {
                    { 1, null, null, "Coca Cola", 35.0 },
                    { 2, null, null, "Fanta", 40.0 },
                    { 3, null, null, "Pepsi", 70.0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coins",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Coins",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Coins",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Coins",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
