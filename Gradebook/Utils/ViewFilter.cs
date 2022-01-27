using System.Web;
using System.Web.Mvc;

namespace Gradebook.Utils
{
    public class ViewFilter : FilterAttribute, IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            // Do something before the result executes.
            ReadLanguageId(context);
        }

        private void ReadLanguageId(ResultExecutingContext context)
        {
            context.Controller.ViewBag.LanguageId = ReadLanguageId(context.RequestContext.HttpContext.Request.Cookies);
        }

        private int ReadLanguageId(HttpCookieCollection cookies)
        {
            // https://stackoverflow.com/a/3140458/14357934
            //Assuming user comes back after several days. several < 365.
            //Read the cookie from Request.
            HttpCookie languageCookie = cookies["gradebook_cookie"];
            if (languageCookie == null)
            {
                //No cookie found or cookie expired.
                //Handle the situation here, Redirect the user or simply return;
                return 0;
            }
            //ok - cookie is found.
            //Gracefully check if the cookie has the key-value as expected.
            if (!string.IsNullOrEmpty(languageCookie.Values["language"]))
            {
                if (int.TryParse(languageCookie.Values["language"], out int ret))
                    return ret;
                //Yes language is found. Mission accomplished.
            }
            return 0;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            // Do something after the result executes.
        }
    }
}