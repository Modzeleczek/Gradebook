using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gradebook.Controllers
{
    [Authorize(Roles = Role.Parent), ViewFilter]
    public class ChildController : Controller
    {
        private ApplicationDbContext Db = ApplicationDbContext.Create();

        // GET: Child
        public ActionResult Index()
        {
            var parentId = User.Identity.GetUserId();
            var children = Db.Student.Where(e => e.ParentId == parentId).ToArray();
            return View(children);
        }

        public ActionResult StudentDetails(string studentId)
        {
            return View(Db.Student.Where(e => e.Id == studentId).Single());
        }

        public ActionResult ClassDetails(int classId)
        {
            return View(Db.Class.Where(e => e.Id == classId).Single());
        }

        public ActionResult GradeList(string studentId)
        {
            IEnumerable<Grade> grades = null;
            IEnumerable<Subject> subjects = null;
            var child = Db.Student.Where(e => e.Id == studentId).Single();
            var childId = child.Id;
            grades = Db.Grade.Where(e => e.StudentId == childId).ToArray();
            var _class = child.Class;
            var list = new LinkedList<Subject>();
            foreach (var tcs in _class.TeacherClassSubjects)
                list.AddLast(tcs.Subject);
            subjects = list;
            var gradesGrouped = GradeController.GroupGradesBySubject(grades, subjects);
            return View(gradesGrouped);
        }

        public ActionResult AbsenceList(string studentId)
        {
            IEnumerable<Absence> absences = Db.Absence.Where(e => e.StudentId == studentId);
            var orderedAbsences = absences.OrderBy(e => e.Date).ToArray();
            return View(orderedAbsences);
        }
    }
}