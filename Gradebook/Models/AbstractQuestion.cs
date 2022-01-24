using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public abstract class AbstractQuestion
    {
        [Key]
        public int Id { get; set; }
        public int? QuizId { get; set; }
        public string Content { get; set; }

        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; }
    }
}