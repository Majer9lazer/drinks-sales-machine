using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Entities
{
    /// <summary>
    /// Хранит информацию об оплате того или иного напитка конкретным пользователем
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// тип данных long, потому что платежей может быть очень много
        /// </summary>
        public long PaymentId { get; set; }

        public IdentityUser IdentityUser { get; set; }

        public Machine Machine { get; set; }

        public ICollection<MachineCoin> MachineCoins { get; set; }

        public DateTime PaymentDateUtc { get; set; }

    }
}
