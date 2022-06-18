using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class Absence
    {
        [Key]
        public int Id { get; set; }
        public string StudentId { get; set; }
        public DateTime Date { get; set; }
        public int LessonId { get; set; }
        public bool IsJustified { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
        [ForeignKey("LessonId")]
        public virtual Lesson Lesson { get; set; }
    }
}