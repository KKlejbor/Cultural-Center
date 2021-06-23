using System;
using System.Collections;
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
    public class StudentGroupsController : Controller
    {
        private readonly CulturalCenterContext _context;

        public StudentGroupsController(CulturalCenterContext context)
        {
            _context = context;
        }

        // GET: StudentGroups
        public async Task<IActionResult> Index()
        {
            var culturalCenterContext = _context.StudentGroups.Include(s => s.Lesson);
            return View(await culturalCenterContext.ToListAsync());
        }

        // GET: StudentGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentGroups = await _context.StudentGroups
                .Include(s => s.Lesson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentGroups == null)
            {
                return NotFound();
            }

            return View(studentGroups);
        }

        // GET: StudentGroups/Create
        public IActionResult Create()
        {
            ViewData["LessonsId"] = new SelectList(_context.Lessons, "Id", "Id");
            List<String> weekDays = new List<String>{"Monday","Tuesday","Wednesday","Thursday","Friday","Saturday","Sunday"};
            ViewData["DayOfTheWeek"] = new SelectList(weekDays);
            return View();
        }

        // POST: StudentGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DayOfTheWeek,StartTime,EndTime,LessonsId")] StudentGroups studentGroups)
        {
            if (ModelState.IsValid)
            {
                _context.Add(studentGroups);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LessonsId"] = new SelectList(_context.Lessons, "Id", "Id", studentGroups.LessonsId);
            return View(studentGroups);
        }

        // GET: StudentGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentGroups = await _context.StudentGroups.FindAsync(id);
            if (studentGroups == null)
            {
                return NotFound();
            }
            ViewData["LessonsId"] = new SelectList(_context.Lessons, "Id", "Id", studentGroups.LessonsId);
            List<String> weekDays = new List<String>{"Monday","Tuesday","Wednesday","Thursday","Friday","Saturday","Sunday"};
            ViewData["DayOfTheWeek"] = new SelectList(weekDays);
            return View(studentGroups);
        }

        // POST: StudentGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DayOfTheWeek,StartTime,EndTime,LessonsId")] StudentGroups studentGroups)
        {
            if (id != studentGroups.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentGroups);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    await e.Entries.Single().ReloadAsync();
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LessonsId"] = new SelectList(_context.Lessons, "Id", "Id", studentGroups.LessonsId);
            return View(studentGroups);
        }

        // GET: StudentGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentGroups = await _context.StudentGroups
                .Include(s => s.Lesson)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentGroups == null)
            {
                return NotFound();
            }

            return View(studentGroups);
        }

        // POST: StudentGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentGroups = await _context.StudentGroups.FindAsync(id);
            _context.StudentGroups.Remove(studentGroups);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentGroupsExists(int id)
        {
            return _context.StudentGroups.Any(e => e.Id == id);
        }
    }
}
