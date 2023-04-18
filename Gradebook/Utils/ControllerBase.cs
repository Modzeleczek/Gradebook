using Gradebook.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Linq;
using System.Web;
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

        protected JsonResult ErrorJson(string message)
        {
            var d = LocalizedStrings.GenericErrorView.GenericError[LanguageCookie.Read(Request.Cookies)];
            return Json(new { errorMessage = d[message] });
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Request.IsAuthenticated) return;
            var userId = User.Identity.GetUserId();
            if (Db.Users.Any(e => e.Id == userId)) return;
            else
            {
                IAuthenticationManager AuthenticationManager = HttpContext.GetOwinContext().Authentication;
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                /* It cannot be so because then the user sees that he/she is
                still logged in and only after performing any next request,
                he/she sees that he/she is logged out. */
                // filterContext.Result = ErrorView("Your account does not exist.");
                filterContext.Result = RedirectToAction("GenericError", "Home", new { message = "Your account does not exist." });
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
