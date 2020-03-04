using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities
{
    public class Coin
    {
        public int Id { get; set; }

        [StringLength(150)]
        public string Name { get; set; }
        public int Value { get; set; }

        /// <summary>
        /// Состояние 
        /// </summary>
        /// <example>0(заблокирована),1(разблокирована)</example>
        public byte State { get; set; }

        public virtual Image Image { get; set; }
    }
}
