using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cultural_Center.Models;
using Cultural_Center.ViewObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Cultural_Center.Controllers
{
    public class LessonsController : Controller
    {
        private readonly CulturalCenterContext _context;

        public LessonsController(CulturalCenterContext context)
        {
            _context = context;
        }

        // GET: Lessons
        public async Task<IActionResult> Index()
        {
            var culturalCenterContext = _context.Lessons.Include(l => l.Subject);
            return View(await culturalCenterContext.ToListAsync());
        }

        // GET: Lessons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var lessons = await _context.Lessons
                .Include(l => l.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lessons == null) return NotFound();

            return View(lessons);
        }

        // GET: Lessons/Create
        public IActionResult Create()
        {
            ViewData["SubjectsId"] = new SelectList(_context.Subjects, "Id", "Id");
            return View();
        }

        // POST: Lessons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,StartDate,EndDate,NumberOfParticipants,ClassroomNumber,SubjectsId")]
            Lessons lessons)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lessons);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SubjectsId"] = new SelectList(_context.Subjects, "Id", "Id", lessons.SubjectsId);
            return View(lessons);
        }

        // GET: Lessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var lessons = await _context.Lessons.FindAsync(id);
            if (lessons == null) return NotFound();
            ViewData["SubjectsId"] = new SelectList(_context.Subjects, "Id", "Id", lessons.SubjectsId);
            return View(lessons);
        }

        // POST: Lessons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,StartDate,EndDate,NumberOfParticipants,ClassroomNumber,SubjectsId")]
            Lessons lessons)
        {
            if (id != lessons.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lessons);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonsExists(lessons.Id))
                        return NotFound();
                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["SubjectsId"] = new SelectList(_context.Subjects, "Id", "Id", lessons.SubjectsId);
            return View(lessons);
        }

        // GET: Lessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var lessons = await _context.Lessons
                .Include(l => l.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lessons == null) return NotFound();

            return View(lessons);
        }

        // POST: Lessons/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lessons = await _context.Lessons.FindAsync(id);
            _context.Lessons.Remove(lessons);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LessonsExists(int id)
        {
            return _context.Lessons.Any(e => e.Id == id);
        }

        public async Task<IActionResult> LessonsOffers()
        {
            // Lessons -> Subjects -> Istructors
            var query = _context.Lessons.Join(_context.Subjects, l => l.SubjectsId,
                s => s.Id,
                (l, s) => new
                {
                    SubjectName = s.Name,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    NumberOfParticipants = l.NumberOfParticipants,
                    ClassroomNumber = l.ClassroomNumber,
                    InstructorsId = s.InstructorsId
                }).Join(_context.Instructors, s => s.InstructorsId, i => i.Id,
                (s, i) => new
                {
                    SubjectName = s.SubjectName,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    NumberOfParticipants = s.NumberOfParticipants,
                    ClassroomNumber = s.ClassroomNumber,
                    InstructorsId = s.InstructorsId,
                    InstructorsFirstName = i.FirstName,
                    InstructorsLastName = i.LastName
                }).ToList();

            List<LessonsOffers> offers = new List<LessonsOffers>();

            foreach (var q in query)
            {
                offers.Add(new LessonsOffers
                {
                    SubjectName = q.SubjectName,
                    InstructorsFirstName = q.InstructorsFirstName,
                    InstructorsLastName = q.InstructorsLastName,
                    StartDate = q.StartDate,
                    EndDate = q.EndDate,
                    NumberOfParticipants = q.NumberOfParticipants,
                    ClassroomNumber = q.ClassroomNumber
                });
            }
            
            return View(offers);
        }
    }
}