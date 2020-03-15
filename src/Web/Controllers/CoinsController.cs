using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Data;
using Persistence.Entities;
using Web.Hubs;

namespace Web.Controllers
{
    public class CoinsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<AdminOperationsHub, IAdminOperationsClient> _adminHubContext;
        private readonly ILogger<CoinsController> _logger;
        public CoinsController(ApplicationDbContext context, IHubContext<AdminOperationsHub, IAdminOperationsClient> adminHubContext, ILogger<CoinsController> logger)
        {
            _context = context;
            _adminHubContext = adminHubContext;
            _logger = logger;
        }

        // GET: Coins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coin = await _context.Coins
                .Include(c => c.Image)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coin == null)
            {
                return NotFound();
            }

            return View(coin);
        }

        // GET: Coins/Create
        public IActionResult Create()
        {
            ViewData["ImageId"] = new SelectList(_context.Images.AsNoTracking(), "Id", "Path");
            return View();
        }

        // POST: Coins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Value,ImageId,Image")] Coin coin)
        {
            if (ModelState.IsValid)
            {
                coin.ImageId = coin.Image.Id;
                _context.Add(coin);
                await _context.SaveChangesAsync();
                await _adminHubContext.Clients.All.AddCoin(coin);

                return RedirectToAction(nameof(Index), "Admin");
            }
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Path", coin.ImageId);
            return View(coin);
        }

        // GET: Coins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coin = await _context.Coins.Include(i => i.Image).FirstOrDefaultAsync(f => f.Id == id);
            if (coin == null)
            {
                return NotFound();
            }
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Path", coin.ImageId);
            return View(coin);
        }

        // POST: Coins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Value,Image,ImageId")] Coin coin)
        {
            if (id != coin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    coin.ImageId = coin.Image.Id;
                    _context.Update(coin);
                    var savedCount = await _context.SaveChangesAsync();

                    _logger.LogInformation("Saved Count While Updating Coin = {savedCount}", savedCount);

                    await _adminHubContext.Clients.All.EditCoin(coin);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoinExists(coin.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }


                return RedirectToAction(nameof(Index), "Admin");
            }
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id", coin.ImageId);
            return View(coin);
        }

        // GET: Coins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coin = await _context.Coins
                .Include(c => c.Image)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (coin == null)
            {
                return NotFound();
            }

            return View(coin);
        }

        // POST: Coins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coin = await _context.Coins.FindAsync(id);
            _context.Coins.Remove(coin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Admin");
        }

        private bool CoinExists(int id)
        {
            return _context.Coins.Any(e => e.Id == id);
        }
    }
}
