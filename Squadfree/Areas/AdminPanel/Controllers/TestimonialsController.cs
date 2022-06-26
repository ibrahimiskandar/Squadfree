using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Squadfree.Areas.AdminPanel.Models;
using Squadfree.Data;
using Squadfree.Models;

namespace Squadfree.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class TestimonialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IWebHostEnvironment _env;
        public TestimonialsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _env = env;
            _context = context;
        }

        // GET: AdminPanel/Testimonials
        public async Task<IActionResult> Index()
        {
            return View(await _context.Testimonials.ToListAsync());
        }

        // GET: AdminPanel/Testimonials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // GET: AdminPanel/Testimonials/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminPanel/Testimonials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Create([Bind("Photo,Name,Role,Comment")] TestimonialsCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = UploadedFile(model);

                Testimonial testimonial = new Testimonial
                {
                    Avatar = uniqueFileName,
                    Name = model.Name,
                    Role = model.Role,
                    Comment = model.Comment
                };

                _context.Add(testimonial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        private string UploadedFile(TestimonialsCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(_env.WebRootPath, "img/testimonials");
                uniqueFileName = Guid.NewGuid().ToString() + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        // GET: AdminPanel/Testimonials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial == null)
            {
                return NotFound();
            }
            return View(testimonial);
        }

        // POST: AdminPanel/Testimonials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Avatar,Name,Role,Comment")] Testimonial testimonial)
        {
            if (id != testimonial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testimonial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestimonialExists(testimonial.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(testimonial);
        }

        // GET: AdminPanel/Testimonials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testimonial = await _context.Testimonials
                .FirstOrDefaultAsync(m => m.Id == id);
            if (testimonial == null)
            {
                return NotFound();
            }

            return View(testimonial);
        }

        // POST: AdminPanel/Testimonials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testimonial = await _context.Testimonials.FindAsync(id);
            _context.Testimonials.Remove(testimonial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestimonialExists(int id)
        {
            return _context.Testimonials.Any(e => e.Id == id);
        }
    }
}
