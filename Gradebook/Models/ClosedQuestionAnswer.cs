using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class ClosedQuestionAnswer
    {
        [Key, Column(Order = 0)]
        public int QuizAttemptId { get; set; }
        [Key, Column(Order = 1)]
        public int SelectedOptionId { get; set; }

        [ForeignKey("QuizAttemptId")]
        public virtual QuizAttempt QuizAttempt { get; set; }
        [ForeignKey("SelectedOptionId")]
        public virtual ClosedQuestionOption SelectedOption { get; set; }
    }
}