using Gradebook.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Gradebook.Utils
{
    public class Role
    {
        public const string Administrator = "Administrator", Teacher = "Teacher", Parent = "Parent", Student = "Student";
        public const string AdministratorTeacher = "Administrator,Teacher";
        public const string AdministratorTeacherStudent = "Administrator,Teacher,Student";
        public const string TeacherParentStudent = "Teacher,Parent,Student";
        public const string TeacherStudent = "Teacher,Student";
        public static string[] All = new string[] { Administrator, Teacher, Parent, Student };

        public class Link
        {
            public string Name, Action, Controller;
            public object RouteValues;
            public Link(string name, string action, string controller, object routeValues = null)
            {
                Name = name;
                Action = action;
                Controller = controller;
                RouteValues = routeValues;
            }
        }

        public static Link[] GetLinks(System.Security.Principal.IPrincipal user)
        {
            var links = new Link[]
            {
                new Link("Announcements", "Index", "GlobalAnnouncement"), // 0
                new Link("Accounts", "Index", "Account"), // 1
                new Link("Subjects", "Index", "Subject"), // 2
                new Link("Classes", "Index", "Class"), // 3
                new Link("Grades", "Index", "Grade"), // 4
                new Link("Messages", "Index", "Message"), // 5
                new Link("Quizzes", "Index", "Quiz"), // 6
                new Link("Children", "Index", "Child"), // 7
                new Link("Absences", "Index", "Absence"), // 8
                new Link("Timetable", "Index", "Timetable"), // 9
            };
            if (user.IsInRole(Administrator))
                return new Link[] { links[0], links[1], links[2], links[3], links[5] };
            if (user.IsInRole(Teacher))
                return new Link[] { links[0], links[2], links[3], links[5], links[6], links[9] };
            if (user.IsInRole(Parent))
            {
                /*var db = ApplicationDbContext.Create();
                var parentId = user.Identity.GetUserId();
                var studentId = db.Student.Where(e => e.ParentId == parentId).Single().Id;*/
                var ret = new Link[] { links[0], links[7], links[5] };
                // ret[1].RouteValues = new { studentId = studentId };
                return ret;
            }
            if (user.IsInRole(Student))
            {
                var studentId = user.Identity.GetUserId();
                var ret = new Link[] { links[0], links[2], links[4], links[8], links[5], links[6], links[9] };
                ret[2].RouteValues = ret[3].RouteValues = new { studentId = studentId };
                return ret;
            }
            return new Link[] { links[0] };
        }
    }
}
