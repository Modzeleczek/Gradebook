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
            var id = User.Identity.GetUserId();
            var absences = Db.Absence.Where(e => e.StudentId == id).ToArray();
            return View(absences);
        }
    }
}
