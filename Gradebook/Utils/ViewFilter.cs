using System;
using System.Collections.Generic;
using System.Linq;
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
            context.Controller.ViewBag.LanguageId = 0;
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            // Do something after the result executes.
        }
    }
}