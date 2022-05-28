using System.Collections.Generic;

namespace Gradebook.Utils
{
    public class Comparer<T> : EqualityComparer<T>
    {
        public delegate bool BoolTT(T x, T y);
        public BoolTT EqualityHandler { get; set; }
        public delegate int IntT(T obj);
        public IntT HashCodeHandler { get; set; }

        public Comparer(BoolTT equalityHandler, IntT hashCodeHandler)
        {
            EqualityHandler = equalityHandler;
            HashCodeHandler = hashCodeHandler;
        }

        public override bool Equals(T x, T y)
        {
            return EqualityHandler(x, y);
        }

        public override int GetHashCode(T obj)
        {
            return HashCodeHandler(obj);
        }
    }
}
