using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Persistence.Entities;

namespace Web.Hubs
{
    public interface ICoinOperationsClient
    {
        Task AddCoin(Coin coin);
        Task RemoveCoin(Coin coin);
        Task EditCoin(Coin coin);
    }

    public class CoinOperationsHub : Hub<ICoinOperationsClient> { }
}
