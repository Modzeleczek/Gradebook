using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Gradebook.Models;
using Gradebook.Utils;
using System.Linq;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Controllers
{
    [ViewFilter]
    public class AccountController : ControllerBase
    {
        #region Helpers
        private IAuthenticationManager AuthenticationManager
        { get { return HttpContext.GetOwinContext().Authentication; } }
        #endregion

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController() { }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }
            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        [AllowAnonymous]
        public async Task<ActionResult> CreateRolesAndAccounts(string password)
        {
            if (password != "caviaporcellus")
                return RedirectToAction("Index", "Home");
            IdentityManager im = new IdentityManager();
            im.CreateRole(Role.Administrator);
            im.CreateRole(Role.Teacher);
            im.CreateRole(Role.Parent);
            im.CreateRole(Role.Student);

            string administatorEmail = "a.adminowski@szkola.pl", teacherEmail = "j.kowalski@szkola.pl";
            ApplicationUser administrator, teacher;

            administrator = new ApplicationUser { Name = "Admin", Surname = "Adminowski", UserName = administatorEmail, Email = administatorEmail, PhoneNumber = "000000001" };
            var result = await UserManager.CreateAsync(administrator, "administrator123");
            if (result.Succeeded)
                Db.Administrator.Add(new Administrator { Id = administrator.Id });
            teacher = new ApplicationUser { Name = "Jan", Surname = "Kowalski", UserName = teacherEmail, Email = teacherEmail, PhoneNumber = "123456789" };
            result = await UserManager.CreateAsync(teacher, "teacher123");
            if (result.Succeeded)
                Db.Teacher.Add(new Teacher { Id = teacher.Id });
            Db.SaveChanges();

            im.AddUserToRoleByUsername(administatorEmail, Role.Administrator);
            im.AddUserToRoleByUsername(teacherEmail, Role.Teacher);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string email, string password, string returnUrl)
        {
            var d = LocalizedStrings.Account.Login[LanguageCookie.Read(Request.Cookies)];
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(email, password, false, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    if (Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                    return RedirectToAction("Index", "Home");
                case SignInStatus.Failure:
                default:
                    ViewBag.ValidationMessage = d["Invalid login attempt."];
                    return View(new LoginViewModel { Email = email, Password = password });
            }
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            var user = await UserManager.FindByNameAsync(email);
            if (user == null) // || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return View("ForgotPasswordConfirmation");
            }
            // Send an email with this link
            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id); // code - token; identyfikator sesji resetowania hasła
            var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            var d = LocalizedStrings.Account.ForgotPassword[LanguageCookie.Read(Request.Cookies)];
            EmailSender.Send("noreply@gradebook.com", user, d["Reset password"], d["Please reset your password by clicking"] + " <a href=\"" + callbackUrl + "\">" + d["here"] + "</a>", null, true);
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return ErrorView("You have provided no token.");
            // jeżeli konto nie istnieje, to tego nie ujawniamy
            return View(new ResetPasswordViewModel { UserId = userId, Code = code });
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(string userId, string code, string password, string confirmPassword)
        {
            var vm = new ResetPasswordViewModel { UserId = userId, Code = code, Password = password, ConfirmPassword = confirmPassword };
            if (string.IsNullOrWhiteSpace(code)) return ErrorView("You have provided no token.");
            var d = LocalizedStrings.Account.ResetPassword[LanguageCookie.Read(Request.Cookies)];
            if (password.Length < 6)
            { ViewBag.ValidationMessage = d["Password must be at least 6 characters long."]; return View(vm); }
            if (confirmPassword != password)
            { ViewBag.ValidationMessage = d["Password and confirmation password do not match."]; return View(vm); }
            var user = await UserManager.FindByIdAsync(userId);
            if (user != null) // nie ujawniamy, że użytkownik nie istnieje
                await UserManager.ResetPasswordAsync(userId, code, password);
            return RedirectToAction("ResetPasswordConfirmation"); // niezależnie od wyniku przechodzimy dalej, aby nie ujawniać, że podano token code niewygenerowany wcześniej dla konta o id równym userId
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [Authorize]
        public ActionResult Details()
        {
            var userId = User.Identity.GetUserId();
            var userSearch = Db.Users.Where(e => e.Id == userId);
            if (userSearch.Count() != 1) return ErrorView("Your account does not exist.");
            return View(userSearch.Single());
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            var d = LocalizedStrings.Account.ChangePassword[LanguageCookie.Read(Request.Cookies)];
            var vm = new ChangePasswordViewModel { OldPassword = oldPassword, NewPassword = newPassword, ConfirmPassword = confirmPassword };
            if (newPassword.Length < 6)
            { ViewBag.ValidationMessage = d["New password must be at least 6 characters long."]; return View(vm); }
            if (confirmPassword != newPassword)
            { ViewBag.ValidationMessage = d["New password and confirmation password do not match."]; return View(vm); }
            var userId = User.Identity.GetUserId();
            var result = await UserManager.ChangePasswordAsync(userId, oldPassword, newPassword);
            if (!result.Succeeded)
            { ViewBag.ValidationMessage = d["Failed to change the password."]; return View(vm); }
            var user = await UserManager.FindByIdAsync(userId);
            if (user != null)
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            return RedirectToAction("ChangePasswordConfirmation");
        }

        [Authorize]
        public ActionResult ChangePasswordConfirmation()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
