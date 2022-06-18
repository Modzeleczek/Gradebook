using System;
using System.ComponentModel.DataAnnotations;

namespace Gradebook.Models
{
    public class GlobalAnnouncement
    {
        [Key]
        public int Id { get; set; }
        public DateTime ModificationTime { get; set; }
        public string Content { get; set; }
    }
}