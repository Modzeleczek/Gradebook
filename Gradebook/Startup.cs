using Gradebook.Models;
using Microsoft.Owin;
using Owin;
using System.Globalization;
using System.Threading;

[assembly: OwinStartupAttribute(typeof(Gradebook.Startup))]
namespace Gradebook
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            var cultBackup = Thread.CurrentThread.CurrentCulture;
            /* Na czas tworzenia bazy danych zmieniamy kulturę, aby zapobiec wyrzucaniu przez
            Database.CreateIfNotExists() wyjątku "Input string was not in a correct format.",
            który może być powodowany np. przez używanie ',' zamiast '.' jako separatora dziesiętnego
            w programie Gradebook. */
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-Us");
            ApplicationDbContext.Create().Database.CreateIfNotExists();
            Thread.CurrentThread.CurrentCulture = cultBackup;
        }
    }
}
