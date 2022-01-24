using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class MessageRecipient
    {
        [Key, Column(Order = 0)]
        public int MessageId { get; set; }
        [Key, Column(Order = 1)]
        public string RecipientId { get; set; }

        [ForeignKey("MessageId")]
        public virtual Message Message { get; set; }
        [ForeignKey("RecipientId")]
        public virtual ApplicationUser Recipient { get; set; }
    }
}