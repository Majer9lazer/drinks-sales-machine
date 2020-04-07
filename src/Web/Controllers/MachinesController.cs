using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Entities;
using Services;
using Services.Models;
using Web.Hubs;

namespace Web.Controllers
{
    public class MachinesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMachineService _machineService;
        private readonly IHubContext<AdminOperationsHub, IAdminOperationsClient> _adminHub;
        public MachinesController(ApplicationDbContext context, IMachineService machineService, IHubContext<AdminOperationsHub, IAdminOperationsClient> adminHub)
        {
            _context = context;
            _machineService = machineService;
            _adminHub = adminHub;
        }

        // GET: Machines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var machine = await _context.Machines
                .FirstOrDefaultAsync(m => m.Id == id);
            if (machine == null)
            {
                return NotFound();
            }

            return View(machine);
        }

        // GET: Machines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Machines/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMachineViewModel model, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (ModelState.IsValid)
            {
                var machine = await _machineService.CreateMachine(model, ct);

                await _context.Entry(machine)
                    .Collection(d => d.Drinks)
                    .Query()
                    .Include(d => d.Drink)
                    .ThenInclude(i => i.Image)
                    .LoadAsync(ct);

                await _context.Entry(machine)
                    .Collection(c => c.Coins)
                    .Query()
                    .Include(c => c.Coin)
                    .ThenInclude(i => i.Image)
                    .LoadAsync(ct);

                await _adminHub.Clients.All.AddMachine(machine);

                return RedirectToAction(nameof(Index), "Admin");
            }
            return View(model);
        }

        // GET: Machines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var machine = await _context.Machines.FindAsync(id);
            if (machine == null)
            {
                return NotFound();
            }
            return View(machine);
        }

        // POST: Machines/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Machine machine)
        {
            if (id != machine.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(machine);
                    await _context.SaveChangesAsync();
                    await _adminHub.Clients.All.EditMachineName(machine);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MachineExists(machine.Id))
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
            return View(machine);
        }

        // GET: Machines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var machine = await _context.Machines
                .FirstOrDefaultAsync(m => m.Id == id);
            if (machine == null)
            {
                return NotFound();
            }

            return View(machine);
        }

        // POST: Machines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var machine = await _context.Machines.FindAsync(id);
            _context.Machines.Remove(machine);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Admin");
        }

        private bool MachineExists(int id)
        {
            return _context.Machines.Any(e => e.Id == id);
        }
    }
}
