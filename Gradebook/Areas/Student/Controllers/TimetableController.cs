using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Student.Controllers
{
    [Authorize(Roles = Role.Student), ViewFilter]
    public class TimetableController : ControllerBase
    {
        public ActionResult List()
        {
            var id = User.Identity.GetUserId();
            var studentSearch = Db.Student.Where(e => e.Id == id);
            if (studentSearch.Count() != 1) return ErrorView("Your account does not exist.");
            var _class = studentSearch.Single().Class;
            var lessons = new LinkedList<Lesson>();
            foreach (var tcs in _class.TeacherClassSubjects)
                foreach (var l in tcs.Lessons)
                    lessons.AddLast(l);
            var lessonMap = new Lesson[Days.Array.Length, LessonHours.Array.Length];
            foreach (var l in lessons)
                lessonMap[l.DayId, l.HourId] = l;
            return View(lessonMap);
        }
    }
}
