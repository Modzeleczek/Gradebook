using Gradebook.Utils;
using System.Web.Mvc;

namespace Gradebook.Controllers
{
    public class LanguageController : Controller
    {
        public ActionResult SelectEnglish()
        {
            LanguageCookie.Save(Response.Cookies, 0);
            return RedirectToAction("Index", "GlobalAnnouncement");
        }

        public ActionResult SelectPolish()
        {
            LanguageCookie.Save(Response.Cookies, 1);
            return RedirectToAction("Index", "GlobalAnnouncement");
        }
    }
}