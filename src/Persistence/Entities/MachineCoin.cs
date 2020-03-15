using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Entities
{
    public class MachineCoin
    {
        public int Id { get; set; }

        public int CoinId { get; set; }
        public virtual Coin Coin { get; set; }

        public int MachineId { get; set; }
        public virtual Machine Machine { get; set; }

        /// <summary>
        /// Состояние <example>0(разблокирована),1(заблокирована)</example>
        /// </summary>
        public byte CoinState { get; set; }
    }
}
