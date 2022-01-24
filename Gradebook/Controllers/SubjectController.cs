using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;

namespace Gradebook.Controllers
{
    [Authorize, ViewFilter]
    public class SubjectController : Controller
    {
        private ApplicationDbContext Db = ApplicationDbContext.Create();

        [Authorize(Roles = Role.AdministratorTeacherStudent)]
        public ActionResult Index()
        {
            Subject[] subjects = null;
            if (User.IsInRole(Role.Administrator))
            {
                subjects = Db.Subject.ToArray();
            }
            else if (User.IsInRole(Role.Teacher))
            {
                var teacherId = User.Identity.GetUserId();
                var teacher = Db.Teacher.Where(e => e.Id == teacherId).Single();
                var teacherClassSubjects = teacher.TeacherClassSubjects.Distinct(new ComparerBySubject());
                var teacherSubjects = teacherClassSubjects.Select(e => e.Subject);
                subjects = teacherSubjects.ToArray();
            }
            else // if (User.IsInRole(Role.Student))
            {
                var studentId = User.Identity.GetUserId();
                var student = Db.Student.Where(e => e.Id == studentId).Single();
                subjects = student.Class.TeacherClassSubjects.Select(e => e.Subject).ToArray();
            }
            return View(subjects);
        }

        [Authorize(Roles = Role.AdministratorTeacherStudent)]
        public ActionResult Details(int id)
        {
            var subject = Db.Subject.Where(e => e.Id == id).Single();
            IEnumerable<File> files = null;
            if (User.IsInRole(Role.Teacher))
            {
                var teacherId = User.Identity.GetUserId();
                files = Db.File.Where(e => e.TeacherId == teacherId && e.SubjectId == id).ToArray();
            }
            else if (User.IsInRole(Role.Student))
            {
                var studentId = User.Identity.GetUserId();
                var student = Db.Student.Where(e => e.Id == studentId).Single();
                var tcs = student.Class.TeacherClassSubjects.Where(e => e.SubjectId == id).Single();
                var teacherId = tcs.TeacherId;
                files = Db.File.Where(e => e.TeacherId == teacherId && e.SubjectId == id).ToArray();
            }
            ViewBag.Files = files;
            return View(subject);
        }

        [Authorize(Roles = Role.Teacher)]
        public ActionResult AddFile(int id) // id - subjectId
        {
            ViewBag.SubjectId = id;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Role.Teacher)]
        public ActionResult AddFile(int id, File bound, HttpPostedFileBase attachedFile)
        {
            if (!ModelState.IsValid || (attachedFile == null && FileType.IsTypeSupported(attachedFile?.ContentType) == false))
            {
                ViewBag.SubjectId = id;
                return View();
            }
            var file = new Models.File();
            file.Name = attachedFile.FileName;
            file.Description = bound.Description;
            file.TeacherId = User.Identity.GetUserId();
            file.SubjectId = id;
            file.FileType = attachedFile.ContentType;
            file.ModificationTime = DateTime.Now;
            file.Data = FileType.StreamToHexString(attachedFile.InputStream);
            Db.File.Add(file);
            Db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        [Authorize(Roles = Role.Teacher)]
        public ActionResult DeleteFile(int id) // id - fileId
        {
            var file = Db.File.Where(e => e.Id == id).Single();
            var subjectId = file.SubjectId;
            Db.File.Remove(file);
            Db.SaveChanges();
            return RedirectToAction("Details", new { id = subjectId });
        }

        [Authorize(Roles = Role.AdministratorTeacherStudent)]
        public ActionResult DownloadFile(int id) // id - fileId
        {
            var file = Db.File.Where(e => e.Id == id).Single();
            var hex = file.Data;
            var type = file.FileType;
            return File(FileType.HexStringToByteArray(hex), type, file.Name);
        }

        [Authorize(Roles = Role.AdministratorTeacherStudent)]
        public ActionResult DownloadSyllabus(int id)
        {
            var subject = Db.Subject.Where(e => e.Id == id).Single();
            var hex = subject.Syllabus;
            var type = FileType.PDF;
            return File(FileType.HexStringToByteArray(hex), type, "Syllabus - " + subject.Name);
        }

        [Authorize(Roles = Role.Administrator)]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public ActionResult Create(Subject subject, HttpPostedFileBase attachedFile)
        {
            if (ModelState.IsValid == false)
                return View();
            if (attachedFile != null && attachedFile.ContentType == FileType.PDF) // administrator wybrał plik
                subject.Syllabus = FileType.StreamToHexString(attachedFile.InputStream);
            Db.Subject.Add(subject);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Role.Administrator)]
        public ActionResult Edit(int id)
        {
            var s = Db.Subject.Where(e => e.Id == id).Single();
            return View(s);
        }

        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public ActionResult Edit(int id, Subject subject, HttpPostedFileBase attachedFile)
        {
            if (ModelState.IsValid == false)
                return View();
            var record = Db.Subject.Where(e => e.Id == id).Single();
            record.Name = subject.Name;
            if (attachedFile != null && attachedFile.ContentType == FileType.PDF) // administrator wybrał plik
                record.Syllabus = FileType.StreamToHexString(attachedFile.InputStream);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Role.Administrator)]
        public ActionResult DeleteSyllabus(int id) // id przedmiotu
        {
            var subject = Db.Subject.Where(e => e.Id == id).Single();
            subject.Syllabus = null;
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = id });
        }

        [Authorize(Roles = Role.Administrator)]
        public ActionResult Delete(int id)
        {
            var s = Db.Subject.Where(e => e.Id == id).Single();
            var quizzes = Db.Quiz.Where(e => e.SubjectId == id);
            foreach (var q in quizzes)
                q.SubjectId = null;
            Db.SaveChanges();
            Db.Subject.Remove(s);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
