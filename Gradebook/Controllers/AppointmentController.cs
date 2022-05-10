using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Gradebook.Controllers
{
    [ViewFilter]
    public class AppointmentController : Controller
    {
        private ApplicationDbContext Db = ApplicationDbContext.Create();

        [Authorize(Roles = Role.TeacherStudent)]
        public ActionResult Index(int? weekDelta)
        {
            weekDelta = weekDelta ?? 0;
            ViewBag.WeekDelta = weekDelta;
            var now = DateTime.Now;
            DayOfWeek weekDay = now.DayOfWeek; // który dzisiaj dzień tygodnia
            int days = weekDay - DayOfWeek.Monday; // ile dni minęło od poniedziałku
            DateTime start = now.AddDays(-days).Date; // poniedziałek w obecnym tygodniu; Date obcina godzinę do 00:00:00
            start = start.AddDays(7 * (int)weekDelta); // przesuwamy się o weekDelta tygodni
            DateTime end = start.AddDays(5).AddSeconds(-1); // piątek 23:59:59 w tygodniu po przesunięciu
            var userId = User.Identity.GetUserId();
            Appointment[] appointments = null;
            if (User.IsInRole(Role.Teacher))
                appointments = Db.Appointment.Where(e => e.TeacherClassSubject.TeacherId == userId &&
                    e.Date >= start && e.Date <= end).ToArray();
            else if (User.IsInRole(Role.Student))
            {
                var classId = Db.Student.Where(e => e.Id == userId).Single().ClassId;
                appointments = Db.Appointment.Where(e => e.TeacherClassSubject.ClassId == classId &&
                    e.Date >= start && e.Date <= end).ToArray();
            }
            var dates = new DateTime[Days.Array.Length];
            for (int i = 0; i < dates.Length; ++i)
                dates[i] = start.AddDays(i);
            ViewBag.Dates = dates;
            return View(GetWeekDayAppointments(appointments));
        }

        private LinkedList<Appointment>[] GetWeekDayAppointments(Appointment[] appointments)
        {
            var ret = new LinkedList<Appointment>[Days.Array.Length];
            for (int i = 0; i < ret.Length; ++i)
                ret[i] = new LinkedList<Appointment>();
            foreach (var a in appointments)
            {
                var weekDayId = a.Date.DayOfWeek - DayOfWeek.Monday; // ile dni minęło od poniedziałku do dnia a.Date
                ret[weekDayId].AddLast(a);
            }
            return ret;
        }

        // TODO: Create z selectify - po wybraniu przez nauczyciela klasy odświeża się dropdown lista przedmiotów, którą uczy w danej klasie

        [Authorize(Roles = Role.TeacherStudent)]
        public ActionResult Details(int? id, int? weekDelta)
        {
            do
            {
                weekDelta = weekDelta ?? 0;
                ViewBag.WeekDelta = weekDelta;
                id = id ?? 0;
                var found = Db.Appointment.Where(e => e.Id == id);
                if (found.Count() != 1) break;
                var a = found.Single();
                var userId = User.Identity.GetUserId();
                if (User.IsInRole(Role.Teacher))
                {
                    if (a.TeacherClassSubject.TeacherId != userId) break;
                }
                else if (User.IsInRole(Role.Student))
                {
                    var foundStudents = Db.Student.Where(e => e.Id == userId);
                    if (foundStudents.Count() != 1) break;
                    var student = foundStudents.Single();
                    if (a.TeacherClassSubject.ClassId != student.ClassId) break;
                }
                return View(a);
            } while (false);
            return RedirectToAction("Index", new { weekDelta = weekDelta });
        }

        [Authorize(Roles = Role.Teacher)]
        public ActionResult Delete(int? id, int? weekDelta)
        {
            do
            {
                weekDelta = weekDelta ?? 0;
                id = id ?? 0;
                var teacherId = User.Identity.GetUserId();
                var found = Db.Appointment.Where(e => e.Id == id);
                if (found.Count() != 1) break;
                var a = found.Single();
                if (teacherId != a.TeacherClassSubject.TeacherId) break;
                Db.Appointment.Remove(a);
                Db.SaveChanges();
            } while (false);
            return RedirectToAction("Index", new { weekDelta = weekDelta });
        }
    }
}
