using Gradebook.Utils;
using System.Linq;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Teacher.Controllers
{
    [Authorize(Roles = Role.Teacher), ViewFilter]
    public class GlobalAnnouncementController : ControllerBase
    {
        public ActionResult List()
        {
            return View(Db.GlobalAnnouncement.ToArray());
        }

        public ActionResult Details(int id)
        {
            var search = Db.GlobalAnnouncement.Where(e => e.Id == id);
            if (search.Count() != 1) return ErrorView("Announcement does not exist.");
            return View(search.Single());
        }
    }
}
