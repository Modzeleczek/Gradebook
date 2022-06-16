using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class Quiz
    {
        [Key]
        public int Id { get; set; }
        public int? SubjectId { get; set; }
        public string AuthorId { get; set; }
        [MinLength(1)]
        public string Name { get; set; }
        [Range(1, int.MaxValue)]
        public int Duration { get; set; }
        public DateTime ModificationTime { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        [ForeignKey("AuthorId")]
        public virtual Teacher Author { get; set; }
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; }
        public virtual ICollection<ClosedQuestion> ClosedQuestions { get; set; }
        public virtual ICollection<QuizSharing> QuizSharings { get; set; }
    }
}