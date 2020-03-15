using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Persistence.Entities;

namespace Services.Models
{
    public class CreateMachineViewModel
    {
        public string MachineName { get; set; }

        [MinLength(1)]
        public IEnumerable<CreateMachineDrinkViewModel> DrinkIds { get; set; }

        [MinLength(1)]
        public IEnumerable<CreateMachineCoinViewModel> CoinIds { get; set; }
    }
}
