using Persistence.Entities;
using Web.Data;

namespace Web.Utils
{
    public class ApplicationDbSeeder
    {
        public static void SeedDatabase(ApplicationDbContext dbContext)
        {
            AddCoins(dbContext);
            AddDrinks(dbContext);
            AddUsers(dbContext);
        }

        private static void AddCoins(ApplicationDbContext db)
        {
            db.Coins.AddRange(new Coin() { Name = "1.руб", State = 1, Value = 1 });
            db.Coins.AddRange(new Coin() { Name = "2.руб", State = 1, Value = 2 });
            db.Coins.AddRange(new Coin() { Name = "5.руб", State = 1, Value = 5 });
            db.Coins.AddRange(new Coin() { Name = "10.руб", State = 1, Value = 10 });
            db.SaveChanges();
        }
        private static void AddUsers(ApplicationDbContext db) { }
        private static void AddDrinks(ApplicationDbContext db)
        {
            db.Drinks.AddRange(new Drink() { Name = "Coca Cola", Price = 35 });
            db.Drinks.AddRange(new Drink() { Name = "Fanta", Price = 40 });
            db.Drinks.AddRange(new Drink() { Name = "Pepsi", Price = 70 });
            db.SaveChanges();
        }
    }
}
