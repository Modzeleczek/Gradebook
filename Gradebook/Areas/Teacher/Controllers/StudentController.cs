using Gradebook.Utils;
using System.Linq;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Teacher.Controllers
{
    [Authorize(Roles = Role.Teacher), ViewFilter]
    public class StudentController : ControllerBase
    {
        public ActionResult Details(string id)
        {
            var search = Db.Student.Where(e => e.Id == id);
            if (search.Count() != 1) return ErrorView("Such student does not exist.");
            return View(search.Single());
        }
    }
}
