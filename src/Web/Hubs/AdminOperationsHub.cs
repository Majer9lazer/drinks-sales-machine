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

        Task AddMachineDrink(MachineDrink machineDrink);
        Task RemoveMachineDrink(MachineDrink machineDrink);

        Task AddMachine(Machine machine);
        Task RemoveMachine(Machine machine);
        Task EditMachineName(Machine machine);
    }
    public class AdminOperationsHub : Hub<IAdminOperationsClient>{}
}
