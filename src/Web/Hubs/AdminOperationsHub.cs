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
    }
    public class AdminOperationsHub : Hub<IAdminOperationsClient>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMachineService _machineService;
        public AdminOperationsHub(ApplicationDbContext db, IMachineService machineService)
        {
            _db = db;
            _machineService = machineService;
        }

        public async Task OnBlockCoinOperation(int coinId, int machineId)
        {
            var coinToBeBlocked = await _machineService.UpdateCoinState(machineId, coinId, 1);

            await Clients.All.BlockMachineCoin(coinToBeBlocked);
        }



    }
}
