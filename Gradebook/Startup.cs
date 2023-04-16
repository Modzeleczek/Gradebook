using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
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
            if (ApplicationDbContext.Create().Database.CreateIfNotExists())
                CreateRolesAndAdminAccount();
            Thread.CurrentThread.CurrentCulture = cultBackup;
        }

        private void CreateRolesAndAdminAccount()
        {
            IdentityManager im = new IdentityManager();
            im.CreateRole(Role.Administrator);
            im.CreateRole(Role.Teacher);
            im.CreateRole(Role.Parent);
            im.CreateRole(Role.Student);

            // https://stackoverflow.com/a/25410859
            var db = ApplicationDbContext.Create();
            var store = new UserStore<ApplicationUser>(db);
            var manager = new ApplicationUserManager(store);
            string email = "a.adminowski@szkola.pl";
            var admin = new ApplicationUser { Name = "Admin", Surname = "Adminowski",
                UserName = email, Email = email, PhoneNumber = "000000001" };
            var result = manager.Create(admin, "administrator123");
            if (!result.Succeeded)
                throw new Exception("Could not create administrator account.");
            db.Administrator.Add(new Administrator { Id = admin.Id });
            db.SaveChanges();
            im.AddUserToRoleByUsername(email, Role.Administrator);
        }
    }
}
