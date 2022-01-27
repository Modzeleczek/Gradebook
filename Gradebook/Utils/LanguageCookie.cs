using System;
using System.Web;

namespace Gradebook.Utils
{
    public class LanguageCookie
    {
        public static void Save(HttpCookieCollection cookies, int value)
        {
            // https://stackoverflow.com/a/3140458/14357934
            //create a cookie
            HttpCookie myCookie = new HttpCookie("gradebook_cookie");
            //Add key-values in the cookie
            myCookie.Values.Add("language", value.ToString());
            //set cookie expiry date-time. Made it to last for next 12 hours.
            myCookie.Expires = DateTime.Now.AddYears(1);
            //Most important, write the cookie to client.
            cookies.Add(myCookie);
        }

        public static int Read(HttpCookieCollection cookies)
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
    }
}