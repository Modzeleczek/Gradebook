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
            if (search.Count() != 1) return ErrorView("Class does not exist.");
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

        private void SendByWebsite(int classId, string content, HttpPostedFileBase attachedFile)
        {
            var message = new Message { Content = content };
            message.SenderId = User.Identity.GetUserId();
            message.SendTime = DateTime.Now;
            Db.Message.Add(message);
            Db.SaveChanges();
            var class_ = Db.Class.Where(e => e.Id == classId).Single();
            var recipients = class_.Students.Where(e => e.ParentId != null).Select(e => e.Parent).Distinct(new ComparerById());
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
            var recipients = class_.Students.Where(e => e.ParentId != null).Select(e => e.Parent).Distinct(new ComparerById());
            if (recipients.Count() == 0) return;
            var recipientUsers = recipients.Select(e => e.ApplicationUser).ToArray();
            EmailSender.Send(teacherEmail, recipientUsers, $"Announcement from {teacher.ApplicationUser.Name} {teacher.ApplicationUser.Surname}", content, attachedFile, false);
        }

        public ActionResult GenerateGradeSheet(int id) // id - classId
        {
            ViewBag.ClassId = id;
            var _class = Db.Class.Where(e => e.Id == id).Single();
            ViewBag.Subjects = _class.TeacherClassSubjects.Select(e => e.Subject).ToArray();
            return View(_class);
        }

        [HttpPost]
        public ActionResult GenerateGradeSheet(int id, DateTime fromDate, DateTime toDate, IList<string> selectedStudentSubjects)
        {
            if (fromDate > toDate)
            {
                var temp = toDate;
                toDate = fromDate;
                fromDate = temp;
            }
            TempData["fromDate"] = fromDate;
            TempData["toDate"] = toDate;
            TempData["selectedStudentSubjects"] = selectedStudentSubjects;
            return RedirectToAction("ShowGradeSheet", new { id = id });
        }

        public ActionResult ShowGradeSheet(int id) // id - classId
        {
            var fromDate = (DateTime)TempData["fromDate"];
            var toDate = (DateTime)TempData["toDate"];
            var selectedStudentSubjects = (IList<string>)TempData["selectedStudentSubjects"];
            if (selectedStudentSubjects == null)
                return RedirectToAction("Index");
            ViewBag.ClassId = id;
            var ssgs = StudentIdSubjectIdsToStudentSubjectGrades(selectedStudentSubjects, fromDate, toDate);
            return View(ssgs);
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

        public ActionResult CreateAppointment(int? classId, int? teacherClassSubjectId)
        {
            do
            {
                if (classId == null || teacherClassSubjectId == null) break;
                if (Db.Class.Where(e => e.Id == classId).Any() == false) break;
                var userId = User.Identity.GetUserId();
                var tcs = Db.TeacherClassSubject.Where(e => e.Id == teacherClassSubjectId);
                if (tcs.Count() != 1) break;
                var aTcs = tcs.Single();
                if (aTcs.TeacherId != userId) break;
                ViewBag.ClassId = classId; // do powrotu
                ViewBag.TeacherClassSubjectId = teacherClassSubjectId;
                var possibleWeekDays = new int[Days.Array.Length];
                foreach (var l in aTcs.Lessons)
                    possibleWeekDays[l.DayId] = 1;
                ViewBag.PossibleWeekDays = string.Join(",", possibleWeekDays);
                return View();
            } while (false);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CreateAppointment(int classId, int teacherClassSubjectId, Appointment appointment)
        {
            // appointment.Date jest bindowane z formatu dd/mm/yyyy
            appointment.TeacherClassSubjectId = teacherClassSubjectId;
            Db.Appointment.Add(appointment);
            Db.SaveChanges();
            return RedirectToAction("Details", new { id = classId });
        }
    }
}
