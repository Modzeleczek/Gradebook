using Gradebook.Models;
using Gradebook.Models.ViewModels;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Parent.Controllers
{
    [Authorize(Roles = Role.Parent), ViewFilter]
    public class ChildController : ControllerBase
    {
        public ActionResult List()
        {
            var parentId = User.Identity.GetUserId();
            var children = Db.Student.Where(e => e.ParentId == parentId).ToArray();
            return View(children);
        }

        public ActionResult StudentDetails(string studentId)
        {
            var parentId = User.Identity.GetUserId();
            var studentSearch = Db.Student.Where(e => e.Id == studentId && e.ParentId == parentId);
            if (studentSearch.Count() != 1) return ErrorView("You are not parent of such student.");
            return View(studentSearch.Single());
        }

        public ActionResult ClassDetails(int? classId)
        {
            var parentId = User.Identity.GetUserId();
            var studentSearch = Db.Student.Where(e => e.ClassId == classId && e.ParentId == parentId);
            if (studentSearch.Count() == 0) return ErrorView("You are not parent of a child in such class.");
            var student = studentSearch.Single();
            // if (student.ClassId == null) return ErrorView("This student does not belong to any class.");
            return View(student.Class);
        }

        public ActionResult GradeList(string studentId)
        {
            var parentId = User.Identity.GetUserId();
            var studentSearch = Db.Student.Where(e => e.Id == studentId && e.ParentId == parentId);
            if (studentSearch.Count() != 1) return ErrorView("You are not parent of such student.");
            var s = studentSearch.Single();
            if (s.ClassId == null) return ErrorView("This student does not belong to any class.");
            var _class = s.Class;
            var grades = Db.Grade.Where(e => e.StudentId == studentId).ToArray();
            var list = new LinkedList<Subject>();
            foreach (var tcs in _class.TeacherClassSubjects)
                list.AddLast(tcs.Subject);
            var subjects = list;
            var gradesGrouped = SubjectGrades.GroupGradesBySubject(grades, subjects);
            return View(gradesGrouped);
        }

        public ActionResult AbsenceList(string studentId)
        {
            var parentId = User.Identity.GetUserId();
            var studentSearch = Db.Student.Where(e => e.Id == studentId && e.ParentId == parentId);
            if (studentSearch.Count() != 1) return ErrorView("You are not parent of such student.");
            var student = studentSearch.Single();
            if (student.ClassId == null) return ErrorView("This student does not belong to any class.");
            var absences = student.Absences.ToArray();
            var orderedAbsences = absences.OrderBy(e => e.Date).ToArray();
            return View(orderedAbsences);
        }
    }
}
