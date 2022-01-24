using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Gradebook.Startup))]
namespace Gradebook
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
