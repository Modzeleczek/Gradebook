using System.Text;
using System.Web;
using System.Web.Routing;

namespace Gradebook.Utils
{
    public static class PathSerializer
    {
        public static string Serialize(HttpRequestBase request)
        {
            var sb = new StringBuilder();
            // Save path of currently rendered page (e.g. /Products/List).
            sb.Append(request.Path);
            /* Query string contains parameters for the server and looks like
            ?parametr1=d&parametr2=5... */
            string queryString = request.QueryString.ToString();
            if (queryString.Length > 0) // If we have any parameters
            {
                if (request.Path.EndsWith("/") == false) // If path does not end with '/'
                    sb.Append('/'); // Append '/'.
                sb.Append('?');
                sb.Append(queryString); // Append parameters (e.g. ?id=2).
            }
            return sb.ToString();
        }

        public static Link Deserialize(string path)
        {
            string action = null, controller = null, area = "";
            RouteValueDictionary routeValues = new RouteValueDictionary();
            var substr = path.Substring(1); // Remove '/' from URL's beginning.
            if (substr.Length > 0)
            {
                var split = substr.Split('/'); // and only then split on '/'.
                switch (split.Length) // kontroler i akcja są obowiązkowe; jedyna sytuacja, kiedy nie są, to ścieżka /Home lub /Home/Index, na której jest natychmiastowy redirect do ogłoszeń
                {
                    case 2: // No (area, controller, action); no query string
                        controller = split[0];
                        action = split[1]; break;
                    case 3:
                        // No (area, controller, action); present query string
                        if (split[2][0] == '=')
                        {
                            controller = split[0];
                            action = split[1];
                            routeValues = ParseQueryString(split[2]);
                        }
                        // Present (area, controller, action); no query string
                        else
                        {
                            area = split[0];
                            controller = split[1];
                            action = split[2];
                        } break;
                    // Present (area, controller, action); present query string
                    case 4:
                        area = split[0];
                        controller = split[1];
                        action = split[2];
                        routeValues = ParseQueryString(split[3]); break;
                }
            }
            return new Link(action, controller, area, routeValues);
        }

        private static RouteValueDictionary ParseQueryString(string str)
        {
            var ret = new RouteValueDictionary();
            // Remove '?' and split on '&'.
            var keyValues = str.Substring(1).Split('&');
            for (int i = 0; i < keyValues.Length; ++i)
            {
                var keyValue = keyValues[i].Split('=');
                ret[keyValue[0]] = keyValue[1];
            }
            return ret;
        }
    }
}