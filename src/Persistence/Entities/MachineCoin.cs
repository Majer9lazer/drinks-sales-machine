using System;
using System.Collections.Generic;
using System.Text;

namespace Persistence.Entities
{
    public class MachineCoin
    {
        public int Id { get; set; }
        public Coin Coin { get; set; }
        public Machine Machine { get; set; }

    }
}
