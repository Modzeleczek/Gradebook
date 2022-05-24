using System.Web.Mvc;
using Gradebook.Models;
using Gradebook.Utils;

namespace Gradebook.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ApplicationDbContext.Create().Database.CreateIfNotExists();
            var roles = new string[] { Role.Administrator, Role.Teacher, Role.Student, Role.Parent };
            var areas = new string[] { "Admin", "Teacher", "Student", "Parent" };
            string area = null;
            for (int i = 0; i < roles.Length; ++i)
                if (User.IsInRole(roles[i]))
                {
                    area = areas[i];
                    break;
                }
            return RedirectToAction("List", "GlobalAnnouncement", new { area = area });
        }
    }
}
