using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Entities;

namespace Services
{
    public interface IMachineService
    {
        Task<MachineCoin> UpdateCoinState(int machineId, int coinId, byte coinState);
    }
    public class MachineService : IMachineService
    {
        private readonly ApplicationDbContext _db;

        public MachineService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<MachineCoin> UpdateCoinState(int machineId, int coinId, byte coinState)
        {
            var coinToBeUpdated = await _db.MachineCoins.SingleAsync(f => f.Coin.Id == coinId && f.Machine.Id == machineId);
            if (coinToBeUpdated != null)
            {
                coinToBeUpdated.CoinState = coinState;
                _db.MachineCoins.Update(coinToBeUpdated);
                _db.SaveChanges();
            }

            return coinToBeUpdated;
        }
    }
}
