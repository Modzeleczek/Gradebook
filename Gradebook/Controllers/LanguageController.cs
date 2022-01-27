using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gradebook.Controllers
{
    public class LanguageController : Controller
    {
        private void SaveLanguageId(int value)
        {
            // https://stackoverflow.com/a/3140458/14357934
            //create a cookie
            HttpCookie myCookie = new HttpCookie("gradebook_cookie");
            //Add key-values in the cookie
            myCookie.Values.Add("language", value.ToString());
            //set cookie expiry date-time. Made it to last for next 12 hours.
            myCookie.Expires = DateTime.Now.AddYears(1);
            //Most important, write the cookie to client.
            Response.Cookies.Add(myCookie);
        }

        public ActionResult SelectEnglish()
        {
            SaveLanguageId(0);
            return RedirectToAction("Index", "GlobalAnnouncement");
        }

        public ActionResult SelectPolish()
        {
            SaveLanguageId(1);
            return RedirectToAction("Index", "GlobalAnnouncement");
        }
    }
}