using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Data;
using Persistence.Entities;
using Services.Models;

namespace Services
{
    public interface IMachineService
    {
        Task<MachineCoin> UpdateCoinState(int machineId, int coinId, byte coinState);
        Task<Machine> CreateMachine(CreateMachineViewModel model, CancellationToken cancellationToken = default);
    }
    public class MachineService : IMachineService
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<MachineService> _logger;
        public MachineService(ApplicationDbContext db, ILogger<MachineService> logger)
        {
            _db = db;
            _logger = logger;
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

        public async Task<Machine> CreateMachine(CreateMachineViewModel model, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await using var tran = await _db.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var machine = new Machine()
                {
                    Name = model.MachineName,
                };

                var machineDrinks = model.DrinkIds.Select(s => new MachineDrink()
                {
                    DrinkId = s.DrinkId,
                    DrinkState = 0,
                }).ToList();

                machine.Drinks = machineDrinks;
                
                var machineCoins = model.CoinIds.Select(s => new MachineCoin()
                {
                    CoinId = s
                }).ToList();
                machine.Coins = machineCoins;

                await _db.Machines.AddAsync(machine,cancellationToken);
                await _db.SaveChangesAsync(cancellationToken);

                await tran.CommitAsync(cancellationToken);

                return machine;
            }

            catch (Exception e)
            {
                _logger.LogError(e, "Error while creating machine");
                await tran.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
