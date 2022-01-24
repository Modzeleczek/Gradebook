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
    [ViewFilter]
    public class GradeController : Controller
    {
        private ApplicationDbContext Db = ApplicationDbContext.Create();

        public struct SubjectGrades
        {
            public string SubjectName;
            public LinkedList<Grade> Grades;
            public SubjectGrades(string subjectName)
            {
                SubjectName = subjectName;
                Grades = new LinkedList<Grade>();
            }
        }

        public static IEnumerable<SubjectGrades> GroupGradesBySubject(IEnumerable<Grade> grades, IEnumerable<Subject> subjects)
        {
            var list = new LinkedList<SubjectGrades>();
            foreach (var subject in subjects)
            {
                list.AddLast(new SubjectGrades(subject.Name));
                foreach (var grade in grades)
                {
                    if (grade.SubjectId == subject.Id)
                        list.Last.Value.Grades.AddLast(grade);
                }
            }
            return list;
        }

        [Authorize(Roles = Role.AdministratorTeacherStudent)]
        public ActionResult Index(string studentId)
        {
            IEnumerable<Grade> grades = null;
            IEnumerable<Subject> subjects = null;
            if (User.IsInRole(Role.Administrator))
            {
                grades = Db.Grade.ToArray();
                subjects = Db.Subject.ToArray();
            }
            else if (User.IsInRole(Role.Teacher))
            {
                if (studentId == null)
                    return RedirectToAction("Index", "Class");
                var teacherId = User.Identity.GetUserId();
                var teacher = Db.Teacher.Where(e => e.Id == teacherId).Single();
                var teacherClassSubjects = teacher.TeacherClassSubjects.Distinct(new ComparerBySubject());
                var teacherSubjectIds = teacherClassSubjects.Select(e => e.SubjectId);
                grades = Db.Grade.Where(e => e.StudentId == studentId && teacherSubjectIds.Contains(e.SubjectId)).ToArray();
                subjects = teacherClassSubjects.Select(e => e.Subject).ToArray();
                ViewBag.StudentId = studentId;
            }
            /* else if (User.IsInRole(Role.Parent))
            {
                var parentId = User.Identity.GetUserId();
                var child = Db.Student.Where(e => e.ParentId == parentId).Single();
                var childId = child.Id;
                grades = Db.Grade.Where(e => e.StudentId == childId).ToArray();
                var _class = child.Class;
                var list = new LinkedList<Subject>();
                foreach (var tcs in _class.TeacherClassSubjects)
                    list.AddLast(tcs.Subject);
                subjects = list;
            } */
            else if (User.IsInRole(Role.Student))
            {
                var id = User.Identity.GetUserId();
                grades = Db.Grade.Where(e => e.StudentId == id).ToArray();
                var student = Db.Student.Where(e => e.Id == id).Single();
                var _class = student.Class;
                var list = new LinkedList<Subject>();
                foreach (var tcs in _class.TeacherClassSubjects)
                    list.AddLast(tcs.Subject);
                subjects = list;
            }
            var gradesGrouped = GroupGradesBySubject(grades, subjects);
            return View(gradesGrouped);
        }

        private LinkedList<SelectListItem> GetSubjects(string studentId, string teacherId)
        {
            var teacher = Db.Teacher.Where(e => e.Id == teacherId).Single();
            var teacherClassSubjects = teacher.TeacherClassSubjects.Distinct(new ComparerBySubject());
            var teacherSubjects = teacherClassSubjects.Select(e => e.Subject);
            var student = Db.Student.Where(e => e.Id == studentId).Single();
            var studentSubjects = student.Class.TeacherClassSubjects.Select(e => e.Subject);
            var subjects = teacherSubjects.Intersect(studentSubjects);
            var list = new LinkedList<SelectListItem>();
            foreach (var s in subjects)
                list.AddLast(new SelectListItem { Text = s.Name, Value = s.Id.ToString(), Selected = false });
            return list;
        }

        [Authorize(Roles = Role.Teacher)]
        public ActionResult Create(string studentId)
        {
            // ViewBag.Values = new float[] { 1, 1.5F, 2, 2.5F, 3, 3.5F, 4, 4.5F, 5, 5.5F, 6 };
            var teacherId = User.Identity.GetUserId();
            var subjects = GetSubjects(studentId, teacherId);
            if (subjects.First != null)
                subjects.First.Value.Selected = true;
            ViewBag.Subjects = subjects;
            ViewBag.StudentId = studentId;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Role.Teacher)]
        public ActionResult Create(string studentId, Grade grade)
        {
            var teacherId = User.Identity.GetUserId();
            if (ModelState.IsValid == false)
            {
                var subjects = GetSubjects(studentId, teacherId);
                foreach (var s in subjects)
                    if (s.Value == grade.SubjectId.ToString())
                    {
                        s.Selected = true;
                        break;
                    }
                ViewBag.StudentId = studentId;
                return View();
            }
            grade.StudentId = studentId;
            grade.TeacherId = teacherId;
            grade.ModificationTime = DateTime.Now;
            Db.Grade.Add(grade);
            Db.SaveChanges();
            return RedirectToAction("Index", new { studentId = studentId });
        }

        [Authorize(Roles = Role.Teacher)]
        public ActionResult Delete(int id)
        {
            var g = Db.Grade.Where(e => e.Id == id).Single();
            var studentId = g.StudentId;
            Db.Grade.Remove(g);
            Db.SaveChanges();
            return RedirectToAction("Index", new { studentId = studentId });
        }
    }
}
