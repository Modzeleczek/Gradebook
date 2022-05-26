using Gradebook.Utils;
using System.Web.Mvc;

namespace Gradebook.Areas.Teacher.Controllers
{
    [Authorize(Roles = Role.Teacher), ViewFilter]
    public class Appointment2Controllerr : Controller
    {
        public ActionResult Index()
        {
            // ViewBag
            return View();
        }
    }
}
