using Gradebook.Models;
using Gradebook.Models.ViewModels;
using Quartz;
using System;
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
                return $"Rodzic {parent.ApplicationUser.Name} {parent.ApplicationUser.Surname} nie posiada dzieci.";
            var students = studentSearch.ToArray();
            var sb = new System.Text.StringBuilder();
            sb.Append($"Zestawienie ocen dzieci rodzica {parent.ApplicationUser.Name} {parent.ApplicationUser.Surname} w okresie do {date}.<br/><br/>");
            foreach (var s in students)
            {
                var grades = Db.Grade.Where(e => e.StudentId == s.Id).ToArray();
                sb.Append($"<h2>{s.ApplicationUser.Name} {s.ApplicationUser.Surname}</h2>");
                if (s.ClassId == null)
                {
                    sb.Append($"<h4>Brak klasy</h4>");
                    continue;
                }
                var subjects = s.Class.TeacherClassSubjects.Select(e => e.Subject).ToArray();
                var subjectGrades = SubjectGrades.GroupGradesBySubject(grades, subjects);
                foreach (var sg in subjectGrades)
                {
                    sb.Append($"<h4>{sg.SubjectName}</h4>");
                    if (sg.Grades.Count == 0)
                    {
                        sb.Append("Brak ocen");
                        continue;
                    }
                    sb.Append("<table>");
                    sb.Append("<tr>");
                    sb.Append("<th>Wartość</th>");
                    sb.Append("<th>Waga</th>");
                    sb.Append("<th>Komentarz</th>");
                    sb.Append("<th>Czas modyfikacji</th>");
                    sb.Append("<th>Imię nauczyciela</th>");
                    sb.Append("<th>Nazwisko nauczyciela</th>");
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
            var subject = "Zestawienie ocen - " + date;
            foreach (var parent in parents)
            {
                var html = GenerateGradeSheet(parent, date);
                EmailSender.Send("noreply@gradebook.com", parent.ApplicationUser, subject, html, null, true);
            }
            return null;
        }
    }
}
