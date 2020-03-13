using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities
{
    public class Coin
    {
        public int Id { get; set; }

        [StringLength(150)]
        public string Name { get; set; }
        public int Value { get; set; }

        public int ImageId { get; set; }

        public virtual Image Image { get; set; }
    }
}
