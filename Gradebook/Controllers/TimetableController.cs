using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Gradebook.Controllers
{
    [Authorize(Roles = Role.TeacherStudent), ViewFilter]
    public class TimetableController : Controller
    {
        private ApplicationDbContext Db = ApplicationDbContext.Create();

        public ActionResult Index()
        {
            if (User.IsInRole(Role.Student))
            {
                var id = User.Identity.GetUserId();
                var student = Db.Student.Where(e => e.Id == id).Single();
                var _class = student.Class;
                var lessons = new LinkedList<Lesson>();
                foreach (var tcs in _class.TeacherClassSubjects)
                    foreach (var l in tcs.Lessons)
                        lessons.AddLast(l);
                var timetable = new string[Days.Array.Length, LessonHours.Array.Length];
                foreach (var l in lessons)
                {
                    var tcl = l.TeacherClassSubject;
                    timetable[l.DayId, l.HourId] = $"{tcl.Subject.Name} | {tcl.Teacher.ApplicationUser.Name} {tcl.Teacher.ApplicationUser.Surname}";
                }
                ViewBag.Timetable = timetable;
                return View();
            }
            else if (User.IsInRole(Role.Teacher))
            {
                var id = User.Identity.GetUserId();
                var tcss = Db.TeacherClassSubject.Where(e => e.TeacherId == id).ToArray();
                var lessons = new LinkedList<Lesson>();
                foreach (var tcs in tcss)
                    foreach (var l in tcs.Lessons)
                        lessons.AddLast(l);
                var timetable = new string[Days.Array.Length, LessonHours.Array.Length];
                foreach (var l in lessons)
                {
                    var tcl = l.TeacherClassSubject;
                    timetable[l.DayId, l.HourId] = $"{tcl.Subject.Name} | {tcl.Class.Year} {tcl.Class.Unit}";
                }
                ViewBag.Timetable = timetable;
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
    }
}