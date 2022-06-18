using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gradebook.Models
{
    public class Attachment
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? MessageId { get; set; }
        public string FileType { get; set; }
        public string Data { get; set; }

        [ForeignKey("MessageId")]
        public virtual Message Message { get; set; }
    }
}