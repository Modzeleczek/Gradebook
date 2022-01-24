using System.Collections.Generic;

namespace Gradebook.Models
{
    public class ClosedQuestion : AbstractQuestion
    {
        public virtual ICollection<ClosedQuestionOption> Options { get; set; }
    }
}