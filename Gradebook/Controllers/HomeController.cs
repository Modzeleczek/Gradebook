using System.Web.Mvc;
using Gradebook.Models;
using Gradebook.Utils;

namespace Gradebook.Controllers
{
    [AllowAnonymous, ViewFilter]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ApplicationDbContext.Create().Database.CreateIfNotExists();
            var l = Role.GetLinks(User)[0];
            return RedirectToAction(l.Action, l.Controller, new { area = l.Area });
        }
    }
}
