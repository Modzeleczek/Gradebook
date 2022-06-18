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
            if (search.Count() != 1) return ErrorView("Such class does not exist.");
            var _class = search.Single();
            return View(_class);
        }

        public JsonResult GetTeachers()
        {
            var list = Db.Teacher.Select(r => new { r.Id, r.ApplicationUser.Email, r.ApplicationUser.Name, r.ApplicationUser.Surname });
            return Json(list);
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
            return View(new Class());
        }

        [HttpPost]
        public ActionResult Create(string supervisorId, string year, string unit)
        {
            var d = LocalizedStrings.Class.Create[LanguageCookie.Read(Request.Cookies)];
            var c = new Class();
            c.SupervisorId = supervisorId;
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

        public JsonResult GetClassUnassignedSubjects(int? classId)
        {
            var classSearch = Db.Class.Where(e => e.Id == classId);
            if (classSearch.Count() != 1) return Json(new LinkedList<object>());
            var class_ = classSearch.Single();
            var allSubjects = Db.Subject.ToArray();
            var alreadyAddedSubjects = class_.TeacherClassSubjects.Select(e => e.Subject).ToList();
            var unusedSubjects = allSubjects.Except(alreadyAddedSubjects);
            var list = unusedSubjects.Select(e => new { e.Id, e.Name });
            return Json(list);
        }

        public JsonResult GetTeacherClassSubjects(int? classId)
        {
            var classSearch = Db.Class.Where(e => e.Id == classId);
            if (classSearch.Count() != 1) return Json(new LinkedList<object>());
            var class_ = classSearch.Single();
            var tcss = class_.TeacherClassSubjects;
            var list = new LinkedList<object>();
            foreach (var tcs in tcss)
            {
                var teacher = tcs.Teacher.ApplicationUser;
                var teacherStr = $"{teacher.Name} {teacher.Surname}";
                list.AddLast(new { tcs.Id, Subject = tcs.Subject.Name, Teacher = teacherStr });
            }
            return Json(list);
        }

        public JsonResult GetFreeDaysForTeacherAndClass(int? teacherClassSubjectId)
        {
            var d = LocalizedStrings.Class.Edit[LanguageCookie.Read(Request.Cookies)];
            var tcsSearch = Db.TeacherClassSubject.Where(e => e.Id == teacherClassSubjectId);
            if (tcsSearch.Count() != 1) return Json(new LinkedList<object>());
            var tcs = tcsSearch.Single();
            var teacher = tcs.Teacher;
            var teacherTcss = teacher.TeacherClassSubjects;
            var dayHourBusy = new bool[Days.Array.Length, LessonHours.Array.Length];
            foreach (var teacherTcs in teacherTcss)
                foreach (var l in teacherTcs.Lessons)
                    dayHourBusy[l.DayId, l.HourId] = true;
            var class_ = tcs.Class;
            var classTcss = class_.TeacherClassSubjects;
            foreach (var classTcs in classTcss)
                foreach (var l in classTcs.Lessons)
                    dayHourBusy[l.DayId, l.HourId] = true;
            var nonFullDays = new LinkedList<object>();
            for (int di = 0; di < Days.Array.Length; ++di)
                for (int hi = 0; hi < LessonHours.Array.Length; ++hi)
                    if (dayHourBusy[di, hi] == false)
                    {
                        nonFullDays.AddLast(new { Id = di, Name = d[Days.Array[di].Name] });
                        break;
                    }
            return Json(nonFullDays);
        }

        public JsonResult GetFreeHoursForTeacherClassAndDay(int? teacherClassSubjectId, int? dayId)
        {
            if (dayId == null || dayId.Value >= Days.Array.Length) return Json(new LinkedList<object>());
            var tcsSearch = Db.TeacherClassSubject.Where(e => e.Id == teacherClassSubjectId);
            if (tcsSearch.Count() != 1) return Json(new LinkedList<object>());
            var tcs = tcsSearch.Single();
            var teacher = tcs.Teacher;
            var teacherTcss = teacher.TeacherClassSubjects;
            var hourBusy = new bool[LessonHours.Array.Length];
            foreach (var teacherTcs in teacherTcss)
                foreach (var l in teacherTcs.Lessons)
                    if (l.DayId == dayId.Value)
                        hourBusy[l.HourId] = true;
            var class_ = tcs.Class;
            var classTcss = class_.TeacherClassSubjects;
            foreach (var classTcs in classTcss)
                foreach (var l in classTcs.Lessons)
                    hourBusy[l.HourId] = true;
            var freeHours = new LinkedList<object>();
            for (int hi = 0; hi < LessonHours.Array.Length; ++hi)
                if (hourBusy[hi] == false)
                {
                    var hour = LessonHours.Array[hi];
                    var hm = hour.ToString();
                    freeHours.AddLast(new { Id = hi, StartHM = hm, hour.DurationM });
                }
            return Json(freeHours);
        }

        public JsonResult GetFreeRoomsForDayAndHour(int? dayId, int? hourId)
        {
            if (dayId == null || dayId.Value >= Days.Array.Length) return Json(new LinkedList<object>());
            if (hourId == null || hourId.Value >= LessonHours.Array.Length) return Json(new LinkedList<object>());
            var roomBusy = new bool[Rooms.Array.Length];
            foreach (var l in Db.Lesson)
                if (l.DayId == dayId.Value && l.HourId == hourId.Value)
                    roomBusy[l.RoomId] = true;
            var freeRooms = new LinkedList<object>();
            for (int ri = 0; ri < Rooms.Array.Length; ++ri)
                if (roomBusy[ri] == false)
                {
                    var room = Rooms.Array[ri];
                    freeRooms.AddLast(new { Id = ri, room.Name });
                }
            return Json(freeRooms);
        }

        public ActionResult Edit(int? id)
        {
            var search = Db.Class.Where(e => e.Id == id);
            if (search.Count() != 1) return ErrorView("Such class does not exist.");
            return View(search.Single());
        }

        [HttpPost]
        public ActionResult Save(int? id, string supervisorId, string year, string unit)
        {
            var d = LocalizedStrings.Class.Create[LanguageCookie.Read(Request.Cookies)];
            var classSearch = Db.Class.Where(e => e.Id == id);
            if (classSearch.Count() != 1) return ErrorView("Such class does not exist.");
            var c = classSearch.Single();
            const string viewName = "Edit";
            if (unit.Length != 1 || !((unit[0] >= 'a' && unit[0] <= 'z') || (unit[0] >= 'A' && unit[0] <= 'Z')))
            { ViewBag.ValidationMessage = d["Unit must be a single letter."]; return View(viewName, c); }
            c.Unit = unit.ToUpper().Substring(0, 1);
            if (!int.TryParse(year, out int intYear) || intYear <= 0)
            { ViewBag.ValidationMessage = d["Year must be positive integer."]; return View(viewName, c); }
            c.Year = intYear;
            var teacherSearch = Db.Teacher.Where(e => e.Id == supervisorId);
            if (teacherSearch.Count() != 1) { ViewBag.ValidationMessage = d["Teacher does not exist."]; return View(viewName, c); }
            var supervisorSearch = Db.Class.Where(e => e.SupervisorId == supervisorId && e.Id != id);
            if (supervisorSearch.Count() != 0) { ViewBag.ValidationMessage = d["Teacher is already supervisor."]; return View(viewName, c); }
            c.SupervisorId = supervisorId;
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = c.Id });
        }

        [HttpPost]
        public ActionResult AddStudentToClass(int? id, string studentId)
        {
            var classSearch = Db.Class.Where(e => e.Id == id);
            if (classSearch.Count() != 1) return ErrorView("Such class does not exist.");
            var class_ = classSearch.Single();
            var studentSearch = Db.Student.Where(e => e.Id == studentId);
            if (studentSearch.Count() != 1) return ErrorView("Such student does not exist.");
            var student = studentSearch.Single();
            student.ClassId = class_.Id;
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = class_.Id });
        }

        [HttpPost]
        public ActionResult AddTeacherSubjectToClass(int? id, string teacherId, int? subjectId)
        {
            var classSearch = Db.Class.Where(e => e.Id == id);
            if (classSearch.Count() != 1) return ErrorView("Such class does not exist.");
            var class_ = classSearch.Single();
            var teacherSearch = Db.Teacher.Where(e => e.Id == teacherId);
            if (teacherSearch.Count() != 1) return ErrorView("Such teacher does not exist.");
            var teacher = teacherSearch.Single();
            var subjectSearch = Db.Subject.Where(e => e.Id == subjectId);
            if (subjectSearch.Count() != 1) return ErrorView("Such subject does not exist.");
            var subject = subjectSearch.Single();
            var tcsSearch = class_.TeacherClassSubjects.Where(e => e.SubjectId == subject.Id);
            if (tcsSearch.Count() != 0) return ErrorView("Such class already has this subject.");
            var tcs = new TeacherClassSubject { ClassId = class_.Id, TeacherId = teacher.Id, SubjectId = subject.Id };
            Db.TeacherClassSubject.Add(tcs);
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = class_.Id });
        }

        [HttpPost]
        public ActionResult AddLesson(int? id, int? teacherClassSubjectId, int? dayId, int? hourId, int? roomId)
        {
            var d = LocalizedStrings.Class.Edit[LanguageCookie.Read(Request.Cookies)];
            var classSearch = Db.Class.Where(e => e.Id == id);
            if (classSearch.Count() != 1) return ErrorView("Such class does not exist.");
            var class_ = classSearch.Single();
            var tcsSearch = Db.TeacherClassSubject.Where(e => e.Id == teacherClassSubjectId);
            if (tcsSearch.Count() != 1) return ErrorView("The teacher does not teach such subject in such class.");
            var tcs = tcsSearch.Single();
            if (dayId == null || dayId.Value >= Days.Array.Length) return ErrorView("Such day does not exist.");
            if (hourId == null || hourId.Value >= LessonHours.Array.Length) return ErrorView("Such lesson hour does not exist.");
            if (roomId == null || roomId.Value >= Rooms.Array.Length) return ErrorView("Such room does not exist.");
            // jeżeli dany nauczyciel ma już jakąś lekcję w danym dniu i godzinie, to nie nadpisujemy jej ani nie dodajemy nowej lekcji w tym samym czasie
            var lessonSearch = Db.Lesson.Where(e => e.TeacherClassSubject.TeacherId == tcs.TeacherId && e.DayId == dayId && e.HourId == hourId);
            if (lessonSearch.Count() != 0)
            { ViewBag.ValidationMessage = d["The teacher has another lesson at specified time."]; return View(class_); }
            var lesson = new Lesson { TeacherClassSubjectId = tcs.Id, DayId = dayId.Value, HourId = hourId.Value, RoomId = roomId.Value};
            Db.Lesson.Add(lesson);
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = class_.Id });
        }

        public JsonResult GetStudents()
        {
            var list = Db.Student.Where(e => e.ClassId == null).Select(r => new { r.Id, r.ApplicationUser.Email, r.ApplicationUser.Name, r.ApplicationUser.Surname });
            return Json(list);
        }

        public ActionResult RemoveStudentFromClass(string studentId)
        {
            var search = Db.Student.Where(e => e.Id == studentId);
            if (search.Count() != 1) return ErrorView("Such student does not exist.");
            var s = search.Single();
            var classId = s.ClassId;
            s.ClassId = null;
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = classId });
        }

        public ActionResult RemoveTeacherSubject(int? teacherClassSubjectId)
        {
            var search = Db.TeacherClassSubject.Where(e => e.Id == teacherClassSubjectId);
            if (search.Count() != 1) return ErrorView("The teacher does not teach such subject in such class.");
            var tcs = search.Single();
            var classId = tcs.ClassId;
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

        public ActionResult Delete(int? id)
        {
            var search = Db.Class.Where(e => e.Id == id);
            if (search.Count() != 1) return ErrorView("Such class does not exist.");
            var c = search.Single();
            foreach (var student in c.Students)
                student.ClassId = null;
            Db.SaveChanges();
            Db.Class.Remove(c);
            Db.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult DeleteLesson(int? lessonId)
        {
            var search = Db.Lesson.Where(e => e.Id == lessonId);
            if (search.Count() != 1) return ErrorView("Such lesson does not exist.");
            var l = search.Single();
            var classId = l.TeacherClassSubject.ClassId;
            Db.Lesson.Remove(l);
            Db.SaveChanges();
            return RedirectToAction("Edit", new { id = classId });
        }
    }
}
