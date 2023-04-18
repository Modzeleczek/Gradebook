using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gradebook.Models
{
    public class Subject
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        // Binary (byte) string of a PDF file with syllabus.
        public string Syllabus { get; set; }

        public virtual ICollection<TeacherClassSubject> TeacherClassSubjects { get; set; }
        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<Quiz> Quizzes { get; set; }
        public virtual ICollection<File> Files { get; set; }
    }
}