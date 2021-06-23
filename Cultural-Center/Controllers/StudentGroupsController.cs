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
using Cultural_Center.ViewObjects;

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
                try
                {
                    _context.Add(studentGroups);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    if (e.InnerException != null)
                    {
                        if (e.InnerException.Message.Contains("are the same"))
                        {
                            return RedirectToAction("TheSameTimesConflict",
                                new {message = e.InnerException.Message});
                        }

                        if (e.InnerException.Message.Contains("the same lesson"))
                        {
                            return RedirectToAction("TheSameLessonConflict",
                                new {message = e.InnerException.Message, studentGroups.LessonsId, studentGroups.DayOfTheWeek});
                        }
                        
                        return RedirectToAction("SimilarTimesConflict",new {message = e.InnerException.Message, studentGroups.LessonsId , studentGroups.DayOfTheWeek});

                    }
                }
                
                return RedirectToAction(nameof(Index));
            }
            ViewData["LessonsId"] = new SelectList(_context.Lessons, "Id", "Id", studentGroups.LessonsId);
            return View(studentGroups);
        }
        
        public ActionResult TheSameTimesConflict(string message)
        {
            ViewBag.Error = message;
            return View();
        }

        public ActionResult TheSameLessonConflict(string message, int LessonsId, string DayOfTheWeek)
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
                }).Where(arg => arg.seSgLS.seSgL.seSg.LessonsId == LessonsId && arg.seSgLS.seSgL.seSg.DayOfTheWeek == DayOfTheWeek);

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
        
        public ActionResult SimilarTimesConflict(string message,int LessonsId, string DayOfTheWeek)
        {
            ViewBag.Error = message;
            
            string classroomNumber = _context.Lessons.Find(LessonsId).ClassroomNumber;
            
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
                }).Where(arg => arg.seSgLS.seSgL.ClassroomNumber == classroomNumber && arg.seSgLS.seSgL.seSg.DayOfTheWeek == DayOfTheWeek);

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
