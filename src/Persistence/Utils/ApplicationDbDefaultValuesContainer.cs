using System.Collections.Generic;
using Persistence.Data;
using Persistence.Entities;

namespace Persistence.Utils
{
    public class ApplicationDbDefaultValuesContainer
    {
        private static Image RubleImage()
        {
            return new Image { Id = 1, Path = "/img/ruble.png" };
        }

        public static IEnumerable<Coin> GetDefaultCoins()
        {
            return new[]
            {
                new Coin() {Id = 1, Name = "1.руб",  Value = 1,  ImageId = 1},
                new Coin() {Id = 2, Name = "2.руб",  Value = 2,  ImageId = 1},
                new Coin() {Id = 3, Name = "5.руб",  Value = 5,  ImageId = 1},
                new Coin() {Id = 4, Name = "10.руб", Value = 10, ImageId = 1}
            };
        }

        public static IEnumerable<Drink> GetDefaultDrinks()
        {
            return new[]
            {
                new Drink() {Id = 1, Name = "Coca Cola", Price = 35},
                new Drink() {Id = 2, Name = "Fanta", Price = 40},
                new Drink() {Id = 3, Name = "Pepsi", Price = 70}
            };
        }

        public static Image[] GetDefaultImages()
        {
            return new[]
            {
                RubleImage(),
                new Image() {Id = 2, Path = "/img/cola.png"},
            };
        }
    }
}