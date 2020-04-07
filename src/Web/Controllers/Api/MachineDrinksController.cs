using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Web.Hubs;

namespace Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineDrinksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<AdminOperationsHub, IAdminOperationsClient> _adminHub;
        public MachineDrinksController(ApplicationDbContext context, IHubContext<AdminOperationsHub, IAdminOperationsClient> adminHub)
        {
            _context = context;
            _adminHub = adminHub;
        }


        // POST: api/MachineDrinks
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("Create/{machineId}/{drinkId}")]
        public async Task<ActionResult<MachineDrink>> CreateMachineDrink(int machineId, int drinkId, CancellationToken ct)
        {
            var machineDrink = new MachineDrink()
            {
                DrinkId = drinkId,
                MachineId = machineId
            };

            _context.MachineDrinks.Add(machineDrink);
            await _context.SaveChangesAsync(ct);

            machineDrink = await
                _context.MachineDrinks.AsNoTracking()
                    .Include(d => d.Drink)
                    .FirstOrDefaultAsync(f => f.Id == machineDrink.Id, ct);

            await _adminHub.Clients.All.AddMachineDrink(machineDrink);

            return machineDrink;
        }

        // DELETE: api/MachineDrinks/5/1
        [HttpDelete("{drinkId}/{machineId}")]
        public async Task<ActionResult<MachineDrink>> DeleteMachineDrink(int drinkId, int machineId, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var machineDrink = await _context.MachineDrinks
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.MachineId == machineId && f.DrinkId == drinkId, ct);
            if (machineDrink == null)
            {
                return NotFound();
            }

            _context.MachineDrinks.Remove(machineDrink);
            await _context.SaveChangesAsync(ct);

            await _adminHub.Clients.All.RemoveMachineDrink(machineDrink);

            return machineDrink;
        }

        // DELETE: api/MachineDrinks/5
        [HttpDelete("{drinkId}")]
        public async Task<ActionResult<MachineDrink>> DeleteMachineDrink(long drinkId, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var machineDrink = await _context.MachineDrinks.FindAsync(drinkId);
            if (machineDrink == null)
            {
                return NotFound();
            }

            _context.MachineDrinks.Remove(machineDrink);
            await _context.SaveChangesAsync(ct);

            await _adminHub.Clients.All.RemoveMachineDrink(machineDrink);

            return machineDrink;
        }

    }
}
