using System.Collections.Generic;
using System.ComponentModel;
using Persistence.Entities;

namespace Services.Models
{
    public class CreateMachineViewModel
    {
        public string MachineName { get; set; }

        public IEnumerable<CreateMachineDrinkViewModel> DrinkIds { get; set; }

        public IEnumerable<int> CoinIds { get; set; }
    }
}
