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
            sb.Append(request.Path); // zapisujemy ścieżkę do aktualnie renderowanej strony (np. /Products/List)
            string queryString = request.QueryString.ToString(); // query string zawiera parametry do strony i ma postać ?parametr1=d&parametr2=5...
            if (queryString.Length > 0) // jeżeli mamy jakiekolwiek parametry do strony
            {
                if (request.Path.EndsWith("/") == false) // jeżeli ścieżka nie ma na końcu "/"
                    sb.Append('/'); // dopisujemy "/"
                sb.Append('?');
                sb.Append(queryString); // dopisujemy parametry (np. ?id=2)
            }
            return sb.ToString();
        }

        public static Link Deserialize(string path)
        {
            string action = null, controller = null, area = "";
            RouteValueDictionary routeValues = new RouteValueDictionary();
            var substr = path.Substring(1); // wyrzucamy '/' z początku URLa
            if (substr.Length > 0)
            {
                var split = substr.Split('/'); // i dopiero splitujemy po '/'
                switch (split.Length) // kontroler i akcja są obowiązkowe; jedyna sytuacja, kiedy nie są, to ścieżka /Home lub /Home/Index, na której jest natychmiastowy redirect do ogłoszeń
                {
                    case 2: // brak area, kontroler, akcja, brak query stringa
                        controller = split[0];
                        action = split[1]; break;
                    case 3:
                        if (split[2][0] == '=') // brak area, kontroler, akcja, query string
                        {
                            controller = split[0];
                            action = split[1];
                            routeValues = ParseQueryString(split[2]);
                        }
                        else // area, kontroler, akcja, brak query stringa
                        {
                            area = split[0];
                            controller = split[1];
                            action = split[2];
                        } break;
                    case 4: // area, kontroler, akcja, query string
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
            var keyValues = str.Substring(1).Split('&'); // usuwamy '?' i splitujemy po '&'
            for (int i = 0; i < keyValues.Length; ++i)
            {
                var keyValue = keyValues[i].Split('=');
                ret[keyValue[0]] = keyValue[1];
            }
            return ret;
        }
    }
}