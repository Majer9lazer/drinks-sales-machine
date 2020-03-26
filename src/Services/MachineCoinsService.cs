using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services
{
    public interface IMachineCoinsService
    {
        Task LockCoinInMachine(MachineCoin coin, CancellationToken ct = default);
        Task UnLockCoinInMachine(MachineCoin coin, CancellationToken ct = default);
    }

    public class MachineCoinsService : IMachineCoinsService
    {
        private readonly ApplicationDbContext _db;

        public MachineCoinsService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task LockCoinInMachine(MachineCoin coin, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            await SetCoinState(coin, 1, ct);
        }

        public async Task UnLockCoinInMachine(MachineCoin coin, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            await (SetCoinState(coin, 0, ct));
        }

        private async Task SetCoinState(MachineCoin coin, byte state, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            var machineCoins = await _db.MachineCoins
                .Where(w => w.CoinId == coin.CoinId && w.MachineId == coin.MachineId)
                .ToListAsync(ct);

            foreach (var machineCoin in machineCoins)
            {
                machineCoin.CoinState = state;
            }

            _db.MachineCoins.UpdateRange(machineCoins);
            await _db.SaveChangesAsync(ct);
        }
    }
}