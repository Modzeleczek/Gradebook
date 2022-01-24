using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Time")]
        public DateTime SendTime { get; set; }
        public string Content { get; set; }
        public string SenderId { get; set; }

        [ForeignKey("SenderId")]
        public virtual ApplicationUser Sender { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}