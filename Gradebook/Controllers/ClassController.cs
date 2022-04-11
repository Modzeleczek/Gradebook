using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gradebook.Models;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;

namespace Gradebook.Controllers
{
    [ViewFilter]
    public class ClassController : Controller
    {
        private ApplicationDbContext Db = ApplicationDbContext.Create();

        // GET: Class
        [Authorize(Roles = Role.AdministratorTeacher)]
        public ActionResult Index()
        {
            return View(Db.Class.ToArray());
        }

        // GET: Class/Details/5
        [Authorize(Roles = Role.AdministratorTeacherStudent)]
        public ActionResult Details(int id, string studentId)
        {
            var _class = Db.Class.Where(e => e.Id == id).Single();
            if (User.IsInRole(Role.Teacher))
            {
                var teacherId = User.Identity.GetUserId();
                ViewBag.IsSupervisor = (_class.SupervisorId == teacherId);
            }
            return View(_class);
        }

        private LinkedList<SelectListItem> GetTeachers()
        {
            var records = Db.Teacher.Select(r => new { r.Id, r.ApplicationUser.Name, r.ApplicationUser.Surname });
            var list = new LinkedList<SelectListItem>();
            foreach (var r in records)
                list.AddLast(new SelectListItem { Text = $"{r.Name} {r.Surname}", Value = r.Id.ToString(), Selected = false });
            return list;
        }

        private LinkedList<SelectListItem> GetSupervisors(string classSupervisorId)
        {
            var classes = Db.Class;
            var freeSupervisors = Db.Teacher.Where(e => classes.All(c => c.SupervisorId != e.Id) || e.Id == classSupervisorId);
            var records = freeSupervisors.Select(r => new { r.Id, r.ApplicationUser.Name, r.ApplicationUser.Surname });
            var list = new LinkedList<SelectListItem>();
            foreach (var r in records)
                list.AddLast(new SelectListItem { Text = $"{r.Name} {r.Surname}", Value = r.Id.ToString(), Selected = false });
            return list;
        }

        // GET: Class/Create
        [Authorize(Roles = Role.Administrator)]
        public ActionResult Create()
        {
            var teachers = GetSupervisors(null);
            if (teachers.First != null)
                teachers.First.Value.Selected = true;
            ViewBag.Teachers = teachers;
            return View();
        }

        // POST: Class/Create
        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public ActionResult Create(Class _class)
        {
            if (ModelState.IsValid == false)
            {
                var teachers = GetSupervisors(null);
                foreach (var t in teachers)
                    if (t.Value == _class.SupervisorId)
                    {
                        t.Selected = true;
                        break;
                    }
                ViewBag.Teachers = teachers;
                return View();
            }
            Db.Class.Add(_class);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        private LinkedList<SelectListItem> GetSubjects(int classId)
        {
            var allSubjects = Db.Subject.ToArray();
            var _class = Db.Class.Where(e => e.Id == classId).Single();
            var alreadyAddedSubjects = _class.TeacherClassSubjects.Distinct(new ComparerBySubject()).Select(e => e.Subject).ToArray();
            var unusedSubjects = allSubjects.Except(alreadyAddedSubjects);
            var list = new LinkedList<SelectListItem>();
            foreach (var r in unusedSubjects)
                list.AddLast(new SelectListItem { Text = r.Name, Value = r.Id.ToString(), Selected = false });
            return list;
        }

        private LinkedList<SelectListItem> GetTeacherClassSubjects(int classId)
        {
            var _class = Db.Class.Where(e => e.Id == classId).Single();
            var tcss = _class.TeacherClassSubjects.ToArray();
            var list = new LinkedList<SelectListItem>();
            foreach (var r in tcss)
                list.AddLast(new SelectListItem { Text = $"{r.Subject.Name} | {r.Teacher.ApplicationUser.Name} {r.Teacher.ApplicationUser.Surname}", Value = r.Id.ToString(), Selected = false });
            return list;
        }

        private LinkedList<SelectListItem> GetDayHours(int classId)
        {
            var _class = Db.Class.Where(e => e.Id == classId).Single();
            var addedLessons = new LinkedList<Lesson>();
            foreach (var tcs in _class.TeacherClassSubjects)
                foreach (var l in tcs.Lessons)
                    addedLessons.AddLast(l);
            var dayHourUsed = new bool[Days.Array.Length, LessonHours.Array.Length];
            foreach (var al in addedLessons)
                dayHourUsed[al.DayId, al.HourId] = true;
            var list = new LinkedList<SelectListItem>();
            for (int di = 0; di < Days.Array.Length; ++di)
                for (int hi = 0; hi < LessonHours.Array.Length; ++hi)
                    if (dayHourUsed[di, hi] == false)
                    {
                        var day = Days.Array[di];
                        var hour = LessonHours.Array[hi];
                        list.AddLast(new SelectListItem { Text = $"{day.Name} {hour.StartH}:{hour.StartM.ToString("00")}", Value = $"{di}|{hi}", Selected = false });
                    }
            return list;
        }

        // GET: Class/Edit/5
        [Authorize(Roles = Role.Administrator)]
        public ActionResult Edit(int id)
        {
            var classSupervisorId = Db.Class.Where(e => e.Id == id).Single().SupervisorId;
            var supervisors = GetSupervisors(classSupervisorId);
            ViewBag.Supervisors = supervisors;
            var students = GetStudents();
            if (students.First != null) students.First.Value.Selected = true;
            ViewBag.Students = students;
            var teachers = GetTeachers();
            if (teachers.First != null) teachers.First.Value.Selected = true;
            ViewBag.Teachers = teachers;
            var subjects = GetSubjects(id);
            if (subjects.First != null) subjects.First.Value.Selected = true;
            ViewBag.Subjects = subjects;
            var teacherClassSubjects = GetTeacherClassSubjects(id);
            if (teacherClassSubjects != null && teacherClassSubjects.Count > 0)
                teacherClassSubjects.First.Value.Selected = true;
            ViewBag.TeacherClassSubjects = teacherClassSubjects;
            var dayHours = GetDayHours(id);
            if (dayHours != null) dayHours.First.Value.Selected = true;
            ViewBag.DayHours = dayHours;
            return View(Db.Class.Where(e => e.Id == id).Single());
        }

        // POST: Class/Edit/5
        [HttpPost]
        [Authorize(Roles = Role.Administrator)]
        public ActionResult Edit(Class _class, string button, string studentId, string teacherId, int? subjectId, int? teacherClassSubjectId, string dayIdHourId)
        {
            if (button == "Save")
            {
                if (ModelState.IsValid == false)
                    return RedirectToAction("Edit", new { id = _class.Id });
                var record = Db.Class.Where(e => e.Id == _class.Id).Single();
                record.SupervisorId = _class.SupervisorId;
                record.Year = _class.Year;
                record.Unit = _class.Unit;
                Db.SaveChanges();
            }
            else if (button == "AddStudent")
            {
                if (studentId == null)
                    return RedirectToAction("Edit", new { id = _class.Id });
                var student = Db.Student.Where(e => e.Id == studentId).Single();
                student.ClassId = _class.Id;
                // alternatywa
                /*{
                    var record = Db.Class.Where(e => e.Id == _class.Id).Single();
                    var student = Db.Student.Where(e => e.Id == studentId).Single();
                    record.Students.Add(student);
                }*/
                Db.SaveChanges();
            }
            else if (button == "AddTeacher")
            {
                var record = Db.Class.Where(e => e.Id == _class.Id).Single();
                var tcs = new TeacherClassSubject { ClassId = _class.Id, TeacherId = teacherId, SubjectId = subjectId ?? 0 };
                Db.TeacherClassSubject.Add(tcs);
                Db.SaveChanges();
            }
            else if (button == "AddLesson")
            {
                var record = Db.TeacherClassSubject.Where(e => e.Id == teacherClassSubjectId).Single();
                var split = dayIdHourId.Split('|');
                if (int.TryParse(split[0], out int dayId) && int.TryParse(split[1], out int hourId))
                {
                    var lesson = new Lesson { TeacherClassSubjectId = teacherClassSubjectId ?? 0, DayId = dayId, HourId = hourId };
                    Db.Lesson.Add(lesson);
                    Db.SaveChanges();
                }
            }
            return RedirectToAction("Edit", new { id = _class.Id });
        }

        private LinkedList<SelectListItem> GetStudents()
        {
            var records = Db.Student.Where(e => e.ClassId == null).Select(r => new { r.Id, r.ApplicationUser.Name, r.ApplicationUser.Surname, r.ApplicationUser.Email });
            var list = new LinkedList<SelectListItem>();
            foreach (var r in records)
                list.AddLast(new SelectListItem { Text = $"{r.Name} {r.Surname} | {r.Email}", Value = r.Id.ToString(), Selected = false });
            return list;
        }

        // GET
        public ActionResult DeleteStudent(int classId, string studentId)
        {
            var record = Db.Class.Where(e => e.Id == classId).Single();
            var student = Db.Student.Where(e => e.Id == studentId).Single();
            record.Students.Remove(student);
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = classId });
        }

        // GET
        public ActionResult DeleteTeacherSubject(int classId, int teacherClassSubjectId)
        {
            // potencjalna alternatywa: w DeleteLesson działa zwykłe usunięcie rekordu z Db.Lesson zamiast przez właściwość nawigacji
            var record = Db.Class.Where(e => e.Id == classId).Single();
            var tcs = record.TeacherClassSubjects.Where(e => e.Id == teacherClassSubjectId).Single();
            record.TeacherClassSubjects.Remove(tcs);
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = classId });
        }

        // GET: Class/Delete/5
        [Authorize(Roles = Role.Administrator)]
        public ActionResult Delete(int id)
        {
            var c = Db.Class.Where(e => e.Id == id).Single();
            foreach (var student in c.Students)
                student.ClassId = null;
            Db.SaveChanges();
            Db.Class.Remove(c);
            Db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Role.Teacher)]
        public ActionResult CreateAnnouncement(int id) // id - classId
        {
            ViewBag.ClassId = id;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Role.Teacher)]
        public ActionResult CreateAnnouncement(int id, string button, Message message, HttpPostedFileBase attachedFile)
        {
            if (button == "SendByWebsite")
                SendByWebsite(id, message, attachedFile);
            else if (button == "SendByEmail")
                SendByEmail(id, message, attachedFile);
            return RedirectToAction("Details", new { id = id });
        }

        private void SendByWebsite(int classId, Message message, HttpPostedFileBase attachedFile)
        {
            message.SenderId = User.Identity.GetUserId();
            message.SendTime = DateTime.Now;
            // message.Content jest bindowane z formularza
            Db.Message.Add(message);
            Db.SaveChanges();
            var recipientIds = Db.Class.Where(e => e.Id == classId).Single().Students.Select(e => e.ParentId).Distinct();
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

        private void SendByEmail(int classId, Message message, HttpPostedFileBase attachedFile)
        {
            var teacherId = User.Identity.GetUserId();
            var teacher = Db.Teacher.Where(e => e.Id == teacherId).Single();
            var teacherEmail = teacher.ApplicationUser.Email;
            var recipients = Db.Class.Where(e => e.Id == classId).Single().Students.Select(e => e.Parent).Distinct(new ComparerById());
            var recipientUsers = recipients.Select(e => e.ApplicationUser).ToArray();
            EmailSender.Send(teacherEmail, recipientUsers, $"Announcement from {teacher.ApplicationUser.Name} {teacher.ApplicationUser.Surname}", message.Content, attachedFile, false);
        }

        [Authorize(Roles = Role.Teacher)]
        public ActionResult GenerateGradeSheet(int id) // id - classId
        {
            ViewBag.ClassId = id;
            var _class = Db.Class.Where(e => e.Id == id).Single();
            ViewBag.Subjects = _class.TeacherClassSubjects.Select(e => e.Subject).ToArray();
            return View(_class);
        }

        [HttpPost]
        [Authorize(Roles = Role.Teacher)]
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

        [Authorize(Roles = Role.Teacher)]
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
            public Student Student;
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
            public StudentSubjectsGrades(Student student)
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

        public ActionResult DeleteLesson(int classId, int lessonId)
        {
            var lesson = Db.Lesson.Where(e => e.Id == lessonId).Single();
            Db.Lesson.Remove(lesson);
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = classId });
        }
    }
}
