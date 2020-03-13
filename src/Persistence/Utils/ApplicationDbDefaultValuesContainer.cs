using System.Collections.Generic;
using Persistence.Data;
using Persistence.Entities;

namespace Persistence.Utils
{
    public class ApplicationDbDefaultValuesContainer
    {
        public static IEnumerable<Coin> GetDefaultCoins()
        {
            return new[]
            {
                new Coin() { Id = 1, Name = "1.руб", State = 1, Value = 1 },
                new Coin() { Id = 2, Name = "2.руб", State = 1, Value = 2 },
                new Coin() { Id = 3, Name = "5.руб", State = 1, Value = 5 },
                new Coin() { Id = 4, Name = "10.руб", State = 1, Value = 10 }
            };
        }
        public static IEnumerable<Drink> GetDefaultDrinks()
        {
            return new[]
            {
                new Drink() { Id = 1, Name = "Coca Cola", Price = 35 },
                new Drink() { Id = 2, Name = "Fanta", Price = 40 },
                new Drink() { Id = 3, Name = "Pepsi", Price = 70 }
            };
        }
    }
}
