using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Teacher.Controllers
{
    [Authorize(Roles = Role.Teacher), ViewFilter]
    public class SubjectController : ControllerBase
    {
        public ActionResult List()
        {
            var teacherId = User.Identity.GetUserId();
            var teacher = Db.Teacher.Where(e => e.Id == teacherId).Single();
            var teacherClassSubjects = teacher.TeacherClassSubjects.Distinct(new ComparerBySubject());
            var teacherSubjects = teacherClassSubjects.Select(e => e.Subject);
            var subjects = teacherSubjects.ToArray();
            return View(subjects);
        }

        public ActionResult Details(int? id)
        {
            var userId = User.Identity.GetUserId();
            var tcsSearch = Db.TeacherClassSubject.Where(e => e.TeacherId == userId && e.SubjectId == id);
            if (tcsSearch.Count() == 0) return ErrorView("You do not teach such subject.");
            var s = tcsSearch.First().Subject;
            var files = Db.File.Where(e => e.TeacherId == userId && e.SubjectId == id).ToArray();
            ViewBag.Files = files;
            return View(s);
        }

        public ActionResult AddFile(int? id) // id - fileId
        {
            var userId = User.Identity.GetUserId();
            var tcsSearch = Db.TeacherClassSubject.Where(e => e.TeacherId == userId && e.SubjectId == id);
            if (tcsSearch.Count() == 0) return ErrorView("You do not teach such subject.");
            ViewBag.SubjectId = id;
            return View();
        }

        [HttpPost]
        public ActionResult AddFile(int? id, string description, HttpPostedFileBase attachedFile) // id - fileId
        {
            var userId = User.Identity.GetUserId();
            var tcsSearch = Db.TeacherClassSubject.Where(e => e.TeacherId == userId && e.SubjectId == id);
            if (tcsSearch.Count() == 0) return ErrorView("You do not teach such subject.");
            if (attachedFile == null)
            {
                var d = LocalizedStrings.Subject.AddFile[LanguageCookie.Read(Request.Cookies)];
                ViewBag.ValidationMessage = d["Upload a file."];
                ViewBag.SubjectId = id;
                return View();
            }
            if (description == null) description = "";
            var file = new File();
            file.Name = attachedFile.FileName;
            file.Description = description;
            file.TeacherId = userId;
            file.SubjectId = id.Value;
            file.FileType = attachedFile.ContentType;
            file.ModificationTime = DateTime.Now;
            file.Data = FileType.StreamToHexString(attachedFile.InputStream);
            Db.File.Add(file);
            Db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        public ActionResult DeleteFile(int id) // id - fileId
        {
            var userId = User.Identity.GetUserId();
            var search = Db.File.Where(e => e.Id == id && e.TeacherId == userId);
            if (search.Count() != 1) return ErrorView("You do not own such file.");
            var file = search.Single();
            var subjectId = file.SubjectId;
            Db.File.Remove(file);
            Db.SaveChanges();
            return RedirectToAction("Details", new { id = subjectId });
        }

        public ActionResult DownloadFile(int? id) // id - fileId
        {
            var userId = User.Identity.GetUserId();
            var search = Db.File.Where(e => e.Id == id && e.TeacherId == userId);
            if (search.Count() != 1) return ErrorView("You do not own such file.");
            var file = search.Single();
            var hex = file.Data;
            var type = file.FileType;
            return File(FileType.HexStringToByteArray(hex), type, file.Name);
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
    }
}
