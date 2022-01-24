using Gradebook.Models;
using System.Collections.Generic;

namespace Gradebook.Utils
{
    public class ComparerById : EqualityComparer<Parent>
    {
        public override bool Equals(Parent x, Parent y)
        {
            return x.Id == y.Id;
        }

        public override int GetHashCode(Parent obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
