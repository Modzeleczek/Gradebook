using Gradebook.Models;
using System.Collections.Generic;

namespace Gradebook.Utils
{
    public class ComparerBySubject : EqualityComparer<TeacherClassSubject>
    {
        public override bool Equals(TeacherClassSubject x, TeacherClassSubject y)
        {
            return x.SubjectId == y.SubjectId;
        }

        public override int GetHashCode(TeacherClassSubject obj)
        {
            return obj.SubjectId.GetHashCode();
        }
    }
}
