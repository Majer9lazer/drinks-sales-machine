using Microsoft.AspNetCore.SignalR;
using Persistence.Entities;
using Services;
using System.Threading.Tasks;

namespace Web.Hubs
{
    public interface IAdminOperationsClient
    {
        Task AddMachineCoin(MachineCoin machineCoin);
        Task BlockMachineCoin(MachineCoin machineCoin);

        Task DeleteCoin(Coin coin);
        Task EditCoin(Coin coin);
        Task AddCoin(Coin coin);
        
        Task DeleteMachine(Machine machine);
    }
    public class AdminOperationsHub : Hub<IAdminOperationsClient>
    {
        private readonly IMachineService _machineService;
        public AdminOperationsHub(IMachineService machineService)
        {
            _machineService = machineService;
        }

        public async Task OnBlockCoinOperation(int coinId, int machineId)
        {
            var coinToBeBlocked = await _machineService.UpdateCoinState(machineId, coinId, 1);

            await Clients.All.BlockMachineCoin(coinToBeBlocked);
        }

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

        public async Task OnDeleteMachine(Machine machine)
        {
            var deletedMachine = await _machineService.DeleteMachine(machine, this.Context.ConnectionAborted);
            await Clients.Others.DeleteMachine(deletedMachine);
        }
    }
}
