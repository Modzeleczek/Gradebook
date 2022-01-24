using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class Student
    {
        [Key]
        public string Id { get; set; }
        public string ParentId { get; set; }
        public int? ClassId { get; set; }
        // public int? SecondForeignLanguageGroupId { get; set; }

        [ForeignKey("Id")]
        public virtual ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("ParentId")]
        public virtual Parent Parent { get; set; }
        [ForeignKey("ClassId")]
        public virtual Class Class { get; set; }
        /*[ForeignKey("SecondForeignLanguageGroupId")]
        public virtual SecondForeignLanguageGroup SecondForeignLanguageGroup { get; set; }*/
        public virtual ICollection<QuizAttempt> QuizAttempts { get; set; }
    }
}