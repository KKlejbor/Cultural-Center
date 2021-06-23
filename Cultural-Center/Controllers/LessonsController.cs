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
                try
                {
                    _context.Add(lessons);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    if (e.InnerException != null)
                    {
                        if (e.InnerException.Message.Contains("at similar time"))
                        {
                            return RedirectToAction("SimilarTimesConflict",new {message = e.InnerException.Message, SubjectsId = lessons.SubjectsId});
                            
                        }
                        
                        return RedirectToAction("SameDatesConflict",
                            new {message = e.InnerException.Message});

                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["SubjectsId"] = new SelectList(_context.Subjects, "Id", "Id", lessons.SubjectsId);
            return View(lessons);
        }

        public ActionResult SimilarTimesConflict(string message, int SubjectsId)
        {
            ViewBag.Error = message;
            var query = _context.Students.Join(_context.Enrollments, s => s.Id,
                e => e.StudentsId,
                (s, e) => new
                {
                    IdStudents = s.Id,
                    StudentsFirstName = s.FirstName,
                    StudentsLastName = s.LastName,
                    s.BirthDate,
                    StudentsPhoneNumber = s.PhoneNumber,
                    StudentsAddress = s.Address,
                    StudentsCity = s.City,
                    StudentsPostcode = s.Postcode,
                    StudentsEmailAddress = s.EmailAddress,
                    e.StudentsId,
                    e.StudentGroupsId
                }).Join(_context.StudentGroups, se => se.StudentGroupsId, sg => sg.Id,
                (se, sg) => new
                {
                    se,
                    IdStudentGroups = sg.Id,
                    sg.DayOfTheWeek,
                    sg.StartTime,
                    sg.EndTime,
                    sg.LessonsId
                }).Join(_context.Lessons, seSg => seSg.LessonsId, l => l.Id,
                (seSg, l) => new
                {
                    seSg,
                    IdLesson = l.Id,
                    l.StartDate,
                    l.EndDate,
                    l.NumberOfParticipants,
                    l.ClassroomNumber,
                    l.SubjectsId
                }).Join(_context.Subjects, seSgL => seSgL.SubjectsId, s => s.Id,
                (seSgL, s) => new
                {
                    seSgL,
                    IdSubjects = s.Id,
                    s.Name,
                    s.InstructorsId
                }).Join(_context.Instructors, seSgLS => seSgLS.InstructorsId, i => i.Id,
                (seSgLS, i) => new
                {
                    seSgLS,
                    IdInstructors = i.Id,
                    InstructorsFirstName = i.FirstName,
                    InstructorsLastName = i.LastName,
                    InstructorsPhoneNumber = i.PhoneNumber,
                    InstructorsAddress = i.PhoneNumber,
                    InstructorsCity = i.City,
                    InstructorsPostcode = i.Postcode,
                    InstructorsEmailAddress = i.EmailAddress
                }).Where(arg =>arg.seSgLS.seSgL.SubjectsId == SubjectsId);

            List<AllTablesJoined> results = new List<AllTablesJoined>();

            foreach (var q in query)
            {
                results.Add(new AllTablesJoined
                {
                    IdStudents = q.seSgLS.seSgL.seSg.se.IdStudents,
                    StudentsFirstName = q.seSgLS.seSgL.seSg.se.StudentsFirstName,
                    StudentsLastName = q.seSgLS.seSgL.seSg.se.StudentsLastName,
                    BirthDate = q.seSgLS.seSgL.seSg.se.BirthDate,
                    StudentsPhoneNumber = q.seSgLS.seSgL.seSg.se.StudentsPhoneNumber,
                    StudentsAddress = q.seSgLS.seSgL.seSg.se.StudentsAddress,
                    StudentsCity = q.seSgLS.seSgL.seSg.se.StudentsCity,
                    StudentsPostcode = q.seSgLS.seSgL.seSg.se.StudentsPostcode,
                    StudentsEmailAddress = q.seSgLS.seSgL.seSg.se.StudentsEmailAddress,
                    StudentGroupsId = q.seSgLS.seSgL.seSg.se.StudentGroupsId,
                    StudentsId = q.seSgLS.seSgL.seSg.se.StudentsId,
                    IdStudentGroups = q.seSgLS.seSgL.seSg.IdStudentGroups,
                    DayOfTheWeek = q.seSgLS.seSgL.seSg.DayOfTheWeek,
                    StartTime = q.seSgLS.seSgL.seSg.StartTime,
                    EndTime = q.seSgLS.seSgL.seSg.EndTime,
                    LessonsId = q.seSgLS.seSgL.seSg.LessonsId,
                    IdLessons = q.seSgLS.seSgL.IdLesson,
                    StartDate = q.seSgLS.seSgL.StartDate,
                    EndDate = q.seSgLS.seSgL.EndDate,
                    NumberOfParticipants = q.seSgLS.seSgL.NumberOfParticipants,
                    ClassroomNumber = q.seSgLS.seSgL.ClassroomNumber,
                    SubjectsId = q.seSgLS.seSgL.SubjectsId,
                    IdSubjects = q.seSgLS.IdSubjects,
                    SubjectName = q.seSgLS.Name,
                    InstructorsId = q.seSgLS.InstructorsId,
                    IdInstructors = q.IdInstructors,
                    InstructorsFirstName = q.InstructorsFirstName,
                    InstructorsLastName = q.InstructorsLastName,
                    InstructorsPhoneNumber = q.InstructorsPhoneNumber,
                    InstructorsAddress = q.InstructorsAddress,
                    InstructorsCity = q.InstructorsCity,
                    InstructorsPostcode = q.InstructorsPostcode,
                    InstructorsEmailAddress = q.InstructorsEmailAddress
                });
            }
            
            return View(results);
        }
        
        public ActionResult SameDatesConflict(string message)
        {
            ViewBag.Error = message;
            return View();
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
                catch (DbUpdateConcurrencyException e)
                {
                    await e.Entries.Single().ReloadAsync();
                    await _context.SaveChangesAsync();
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
            var query = _context.Students.Join(_context.Enrollments, s => s.Id,
                e => e.StudentsId,
                (s, e) => new
                {
                    IdStudents = s.Id,
                    StudentsFirstName = s.FirstName,
                    StudentsLastName = s.LastName,
                    s.BirthDate,
                    StudentsPhoneNumber = s.PhoneNumber,
                    StudentsAddress = s.Address,
                    StudentsCity = s.City,
                    StudentsPostcode = s.Postcode,
                    StudentsEmailAddress = s.EmailAddress,
                    e.StudentsId,
                    e.StudentGroupsId
                }).Join(_context.StudentGroups, se => se.StudentGroupsId, sg => sg.Id,
                (se, sg) => new
                {
                    se,
                    IdStudentGroups = sg.Id,
                    sg.DayOfTheWeek,
                    sg.StartTime,
                    sg.EndTime,
                    sg.LessonsId
                }).Join(_context.Lessons, seSg => seSg.LessonsId, l => l.Id,
                (seSg, l) => new
                {
                    seSg,
                    IdLesson = l.Id,
                    l.StartDate,
                    l.EndDate,
                    l.NumberOfParticipants,
                    l.ClassroomNumber,
                    l.SubjectsId
                }).Join(_context.Subjects, seSgL => seSgL.SubjectsId, s => s.Id,
                (seSgL, s) => new
                {
                    seSgL,
                    IdSubjects = s.Id,
                    s.Name,
                    s.InstructorsId
                }).Join(_context.Instructors, seSgLS => seSgLS.InstructorsId, i => i.Id,
                (seSgLS, i) => new
                {
                    seSgLS,
                    IdInstructors = i.Id,
                    InstructorsFirstName = i.FirstName,
                    InstructorsLastName = i.LastName,
                    InstructorsPhoneNumber = i.PhoneNumber,
                    InstructorsAddress = i.PhoneNumber,
                    InstructorsCity = i.City,
                    InstructorsPostcode = i.Postcode,
                    InstructorsEmailAddress = i.EmailAddress
                });

            List<AllTablesJoined> results = new List<AllTablesJoined>();

            foreach (var q in query)
            {
                results.Add(new AllTablesJoined
                {
                    IdStudents = q.seSgLS.seSgL.seSg.se.IdStudents,
                    StudentsFirstName = q.seSgLS.seSgL.seSg.se.StudentsFirstName,
                    StudentsLastName = q.seSgLS.seSgL.seSg.se.StudentsLastName,
                    BirthDate = q.seSgLS.seSgL.seSg.se.BirthDate,
                    StudentsPhoneNumber = q.seSgLS.seSgL.seSg.se.StudentsPhoneNumber,
                    StudentsAddress = q.seSgLS.seSgL.seSg.se.StudentsAddress,
                    StudentsCity = q.seSgLS.seSgL.seSg.se.StudentsCity,
                    StudentsPostcode = q.seSgLS.seSgL.seSg.se.StudentsPostcode,
                    StudentsEmailAddress = q.seSgLS.seSgL.seSg.se.StudentsEmailAddress,
                    StudentGroupsId = q.seSgLS.seSgL.seSg.se.StudentGroupsId,
                    StudentsId = q.seSgLS.seSgL.seSg.se.StudentsId,
                    IdStudentGroups = q.seSgLS.seSgL.seSg.IdStudentGroups,
                    DayOfTheWeek = q.seSgLS.seSgL.seSg.DayOfTheWeek,
                    StartTime = q.seSgLS.seSgL.seSg.StartTime,
                    EndTime = q.seSgLS.seSgL.seSg.EndTime,
                    LessonsId = q.seSgLS.seSgL.seSg.LessonsId,
                    IdLessons = q.seSgLS.seSgL.IdLesson,
                    StartDate = q.seSgLS.seSgL.StartDate,
                    EndDate = q.seSgLS.seSgL.EndDate,
                    NumberOfParticipants = q.seSgLS.seSgL.NumberOfParticipants,
                    ClassroomNumber = q.seSgLS.seSgL.ClassroomNumber,
                    SubjectsId = q.seSgLS.seSgL.SubjectsId,
                    IdSubjects = q.seSgLS.IdSubjects,
                    SubjectName = q.seSgLS.Name,
                    InstructorsId = q.seSgLS.InstructorsId,
                    IdInstructors = q.IdInstructors,
                    InstructorsFirstName = q.InstructorsFirstName,
                    InstructorsLastName = q.InstructorsLastName,
                    InstructorsPhoneNumber = q.InstructorsPhoneNumber,
                    InstructorsAddress = q.InstructorsAddress,
                    InstructorsCity = q.InstructorsCity,
                    InstructorsPostcode = q.InstructorsPostcode,
                    InstructorsEmailAddress = q.InstructorsEmailAddress
                });
            }
            
            return View(results);
        }
    }
}