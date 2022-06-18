using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Admin.Controllers
{
    [Authorize(Roles = Role.Administrator), ViewFilter]
    public class AccountController : ControllerBase
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController() { }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        public struct RoleUsers
        {
            public string RoleName;
            public LinkedList<ApplicationUser> Users;
            public RoleUsers(string roleName)
            {
                RoleName = roleName;
                Users = new LinkedList<ApplicationUser>();
            }
        }

        public ActionResult List()
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
                    roleUsers[i].Users.AddLast(user);
                }
                ++i;
            }
            ViewBag.UserId = User.Identity.GetUserId();
            return View(roleUsers);
        }

        public ActionResult Create()
        {
            return View();
        }

        public JsonResult GetRoles()
        {
            var d = LocalizedStrings.Account.Create[LanguageCookie.Read(Request.Cookies)];
            var list = new LinkedList<object>();
            list.AddLast(new { Id = 0, Name = d["Administrator"] });
            list.AddLast(new { Id = 1, Name = d["Teacher"] });
            list.AddLast(new { Id = 2, Name = d["Parent"] });
            list.AddLast(new { Id = 3, Name = d["Student"] });
            return Json(list);
        }

        private bool IsValidEmail(string email)
        {
            // https://stackoverflow.com/a/1374644/14357934
            var trimmedEmail = email.Trim();
            if (trimmedEmail.EndsWith(".")) return false;
            try { var addr = new System.Net.Mail.MailAddress(email); return addr.Address == trimmedEmail; }
            catch { return false; }
        }

        private bool IsValidOrEmptyPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Length == 0) return true;
            if (phoneNumber.Length != 9) return false;
            for (int i = 0; i < phoneNumber.Length; ++i) if (phoneNumber[i] < '0' || phoneNumber[i] > '9') return false;
            return true;
        }

        [HttpPost]
        public async Task<ActionResult> Create(string name, string surname, string email, string phoneNumber, int? roleId)
        {
            var d = LocalizedStrings.Account.Create[LanguageCookie.Read(Request.Cookies)];
            var error = false;
            if (string.IsNullOrWhiteSpace(name)) { ViewBag.ValidationMessage = d["Specify name."]; error = true; }
            else if (string.IsNullOrWhiteSpace(surname))
            { ViewBag.ValidationMessage = d["Specify surname."]; error = true; }
            else if (!IsValidEmail(email)) { ViewBag.ValidationMessage = d["Specify email."]; error = true; }
            else if (Db.Users.Any(e => e.Email == email))
            { ViewBag.ValidationMessage = d["Account with such email already exists."]; error = true; }
            else if (!roleId.HasValue) { ViewBag.ValidationMessage = d["Select type."]; error = true; }
            else if (!IsValidOrEmptyPhoneNumber(phoneNumber))
            { ViewBag.ValidationMessage = d["Specify phone number consisting of 9 digits."]; error = true; }
            var user = new ApplicationUser { Name = name, Surname = surname, UserName = email, Email = email,
                PhoneNumber = phoneNumber };
            if (error)
            {
                ViewBag.SelectedRoleId = roleId.HasValue ? roleId.Value : -1;
                return View(user);
            }
            string password = System.Web.Security.Membership.GeneratePassword(10, 4);
            var result = await UserManager.CreateAsync(user, password);
            if (!result.Succeeded)
            { ViewBag.ValidationMessage = d["Failed to create the account."]; return View(user); }
            var im = new IdentityManager();
            switch (roleId.Value)
            {
                case 0: // administrator
                    Db.Administrator.Add(new Models.Administrator { Id = user.Id });
                    im.AddUserToRole(user.Id, Role.Administrator);
                    break;
                case 1: // teacher
                    Db.Teacher.Add(new Models.Teacher { Id = user.Id });
                    im.AddUserToRole(user.Id, Role.Teacher);
                    break;
                case 2: // parent
                    Db.Parent.Add(new Models.Parent { Id = user.Id });
                    im.AddUserToRole(user.Id, Role.Parent);
                    break;
                case 3: // student
                    Db.Student.Add(new Models.Student { Id = user.Id });
                    im.AddUserToRole(user.Id, Role.Student);
                    break;
            }
            Db.SaveChanges();
            ViewBag.UserName = user.UserName;
            ViewBag.Password = password;
            return View("LoginDetails");
        }

        public ActionResult ResetPassword(string id)
        {
            var userSearch = Db.Users.Where(e => e.Id == id);
            if (userSearch.Count() != 1) return ErrorView("Such account does not exist.");
            var newPassword = System.Web.Security.Membership.GeneratePassword(10, 4);
            var token = UserManager.GeneratePasswordResetToken(id);
            UserManager.ResetPasswordAsync(id, token, newPassword);
            var userName = Db.Users.Where(e => e.Id == id).Single().UserName;
            ViewBag.UserName = userName;
            ViewBag.Password = newPassword;
            return View("LoginDetails");
        }

        public ActionResult Edit(string id)
        {
            var userSearch = Db.Users.Where(e => e.Id == id);
            if (userSearch.Count() != 1) return ErrorView("Such account does not exist.");
            var user = userSearch.Single();
            var roleSearch = user.Roles;
            if (roleSearch.Count != 1) return ErrorView("This account has more than 1 type.");
            var roleId = roleSearch.Single().RoleId;
            var roleName = Db.Roles.Where(e => e.Id == roleId).Single().Name;
            if (roleName == Role.Student)
            {
                var studentSearch = Db.Student.Where(e => e.Id == id);
                if (studentSearch.Count() != 1) return ErrorView("Such student does not exist.");
                ViewBag.SelectedParentId = studentSearch.Single().ParentId;
                return View("EditStudent", user);
            }
            else return View("EditOther", user);
        }

        [HttpPost]
        public ActionResult Edit(string id, string name, string surname, string email, string phoneNumber, string parentId)
        {
            var userSearch = Db.Users.Where(e => e.Id == id);
            if (userSearch.Count() != 1) return ErrorView("Such account does not exist.");
            var user = userSearch.Single();
            var roleSearch = user.Roles;
            if (roleSearch.Count != 1) return ErrorView("This account has more than 1 type.");
            var roleId = roleSearch.Single().RoleId;
            var roleName = Db.Roles.Where(e => e.Id == roleId).Single().Name;
            if (roleName == Role.Student)
            {
                var studentSearch = Db.Student.Where(e => e.Id == id);
                if (studentSearch.Count() != 1) return ErrorView("Such student does not exist.");
                var student = studentSearch.Single();
                if (!ValidateEdition(id, name, surname, email, phoneNumber))
                {
                    ViewBag.SelectedParentId = student.ParentId;
                    return View("EditStudent", user);
                }
                student.ApplicationUser.Name = name;
                student.ApplicationUser.Surname = surname;
                student.ApplicationUser.Email = email;
                student.ApplicationUser.UserName = email;
                student.ApplicationUser.PhoneNumber = phoneNumber;
                if (string.IsNullOrWhiteSpace(parentId)) student.ParentId = null;
                else
                {
                    if (!Db.Parent.Any(e => e.Id == parentId))
                        return ErrorView("Such parent does not exist.");
                    student.ParentId = parentId;
                }
            }
            else
            {
                if (!ValidateEdition(id, name, surname, email, phoneNumber)) return View("EditOther", user);
                user.Name = name;
                user.Surname = surname;
                user.Email = email;
                user.UserName = email;
                user.PhoneNumber = phoneNumber;
            }
            Db.SaveChanges();
            return RedirectToAction("List");
        }

        private bool ValidateEdition(string id, string name, string surname, string email, string phoneNumber)
        {
            var d = LocalizedStrings.Account.Edit[LanguageCookie.Read(Request.Cookies)];
            if (string.IsNullOrWhiteSpace(name))
            { ViewBag.ValidationMessage = d["Specify name."]; return false; }
            else if (string.IsNullOrWhiteSpace(surname))
            { ViewBag.ValidationMessage = d["Specify surname."]; return false; }
            else if (!IsValidEmail(email))
            { ViewBag.ValidationMessage = d["Specify valid email."]; return false; }
            else if (Db.Users.Any(e => e.Email == email && e.Id != id))
            { ViewBag.ValidationMessage = d["Account with such email already exists."]; return false; }
            else if (!IsValidOrEmptyPhoneNumber(phoneNumber))
            { ViewBag.ValidationMessage = d["Specify phone number consisting of 9 digits."]; return false; }
            return true;
        }

        public JsonResult GetParents()
        {
            var parents = Db.Parent.ToArray();
            var list = new LinkedList<object>();
            foreach (var p in parents)
                list.AddLast(new { Id = p.Id, Name = p.ApplicationUser.Name, Surname = p.ApplicationUser.Surname, Email = p.ApplicationUser.Email });
            return Json(list);
        }

        public async Task<ActionResult> Delete(string id)
        {
            var userId = User.Identity.GetUserId();
            if (id == userId) return ErrorView("You cannot delete your own account.");
            var userSearch = Db.Users.Where(e => e.Id == id);
            if (userSearch.Count() != 1) return ErrorView("Such account does not exist.");
            var roleSearch = userSearch.Single().Roles;
            if (roleSearch.Count != 1) return ErrorView("This account has more than 1 type.");
            var roleId = roleSearch.Single().RoleId;
            var roleName = Db.Roles.Where(e => e.Id == roleId).Single().Name;
            switch (roleName)
            {
                case Role.Administrator:
                    // var user = await UserManager.FindByIdAsync(id);
                    /* usuwamy jego rekordy w tabeli MessageRecipient
                    jego wiadomości lub ustawiamy null jako id nadawcy w jego wiadomościach */
                    Db.MessageRecipient.Where(e => e.RecipientId == id).DeleteFromQuery();
                    Db.Message.Where(e => e.SenderId == id).UpdateFromQuery(e => new Message { SenderId = null });
                    Db.Administrator.Where(e => e.Id == id).DeleteFromQuery();
                    break;
                case Role.Teacher:
                    /* usuwamy jego rekordy w tabeli MessageRecipient
                    jego wiadomości lub ustawiamy null jako id nadawcy w jego wiadomościach
                    nieobecności na lekcjach
                    lekcje tcsów
                    zdarzenia tcsów
                    tcsy, czyli przedmioty, których uczy w klasach
                    pliki do przedmiotów
                    oceny
                    ustawiamy null klasie, której jest wychowawcą
                    quizy */
                    Db.MessageRecipient.Where(e => e.RecipientId == id).DeleteFromQuery();
                    Db.Message.Where(e => e.SenderId == id).UpdateFromQuery(e => new Message { SenderId = null });
                    Db.Absence.Where(e => e.Lesson.TeacherClassSubject.TeacherId == id).DeleteFromQuery();
                    Db.Lesson.Where(e => e.TeacherClassSubject.TeacherId == id).DeleteFromQuery();
                    Db.Appointment.Where(e => e.TeacherClassSubject.TeacherId == id).DeleteFromQuery();
                    Db.TeacherClassSubject.Where(e => e.TeacherId == id).DeleteFromQuery();
                    Db.File.Where(e => e.TeacherId == id).DeleteFromQuery();
                    Db.Grade.Where(e => e.TeacherId == id).DeleteFromQuery();
                    Db.Class.Where(e => e.SupervisorId == id).UpdateFromQuery(e => new Class { SupervisorId = null });
                    Db.QuizSharing.Where(e => e.Quiz.AuthorId == id).DeleteFromQuery();
                    Db.ClosedQuestionAnswer.Where(e => e.QuizAttempt.Quiz.AuthorId == id).DeleteFromQuery();
                    Db.QuizAttempt.Where(e => e.Quiz.AuthorId == id).DeleteFromQuery();
                    Db.ClosedQuestionOption.Where(e => e.ClosedQuestion.Quiz.AuthorId == id).DeleteFromQuery();
                    Db.ClosedQuestion.Where(e => e.Quiz.AuthorId == id).DeleteFromQuery();
                    Db.Quiz.Where(e => e.AuthorId == id).DeleteFromQuery();
                    Db.Teacher.Where(e => e.Id == id).DeleteFromQuery();
                    break;
                case Role.Parent:
                    /* usuwamy jego rekordy w tabeli MessageRecipient
                    jego wiadomości lub ustawiamy null jako id nadawcy w jego wiadomościach
                    ustawiamy null jako id rodzica w jego dzieciach */
                    Db.MessageRecipient.Where(e => e.RecipientId == id).DeleteFromQuery();
                    Db.Message.Where(e => e.SenderId == id).UpdateFromQuery(e => new Message { SenderId = null });
                    Db.Student.Where(e => e.ParentId == id).UpdateFromQuery(e => new Models.Student { ParentId = null });
                    Db.Parent.Where(e => e.Id == id).DeleteFromQuery();
                    break;
                case Role.Student:
                    /* usuwamy jego rekordy w tabeli MessageRecipient
                    jego wiadomości lub ustawiamy null jako id nadawcy w jego wiadomościach
                    oceny
                    nieobecności
                    odpowiedzi w pytaniach w quizach
                    podjeścia do quizów */
                    Db.MessageRecipient.Where(e => e.RecipientId == id).DeleteFromQuery();
                    Db.Message.Where(e => e.SenderId == id).UpdateFromQuery(e => new Message { SenderId = null });
                    Db.Grade.Where(e => e.StudentId == id).DeleteFromQuery();
                    Db.Absence.Where(e => e.StudentId == id).DeleteFromQuery();
                    Db.ClosedQuestionAnswer.Where(e => e.QuizAttempt.DoerId == id).DeleteFromQuery();
                    Db.QuizAttempt.Where(e => e.DoerId == id).DeleteFromQuery();
                    Db.Student.Where(e => e.Id == id).DeleteFromQuery();
                    break;
                default:
                    return ErrorView("This account does not have any known type.");
            }
            var userManager = UserManager;
            var user = await userManager.FindByIdAsync(id); // aby usunąć konto, musimy je pobrać z UserManagera
            var logins = user.Logins;
            var rolesForUser = await userManager.GetRolesAsync(id);
            using (var transaction = Db.Database.BeginTransaction())
            {
                foreach (var login in logins.ToList())
                    await userManager.RemoveLoginAsync(login.UserId, new UserLoginInfo(login.LoginProvider,
                        login.ProviderKey));
                IdentityResult result;
                if (rolesForUser.Count() > 0)
                    foreach (var item in rolesForUser.ToList())
                    {
                        // item should be the name of the role
                        result = await userManager.RemoveFromRoleAsync(user.Id, item);
                        if (!result.Succeeded) return ErrorView("Failed to delete account role.");
                    }
                result = await userManager.DeleteAsync(user);
                if (!result.Succeeded) return ErrorView("Failed to delete the account.");
                transaction.Commit();
            }
            return RedirectToAction("List");
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
