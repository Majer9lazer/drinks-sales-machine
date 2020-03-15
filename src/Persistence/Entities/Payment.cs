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

        public virtual IdentityUser IdentityUser { get; set; }
        public string AdditionUserInfoInJson { get; set; }
        public virtual MachineDrink MachineDrink { get; set; }
        public virtual Machine Machine { get; set; }

        /// <summary>
        /// Сдача. Статус платежа = 4 <see cref="PaymentStatus"/>
        /// </summary>
        public double ShortChange { get; set; }
        public virtual ICollection<MachineCoin> MachineCoins { get; set; }

        public DateTime PaymentDateUtc { get; set; }


        /// <summary>
        /// Состояние платежа;
        /// <example>0 - не оплачен, 1 - оплачен, 2 - недостаточно средств, 3 - отменен, 4 - оплачен успешно со сдачей</example>
        /// </summary>
        public byte PaymentStatus { get; set; }

    }
}
