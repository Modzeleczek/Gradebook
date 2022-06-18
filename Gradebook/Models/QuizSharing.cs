using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class QuizSharing
    {
        [Key, Column(Order = 0)]
        public int ClassId { get; set; }
        [Key, Column(Order = 1)]
        public int QuizId { get; set; }
        public float GradeWeight { get; set; }

        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }
        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; }
    }
}