using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Gradebook.Models;
using Gradebook.Utils;
using ControllerBase = Gradebook.Utils.ControllerBase;

namespace Gradebook.Areas.Admin.Controllers
{
    [Authorize(Roles = Role.Administrator), ViewFilter]
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

        public JsonResult GetSupervisors(string classSupervisorId)
        {
            var classes = Db.Class;
            var freeSupervisors = Db.Teacher.Where(e => classes.All(c => c.SupervisorId != e.Id) || e.Id == classSupervisorId);
            var records = freeSupervisors.Select(r => new { r.Id, r.ApplicationUser.Email, r.ApplicationUser.Name, r.ApplicationUser.Surname });
            var list = records.ToList();
            return Json(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string supervisorId, string year, string unit)
        {
            var d = LocalizedStrings.Class.Create[LanguageCookie.Read(Request.Cookies)];
            var c = new Class();
            if (unit.Length != 1 || !((unit[0] >= 'a' && unit[0] <= 'z') || (unit[0] >= 'A' && unit[0] <= 'Z')))
            { ViewBag.ValidationMessage = d["Unit must be a single letter."]; return View(c); }
            c.Unit = unit.ToUpper().Substring(0, 1);
            if (!int.TryParse(year, out int intYear) || intYear <= 0)
            { ViewBag.ValidationMessage = d["Year must be positive integer."]; return View(c); }
            c.Year = intYear;
            var teacherSearch = Db.Teacher.Where(e => e.Id == supervisorId);
            if (teacherSearch.Count() != 1) { ViewBag.ValidationMessage = d["Teacher does not exist."]; return View(c); }
            var supervisorSearch = Db.Class.Where(e => e.SupervisorId == supervisorId);
            if (supervisorSearch.Count() != 0) { ViewBag.ValidationMessage = d["Teacher is already supervisor."]; return View(c); }
            c.SupervisorId = supervisorId;
            Db.Class.Add(c);
            Db.SaveChanges();
            return RedirectToAction("List");
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
            var D = LocalizedStrings.Class.Edit[LanguageCookie.Read(Request.Cookies)];
            for (int di = 0; di < Days.Array.Length; ++di)
                for (int hi = 0; hi < LessonHours.Array.Length; ++hi)
                    if (dayHourUsed[di, hi] == false)
                    {
                        var day = Days.Array[di];
                        var hour = LessonHours.Array[hi];
                        list.AddLast(new SelectListItem { Text = $"{D[day.Name]} {hour.StartH}:{hour.StartM.ToString("00")}", Value = $"{di}|{hi}", Selected = false });
                    }
            return list;
        }

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

        [HttpPost]
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
                    // jeżeli dany nauczyciel ma już jakąś lekcję w danym dniu i godzinie, to nie nadpisujemy jej ani nie dodajemy nowej lekcji w tym samym czasie
                    if (Db.Lesson.Where(e => e.TeacherClassSubject.TeacherId == record.TeacherId && e.DayId == dayId && e.HourId == hourId).Any() == false)
                    {
                        var lesson = new Lesson { TeacherClassSubjectId = teacherClassSubjectId ?? 0, DayId = dayId, HourId = hourId };
                        Db.Lesson.Add(lesson);
                        Db.SaveChanges();
                    }
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

        public ActionResult DeleteStudent(int classId, string studentId)
        {
            var record = Db.Class.Where(e => e.Id == classId).Single();
            var student = Db.Student.Where(e => e.Id == studentId).Single();
            record.Students.Remove(student);
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = classId });
        }

        public ActionResult DeleteTeacherSubject(int classId, int teacherClassSubjectId)
        {
            // potencjalna alternatywa: w DeleteLesson działa zwykłe usunięcie rekordu z Db.Lesson zamiast przez właściwość nawigacji
            var record = Db.Class.Where(e => e.Id == classId).Single();
            var tcs = record.TeacherClassSubjects.Where(e => e.Id == teacherClassSubjectId).Single();
            var lessons = tcs.Lessons.ToList();
            foreach (var l in lessons)
                Db.Lesson.Remove(l);
            var appointments = tcs.Appointments.ToList();
            foreach (var a in appointments)
                Db.Appointment.Remove(a);
            Db.SaveChanges();
            Db.TeacherClassSubject.Remove(tcs);
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = classId });
        }

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

        public ActionResult DeleteLesson(int classId, int lessonId)
        {
            var lesson = Db.Lesson.Where(e => e.Id == lessonId).Single();
            Db.Lesson.Remove(lesson);
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = classId });
        }
    }
}
