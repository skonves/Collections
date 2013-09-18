using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konves.Collections
{
    public sealed class Bound<T> : IBound<T> where T : IComparable<T>, IEquatable<T>
    {
        public Bound(T value, BoundType type)
        {
            this.Value = value;
            this.Type = type;
        }

        public T Value { get; private set; }

        public BoundType Type { get; private set; }

        public static IBound<T> Max(IBound<T> a, IBound<T> b)
        {
            int result = a.Value.CompareTo(b.Value);

            if (result < 0)
                return b;
            else if (result > 0)
                return a;
            else if (a.Type == BoundType.Exclusive)
                return b;
            else
                return a;
        }

        public static IBound<T> Min(IBound<T> a, IBound<T> b)
        {
            int result = a.Value.CompareTo(b.Value);

            if (result < 0)
                return a;
            else if (result > 0)
                return b;
            else if (a.Type == BoundType.Exclusive)
                return b;
            else
                return a;
        }
    }
}
