using System.Linq;
using System.Web.Mvc;
using Gradebook.Models;
using Gradebook.Utils;

namespace Gradebook.Controllers
{
    [AllowAnonymous, ViewFilter]
    public class GlobalAnnouncementController : Controller
    {
        private ApplicationDbContext Db = ApplicationDbContext.Create();

        public ActionResult List()
        {
            return View(Db.GlobalAnnouncement.ToArray());
        }

        public ActionResult Details(int id)
        {
            return View(Db.GlobalAnnouncement.Where(e => e.Id == id).Single());
        }
    }
}
