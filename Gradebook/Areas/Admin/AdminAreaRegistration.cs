using System.Web.Mvc;

namespace Gradebook.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName { get { return "Admin"; } }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            // bez przekazanych do MapRoute jako 3 argument domyślnych wartości controller i action cała ścieżka zawsze musi być określona
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}"
            );
        }
    }
}