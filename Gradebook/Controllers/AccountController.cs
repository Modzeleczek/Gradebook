using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Gradebook.Models;
using Gradebook.Utils;
using System.Collections.Generic;
using Gradebook.Models.ViewModels;

namespace Gradebook.Controllers
{
    [Authorize, ViewFilter]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext Db = ApplicationDbContext.Create();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
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

        public struct RoleUsers
        {
            public string RoleName;
            public List<ApplicationUser> Users;
            public RoleUsers(string roleName)
            {
                RoleName = roleName;
                Users = new List<ApplicationUser>();
            }
        }

        // GET: Account/Index
        [Authorize(Roles = Role.Administrator)]
        public ActionResult Index()
        {
            var roles = Db.Roles.ToArray();
            var users = Db.Users.ToArray();
            var roleUsers = new RoleUsers[roles.Length];
            int i = 0;
            foreach (var role in roles)
            {
                roleUsers[i] = new RoleUsers(role.Name);
                var roleUserIds = role.Users.Select(e => e.UserId);
                foreach (var userId in roleUserIds)
                {
                    var user = users.Where(e => e.Id == userId).Single();
                    roleUsers[i].Users.Add(user);
                }
                ++i;
            }
            for (i = 0; i < roleUsers.Length; ++i)
            {
                var ru = roleUsers[i];
                var orderedUsers = ru.Users.OrderBy(e => e.Surname);
                roleUsers[i].Users = orderedUsers.ToList();
            }
            return View(roleUsers);
        }

        /*// GET: Account/Details/5
        [Authorize(Roles = Role.Administrator)]
        public ActionResult Details(string id)
        {
            return View();
        }*/

        // GET
        [Authorize(Roles = Role.Administrator)]
        public ActionResult Create()
        {
            return RedirectToAction("Register");
        }

        // GET
        [Authorize(Roles = Role.Administrator)]
        public ActionResult AdminResetPassword(string id)
        {
            var newPassword = System.Web.Security.Membership.GeneratePassword(10, 4);
            var token = UserManager.GeneratePasswordResetToken(id);
            UserManager.ResetPasswordAsync(id, token, newPassword);
            var userName = Db.Users.Where(e => e.Id == id).Single().UserName;
            return RedirectToAction("LoginDetails", new { userName = userName, password = newPassword });
        }

        // GET: Account/Edit/5
        [Authorize(Roles = Role.Administrator)]
        public ActionResult Edit(string id)
        {
            var roleId = Db.Users.Where(e => e.Id == id).Single().Roles.Single().RoleId;
            var roleName = Db.Roles.Where(e => e.Id == roleId).Single().Name;
            switch (roleName)
            {
                case Role.Student:
                    return RedirectToAction("EditStudent", new { id = id });
                default:
                    return RedirectToAction("EditOther", new { id = id });
            }
        }

        private LinkedList<SelectListItem> GetParents()
        {
            var records = Db.Parent.Select(r => new { r.Id, r.ApplicationUser.Name, r.ApplicationUser.Surname, r.ApplicationUser.Email });
            var list = new LinkedList<SelectListItem>();
            list.AddLast(new SelectListItem { Text = "null", Value = "null", Selected = false });
            foreach (var r in records)
                list.AddLast(new SelectListItem { Text = $"{r.Name} {r.Surname} | {r.Email}", Value = r.Id, Selected = false });
            return list;
        }

        private void CreateViewBagParents(string parentId)
        {
            var parents = GetParents();
            ViewBag.Parents = parents;
            if (parents.First == null)
                return;
            if (parentId == null)
                parents.First.Value.Selected = true;
            else
                foreach (var p in parents)
                    if (p.Value == parentId)
                    {
                        p.Selected = true;
                        break;
                    }
        }

        [Authorize(Roles = Role.Administrator)]
        public ActionResult EditStudent(string id)
        {
            var s = Db.Student.Where(e => e.Id == id).Single();
            var svm = new StudentViewModel(s.Id, s.ApplicationUser.Name, s.ApplicationUser.Surname, s.ApplicationUser.Email, s.ApplicationUser.PhoneNumber, s.ParentId);
            CreateViewBagParents(s.ParentId);
            return View(svm);
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public ActionResult EditStudent(StudentViewModel student)
        {
            if (ModelState.IsValid == false)
            {
                CreateViewBagParents(student.ParentId);
                return View(student);
            }
            var record = Db.Student.Where(e => e.Id == student.Id).Single();
            record.ApplicationUser.Name = student.Name;
            record.ApplicationUser.Surname = student.Surname;
            record.ApplicationUser.Email = student.Email;
            record.ApplicationUser.UserName = student.Email;
            record.ApplicationUser.PhoneNumber = student.PhoneNumber;
            record.ParentId = student.ParentId == "null" ? null : student.ParentId;
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Role.Administrator)]
        public ActionResult EditOther(string id)
        {
            var u = Db.Users.Where(e => e.Id == id).Single();
            var ovm = new OtherViewModel(u.Id, u.Name, u.Surname, u.Email, u.PhoneNumber);
            return View(ovm);
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public ActionResult EditOther(OtherViewModel user)
        {
            if (ModelState.IsValid == false)
                return View(user);
            var emails = Db.Users.Where(e => e.Id != user.Id).Select(e => e.Email).ToArray();
            if (emails.Contains(user.Email))
                return View(user);
            var record = Db.Users.Where(e => e.Id == user.Id).Single();
            record.Name = user.Name;
            record.Surname = user.Surname;
            record.Email = user.Email;
            record.UserName = user.Email;
            record.PhoneNumber = user.PhoneNumber;
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        /* // GET: Account/Delete/5
        [Authorize(Roles = Role.Administrator)]
        public ActionResult Delete(string id)
        {
            return View();
        }

        // POST: Account/Delete/5
        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public ActionResult Delete(string id, FormCollection collection)
        {
            return View();
        } */

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        private LinkedList<SelectListItem> GetRoles()
        {
            var records = Db.Roles.Select(r => new { r.Name });
            var list = new LinkedList<SelectListItem>();
            foreach (var r in records)
                list.AddLast(new SelectListItem { Text = r.Name, Value = r.Name, Selected = false });
            return list;
        }

        //
        // GET: /Account/Register
        [Authorize(Roles = Role.Administrator)]
        public ActionResult Register()
        {
            var roles = GetRoles();
            if (roles.First != null)
                roles.First.Value.Selected = true;
            ViewBag.Roles = roles;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Role.Administrator)]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { Name = model.Name, Surname = model.Surname, UserName = model.Email, Email = model.Email, PhoneNumber = model.PhoneNumber };
                string password = System.Web.Security.Membership.GeneratePassword(10, 4);
                var result = await UserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    var im = new IdentityManager();
                    im.AddUserToRole(user.Id, model.RoleName);
                    switch (model.RoleName)
                    {
                        case Role.Administrator:
                            Db.Administrator.Add(new Administrator { Id = user.Id });
                            break;
                        case Role.Teacher:
                            Db.Teacher.Add(new Teacher { Id = user.Id });
                            break;
                        case Role.Parent:
                            Db.Parent.Add(new Parent { Id = user.Id });
                            break;
                        case Role.Student:
                            Db.Student.Add(new Student { Id = user.Id });
                            break;
                    }
                    Db.SaveChanges();

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("LoginDetails", new { userName = user.UserName, password = password });
                }
                AddErrors(result);
            }

            var roles = GetRoles();
            foreach (var r in roles)
                if (r.Value == model.RoleName)
                {
                    r.Selected = true;
                    break;
                }
            ViewBag.Roles = roles;
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET
        [Authorize(Roles = Role.Administrator)]
        public ActionResult LoginDetails(string userName, string password)
        {
            ViewBag.UserName = userName;
            ViewBag.Password = password;
            return View();
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
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

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}