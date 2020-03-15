using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Entities;
using Services;

namespace Web.Hubs
{
    public interface IAdminOperationsClient
    {
        Task BlockMachineCoin(MachineCoin machineCoin);
        Task DeleteCoin(Coin coin);
        Task EditCoin(Coin coin);
        Task AddCoin(Coin coin);
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
            await Clients.Caller.EditCoin(coin);
        }
        public async Task OnAddCoin(Coin coin)
        {
            await Clients.Caller.AddCoin(coin);
        }

    }
}
