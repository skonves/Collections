using System;
using System.Diagnostics;

namespace Konves.Collections
{
    /// <summary>
    /// Defines a interval/value pair that can be set or retrieved.
    /// </summary>
    /// <typeparam name="TBound">The type of the interval bounds.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public struct IntervalValuePair<TBound, TValue>
        where TBound : IComparable<TBound>, IEquatable<TBound>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntervalValuePair&lt;TBound,TValue&gt;"/> structure
        /// with the specified <see cref="interval"/> and <see cref="value"/>.
        /// </summary>
        /// <param name="interval">The object defined in each interval/value pair.</param>
        /// <param name="value">The definition of the associated <see cref="interval"/>.</param>
        public IntervalValuePair(IInterval<TBound> interval, TValue value) : this()
        {
            this.Interval = interval;
            Value = value;
        }

        /// <summary>
        /// Gets the interval in the interval/value pair.
        /// </summary>
        /// <value>
        /// A <see cref="IInterval&lt;TBound&gt;"/> that is the interval of the <see cref="IntervalValuePair&lt;TBound,TValue&gt;"/>.
        /// </value>
        public IInterval<TBound> Interval { get; private set; }
        /// <summary>
        /// Gets the value in the interval/value pair.
        /// </summary>
        /// <value>
        /// A <typeparamref name="TValue"/> that is the value of the <see cref="IntervalValuePair&lt;TBound,TValue&gt;"/>.
        /// </value>
        public TValue Value { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Interval.ToString(), Value);
        }
    }
}
