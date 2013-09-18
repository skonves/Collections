using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konves.Collections
{
    /// <summary>
    /// Defines an interval between two bounds.
    /// </summary>
    /// <typeparam name="TBound">The type of the bounds.</typeparam>
    public interface IInterval<TBound>
        : IComparable, IComparable<IInterval<TBound>>, IEquatable<IInterval<TBound>>, IComparable<IBound<TBound>>, IComparable<TBound>
        where TBound : IComparable<TBound>, IEquatable<TBound>
    {
        /// <summary>
        /// Gets the lower bound of this interval.
        /// </summary>
        IBound<TBound> LowerBound { get; }
        /// <summary>
        /// Gets the upper bound of this interval.
        /// </summary>
        IBound<TBound> UpperBound { get; }

        /// <summary>
        /// Determines whether the specified <paramref name="key"/> lies between or is equal to
        /// either the <see cref="P:LowerBound"/> and/or <see cref="P:UpperBound"/> of this interval.
        /// </summary>
        /// <param name="key">The key of type <typeparamref name="TBound"/>.</param>
        /// <returns>
        ///   <c>true</c> if the <paramref name="key"/> lies between or is equal to either of the bounds; otherwise, <c>false</c>.
        /// </returns>
        bool Contains(TBound key);
        bool Contains(IBound<TBound> bound);

        /// <summary>
        /// Determines whether the specified interval intersects this instance.
        /// </summary>
        /// <param name="other">The other interval.</param>
        /// <returns>
        ///   <c>true</c> if the interval intersects this instance; otherwise, <c>false</c>.
        /// </returns>
        bool Intersects(IInterval<TBound> other);

        /// <summary>
        /// Determines whether this instance is a subset of the specified other interval.
        /// </summary>
        /// <param name="other">The other interval.</param>
        /// <returns>
        ///   <c>true</c> if this instance is a subset of the specified other interval; otherwise, <c>false</c>.
        /// </returns>
        bool IsSubsetOf(IInterval<TBound> other);
        /// <summary>
        /// Determines whether this instance is a superset of the specified other interval.
        /// </summary>
        /// <param name="other">The other interval.</param>
        /// <returns>
        ///   <c>true</c> if this instance is a superset of the specified other interval; otherwise, <c>false</c>.
        /// </returns>
        bool IsSupersetOf(IInterval<TBound> other);

        /// <summary>
        /// Gets an interval representing the intersection of this instance and the specified other interval.
        /// </summary>
        /// <param name="other">The other interval.</param>
        /// <returns>
        /// Returns an <see cref="IInterval&lt;TBound&gt;"/> representing the intersection of the two intervals.
        /// </returns>
        IInterval<TBound> Intersect(IInterval<TBound> other);
        /// <summary>
        /// Gets an interval representing the union of this instance and the specified other interval.
        /// </summary>
        /// <param name="other">The other interval.</param>
        /// <returns>
        /// Returns an <see cref="IInterval&lt;TBound&gt;"/> representing the union of the two intervals.
        /// </returns>
        IInterval<TBound> Union(IInterval<TBound> other);
        /// <summary>
        /// Gets an interval representing the subtraction of the other specified interval and this instance.
        /// </summary>
        /// <param name="other">The other interval.</param>
        /// <returns>
        /// Returns an <see cref="IInterval&lt;TBound&gt;"/> representing the subtraction of the other interval from this instance.
        /// </returns>
        IInterval<TBound> Subtract(IInterval<TBound> other);
    }
}
