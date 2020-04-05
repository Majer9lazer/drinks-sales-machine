using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Persistence.Entities;

namespace Web.Hubs
{
    public interface IDrinkOperationsClient
    {
        Task AddDrink(Drink drink);
        Task RemoveDrink(Drink drink);
        Task EditDrink(Drink drink);
    }

    public class DrinkOperationsHub : Hub<IDrinkOperationsClient> { }
}
