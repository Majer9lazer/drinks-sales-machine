using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Data;
using Persistence.Entities;
using System;
using System.IO;
using System.Threading.Tasks;
using Web.Hubs;

namespace Web.Controllers
{
    public class ImagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ImageOperationsHub, IImageOperationsClient> _imageHub;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ImagesController> _logger;
        public ImagesController(ApplicationDbContext context, IHubContext<ImageOperationsHub, IImageOperationsClient> imageHub, IWebHostEnvironment env, ILogger<ImagesController> logger)
        {
            _context = context;
            _imageHub = imageHub;
            _env = env;
            _logger = logger;
        }


        // GET: Images/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([BindRequired]IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var fullPath = Path.Combine(_env.WebRootPath, "img", imageFile.FileName);
                var image = new Image { Path = $"/img/{imageFile.FileName}"};

                try
                {
                    await using var fileStream = new FileStream(fullPath, FileMode.Create);
                    await imageFile.CopyToAsync(fileStream);

                    _context.Add(image);
                    await _context.SaveChangesAsync();
                    await _imageHub.Clients.All.AddImage(image);
                }
                catch (UnauthorizedAccessException e)
                {
                    _logger.LogWarning(e, "Cannot access file path = {path}", fullPath);
                    return BadRequest(e);
                }

                return RedirectToAction(nameof(Index), "Admin");
            }
            return View();
        }

        // GET: Images/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.Images.FindAsync(id);
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            await _imageHub.Clients.All.RemoveImage(image);
            return RedirectToAction(nameof(Index), "Admin");
        }

    }
}
