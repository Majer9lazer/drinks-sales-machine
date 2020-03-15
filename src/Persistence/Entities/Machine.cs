using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Persistence.Entities
{
    public class Machine
    {
        public int Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        public virtual ICollection<MachineDrink> Drinks{ get; set; }
        public virtual ICollection<MachineCoin> Coins { get; set; }
    }
}
