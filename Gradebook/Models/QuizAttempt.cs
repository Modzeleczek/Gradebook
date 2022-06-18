using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class QuizAttempt
    {
        [Key]
        public int Id { get; set; }
        public int? QuizId { get; set; }
        public string DoerId { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public int? GradeId { get; set; }

        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; }
        [ForeignKey("DoerId")]
        public virtual Student Doer { get; set; }
        [ForeignKey("GradeId")]
        public virtual Grade Grade { get; set; }
        public virtual ICollection<ClosedQuestionAnswer> ClosedQuestionAnswers { get; set; }
    }
}