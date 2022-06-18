using System.Linq;
using System.Web.Mvc;
using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Student.Controllers
{
    [Authorize(Roles = Role.Student), ViewFilter]
    public class SubjectController : ControllerBase
    {
        public ActionResult List()
        {
            var studentId = User.Identity.GetUserId();
            var student = Db.Student.Where(e => e.Id == studentId).Single();
            if (student.ClassId == null) return ErrorView("You do not belong to any class.");
            var subjects = student.Class.TeacherClassSubjects.Select(e => e.Subject).ToArray();
            return View(subjects);
        }

        public ActionResult Details(int? id)
        {
            var userId = User.Identity.GetUserId();
            var student = Db.Student.Where(e => e.Id == userId).Single();
            var tcsSearch = student.Class.TeacherClassSubjects.Where(e => e.SubjectId == id);
            if (tcsSearch.Count() != 1) return ErrorView("Your class does not have such subject.");
            var tcs = tcsSearch.Single();
            var teacherId = tcs.TeacherId;
            var files = Db.File.Where(e => e.TeacherId == teacherId && e.SubjectId == id).ToArray();
            ViewBag.Files = files;
            return View(tcs.Subject);
        }

        public ActionResult DownloadFile(int? id) // id - fileId
        {
            var fileSearch = Db.File.Where(e => e.Id == id);
            if (fileSearch.Count() != 1) return ErrorView("File does not exist.");
            var userId = User.Identity.GetUserId();
            var student = Db.Student.Where(e => e.Id == userId).Single();
            var file = fileSearch.Single();
            var tcsSearch = student.Class.TeacherClassSubjects.Where(e => e.SubjectId == file.SubjectId && e.TeacherId == file.TeacherId);
            if (tcsSearch.Count() != 1) return ErrorView("Your class does not have access to such file.");
            var hex = file.Data;
            var type = file.FileType;
            return File(FileType.HexStringToByteArray(hex), type, file.Name);
        }

        public ActionResult DownloadSyllabus(int? id)
        {
            var userId = User.Identity.GetUserId();
            var student = Db.Student.Where(e => e.Id == userId).Single();
            var tcsSearch = student.Class.TeacherClassSubjects.Where(e => e.SubjectId == id);
            if (tcsSearch.Count() != 1) return ErrorView("Your class does not have such subject.");
            var tcs = tcsSearch.Single();
            var subject = tcs.Subject;
            var hex = subject.Syllabus;
            var type = FileType.PDF;
            return File(FileType.HexStringToByteArray(hex), type, "Syllabus - " + subject.Name);
        }
    }
}
