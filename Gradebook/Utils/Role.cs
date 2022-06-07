﻿using System;

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

        public static NamedLink[] GetLinks(System.Security.Principal.IPrincipal user)
        {
            if (user.IsInRole(Administrator))
            {
                const string area = "Admin";
                return new NamedLink[]
                {
                    new NamedLink("Announcements", "List", "GlobalAnnouncement", ""),
                    new NamedLink("Messages", "List", "Message", ""),
                    new NamedLink("Accounts", "List", "Account", area),
                    new NamedLink("Subjects", "List", "Subject", area),
                    new NamedLink("Classes", "List", "Class", area)
                };
            }
            if (user.IsInRole(Teacher))
            {
                const string area = "Teacher";
                return new NamedLink[]
                {
                    new NamedLink("Announcements", "List", "GlobalAnnouncement", ""),
                    new NamedLink("Messages", "List", "Message", ""),
                    new NamedLink("Subjects", "List", "Subject", area),
                    new NamedLink("Classes", "List", "Class", area),
                    new NamedLink("Quizzes", "List", "Quiz", area),
                    new NamedLink("Timetable", "List", "Timetable", area),
                    new NamedLink("Appointments", "List", "Appointment", area)
                };
            }
            if (user.IsInRole(Parent))
            {
                const string area = "Parent";
                return new NamedLink[]
                {
                    new NamedLink("Announcements", "List", "GlobalAnnouncement", ""),
                    new NamedLink("Messages", "List", "Message", ""),
                    new NamedLink("Children", "List", "Child", area)
                };
            }
            if (user.IsInRole(Student))
            {
                const string area = "Student";
                return new NamedLink[]
                {
                    new NamedLink("Announcements", "List", "GlobalAnnouncement", ""),
                    new NamedLink("Messages", "List", "Message", ""),
                    new NamedLink("Subjects", "List", "Subject", area),
                    new NamedLink("Class", "Details", "Class", area),
                    new NamedLink("Grades", "List", "Grade", area),
                    new NamedLink("Absences", "List", "Absence", area),
                    new NamedLink("Quizzes", "List", "Quiz", area),
                    new NamedLink("Timetable", "List", "Timetable", area),
                    new NamedLink("Appointments", "List", "Appointment", area)
                };
            }
            {
                const string area = "";
                return new NamedLink[] { new NamedLink("Announcements", "List", "GlobalAnnouncement", area) };
            }
        }
    }
}
