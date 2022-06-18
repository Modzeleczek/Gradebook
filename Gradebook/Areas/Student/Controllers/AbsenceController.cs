using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Student.Controllers
{
    [Authorize(Roles = Role.Student), ViewFilter]
    public class AbsenceController : ControllerBase
    {
        public ActionResult List()
        {
            var userId = User.Identity.GetUserId();
            var studentSearch = Db.Student.Where(e => e.Id == userId);
            if (studentSearch.Count() != 1) return ErrorView("Your account does not exist.");
            var student = studentSearch.Single();
            if (student.ClassId == null) return ErrorView("You do not belong to any class.");
            var absences = student.Absences.ToArray();
            var orderedAbsences = absences.OrderBy(e => e.Date).ToArray();
            return View(orderedAbsences);
        }
    }
}
