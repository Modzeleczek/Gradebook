using System.Collections.Generic;

namespace Gradebook.Models.ViewModels
{
    public class SubjectGrades
    {
        public string SubjectName;
        public LinkedList<Grade> Grades;
        public SubjectGrades(string subjectName)
        {
            SubjectName = subjectName;
            Grades = new LinkedList<Grade>();
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
    }
}
