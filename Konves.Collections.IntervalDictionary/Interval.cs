using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Konves.Collections
{
    //[DebuggerDisplay("[{LowerBound},{UpperBound}]")]
    // TODO: Add XML Documentation
    public class Interval<TBound> : IInterval<TBound> where TBound : IComparable<TBound>, IEquatable<TBound>
    {
        public Interval(TBound lowerBound, TBound upperBound)
            : this(lowerBound, BoundType.Inclusive, upperBound, BoundType.Inclusive) { }

        public Interval(TBound lowerBound, BoundType lowerBoundType, TBound upperBound, BoundType upperBoundType)
            : this(new Bound<TBound>(lowerBound, lowerBoundType), new Bound<TBound>(upperBound, upperBoundType)) { }

        public Interval(IBound<TBound> lowerBound, IBound<TBound> upperBound)
        {
            if (object.ReferenceEquals(lowerBound, null))
                throw new ArgumentNullException("lowerBound", "lower is null");

            if (object.ReferenceEquals(upperBound, null))
                throw new ArgumentNullException("upperBound", "upper is null");

            if (upperBound.Value.CompareTo(lowerBound.Value) < 0)
                throw new ArgumentOutOfRangeException("upperBound", "upperBound is less than lowerBound.");

            if (upperBound.Value.Equals(lowerBound.Value) && (lowerBound.Type == BoundType.Exclusive || upperBound.Type == BoundType.Exclusive))
                throw new ArgumentOutOfRangeException("upperBound", "upperBound is equal to lowerBound and one of the bounds is exclusive.");

            this.LowerBound = lowerBound;
            this.UpperBound = upperBound;
        }

        public IBound<TBound> LowerBound { get; private set; }

        public IBound<TBound> UpperBound { get; private set; }

        public bool Contains(TBound key)
        {
            switch (this.LowerBound.Type)
            {
                case BoundType.Inclusive:
                    switch (this.UpperBound.Type)
                    {
                        case BoundType.Inclusive:
                            return
                                this.LowerBound.Value.CompareTo(key) <= 0
                                && key.CompareTo(this.UpperBound.Value) <= 0;
                        case BoundType.Exclusive:
                            return
                                this.LowerBound.Value.CompareTo(key) <= 0
                                && key.CompareTo(this.UpperBound.Value) < 0;
                        default:
                            return false;
                    }
                case BoundType.Exclusive:
                    switch (this.UpperBound.Type)
                    {
                        case BoundType.Inclusive:
                            return
                                this.LowerBound.Value.CompareTo(key) < 0
                                && key.CompareTo(this.UpperBound.Value) <= 0;
                        case BoundType.Exclusive:
                            return
                                this.LowerBound.Value.CompareTo(key) < 0
                                && key.CompareTo(this.UpperBound.Value) < 0;
                        default:
                            return false;
                    }
                default:
                    return false;
            }
        }

        public bool Contains(IBound<TBound> bound)
        {
            return CompareTo(bound) == 0;
        }

        public bool Intersects(IInterval<TBound> other)
        {
            return
                this.Contains(other.LowerBound)
                || other.Contains(this.LowerBound);
        }

        public bool IsSubsetOf(IInterval<TBound> other)
        {
            return
                other.Contains(this.LowerBound)
                && other.Contains(this.UpperBound);
        }

        public bool IsSupersetOf(IInterval<TBound> other)
        {
            return
                this.Contains(other.LowerBound)
                && this.Contains(other.UpperBound);
        }

        public IInterval<TBound> Intersect(IInterval<TBound> other)
        {
            if (!this.Intersects(other))
                throw new ArgumentException("other", "other and this instance do not intersect.");

            return
                new Interval<TBound>(
                    Bound<TBound>.Max(this.LowerBound, other.LowerBound),
                    Bound<TBound>.Min(this.UpperBound, other.UpperBound)
                    );
        }

        public IInterval<TBound> Union(IInterval<TBound> other)
        {
            if (!this.Intersects(other))
                throw new ArgumentException("other", "other and this instance do not intersect.");

            return
                new Interval<TBound>(
                    Bound<TBound>.Min(this.LowerBound, other.LowerBound),
                    Bound<TBound>.Max(this.UpperBound, other.UpperBound)
                    );
        }

        public IInterval<TBound> Subtract(IInterval<TBound> other)
        {
            if (!this.Intersects(other))
                throw new ArgumentException("other", "other and this instance do not intersect.");

            IBound<TBound> lower = Bound<TBound>.Max(this.LowerBound, other.LowerBound);
            IBound<TBound> upper = Bound<TBound>.Min(this.UpperBound, other.UpperBound);

            return new Interval<TBound>(
                lower.Value, lower.Type == BoundType.Inclusive ? BoundType.Exclusive : lower.Type,
                upper.Value, upper.Type == BoundType.Inclusive ? BoundType.Exclusive : upper.Type
                );
        }

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>
        /// A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance is less than <paramref name="obj"/>. Zero This instance is equal to <paramref name="obj"/>. Greater than zero This instance is greater than <paramref name="obj"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="obj"/> is not the same type as this instance. </exception>
        public int CompareTo(object obj)
        {
            if (!(obj is Interval<TBound>))
                throw new ArgumentException("obj", "obj is not of same type as this instance.");
            else
                return CompareTo(obj as Interval<TBound>);
        }

        public int CompareTo(TBound key)
        {
            // TODO: consider refactoring into a switch statement

            int lres = key.CompareTo(this.LowerBound.Value);
            int ures = key.CompareTo(this.UpperBound.Value);

            if (lres < 0)
                return -1;
            if (ures > 0)
                return 1;
            else if (lres == 0 && this.LowerBound.Type == BoundType.Exclusive)
                return -1;
            else if (ures == 0 && this.UpperBound.Type == BoundType.Exclusive)
                return 1;
            else
                return 0;
        }

        public int CompareTo(IBound<TBound> bound)
        {
            // TODO: consider refactoring into a switch statement

            int lres = bound.Value.CompareTo(this.LowerBound.Value);
            int ures = bound.Value.CompareTo(this.UpperBound.Value);

            if (lres < 0)
                return -1;
            if (ures > 0)
                return 1;
            else if (bound.Type == BoundType.Exclusive)
                return 0;
            else if (lres == 0 && this.LowerBound.Type == BoundType.Exclusive)
                return -1;
            else if (ures == 0 && this.UpperBound.Type == BoundType.Exclusive)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// Compares the current object with another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
        /// Value
        /// Meaning
        /// Less than zero
        /// This object is less than the <paramref name="other"/> parameter.
        /// Zero
        /// This object is equal to <paramref name="other"/>.
        /// Greater than zero
        /// This object is greater than <paramref name="other"/>.
        /// </returns>
        public int CompareTo(IInterval<TBound> other)
        {
            int result = this.LowerBound.Value.CompareTo(other.LowerBound.Value);

            if (result == 0)
            {
                if (this.LowerBound.Type == BoundType.Exclusive && other.LowerBound.Type == BoundType.Inclusive)
                    return -1;
                else if (this.LowerBound.Type == BoundType.Inclusive && other.LowerBound.Type == BoundType.Exclusive)
                    return 1;
                else
                    return 0;
            }
            else
                return result;
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        public bool Equals(IInterval<TBound> other)
        {
            return
                this.LowerBound.Equals(other.LowerBound)
                && this.UpperBound.Equals(other.UpperBound);
        }

        

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(this.LowerBound.Type == BoundType.Exclusive ? '(' : '[');

            sb.Append(this.LowerBound.Value);

            sb.Append(',');

            sb.Append(this.UpperBound.Value);

            sb.Append(this.UpperBound.Type == BoundType.Exclusive ? ')' : ']');

            return sb.ToString();
        }
    }
}
