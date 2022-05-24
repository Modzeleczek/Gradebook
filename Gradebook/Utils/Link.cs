using System.Web.Routing;

namespace Gradebook.Utils
{
    public class Link
    {
        public string Area, Action, Controller;
        public RouteValueDictionary RouteValues;

        public Link(string action, string controller, string area, RouteValueDictionary routeValues)
        {
            Action = action;
            Controller = controller;
            Area = area;
            RouteValues = routeValues;
        }
    }

    public class NamedLink : Link
    {
        public string Name;

        public NamedLink(string name, string action, string controller, string area, RouteValueDictionary routeValues = null)
            : base(action, controller, area, routeValues)
        {
            Name = name;
        }
    }
}