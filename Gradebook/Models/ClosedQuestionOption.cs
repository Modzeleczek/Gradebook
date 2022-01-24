using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class ClosedQuestionOption
    {
        [Key]
        public int Id { get; set; }
        public int? ClosedQuestionId { get; set; }
        public string Content { get; set; }
        public bool IsCorrect { get; set; }

        [ForeignKey("ClosedQuestionId")]
        public virtual ClosedQuestion ClosedQuestion { get; set; }
        public virtual ICollection<ClosedQuestionAnswer> Answers { get; set; }
    }
}