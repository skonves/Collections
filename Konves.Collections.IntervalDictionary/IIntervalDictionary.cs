using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Konves.Collections
{
    /// <summary>
    /// Represents a generic collection of interval/value pairs
    /// </summary>
    /// <typeparam name="TBound">The type of the bounds of the intervals in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    public interface IIntervalDictionary<TBound, TValue>
        : ICollection<IntervalValuePair<TBound, TValue>>
        where TBound : IComparable<TBound>, IEquatable<TBound>
    {
        /// <summary>
        /// Gets an <see cref="ICollection&lt;IInterval&lt;TBound&gt;&gt;"/>
        /// containing the intervals of the <see cref="IIntervalDictionary&lt;TBound,TValue&gt;"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="ICollection&lt;IInterval&lt;TBound&gt;&gt;"/> containing the intervals of the object that implements <see cref="Konves.Collections.IIntervalDictionary&lt;TBound,TValue&gt;"/>.
        /// </returns>
        ICollection<IInterval<TBound>> Intervals { get; }
        /// <summary>
        /// Gets an <see cref="System.Collections.Generic.ICollection&lt;TValue&gt;"/> containing the values of the <see cref="Konves.Collections.IIntervalDictionary&lt;TBound,TValue&gt;"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="System.Collections.Generic.ICollection&lt;TValue&gt;"/> containing the values in the object that implements <see cref="Konves.Collections.IIntervalDictionary&lt;TBound,TValue&gt;"/>.
        /// </returns>
        ICollection<TValue> Values { get; }

        /// <summary>
        /// Gets or sets the element with the specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval">
        /// The interval of the element to get or set.
        /// </param>
        /// <returns>
        /// The element with the specified <paramref name="interval"/>.
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="key"/> is <c>null</c>.</exception>
        /// 
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// The property is retrieved and <paramref name="interval"/> is not found.</exception>
        /// 
        /// <exception cref="System.NotSupportedException">
        /// The property is set and the <see cref="IIntervalDictionary&lt;TBound,TValue&gt;"/>
        /// is read-only.</exception>
        TValue this[IInterval<TBound> interval] { get; set; }
        /// <summary>
        /// Gets or sets the element whose interval contains the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">
        /// A value of type <typeparamref name="TBound"/> that is contained within the interval of the element to get or set.
        /// </param>
        /// <returns>
        /// The element whose interval contains the specified <paramref name="key"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="key"/> is <c>null</c>.</exception>
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// The property is retrieved and an element containing <paramref name="key"/> is not found.</exception>
        /// <exception cref="System.NotSupportedException">
        /// The property is set and the <see cref="Konves.Collections.IIntervalDictionary&lt;TBound,TValue&gt;"/>
        /// is read-only.</exception>
        TValue this[TBound key] { get; set; }

        /// <summary>
        /// Adds an element with the provided <paramref name="interval"/> and <paramref name="value"/> to the <see cref="Konves.Collections.IIntervalDictionary&lt;TBound,TValue&gt;"/>.
        /// </summary>
        /// <param name="interval">The object to use as the interval of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="key"/> is <c>null</c>.</exception>
        /// 
        /// <exception cref="System.ArgumentException">
        ///   An element with the same or intersecting interval already exists in the <see cref="Konves.Collections.IIntervalDictionary&lt;TBound,TValue&gt;"/>.</exception>
        ///   
        /// <exception cref="System.NotSupportedException">
        ///   The <see cref="IIntervalDictionary&lt;TBound,TValue&gt;"/> is read-only.</exception>
        void Add(IInterval<TBound> interval, TValue value);

        /// <summary>
        /// Determines whether the <see cref="IIntervalDictionary&lt;TBound,TValue&gt;"/>
        /// contains an element with the specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval">The interval to locate in the <see cref="IIntervalDictionary&lt;TBound,TValue&gt;"/>.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="IIntervalDictionary&lt;TBound,TValue&gt;"/>
        /// contains an element with the interval; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="interval"/> is <c>null</c>.</exception>
        bool ContainsInterval(IInterval<TBound> interval);
        /// <summary>
        /// Determines whether the <see cref="IIntervalDictionary&lt;TBound,TValue&gt;"/>
        /// contains an element whose interval contains the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">A value of type <typeparamref name="TBound"/> that
        /// is contained within the interval of the element to locate in
        /// the <see cref="IIntervalDictionary&lt;TBound,TValue&gt;"/>.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="IIntervalDictionary&lt;TBound,TValue&gt;"/>
        /// contains an element whose interval contains the specified <paramref name="key"/>;
        /// otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="key"/> is <c>null</c>.</exception>
        bool ContainsKey(TBound key);

        /// <summary>
        /// Removes the element with the specified <paramref name="interval"/> from
        /// the <see cref="IIntervalDictionary&lt;TBound,TValue&gt;"/>.
        /// </summary>
        /// <param name="interval">The interval of the element to remove.</param>
        /// <returns>
        /// <c>true</c> if the element is successfully removed; otherwise, <c>false</c>.
        /// This method also returns false if the <paramref name="interval"/> was not found
        /// in the original <see cref="IIntervalDictionary&lt;TBound,TValue&gt;"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="interval"/> is <c>null</c>.</exception>
        ///   
        /// <exception cref="System.NotSupportedException">
        ///   The <see cref="IIntervalDictionary&lt;TBound,TValue&gt;"/> is read-only.</exception>
        bool Remove(IInterval<TBound> interval);
        /// <summary>
        /// Removes the element whose interval contains the specified <paramref name="key"/> from
        /// the <see cref="IIntervalDictionary&lt;TBound,TValue&gt;"/>.
        /// </summary>
        /// <param name="key">The key contained by the interval of the element to remove.</param>
        /// <returns>
        /// <c>true</c> if the element is successfully removed; otherwise, <c>false</c>.
        /// This method also returns false if the <paramref name="key"/> was not found
        /// in the original <see cref="IIntervalDictionary&lt;TBound,TValue&gt;"/>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="key"/> is <c>null</c>.</exception>
        ///   
        /// <exception cref="System.NotSupportedException">
        ///   The <see cref="IIntervalDictionary&lt;TBound,TValue&gt;"/> is read-only.</exception>
        bool Remove(TBound key);

        /// <summary>
        /// Attempts to get the element with the specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval">The interval of the element to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified interval
        /// if the interval is found; otherwise, the default value of the type of the value parameter.
        /// This parameter is passed uninitialized.</param>
        /// <returns>
        /// <c>true</c> if the object that implements <see cref="IIntervalDictionary&lt;TBound,TValue&gt;"/>
        /// contains an element with the specified interval; otherwise, false.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="interval"/> is <c>null</c>.</exception>
        bool TryGetValue(IInterval<TBound> interval, out TValue value);
        /// <summary>
        /// Attempts to get the element whose interval contains the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key contained by the interval of the element to get.</param>
        /// <param name="value">When this method returns, the value associated with the interval which
        /// contains the specified key if the interval is found; otherwise, the default value
        /// of the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>
        /// <c>true</c> if the object that implements <see cref="IIntervalDictionary&lt;TBound,TValue&gt;"/>
        /// contains an element whose interval contains the specified <paramref name="key"/>; otherwise, false.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="key"/> is <c>null</c>.</exception>
        bool TryGetValue(TBound key, out TValue value);
    }
}
