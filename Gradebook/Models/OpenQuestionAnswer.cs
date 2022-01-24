using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class OpenQuestionAnswer
    {
        [Key, Column(Order = 0)]
        public int QuizAttemptId { get; set; }
        [Key, Column(Order = 1)]
        public int OpenQuestionId { get; set; }
        public string Content { get; set; }

        [ForeignKey("QuizAttemptId")]
        public virtual QuizAttempt QuizAttempt { get; set; }
        [ForeignKey("OpenQuestionId")]
        public virtual OpenQuestion OpenQuestion { get; set; }
    }
}