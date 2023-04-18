using Gradebook.Models;
using Gradebook.Utils;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Admin.Controllers
{
    [Authorize(Roles = Role.Administrator), ViewFilter]
    public class SubjectController : ControllerBase
    {
        public ActionResult List()
        {
            return View(Db.Subject.ToArray());
        }

        public ActionResult Details(int? id)
        {
            var search = Db.Subject.Where(e => e.Id == id);
            if (search.Count() != 1) return ErrorView("Such subject does not exist.");
            return View(search.Single());
        }

        public ActionResult DownloadSyllabus(int? id)
        {
            var d = LocalizedStrings.Subject.Details[LanguageCookie.Read(Request.Cookies)];
            var search = Db.Subject.Where(e => e.Id == id);
            if (search.Count() != 1) return ErrorView("Such subject does not exist.");
            var s = search.Single();
            var hex = s.Syllabus;
            var type = FileType.PDF;
            return File(FileType.HexStringToByteArray(hex), type, d["Syllabus"] + " - " + s.Name);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string name, HttpPostedFileBase attachedFile)
        {
            var d = LocalizedStrings.Subject.Create[LanguageCookie.Read(Request.Cookies)];
            if (string.IsNullOrEmpty(name)) { ViewBag.ValidationMessage = d["Specify a name."]; return View(); }
            var s = new Subject();
            s.Name = name;
            if (attachedFile != null) // Administrator chose a file.
            {
                if (attachedFile.ContentType != FileType.PDF) { ViewBag.ValidationMessage = d["Select a PDF file."]; return View(s); }
                s.Syllabus = FileType.StreamToHexString(attachedFile.InputStream);
            }
            Db.Subject.Add(s);
            Db.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult Edit(int? id)
        {
            var search = Db.Subject.Where(e => e.Id == id);
            if (search.Count() != 1) return ErrorView("Such subject does not exist.");
            return View(search.Single());
        }

        [HttpPost]
        public ActionResult Edit(int? id, string name, HttpPostedFileBase attachedFile)
        {
            var d = LocalizedStrings.Subject.Edit[LanguageCookie.Read(Request.Cookies)];
            var search = Db.Subject.Where(e => e.Id == id);
            if (search.Count() != 1) return ErrorView("Such subject does not exist.");
            var s = search.Single();
            if (string.IsNullOrEmpty(name)) { ViewBag.ValidationMessage = d["Specify a name."]; return View(s); }
            s.Name = name;
            if (attachedFile != null) // Administrator chose a file.
            {
                if (attachedFile.ContentType != FileType.PDF) { ViewBag.ValidationMessage = d["Select a PDF file."]; return View(s); }
                s.Syllabus = FileType.StreamToHexString(attachedFile.InputStream);
            }
            Db.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult DeleteSyllabus(int? id) // Subject id
        {
            var search = Db.Subject.Where(e => e.Id == id);
            if (search.Count() != 1) return ErrorView("Such subject does not exist.");
            var s = search.Single();
            s.Syllabus = null;
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = id });
        }

        public ActionResult Delete(int? id)
        {
            var search = Db.Subject.Where(e => e.Id == id);
            if (search.Count() != 1) return ErrorView("Such subject does not exist.");
            var s = search.Single();
            var quizzes = Db.Quiz.Where(e => e.SubjectId == id);
            foreach (var q in quizzes)
                q.SubjectId = null;
            Db.SaveChanges();
            Db.Subject.Remove(s);
            Db.SaveChanges();
            return RedirectToAction("List");
        }
    }
}
