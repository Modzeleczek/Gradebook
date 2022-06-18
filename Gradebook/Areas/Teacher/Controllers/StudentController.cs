using Gradebook.Models;
using Gradebook.Models.ViewModels;
using Gradebook.Utils;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Teacher.Controllers
{
    [Authorize(Roles = Role.Teacher), ViewFilter]
    public class StudentController : ControllerBase
    {
        public ActionResult Details(string id)
        {
            var teacherId = User.Identity.GetUserId();
            var teacherSearch = Db.Teacher.Where(e => e.Id == teacherId);
            if (teacherSearch.Count() != 1) return ErrorView("Your account does not exist.");
            var teacher = teacherSearch.Single();
            var teacherClassSubjects = teacher.TeacherClassSubjects;
            var studentSearch = Db.Student.Where(e => e.Id == id);
            if (studentSearch.Count() != 1) return ErrorView("Such student does not exist.");
            var student = studentSearch.Single();
            var studentClassId = student.ClassId;
            var commonTcss = teacherClassSubjects.Where(e => e.ClassId == studentClassId);
            if (commonTcss.Count() == 0) return ErrorView("You do not teach in such class.");
            var teacherSubjectIds = commonTcss.Select(e => e.SubjectId);
            var grades = Db.Grade.Where(e => e.StudentId == id && teacherSubjectIds.Contains(e.SubjectId)).ToArray();
            var subjects = commonTcss.Select(e => e.Subject).ToArray();
            var gradesGrouped = SubjectGrades.GroupGradesBySubject(grades, subjects);
            ViewBag.SubjectGrades = gradesGrouped;
            Absence[] absences = null;
            if (student.Class.SupervisorId == teacherId) // wychowawca widzi wszystkie nieobecności
                absences = Db.Absence.Where(e => e.StudentId == id).ToArray();
            else // niewychowawca widzi tylko swoje nieobecności
                absences = Db.Absence.Where(e => e.StudentId == id && e.Lesson.TeacherClassSubject.TeacherId == teacherId).ToArray();
            ViewBag.Absences = absences;
            ViewBag.TeacherId = teacherId;
            return View(student);
        }

        public ActionResult CreateGrade(string studentId)
        {
            var studentSearch = Db.Student.Where(e => e.Id == studentId);
            if (studentSearch.Count() != 1) return ErrorView("Such student does not exist.");
            var student = studentSearch.Single();
            var teacherId = User.Identity.GetUserId();
            if (!student.Class.TeacherClassSubjects.Any(e => e.TeacherId == teacherId)) return ErrorView("You do not teach in such student's class.");
            ViewBag.StudentId = studentId;
            return View(new Grade());
        }

        [HttpPost]
        public ActionResult CreateGrade(string studentId, float? value, float? weight, string comment, int?teacherClassSubjectId)
        {
            var d = LocalizedStrings.Student.CreateGrade[LanguageCookie.Read(Request.Cookies)];
            bool error = false;
            if (!value.HasValue || (value.Value < 1 || value.Value > 6))
            { ViewBag.ValidationMessage = d["Specify value in range <1, 6>."]; error = true; }
            else if (!weight.HasValue || weight < 0)
            { ViewBag.ValidationMessage = d["Specify positive weight."]; error = true; }
            else if (!teacherClassSubjectId.HasValue)
            { ViewBag.ValidationMessage = d["Select subject."]; error = true; }
            if (error)
            {
                ViewBag.StudentId = studentId;
                ViewBag.SelectedTeacherClassSubjectId = teacherClassSubjectId;
                return View(new Grade { Value = value ?? 0, Weight = weight ?? 0, Comment = comment });
            }
            var studentSearch = Db.Student.Where(e => e.Id == studentId);
            if (studentSearch.Count() != 1) return ErrorView("Such student does not exist.");
            var student = studentSearch.Single();
            var teacherId = User.Identity.GetUserId();
            if (!student.Class.TeacherClassSubjects.Any(e => e.TeacherId == teacherId)) return ErrorView("You do not teach in such student's class.");
            var tcsSearch = Db.TeacherClassSubject.Where(e => e.Id == teacherClassSubjectId);
            if (tcsSearch.Count() != 1) return ErrorView("You do not teach such subject in such class.");
            var tcs = tcsSearch.Single();
            var grade = new Grade();
            grade.Value = value.Value;
            grade.Weight = weight.Value;
            grade.Comment = comment;
            grade.StudentId = studentId;
            grade.TeacherId = teacherId;
            grade.SubjectId = tcs.SubjectId;
            grade.ModificationTime = DateTime.Now;
            Db.Grade.Add(grade);
            Db.SaveChanges();
            return RedirectToAction("Details", new { id = studentId });
        }

        [Authorize(Roles = Role.Teacher)]
        public ActionResult DeleteGrade(int? gradeId)
        {
            var teacherId = User.Identity.GetUserId();
            var gradeSearch = Db.Grade.Where(e => e.Id == gradeId && e.TeacherId == teacherId);
            if (gradeSearch.Count() != 1) return ErrorView("You have not created such grade.");
            var g = gradeSearch.Single();
            var studentId = g.StudentId;
            Db.QuizAttempt.Where(e => e.GradeId == gradeId).UpdateFromQuery(e => new QuizAttempt { GradeId = null });
            Db.Grade.Remove(g);
            Db.SaveChanges();
            return RedirectToAction("Details", new { id = studentId });
        }

        public ActionResult CreateAbsence(string studentId)
        {
            var studentSearch = Db.Student.Where(e => e.Id == studentId);
            if (studentSearch.Count() != 1) return ErrorView("Such student does not exist.");
            var student = studentSearch.Single();
            var teacherId = User.Identity.GetUserId();
            if (!student.Class.TeacherClassSubjects.Any(e => e.TeacherId == teacherId)) return ErrorView("You do not teach in such class.");
            ViewBag.StudentId = studentId;
            ViewBag.DatepickerLanguage = LanguageCookie.ReadCode(Request.Cookies);
            return View(new Absence());
        }

        public JsonResult GetTeacherClassSubjects(string studentId)
        {
            var teacherId = User.Identity.GetUserId();
            var studentSearch = Db.Student.Where(e => e.Id == studentId);
            if (studentSearch.Count() != 1) return ErrorJson("Such student does not exist.");
            var student = studentSearch.Single();
            var studentClassId = student.ClassId;
            var tcssSearch = Db.TeacherClassSubject.Where(e => e.TeacherId == teacherId && e.ClassId == studentClassId);
            if (tcssSearch.Count() == 0) return ErrorJson("You do not teach in such class.");
            var tcss = tcssSearch.ToArray();
            var subjects = new LinkedList<object>();
            foreach (var tcs in tcss)
                subjects.AddLast(new { Id = tcs.Id, Name = tcs.Subject.Name });
            return Json(subjects);
        }

        public JsonResult GetLessonDays(int? teacherClassSubjectId)
        {
            var teacherId = User.Identity.GetUserId();
            var tcsSearch = Db.TeacherClassSubject.Where(e => e.Id == teacherClassSubjectId && e.TeacherId == teacherId);
            if (tcsSearch.Count() != 1) return ErrorJson("You do not teach such subject in such class.");
            var tcs = tcsSearch.Single();
            var possibleWeekDays = new int[Days.Array.Length];
            foreach (var l in tcs.Lessons) possibleWeekDays[l.DayId] = 1;
            return Json(possibleWeekDays);
        }

        public JsonResult GetLessons(int? teacherClassSubjectId, int? weekDay, int? day, int? month, int? year, string studentId)
        {
            if (!weekDay.HasValue || !day.HasValue || !month.HasValue || !year.HasValue)
                return ErrorJson("Week day and date must be specified.");
            var teacherId = User.Identity.GetUserId();
            var tcsSearch = Db.TeacherClassSubject.Where(e => e.Id == teacherClassSubjectId && e.TeacherId == teacherId);
            if (tcsSearch.Count() != 1) return ErrorJson("You do not teach such subject in such class.");
            var tcs = tcsSearch.Single();
            var studentSearch = Db.Student.Where(e => e.Id == studentId);
            if (studentSearch.Count() != 1) return ErrorJson("Such student does not exist.");
            var student = studentSearch.Single();
            var lessonSearch = tcs.Lessons.Where(e => e.DayId == weekDay).ToArray();
            var onlyDate = new DateTime(year.Value, month.Value, day.Value).Date;
            var absences = student.Absences.Where(e => e.Date.Date == onlyDate);
            var alreadyHasAbsence = new bool[LessonHours.Array.Length];
            foreach (var a in absences)
                alreadyHasAbsence[a.Lesson.HourId] = true;
            var lessons = new LinkedList<object>();
            foreach (var l in lessonSearch)
                if (!alreadyHasAbsence[l.HourId])
                    lessons.AddLast(new { Id = l.Id, Hour = LessonHours.Array[l.HourId].ToString() });
            return Json(lessons);
        }

        [HttpPost]
        public ActionResult CreateAbsence(string studentId, int? teacherClassSubjectId, DateTime? date, int? lessonId)
        {
            var d = LocalizedStrings.Student.CreateAbsence[LanguageCookie.Read(Request.Cookies)];
            bool error = false;
            if (!teacherClassSubjectId.HasValue)
            { ViewBag.ValidationMessage = d["Select subject."]; error = true; }
            else if (!date.HasValue)
            { ViewBag.ValidationMessage = d["Select date."]; error = true; }
            else if (!lessonId.HasValue)
            { ViewBag.ValidationMessage = d["Select time."]; error = true; }
            if (error)
            {
                ViewBag.StudentId = studentId;
                ViewBag.DatepickerLanguage = LanguageCookie.ReadCode(Request.Cookies);
                ViewBag.SelectedTeacherClassSubjectId = teacherClassSubjectId;
                ViewBag.SelectedDate = date;
                return View(new Absence());
            }
            var teacherId = User.Identity.GetUserId();
            var tcsSearch = Db.TeacherClassSubject.Where(e => e.Id == teacherClassSubjectId && e.TeacherId == teacherId);
            if (tcsSearch.Count() != 1) return ErrorView("You do not teach such subject in such class.");
            var tcs = tcsSearch.Single();
            var lessonSearch = tcs.Lessons.Where(e => e.Id == lessonId);
            if (lessonSearch.Count() != 1) return ErrorView("Such class does not have such lesson within your subject.");
            var studentSearch = Db.Student.Where(e => e.Id == studentId);
            if (studentSearch.Count() != 1) return ErrorView("Such student does not exist.");
            var a = new Absence();
            a.Date = date.Value;
            a.IsJustified = false;
            a.StudentId = studentId;
            a.LessonId = lessonId.Value;
            Db.Absence.Add(a);
            Db.SaveChanges();
            return RedirectToAction("Details", new { id = studentId });
        }

        public ActionResult DeleteAbsence(int? absenceId)
        {
            var teacherId = User.Identity.GetUserId();
            var absenceSearch = Db.Absence.Where(e => e.Id == absenceId && e.Lesson.TeacherClassSubject.TeacherId == teacherId);
            if (absenceSearch.Count() != 1) return ErrorView("You have not created such absence.");
            var a = absenceSearch.Single();
            var studentId = a.StudentId;
            Db.Absence.Remove(a);
            Db.SaveChanges();
            return RedirectToAction("Details", new { id = studentId });
        }

        public ActionResult FlipIsJustified(string studentId, int? absenceId)
        {
            var studentSearch = Db.Student.Where(e => e.Id == studentId);
            if (studentSearch.Count() != 1) return ErrorView("Such student does not exist.");
            var student = studentSearch.Single();
            var teacherId = User.Identity.GetUserId();
            if (student.Class.SupervisorId != teacherId) return ErrorView("You are not supervisor of such class.");
            var absenceSearch = student.Absences.Where(e => e.Id == absenceId);
            if (absenceSearch.Count() != 1) return ErrorView("Such student does not have such absence.");
            var absence = absenceSearch.Single();
            absence.IsJustified = !absence.IsJustified;
            Db.SaveChanges();
            return RedirectToAction("Details", new { id = studentId });
        }
    }
}
