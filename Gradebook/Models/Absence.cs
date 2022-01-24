using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gradebook.Models
{
    public class Absence
    {
        [Key]
        public int Id { get; set; }
        public string StudentId { get; set; }
        public DateTime Date { get; set; }
        [Range(1, 12)]
        public string LessonNumber { get; set; }
        public bool IsJustified { get; set; }

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
    }
}