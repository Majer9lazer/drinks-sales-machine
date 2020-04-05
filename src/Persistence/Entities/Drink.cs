using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities
{
    public class Drink
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public virtual Image Image { get; set; }
        public int ImageId { get; set; }

    }
}
