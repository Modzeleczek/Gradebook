using System.Web.Mvc;

namespace Gradebook.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName { get { return "Admin"; } }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            /* If we do not pass default controller and action values in the 3rd
            argument, a complete route must always be specified when navigating
            the application. */
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}"
            );
        }
    }
}