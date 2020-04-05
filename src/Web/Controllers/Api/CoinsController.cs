using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account.Manage;
using Microsoft.AspNetCore.SignalR;
using Persistence.Data;
using Web.Hubs;

namespace Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<AdminOperationsHub, IAdminOperationsClient> _adminOperationsHub;
        public CoinsController(ApplicationDbContext context, IHubContext<AdminOperationsHub, IAdminOperationsClient> adminOperationsHub)
        {
            _context = context;
            _adminOperationsHub = adminOperationsHub;
        }

        // GET: api/Coins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Coin>>> GetCoins(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            return await _context.Coins.AsNoTracking().Include(i => i.Image).ToListAsync(ct);
        }

        // GET: api/Coins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Coin>> GetCoin(int id)
        {
            var coin = await _context.Coins.FindAsync(id);

            if (coin == null)
            {
                return NotFound();
            }

            return coin;
        }

        // PUT: api/Coins/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCoin(int id, Coin coin, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            if (id != coin.Id)
            {
                return BadRequest();
            }

            _context.Entry(coin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CoinExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Coins
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Coin>> PostCoin(Coin coin, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            await _context.Coins.AddAsync(coin, ct);
            await _context.SaveChangesAsync(ct);

            return CreatedAtAction("GetCoin", new { id = coin.Id }, coin);
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

            await _adminOperationsHub.Clients.All.DeleteCoin(coin);
            
            return coin;
        }

        private bool CoinExists(int id)
        {
            return _context.Coins.Any(e => e.Id == id);
        }
    }
}
