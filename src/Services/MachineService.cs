﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        Task<Machine> DeleteMachine(Machine machine, CancellationToken cancellationToken = default);
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

        public async Task<Machine> DeleteMachine(Machine machine, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _db.Machines.Remove(machine);
            await _db.SaveChangesAsync(cancellationToken);
            return machine;
        }

        public async Task<Machine> CreateMachine(CreateMachineViewModel model, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await using var tran = await _db.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                var machine = new Machine
                {
                    Name = model.MachineName
                };

                var machineDrinks = new List<MachineDrink>(model.DrinkIds.Sum(s => s.Quantity));

                foreach (var drinkViewModel in model.DrinkIds)
                {
                    for (int i = 0; i < drinkViewModel.Quantity; i++)
                    {
                        machineDrinks.Add(new MachineDrink()
                        {
                            DrinkId = drinkViewModel.DrinkId,
                            DrinkState = 0
                        });
                    }
                }

                machine.Drinks = machineDrinks;

                var machineCoins = new List<MachineCoin>(model.CoinIds.Sum(s => s.Quantity));

                foreach (var coinViewModel in model.CoinIds)
                {
                    for (int i = 0; i < coinViewModel.Quantity; i++)
                    {
                        machineCoins.Add(new MachineCoin()
                        {
                            CoinId = coinViewModel.CoinId,
                            CoinState = 0
                        });
                    }
                }

                machine.Coins = machineCoins;

                await _db.Machines.AddAsync(machine, cancellationToken);
                var savedCount = await _db.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("saved elements count in Machine entity = {count}", savedCount);

                await tran.CommitAsync(cancellationToken);

                return machine;
            }
            catch (TaskCanceledException e)
            {
                _logger.LogWarning(e, "Machine Add Operation was cancelled.");
                return default;
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
