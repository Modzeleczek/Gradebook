using Gradebook.Controllers;
using Gradebook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gradebook.Utils
{
    public static class GradeSheetGenerator
    {
        public static string GenerateHtml(string parentId)
        {
            var db = ApplicationDbContext.Create();
            var parent = db.Parent.Where(e => e.Id == parentId).Single();
            var students = db.Student.Where(e => e.ParentId == parentId).ToArray();
            var sb = new System.Text.StringBuilder();
            sb.Append($"Oceny dzieci rodzica {parent.ApplicationUser.Name} {parent.ApplicationUser.Surname}.<br/><br/>");
            foreach (var s in students)
            {
                var grades = db.Grade.Where(e => e.StudentId == s.Id).ToArray();
                sb.Append($"<h2>{s.ApplicationUser.Name} {s.ApplicationUser.Surname}</h2>");
                var subjects = s.Class.TeacherClassSubjects.Select(e => e.Subject).ToArray();
                var subjectGrades = GradeController.GroupGradesBySubject(grades, subjects);
                foreach (var sg in subjectGrades)
                {
                    sb.Append($"<h4>{sg.SubjectName}</h4>");
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
    }
}