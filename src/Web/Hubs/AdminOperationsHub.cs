using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Persistence.Entities;
using Services;
using System.Threading.Tasks;

namespace Web.Hubs
{
    public interface IAdminOperationsClient
    {
        Task DeleteMachineCoins(IEnumerable<MachineCoin> coinsToDelete);
        Task AddMachineCoin(MachineCoin machineCoin);
        Task LockMachineCoin(MachineCoin machineCoin);
        Task UnLockMachineCoin(MachineCoin machineCoin);

        Task DeleteCoin(Coin coin);
        Task EditCoin(Coin coin);
        Task AddCoin(Coin coin);
        
        Task DeleteMachine(Machine machine);
    }
    public class AdminOperationsHub : Hub<IAdminOperationsClient>
    {

        public async Task OnDeleteCoin(Coin coin)
        {
            await Clients.Others.DeleteCoin(coin);
        }

        public async Task OnEditCoin(Coin coin)
        {
            await Clients.Others.EditCoin(coin);
        }
        public async Task OnAddCoin(Coin coin)
        {
            await Clients.Others.AddCoin(coin);
        }

    }
}
