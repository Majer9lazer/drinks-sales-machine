using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Persistence.Data;
using Web.Hubs;

namespace Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrinksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<DrinkOperationsHub, IDrinkOperationsClient> _drinkHub;
        public DrinksController(ApplicationDbContext context, IHubContext<DrinkOperationsHub, IDrinkOperationsClient> drinkHub)
        {
            _context = context;
            _drinkHub = drinkHub;
        }

        // GET: api/Drinks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drink>>> GetDrinks(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            return await _context
                .Drinks
                .AsNoTracking()
                .Include(i => i.Image)
                .ToListAsync(ct);
        }


        // DELETE: api/Drinks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Drink>> DeleteDrink(int id,CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var drink = await _context.Drinks.FindAsync(id);
            if (drink == null)
            {
                return NotFound();
            }

            _context.Drinks.Remove(drink);
            await _context.SaveChangesAsync(ct);

            await _drinkHub.Clients.All.RemoveDrink(drink);
            return drink;
        }

    }
}
