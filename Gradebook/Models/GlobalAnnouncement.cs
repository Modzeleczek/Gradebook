using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class GlobalAnnouncement
    {
        [Key]
        public int Id { get; set; }
        public string AuthorId { get; set; }
        public DateTime ModificationTime { get; set; }
        public string Content { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Administrator Author { get; set; }
    }
}