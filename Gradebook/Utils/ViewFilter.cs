using System.Web.Mvc;

namespace Gradebook.Utils
{
    public class ViewFilter : FilterAttribute, IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            // Do something before the result executes.
            SaveLanguageIdToViewBag(context);
            SaveCurrentPathToViewBag(context);
        }

        private void SaveLanguageIdToViewBag(ResultExecutingContext context)
        {
            context.Controller.ViewBag.LanguageId = LanguageCookie.Read(context.RequestContext.HttpContext.Request.Cookies);
        }

        private void SaveCurrentPathToViewBag(ResultExecutingContext context)
        {
            context.Controller.ViewBag.CurrentPath = PathSerializer.Serialize(context.HttpContext.Request);
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            // Do something after the result executes.
        }
    }
}