#define ENGLISH
using Gradebook.Models;
using Gradebook.Models.ViewModels;
using Quartz;
using System;
#if !ENGLISH
using System.Collections.Generic;
#endif
using System.Linq;
using System.Threading.Tasks;

namespace Gradebook.Utils.PeriodicGradeSheet
{
    public class GradeEmailJob : IJob
    {
        ApplicationDbContext Db = ApplicationDbContext.Create();

        private string GenerateGradeSheet(Parent parent, string date)
        {
            var studentSearch = Db.Student.Where(e => e.ParentId == parent.Id);
            if (studentSearch.Count() == 0)
                return $"{D("Parent")} {parent.ApplicationUser.Name} {parent.ApplicationUser.Surname} {D("does not have children.")}";
            var students = studentSearch.ToArray();
            var sb = new System.Text.StringBuilder();
            sb.Append($"{D("Grade report of the children of parent")} {parent.ApplicationUser.Name} {parent.ApplicationUser.Surname} {D("to")} {date}.<br/><br/>");
            foreach (var s in students)
            {
                var grades = Db.Grade.Where(e => e.StudentId == s.Id).ToArray();
                sb.Append($"<h2>{s.ApplicationUser.Name} {s.ApplicationUser.Surname}</h2>");
                if (s.ClassId == null)
                {
                    sb.Append($"<h4>{D("No class")}</h4>");
                    continue;
                }
                var subjects = s.Class.TeacherClassSubjects.Select(e => e.Subject).ToArray();
                var subjectGrades = SubjectGrades.GroupGradesBySubject(grades, subjects);
                foreach (var sg in subjectGrades)
                {
                    sb.Append($"<h4>{sg.SubjectName}</h4>");
                    if (sg.Grades.Count == 0)
                    {
                        sb.Append(D("No grades"));
                        continue;
                    }
                    sb.Append("<table>");
                    sb.Append("<tr>");
                    sb.Append($"<th>{D("Value")}</th>");
                    sb.Append($"<th>{D("Weight")}</th>");
                    sb.Append($"<th>{D("Comment")}</th>");
                    sb.Append($"<th>{D("Modification time")}</th>");
                    sb.Append($"<th>{D("Teacher name")}</th>");
                    sb.Append($"<th>{D("Teacher surname")}</th>");
                    sb.Append("</tr>");
                    foreach (var grade in sg.Grades)
                    {
                        sb.Append("<tr>");
                        sb.Append($"<td>{grade.Value}</td>");
                        sb.Append($"<td>{grade.Weight}</td>");
                        sb.Append($"<td>{grade.Comment}</td>");
                        sb.Append($"<td>{grade.ModificationTime}</td>");
                        sb.Append($"<td>{grade.Teacher.ApplicationUser.Name}</td>");
                        sb.Append($"<td>{grade.Teacher.ApplicationUser.Surname}</td>");
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                }
            }
            return sb.ToString();
        }

        Task IJob.Execute(IJobExecutionContext context)
        {
            var parents = Db.Parent.ToArray();
            var today = DateTime.Now;
            var date = $"{today.Day}.{today.Month}.{today.Year}";
            var subject = D("Grade report - ") + date;
            foreach (var parent in parents)
            {
                var html = GenerateGradeSheet(parent, date);
                EmailSender.Send("noreply@gradebook.com", parent.ApplicationUser, subject, html, null, true);
            }
            return null;
        }

        private string D(string key)
        {
#if ENGLISH
            return key;
#else
            var d = new Dictionary<string, string>();
            d["Parent"] = "Rodzic";
            d["does not have children."] = "nie posiada dzieci.";
            d["Grade report of the children of parent"] = "Zestawienie ocen dzieci rodzica";
            d["to"] = "w okresie do";
            d["No class"] = "Brak klasy";
            d["No grades"] = "Brak ocen";
            d["Value"] = "Wartość";
            d["Weight"] = "Waga";
            d["Comment"] = "Komentarz";
            d["Modification time"] = "Czas modyfikacji";
            d["Teacher name"] = "Imię nauczyciela";
            d["Teacher surname"] = "Nazwisko nauczyciela";
            d["Grade report - "] = "Zestawienie ocen - ";
            return d[key];
#endif
        }
    }
}
