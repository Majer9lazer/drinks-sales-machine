﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Entities;
using Web.Hubs;

namespace Web.Controllers
{
    public class DrinksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<DrinkOperationsHub, IDrinkOperationsClient> _drinkHub;

        public DrinksController(ApplicationDbContext context, IHubContext<DrinkOperationsHub, IDrinkOperationsClient> drinkHub)
        {
            _context = context;
            _drinkHub = drinkHub;
        }

        // GET: Drinks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Price,Name,ImageId")] Drink drink)
        {
            if (ModelState.IsValid)
            {
                _context.Add(drink);
                await _context.SaveChangesAsync();

                await _context.Entry(drink)
                    .Reference(i => i.Image)
                    .LoadAsync();

                await _drinkHub.Clients.All.AddDrink(drink);

                return RedirectToAction(nameof(Index),"Admin");
            }
            return View(drink);
        }

        // GET: Drinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drink = await _context.Drinks.FindAsync(id);
            if (drink == null)
            {
                return NotFound();
            }
            return View(drink);
        }

        // POST: Drinks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,Name,ImageId")] Drink drink)
        {
            if (id != drink.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(drink);
                    await _context.SaveChangesAsync();

                    await _context.Entry(drink)
                        .Reference(i => i.Image)
                        .LoadAsync();

                    await _drinkHub.Clients.All.EditDrink(drink);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrinkExists(drink.Id))
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
            return View(drink);
        }

        private bool DrinkExists(int id)
        {
            return _context.Drinks.Any(e => e.Id == id);
        }
    }
}
