﻿using Microsoft.AspNetCore.Mvc;
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
    public class MachinesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<AdminOperationsHub, IAdminOperationsClient> _adminHub;
        public MachinesController(ApplicationDbContext context, IHubContext<AdminOperationsHub, IAdminOperationsClient> adminHub)
        {
            _context = context;
            _adminHub = adminHub;
        }

        // GET: api/Machines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Machine>>> GetMachines()
        {
            return await
                _context.Machines.AsNoTracking()
                    .Where(w => w.Drinks.All(s => s.DrinkState == 0) && w.Coins.All(c => c.CoinState == 0))
                    .Include(d => d.Coins)
                    .Include(d => d.Drinks)
                    .ToListAsync();
        }

        // GET: api/Machines/Coins(5)
        [HttpGet("Coins({id})")]
        public async Task<ActionResult<IEnumerable<MachineCoin>>> GetMachineCoinsByMachineId(int id)
        {
            return await _context.MachineCoins.AsNoTracking().Where(w => w.Machine.Id == id).ToListAsync();
        }

        /// <summary>
        /// Получить самый подходящий автомат для дальнейшей работы с ним.
        /// </summary>
        /// <returns></returns>
        [HttpGet("MostAppropriate")]
        public async Task<ActionResult<Machine>> GetMostAppropriateMachine()
        {
            return await _context.Machines.AsNoTracking()
                    .Where(w => w.Drinks.All(d => d.DrinkState == 0))
                    .OrderByDescending(o => o.Drinks.Count)
                        .ThenByDescending(o => o.Coins.Count)
                    .Include(d => d.Drinks)
                        .ThenInclude(d => d.Drink)
                            .ThenInclude(i => i.Image)
                    .Include(c => c.Coins)
                        .ThenInclude(c => c.Coin)
                        .ThenInclude(i => i.Image)
                    .FirstOrDefaultAsync();
        }

        // GET: api/Machines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Machine>> GetMachine(int id)
        {
            var machine = await _context.Machines.FindAsync(id);

            if (machine == null)
            {
                return NotFound();
            }

            return machine;
        }

        // PUT: api/Machines/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMachine(int id, Machine machine)
        {
            if (id != machine.Id)
            {
                return BadRequest();
            }

            _context.Entry(machine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MachineExists(id))
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

        // POST: api/Machines
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Machine>> PostMachine(Machine machine)
        {
            _context.Machines.Add(machine);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMachine", new { id = machine.Id }, machine);
        }

        // DELETE: api/Machines/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Machine>> DeleteMachine(int id)
        {
            var machine = await _context.Machines.FindAsync(id);
            if (machine == null)
            {
                return NotFound();
            }

            _context.Machines.Remove(machine);
            await _context.SaveChangesAsync();

            return machine;
        }

        private bool MachineExists(int id)
        {
            return _context.Machines.Any(e => e.Id == id);
        }
    }
}
