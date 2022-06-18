using Gradebook.Utils;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Controllers
{
    public class LanguageController : ControllerBase
    {
        public ActionResult SelectEnglish(string returnPath)
        {
            LanguageCookie.Save(Response.Cookies, 0);
            return RedirectToAction(returnPath);
        }

        public ActionResult SelectPolish(string returnPath)
        {
            LanguageCookie.Save(Response.Cookies, 1);
            return RedirectToAction(returnPath);
        }

        new private RedirectToRouteResult RedirectToAction(string path)
        {
            var link = PathSerializer.Deserialize(path);
            var withArea = link.RouteValues;
            withArea["area"] = link.Area;
            return RedirectToAction(link.Action, link.Controller, withArea);
        }
    }
}