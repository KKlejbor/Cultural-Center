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
    public class StudentGroupsStudentsController : Controller
    {
        private readonly CulturalCenterContext _context;

        public StudentGroupsStudentsController(CulturalCenterContext context)
        {
            _context = context;
        }

        // GET: StudentGroupsStudents
        public async Task<IActionResult> Index()
        {
            var culturalCenterContext = _context.StudentGroupsStudents.Include(s => s.Student).Include(s => s.StudentGroup);
            return View(await culturalCenterContext.ToListAsync());
        }

        // GET: StudentGroupsStudents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentGroupsStudents = await _context.StudentGroupsStudents
                .Include(s => s.Student)
                .Include(s => s.StudentGroup)
                .FirstOrDefaultAsync(m => m.StudentGroupsId == id);
            if (studentGroupsStudents == null)
            {
                return NotFound();
            }

            return View(studentGroupsStudents);
        }

        // GET: StudentGroupsStudents/Create
        public IActionResult Create()
        {
            ViewData["StudentsId"] = new SelectList(_context.Students, "Id", "Address");
            ViewData["StudentGroupsId"] = new SelectList(_context.StudentGroups, "Id", "EndTime");
            return View();
        }

        // POST: StudentGroupsStudents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentGroupsId,StudentsId")] StudentGroupsStudents studentGroupsStudents)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentGroupsStudents);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentsId"] = new SelectList(_context.Students, "Id", "Address", studentGroupsStudents.StudentsId);
            ViewData["StudentGroupsId"] = new SelectList(_context.StudentGroups, "Id", "EndTime", studentGroupsStudents.StudentGroupsId);
            return View(studentGroupsStudents);
        }

        // GET: StudentGroupsStudents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentGroupsStudents = await _context.StudentGroupsStudents.FindAsync(id);
            if (studentGroupsStudents == null)
            {
                return NotFound();
            }
            ViewData["StudentsId"] = new SelectList(_context.Students, "Id", "Address", studentGroupsStudents.StudentsId);
            ViewData["StudentGroupsId"] = new SelectList(_context.StudentGroups, "Id", "EndTime", studentGroupsStudents.StudentGroupsId);
            return View(studentGroupsStudents);
        }

        // POST: StudentGroupsStudents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentGroupsId,StudentsId")] StudentGroupsStudents studentGroupsStudents)
        {
            if (id != studentGroupsStudents.StudentGroupsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentGroupsStudents);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentGroupsStudentsExists(studentGroupsStudents.StudentGroupsId))
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
            ViewData["StudentsId"] = new SelectList(_context.Students, "Id", "Address", studentGroupsStudents.StudentsId);
            ViewData["StudentGroupsId"] = new SelectList(_context.StudentGroups, "Id", "EndTime", studentGroupsStudents.StudentGroupsId);
            return View(studentGroupsStudents);
        }

        // GET: StudentGroupsStudents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentGroupsStudents = await _context.StudentGroupsStudents
                .Include(s => s.Student)
                .Include(s => s.StudentGroup)
                .FirstOrDefaultAsync(m => m.StudentGroupsId == id);
            if (studentGroupsStudents == null)
            {
                return NotFound();
            }

            return View(studentGroupsStudents);
        }

        // POST: StudentGroupsStudents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentGroupsStudents = await _context.StudentGroupsStudents.FindAsync(id);
            _context.StudentGroupsStudents.Remove(studentGroupsStudents);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentGroupsStudentsExists(int id)
        {
            return _context.StudentGroupsStudents.Any(e => e.StudentGroupsId == id);
        }
    }
}
