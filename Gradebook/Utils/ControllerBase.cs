using Gradebook.Models;
using System.Web.Mvc;

namespace Gradebook.Utils
{
    public abstract class ControllerBase : Controller
    {
        protected readonly ApplicationDbContext Db = ApplicationDbContext.Create();
        protected ViewResult ErrorView(string message)
        {
            return View("~/Views/Shared/GenericError.cshtml", model: message);
        }
    }
}
