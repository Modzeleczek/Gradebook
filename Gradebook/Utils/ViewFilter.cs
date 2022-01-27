using System.Web;
using System.Web.Mvc;

namespace Gradebook.Utils
{
    public class ViewFilter : FilterAttribute, IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            // Do something before the result executes.
            SaveLanguageIdToViewBag(context);
        }

        private void SaveLanguageIdToViewBag(ResultExecutingContext context)
        {
            context.Controller.ViewBag.LanguageId = LanguageCookie.Read(context.RequestContext.HttpContext.Request.Cookies);
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            // Do something after the result executes.
        }
    }
}