using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Teacher.Controllers
{
    [Authorize(Roles = Role.Teacher), ViewFilter]
    public class AppointmentController : ControllerBase
    {
        public ActionResult List()
        {
            var userId = User.Identity.GetUserId();
            var appointments = Db.Appointment.Where(e => e.TeacherClassSubject.TeacherId == userId).ToArray();
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
            if (a.TeacherClassSubject.TeacherId != userId) return ErrorView("You do not own such appointment.");
            return View(a);
        }

        public ActionResult Delete(int? id)
        {
            int intId = -1;
            if (id.HasValue) intId = id.Value;
            var appointmentSearch = Db.Appointment.Where(e => e.Id == id);
            if (appointmentSearch.Count() != 1) return ErrorView("Such appointment does not exist.");
            var a = appointmentSearch.Single();
            var userId = User.Identity.GetUserId();
            if (a.TeacherClassSubject.TeacherId != userId) return ErrorView("You do not own such appointment.");
            Db.Appointment.Remove(a);
            Db.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
