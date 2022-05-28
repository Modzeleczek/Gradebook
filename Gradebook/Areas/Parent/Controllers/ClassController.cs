using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Parent.Controllers
{
    [Authorize(Roles = Role.Parent), ViewFilter]
    public class ClassController : ControllerBase
    {
        public ActionResult Details(int? id)
        {
            var userId = User.Identity.GetUserId();
            var parent = Db.Parent.Where(e => e.Id == userId).Single();
            var classSearch = Db.Class.Where(e => e.Id == id);
            if (classSearch.Count() != 1) return ErrorView("Such class does not exist.");
            var childrenSearch = Db.Student.Where(e => e.ParentId == userId);
            bool hasChildInClass = false;
            foreach (var child in childrenSearch)
                if (child.ClassId == id.Value)
                {
                    hasChildInClass = true;
                    break;
                }
            if (!hasChildInClass) return ErrorView("You are not parent of a child in such class.");
            return View(classSearch.Single());
        }
    }
}
