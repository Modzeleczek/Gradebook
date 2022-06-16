using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class Lesson
    {
        [Key]
        public int Id { get; set; }
        public int DayId { get; set; }
        public int HourId { get; set; }
        public int RoomId { get; set; }
        public int TeacherClassSubjectId { get; set; }

        [ForeignKey("TeacherClassSubjectId")]
        public virtual TeacherClassSubject TeacherClassSubject { get; set; }
        public virtual ICollection<Absence> Absences { get; set; }
    }
}