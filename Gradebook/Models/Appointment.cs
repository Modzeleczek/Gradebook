using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public int TeacherClassSubjectId { get; set; } // not null

        [ForeignKey("TeacherClassSubjectId")]
        public virtual TeacherClassSubject TeacherClassSubject { get; set; }
    }
}