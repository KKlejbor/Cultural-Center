using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cultural_Center;
using Cultural_Center.Models;

namespace Cultural_Center.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly CulturalCenterContext _context;

        public EnrollmentsController(CulturalCenterContext context)
        {
            _context = context;
        }

        // GET: Enrollments
        public async Task<IActionResult> Index()
        {
            var culturalCenterContext = _context.Enrollments.Include(e => e.Student).Include(e => e.StudentGroup);
            return View(await culturalCenterContext.ToListAsync());
        }

        // GET: Enrollments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.StudentGroup)
                .FirstOrDefaultAsync(m => m.StudentGroupsId == id);
            if (enrollments == null)
            {
                return NotFound();
            }

            return View(enrollments);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            ViewData["StudentsId"] = new SelectList(_context.Students, "Id", "Address");
            ViewData["StudentGroupsId"] = new SelectList(_context.StudentGroups, "Id", "EndTime");
            return View();
        }

        // POST: Enrollments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentGroupsId,StudentsId")] Enrollments enrollments)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enrollments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentsId"] = new SelectList(_context.Students, "Id", "Address", enrollments.StudentsId);
            ViewData["StudentGroupsId"] = new SelectList(_context.StudentGroups, "Id", "EndTime", enrollments.StudentGroupsId);
            return View(enrollments);
        }

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollments = await _context.Enrollments.FindAsync(id);
            if (enrollments == null)
            {
                return NotFound();
            }
            ViewData["StudentsId"] = new SelectList(_context.Students, "Id", "Address", enrollments.StudentsId);
            ViewData["StudentGroupsId"] = new SelectList(_context.StudentGroups, "Id", "EndTime", enrollments.StudentGroupsId);
            return View(enrollments);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentGroupsId,StudentsId")] Enrollments enrollments)
        {
            if (id != enrollments.StudentGroupsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enrollments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnrollmentsExists(enrollments.StudentGroupsId))
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
            ViewData["StudentsId"] = new SelectList(_context.Students, "Id", "Address", enrollments.StudentsId);
            ViewData["StudentGroupsId"] = new SelectList(_context.StudentGroups, "Id", "EndTime", enrollments.StudentGroupsId);
            return View(enrollments);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.StudentGroup)
                .FirstOrDefaultAsync(m => m.StudentGroupsId == id);
            if (enrollments == null)
            {
                return NotFound();
            }

            return View(enrollments);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enrollments = await _context.Enrollments.FindAsync(id);
            _context.Enrollments.Remove(enrollments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentsExists(int id)
        {
            return _context.Enrollments.Any(e => e.StudentGroupsId == id);
        }
    }
}
