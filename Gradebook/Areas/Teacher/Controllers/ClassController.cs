using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Teacher.Controllers
{
    [Authorize(Roles = Role.Teacher), ViewFilter]
    public class ClassController : ControllerBase
    {
        public ActionResult List()
        {
            return View(Db.Class.ToArray());
        }

        public ActionResult Details(int? id)
        {
            var search = Db.Class.Where(e => e.Id == id);
            if (search.Count() != 1) return ErrorView("Such class does not exist.");
            var _class = search.Single();
            var userId = User.Identity.GetUserId();
            ViewBag.IsSupervisor = (_class.SupervisorId == userId);
            return View(_class);
        }

        public ActionResult CreateAnnouncement(int? id) // id - classId
        {
            var userId = User.Identity.GetUserId();
            var search = Db.Class.Where(e => e.Id == id && e.SupervisorId == userId);
            if (search.Count() != 1) return ErrorView("You are not supervisor of such class.");
            ViewBag.ClassId = id;
            return View();
        }

        [HttpPost]
        public ActionResult CreateAnnouncement(int? id, string button, string content, HttpPostedFileBase attachedFile)
        {
            var d = LocalizedStrings.Class.CreateAnnouncement[LanguageCookie.Read(Request.Cookies)];
            var userId = User.Identity.GetUserId();
            var search = Db.Class.Where(e => e.Id == id && e.SupervisorId == userId);
            if (search.Count() != 1) return ErrorView("You are not supervisor of such class.");
            if (string.IsNullOrEmpty(content))
            { ViewBag.ValidationMessage = d["Specify content."]; ViewBag.ClassId = id.Value; return View(); }
            if (button == "SendByWebsite")
                SendByWebsite(id.Value, content, attachedFile);
            else if (button == "SendByEmail")
                SendByEmail(id.Value, content, attachedFile);
            return RedirectToAction("Details", new { id = id });
        }

        private static Utils.Comparer<Models.Parent> ParentComparer = new Utils.Comparer<Models.Parent>(
            (x, y) => x.Id == y.Id, obj => obj.Id.GetHashCode());

        private void SendByWebsite(int classId, string content, HttpPostedFileBase attachedFile)
        {
            var message = new Message { Content = content };
            message.SenderId = User.Identity.GetUserId();
            message.SendTime = DateTime.Now;
            Db.Message.Add(message);
            Db.SaveChanges();
            var class_ = Db.Class.Where(e => e.Id == classId).Single();
            var recipients = class_.Students.Where(e => e.ParentId != null).Select(e => e.Parent).Distinct(ParentComparer);
            var recipientIds = recipients.Select(e => e.Id).ToList();
            foreach (var rid in recipientIds)
            {
                var messageRecipient = new MessageRecipient { MessageId = message.Id, RecipientId = rid };
                Db.MessageRecipient.Add(messageRecipient);
            }
            if (attachedFile != null) // nauczyciel wybrał plik
            {
                var attachment = new Attachment { MessageId = message.Id, Name = attachedFile.FileName, FileType = attachedFile.ContentType, Data = FileType.StreamToHexString(attachedFile.InputStream) };
                Db.Attachment.Add(attachment);
            }
            Db.SaveChanges();
        }

        private void SendByEmail(int classId, string content, HttpPostedFileBase attachedFile)
        {
            var teacherId = User.Identity.GetUserId();
            var teacher = Db.Teacher.Where(e => e.Id == teacherId).Single();
            var teacherEmail = teacher.ApplicationUser.Email;
            var class_ = Db.Class.Where(e => e.Id == classId).Single();
            var recipients = class_.Students.Where(e => e.ParentId != null).Select(e => e.Parent).Distinct(ParentComparer);
            if (recipients.Count() == 0) return;
            var recipientUsers = recipients.Select(e => e.ApplicationUser).ToArray();
            EmailSender.Send(teacherEmail, recipientUsers, $"Announcement from {teacher.ApplicationUser.Name} {teacher.ApplicationUser.Surname}", content, attachedFile, false);
        }

        public ActionResult GenerateGradeSheet(int? id) // id - classId
        {
            var d = LocalizedStrings.Class.GenerateGradeSheet[LanguageCookie.Read(Request.Cookies)];
            var userId = User.Identity.GetUserId();
            var search = Db.Class.Where(e => e.Id == id && e.SupervisorId == userId);
            if (search.Count() != 1) return ErrorView("You are not supervisor of such class.");
            var class_ = search.Single();
            ViewBag.Subjects = class_.TeacherClassSubjects.Select(e => e.Subject).ToArray();
            return View(class_);
        }

        [HttpPost]
        public ActionResult GenerateGradeSheet(int? id, DateTime fromDate, DateTime toDate, IList<string> selectedStudentSubjects)
        {
            var d = LocalizedStrings.Class.GenerateGradeSheet[LanguageCookie.Read(Request.Cookies)];
            var userId = User.Identity.GetUserId();
            var search = Db.Class.Where(e => e.Id == id && e.SupervisorId == userId);
            if (search.Count() != 1) return ErrorView("You are not supervisor of such class.");
            var class_ = search.Single();
            if (selectedStudentSubjects == null)
            {
                ViewBag.ValidationMessage = d["You did not select any students and subjects."];
                ViewBag.Subjects = class_.TeacherClassSubjects.Select(e => e.Subject).ToArray();
                return View("GenerateGradeSheet", class_);
            }
            if (fromDate > toDate)
            {
                var temp = toDate;
                toDate = fromDate;
                fromDate = temp;
            }
            ViewBag.Class = class_;
            var ssgs = StudentIdSubjectIdsToStudentSubjectGrades(selectedStudentSubjects, fromDate, toDate);
            return View("ShowGradeSheet", ssgs);
        }

        public struct StudentSubjectsGrades
        {
            public Models.Student Student;
            public struct SubjectGrades
            {
                public Subject Subject;
                public LinkedList<Grade> GradesList;
                public SubjectGrades(Subject subject)
                {
                    Subject = subject;
                    GradesList = new LinkedList<Grade>();
                }
            }
            public LinkedList<SubjectGrades> SubjectGradesList;
            public StudentSubjectsGrades(Models.Student student)
            {
                Student = student;
                SubjectGradesList = new LinkedList<SubjectGrades>();
            }
        }

        private LinkedList<StudentSubjectsGrades> StudentIdSubjectIdsToStudentSubjectGrades(IList<string> selectedStudentSubjects, DateTime fromDate, DateTime toDate)
        {
            var dict = new Dictionary<string, LinkedList<string>>(); // ["studentId"] = { "subjectId1", "subjectId2", ... }
            foreach (var sss in selectedStudentSubjects)
            {
                var splitted = sss.Split('|');
                var studentId = splitted[0];
                var subjectId = splitted[1];
                if (dict.ContainsKey(studentId))
                    dict[studentId].AddLast(subjectId);
                else
                {
                    dict.Add(studentId, new LinkedList<string>());
                    dict[studentId].AddLast(subjectId);
                }
            }
            var db = ApplicationDbContext.Create();
            var ssgs = new LinkedList<StudentSubjectsGrades>();
            foreach (var sisi in dict)
            {
                var studentId = sisi.Key;
                var student = db.Student.Where(e => e.Id == studentId).Single();
                ssgs.AddLast(new StudentSubjectsGrades(student));
                var addedSSG = ssgs.Last.Value;
                foreach (var subjectId in sisi.Value)
                {
                    var intSubjectId = int.Parse(subjectId);
                    var subject = db.Subject.Where(e => e.Id == intSubjectId).Single();
                    addedSSG.SubjectGradesList.AddLast(new StudentSubjectsGrades.SubjectGrades(subject));
                    var added = addedSSG.SubjectGradesList.Last.Value;
                    var grades = db.Grade.Where(e => e.StudentId == studentId && e.SubjectId == intSubjectId &&
                        e.ModificationTime >= fromDate && e.ModificationTime <= toDate).ToArray();
                    foreach (var grade in grades)
                        added.GradesList.AddLast(grade);
                }
            }
            return ssgs;
        }

        public ActionResult CreateAppointment(int? teacherClassSubjectId)
        {
            var d = LocalizedStrings.Class.CreateAppointment[LanguageCookie.Read(Request.Cookies)];
            var userId = User.Identity.GetUserId();
            var search = Db.TeacherClassSubject.Where(e => e.Id == teacherClassSubjectId);
            if (search.Count() != 1) return ErrorView("You do not teach such subject in such class.");
            var tcs = search.Single();
            ViewBag.ClassId = tcs.ClassId; // do powrotu
            ViewBag.TeacherClassSubjectId = teacherClassSubjectId;
            var possibleWeekDays = new int[Days.Array.Length];
            foreach (var l in tcs.Lessons) possibleWeekDays[l.DayId] = 1;
            ViewBag.PossibleWeekDays = string.Join(",", possibleWeekDays);
            return View(new Appointment { Name = "", Description = "", Date = DateTime.Today });
        }

        [HttpPost]
        public ActionResult CreateAppointment(int? teacherClassSubjectId, string name, string description, DateTime? date)
        {
            var d = LocalizedStrings.Class.CreateAppointment[LanguageCookie.Read(Request.Cookies)];
            var userId = User.Identity.GetUserId();
            var search = Db.TeacherClassSubject.Where(e => e.Id == teacherClassSubjectId);
            if (search.Count() != 1) return ErrorView("You do not teach such subject in such class.");
            var tcs = search.Single();
            do
            {
                if (string.IsNullOrEmpty(name)) ViewBag.ValidationMessage = d["Specify name."];
                else if (date == null) ViewBag.ValidationMessage = d["Specify date."];
                else break;
                ViewBag.ClassId = tcs.ClassId;
                ViewBag.TeacherClassSubjectId = teacherClassSubjectId;
                var possibleWeekDays = new int[Days.Array.Length];
                foreach (var l in tcs.Lessons) possibleWeekDays[l.DayId] = 1;
                ViewBag.PossibleWeekDays = string.Join(",", possibleWeekDays);
                return View(new Appointment { Name = name, Description = description, Date = (date == null ? DateTime.Today : date.Value) });
            }
            while (false);
            // date jest bindowane z formatu dd/mm/yyyy
            var a = new Appointment();
            a.Name = name;
            a.Description = description;
            a.Date = date.Value;
            a.TeacherClassSubjectId = teacherClassSubjectId.Value;
            Db.Appointment.Add(a);
            Db.SaveChanges();
            return RedirectToAction("Details", new { id = tcs.ClassId });
        }
    }
}
