using Gradebook.Models;
using Gradebook.Models.ViewModels;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Student.Controllers
{
    public class GradeController : ControllerBase
    {
        [Authorize(Roles = Role.Student), ViewFilter]
        public ActionResult List()
        {
            var userId = User.Identity.GetUserId();
            var studentSearch = Db.Student.Where(e => e.Id == userId);
            if (studentSearch.Count() != 1) return ErrorView("Your account does not exist.");
            var s = studentSearch.Single();
            if (s.ClassId == null) return ErrorView("You do not belong to any class.");
            var _class = s.Class;
            var grades = Db.Grade.Where(e => e.StudentId == userId).ToArray();
            var list = new LinkedList<Subject>();
            foreach (var tcs in _class.TeacherClassSubjects)
                list.AddLast(tcs.Subject);
            var subjects = list;
            var gradesGrouped = SubjectGrades.GroupGradesBySubject(grades, subjects);
            return View(gradesGrouped);
        }
    }
}
