using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class Grade
    {
        [Key]
        public int Id { get; set; }
        [Range(1, 6)]
        public float Value { get; set; }
        public float Weight { get; set; }
        public string Comment { get; set; }
        public DateTime ModificationTime { get; set; }
        public string StudentId { get; set; }
        public string TeacherId { get; set; }
        public int SubjectId { get; set; } // not null

        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }
        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
    }
}