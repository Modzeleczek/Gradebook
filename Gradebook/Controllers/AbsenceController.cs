using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;

namespace Gradebook.Controllers
{
    [ViewFilter]
    public class AbsenceController : Controller
    {
        private ApplicationDbContext Db = ApplicationDbContext.Create();

        [Authorize(Roles = Role.AdministratorTeacherStudent)]
        public ActionResult Index(string studentId)
        {
            IEnumerable<Absence> absences = null;
            if (User.IsInRole(Role.Administrator))
                absences = Db.Absence.ToArray();
            else if (User.IsInRole(Role.Teacher))
            {
                if (studentId == null)
                    return RedirectToAction("Index", "Class");
                absences = Db.Absence.Where(e => e.StudentId == studentId).ToArray();
                ViewBag.StudentId = studentId;
                var supervisorId = Db.Student.Where(e => e.Id == studentId).Single().Class.SupervisorId;
                var teacherId = User.Identity.GetUserId();
                ViewBag.IsSupervisor = (teacherId == supervisorId);
            }
            else if (User.IsInRole(Role.Student))
            {
                var id = User.Identity.GetUserId();
                absences = Db.Absence.Where(e => e.StudentId == id).ToArray();
            }
            return View(absences);
        }

        [Authorize(Roles = Role.Teacher)]
        public ActionResult Create(string studentId)
        {
            ViewBag.StudentId = studentId;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Role.Teacher)]
        public ActionResult Create(string studentId, Absence absence)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.StudentId = studentId;
                return View(absence);
            }
            absence.Date = DateTime.Now;
            absence.StudentId = studentId;
            absence.IsJustified = false;
            Db.Absence.Add(absence);
            Db.SaveChanges();
            return RedirectToAction("Index", new { studentId = studentId });
        }

        [Authorize(Roles = Role.Teacher)]
        public ActionResult FlipIsJustified(string studentId, int id)
        {
            var absence = Db.Absence.Where(e => e.Id == id).Single();
            absence.IsJustified = !absence.IsJustified;
            Db.SaveChanges();
            return RedirectToAction("Index", new { studentId = studentId });
        }
    }
}
