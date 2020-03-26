﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Entities;
using Web.Hubs;

namespace Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineCoinsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<AdminOperationsHub, IAdminOperationsClient> _adminHub;
        public MachineCoinsController(ApplicationDbContext context, IHubContext<AdminOperationsHub, IAdminOperationsClient> adminHub)
        {
            _context = context;
            _adminHub = adminHub;
        }

        // GET: api/MachineCoins
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MachineCoin>>> GetMachineCoins(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            return await _context.MachineCoins.AsNoTracking().ToListAsync(ct);
        }

        // GET: api/MachineCoins/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MachineCoin>> GetMachineCoin(int id, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var machineCoin = await _context.MachineCoins.FirstOrDefaultAsync(f => f.Id == id, ct);

            if (machineCoin == null)
            {
                return NotFound();
            }

            return machineCoin;
        }

        // PUT: api/MachineCoins/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMachineCoin(int id, MachineCoin machineCoin, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            if (id != machineCoin.Id)
            {
                return BadRequest();
            }

            _context.Entry(machineCoin).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MachineCoinExists(id))
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

        // POST: api/MachineCoins
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<MachineCoin>> PostMachineCoin(MachineCoin machineCoin, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            await _context.MachineCoins.AddAsync(machineCoin, ct);
            await _context.SaveChangesAsync(ct);

            await _adminHub.Clients.All.AddMachineCoin(machineCoin);
            
            return CreatedAtAction("GetMachineCoin", new { id = machineCoin.Id }, machineCoin);
        }

        // DELETE: api/MachineCoins/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<MachineCoin>> DeleteMachineCoin(int id, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var machineCoin = await _context.MachineCoins.FirstOrDefaultAsync(f => f.Id == id, ct);
            if (machineCoin == null)
            {
                return NotFound();
            }

            _context.MachineCoins.Remove(machineCoin);
            await _context.SaveChangesAsync(ct);

            return machineCoin;
        }

        private bool MachineCoinExists(int id)
        {
            return _context.MachineCoins.Any(e => e.Id == id);
        }
    }
}
