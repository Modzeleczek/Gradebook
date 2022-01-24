using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class Class
    {
        [Key]
        public int Id { get; set; }
        public string SupervisorId { get; set; }
        [Range(1, int.MaxValue)]
        public int Year { get; set; }
        [Column(TypeName = "CHAR"), StringLength(maximumLength: 1, MinimumLength = 1), Required]
        public string Unit { get; set; }

        [ForeignKey("SupervisorId")]
        public virtual Teacher Supervisor { get; set; }
        public virtual ICollection<TeacherClassSubject> TeacherClassSubjects { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<QuizSharing> QuizSharings { get; set; }
    }
}