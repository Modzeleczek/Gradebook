using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class TeacherClassSubject
    {
        [Key]
        public int Id { get; set; }
        public string TeacherId { get; set; }
        public int ClassId { get; set; }
        public int SubjectId { get; set; }

        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }
        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}