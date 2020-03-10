using System;
using System.Collections.Generic;
using System.Text;
using Persistence.Data;

namespace Services
{
    public class MachineService
    {
        private readonly ApplicationDbContext _db;

        public MachineService(ApplicationDbContext db)
        {
            _db = db;
        }

        
    }
}
