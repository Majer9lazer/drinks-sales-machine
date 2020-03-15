using System.Collections.Generic;
using Persistence.Entities;

namespace Persistence.Utils
{
    public class ApplicationDbDefaultValuesContainer
    {
        public static IEnumerable<Coin> GetDefaultCoins()
        {
            return new[]
            {
                new Coin {Id = 1, Name = "1. руб", Value = 1, ImageId = 1},
                new Coin {Id = 2, Name = "2. руб", Value = 2, ImageId = 1},
                new Coin {Id = 3, Name = "5. руб", Value = 5, ImageId = 1},
                new Coin {Id = 4, Name = "10. руб", Value = 10, ImageId = 1}
            };
        }

        public static IEnumerable<Drink> GetDefaultDrinks()
        {
            return new[]
            {
                new Drink {Id = 1, Name = "Coca Cola", Price = 35, ImageId = 2},
                new Drink {Id = 2, Name = "Fanta", Price = 40, ImageId = 4},
                new Drink {Id = 3, Name = "Pepsi", Price = 70, ImageId = 3}
            };
        }

        public static Image[] GetDefaultImages()
        {
            return new[]
            {
                new Image { Id = 1, Path = "/img/ruble.png" },
                new Image {Id = 2, Path = "/img/cola.png"},
                new Image {Id = 3, Path = "/img/pepsi.png"},
                new Image {Id = 4, Path = "/img/fanta.png"},
            };
        }
    }
}