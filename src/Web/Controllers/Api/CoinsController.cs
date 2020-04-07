using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Web.Hubs;

namespace Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<CoinOperationsHub, ICoinOperationsClient> _coinHub;
        public CoinsController(ApplicationDbContext context, IHubContext<CoinOperationsHub, ICoinOperationsClient> coinHub)
        {
            _context = context;
            _coinHub = coinHub;
        }

        // GET: api/Coins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coin>>> GetCoins(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            return await _context.Coins.AsNoTracking().Include(i => i.Image).ToListAsync(ct);
        }

        // DELETE: api/Coins/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Coin>> DeleteCoin(int id, CancellationToken ct)
        {
            var coin = await _context.Coins.FindAsync(id);
            if (coin == null)
            {
                return NotFound();
            }
            _context.Coins.Remove(coin);
            await _context.SaveChangesAsync(ct);

            await _coinHub.Clients.All.RemoveCoin(coin);
            
            return coin;
        }
    }
}
