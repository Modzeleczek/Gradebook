using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class TeacherClassSubject
    {
        [Key, Column(Order = 0)]
        public string TeacherId { get; set; }
        [Key, Column(Order = 1)]
        public int ClassId { get; set; }
        [Key, Column(Order = 2)]
        public int SubjectId { get; set; }

        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }
        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
    }
}