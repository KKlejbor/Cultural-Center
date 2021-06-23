using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cultural_Center;
using Cultural_Center.Models;
using Cultural_Center.ViewObjects;
using Microsoft.Data.SqlClient;

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
        public async Task<IActionResult> Details(int? StudentGroupsId, int? StudentsId)
        {
            if (StudentGroupsId == null || StudentsId == null)
            {
                return NotFound();
            }

            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.StudentGroup)
                .FirstOrDefaultAsync(m => m.StudentGroupsId == StudentGroupsId && m.StudentsId == StudentsId);
            if (enrollments == null)
            {
                return NotFound();
            }

            return View(enrollments);
        }

        // GET: Enrollments/Create
        public IActionResult Create()
        {
            ViewData["StudentsId"] = new SelectList(_context.Students, "Id", "Id");
            ViewData["StudentGroupsId"] = new SelectList(_context.StudentGroups, "Id", "Id");
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

                try
                {
                    _context.Add(enrollments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    if (e.InnerException != null)
                    {
                        if (e.InnerException.Message.Contains("does not accept"))
                        {
                            return RedirectToAction("ParticipantsLimitConflict",
                                new {message = e.InnerException.Message});
                        }
                        
                        return RedirectToAction("SimilarTimesConflict",new {message = e.InnerException.Message, StudentId = enrollments.StudentsId, enrollments.StudentGroupsId});

                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["StudentsId"] = new SelectList(_context.Students, "Id", "Id", enrollments.StudentsId);
            ViewData["StudentGroupsId"] =
                new SelectList(_context.StudentGroups, "Id", "Id", enrollments.StudentGroupsId);
            return View(enrollments);
        }

        public ActionResult ParticipantsLimitConflict(string message)
        {
            ViewBag.Error = message;
            return View();
        }

        [HttpGet]
        public ActionResult SimilarTimesConflict(string message, int StudentId, int StudentGroupsId)
        {
            
            ViewBag.Error = message;

            string DayOfTheWeek = _context.StudentGroups.Find(StudentGroupsId).DayOfTheWeek;
            
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
                }).Where(arg => arg.seSgLS.seSgL.seSg.se.IdStudents == StudentId && arg.seSgLS.seSgL.seSg.DayOfTheWeek  == DayOfTheWeek);

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

        // GET: Enrollments/Edit/5
        public async Task<IActionResult> Edit(int? StudentGroupsId, int? StudentsId)
        {
            if (StudentGroupsId == null || StudentsId == null)
            {
                return NotFound();
            }

            var enrollments = await _context.Enrollments.FindAsync(StudentGroupsId, StudentsId);
            if (enrollments == null)
            {
                return NotFound();
            }
            ViewData["StudentsId"] = new SelectList(_context.Students, "Id", "Id", enrollments.StudentsId);
            ViewData["StudentGroupsId"] = new SelectList(_context.StudentGroups, "Id", "Id", enrollments.StudentGroupsId);
            return View(enrollments);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int StudentGroupsId, int StudentsId, [Bind("StudentGroupsId,StudentsId")] Enrollments enrollments)
        {
            if (StudentGroupsId != enrollments.StudentGroupsId || StudentsId != enrollments.StudentsId)
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
                catch (DbUpdateConcurrencyException e)
                {
                    await e.Entries.Single().ReloadAsync();
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["StudentsId"] = new SelectList(_context.Students, "Id", "Id", enrollments.StudentsId);
            ViewData["StudentGroupsId"] = new SelectList(_context.StudentGroups, "Id", "Id", enrollments.StudentGroupsId);
            return View(enrollments);
        }

        // GET: Enrollments/Delete/5
        public async Task<IActionResult> Delete(int? StudentGroupsId, int? StudentsId)
        {
            if (StudentGroupsId == null || StudentsId == null)
            {
                return NotFound();
            }

            var enrollments = await _context.Enrollments
                .Include(e => e.Student)
                .Include(e => e.StudentGroup)
                .FirstOrDefaultAsync(m => m.StudentGroupsId == StudentGroupsId && m.StudentsId == StudentsId);
            if (enrollments == null)
            {
                return NotFound();
            }

            return View(enrollments);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int StudentGroupsId, int StudentsId)
        {
            var enrollments = await _context.Enrollments.FindAsync(StudentGroupsId, StudentsId);
            _context.Enrollments.Remove(enrollments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnrollmentsExists(int StudentGroupsId, int StudentsId)
        {
            return _context.Enrollments.Any(e => e.StudentGroupsId == StudentGroupsId && e.StudentsId == StudentsId);
        }
    }
}
