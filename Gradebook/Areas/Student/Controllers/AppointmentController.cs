using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Student.Controllers
{
    [Authorize(Roles = Role.Student), ViewFilter]
    public class AppointmentController : ControllerBase
    {
        public ActionResult List()
        {
            var userId = User.Identity.GetUserId();
            var studentSearch = Db.Student.Where(e => e.Id == userId);
            if (studentSearch.Count() != 1) return ErrorView("Your account does not exist.");
            var classId = studentSearch.Single().ClassId;
            var appointments = Db.Appointment.Where(e => e.TeacherClassSubject.ClassId == classId).ToArray();
            return View(appointments);
        }

        public ActionResult Details(int? id)
        {
            int intId = -1;
            if (id.HasValue) intId = id.Value;
            var appointmentSearch = Db.Appointment.Where(e => e.Id == intId);
            if (appointmentSearch.Count() != 1) return ErrorView("Such appointment does not exist.");
            var a = appointmentSearch.Single();
            var userId = User.Identity.GetUserId();
            var studentSearch = Db.Student.Where(e => e.Id == userId);
            if (studentSearch.Count() != 1) return ErrorView("Your account does not exist.");
            var student = studentSearch.Single();
            if (a.TeacherClassSubject.ClassId != student.ClassId) return ErrorView("You do not belong to such class.");
            return View(a);
        }
    }
}
