using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Konves.Collections
{
    /// <summary>
    /// Represents a collection of intervals and values.
    /// </summary>
    /// <typeparam name="TBound">The type of the bounds of the intervals in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    public sealed partial class IntervalDictionary<TBound, TValue> : IIntervalDictionary<TBound, TValue>
        where TBound : IComparable<TBound>, IEquatable<TBound>
    {
        private IntervalNode _root;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntervalDictionary&lt;TBound, TValue&gt;"/> class
        /// that is empty.
        /// </summary>
        public IntervalDictionary() { }

        /// <summary>
        /// Gets an collection containing the intervals of
        /// the <see cref="IntervalDictionary&lt;TBound,TValue&gt;"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="ICollection&lt;IInterval&lt;TBound&gt;&gt;"/> containing the
        /// intervals in the <see cref="IntervalDictionary&lt;TBound,TValue&gt;"/>.
        /// </returns>
        // TODO: define and return an IntervalCollection
        public ICollection<IInterval<TBound>> Intervals
        {
            get
            {
                if (_root == null)
                    return new List<IInterval<TBound>>();
                else
                    return _root.GetIntervals().ToList();
            }
        }
        /// <summary>
        /// Gets an collection containing the values of
        /// the <see cref="IntervalDictionary&lt;TBound,TValue&gt;"/>.
        /// </summary>
        /// <returns>
        /// An <see cref="ICollection&lt;IInterval&lt;TBound&gt;&gt;"/> containing the
        /// values in the <see cref="IntervalDictionary&lt;TBound,TValue&gt;"/>.
        /// </returns>
        // TODO: define and return an ValueCollection
        public ICollection<TValue> Values
        {
            get
            {
                if (_root == null)
                    return new List<TValue>();
                else
                    return _root.GetValues().ToList();
            }
        }

        /// <summary>
        /// Gets or sets the value associated with the specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval">The interval of the value to get or set.</param>
        /// <returns>
        /// The value associated with the specified <paramref name="interval"/>. If the
        /// specified <paramref name="interval"/> is not found, a get operation throws
        /// a <see cref="System.Collections.Generic.KeyNotFoundException"/>, and a set
        /// operation creates a new element with the specified <paramref name="interval"/>.
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="interval"/> is <c>null</c>.</exception>
        /// 
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// The property is retrieved and <paramref name="interval"/> does not exist in the collection.</exception>
        public TValue this[IInterval<TBound> interval]
        {
            get
            {
                if (object.ReferenceEquals(interval, null))
                    throw new ArgumentNullException("interval");

                if (_root == null)
                    throw new KeyNotFoundException();
                else
                {
                    TValue v;
                    if (_root.TryFind(interval, out v))
                        return v;
                    else
                        throw new KeyNotFoundException();
                }
            }
            set
            {
                if (object.ReferenceEquals(interval, null))
                    throw new ArgumentNullException("interval");

                if (_root == null)
                    this.Add(interval, value);
                else
                    //TODO: implement TrySetValue and then add new element if returns false.
                    _root.SetValue(interval, value);
            }
        }
        /// <summary>
        /// Gets or sets the value associated with the interval which contains the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key contained by an interval of the value to get or set.</param>
        /// <returns>
        /// The value associated with the interval which contains the specified <paramref name="interval"/>.
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="interval"/> is <c>null</c>.</exception>
        /// 
        /// <exception cref="System.Collections.Generic.KeyNotFoundException">
        /// The <paramref name="interval"/> does not exist in the collection.</exception>
        public TValue this[TBound key]
        {
            get
            {
                if (_root == null)
                    throw new KeyNotFoundException();
                else
                {
                    TValue v;
                    if (_root.TryFind(key, out v))
                        return v;
                    else
                        throw new KeyNotFoundException();
                }
            }
            set
            {
                if (_root == null)
                    throw new KeyNotFoundException();
                else
                    _root.SetValue(key, value);
            }
        }

        /// <summary>
        /// Adds the specified <paramref name="interval"/> and <paramref name="value"/> to the dictionary.
        /// </summary>
        /// <param name="interval">The interval of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be <c>null</c> for reference types.</param>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="interval"/> is <c>null</c>.</exception>
        ///   
        /// <exception cref="System.ArgumentException">
        ///   An element with the same or intersecting <paramref name="interval"/> already exists in the <see cref="IntervalDictionary&lt;TBound,TValue&gt;"/>.</exception>
        public void Add(IInterval<TBound> interval, TValue value)
        {
            if (object.ReferenceEquals(interval, null))
                throw new ArgumentNullException("interval");

            if (_root == null)
                _root = new IntervalNode(interval, value, null);
            else
            {
                if(!_root.TryInsert(interval, value))
                    throw new ArgumentException("interval", "interval exists or intersects an existing interval");
            }

            IntervalNode.Balance(ref _root);
        }
        /// <summary>
        /// Adds the interval defined by the specified <paramref name="lower"/> and <paramref name="upper"/>
        /// bounds and a <paramref name="value"/> to the dictionary.
        /// </summary>
        /// <param name="lower">The lower bound of the interval of the element to add.</param>
        /// <param name="upper">The lower upper of the interval of the element to add.</param>
        /// <param name="value">The value of the element to add. The value can be <c>null</c> for reference types.</param>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="lower"/> is <c>null</c>.</exception>
        ///   
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="upper"/> is <c>null</c>.</exception>
        ///   
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///   <paramref name="upper"/> is less than <paramref name="upper"/>.</exception>
        ///   
        /// <exception cref="System.ArgumentException">
        /// An element with an interval equivalent to or intersecting the interval
        /// defined by the specified <paramref name="lower"/> and <paramref name="upper"/> 
        /// bounds already exists in the <see cref="IntervalDictionary&lt;TBound,TValue&gt;"/>.</exception>
        public void Add(TBound lower, TBound upper, TValue value)
        {
            if (object.ReferenceEquals(lower, null))
                throw new ArgumentNullException("lower");

            if (object.ReferenceEquals(upper, null))
                throw new ArgumentNullException("upper");

            if (upper.CompareTo(lower) < 0)
                throw new ArgumentOutOfRangeException("upper", "An element with an interval equivalent to or intersecting the interval defined by the specified lower and upper bounds already exists in the Konves.Collections.IntervalDictionary<TBound,TValue>.");

            this.Add(new Interval<TBound>(lower, upper), value);
        }

        /// <summary>
        /// Determines whether the <see cref="IntervalDictionary&lt;TBound,TValue&gt;"/>
        /// contains the specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval">The interval to locate in the <see cref="IntervalDictionary&lt;TBound,TValue&gt;"/>.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="IntervalDictionary&lt;TBound,TValue&gt;"/> contains an
        ///   element with the specified <paramref name="interval"/>; otherwise, <c>false</c>.
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="interval"/> is <c>null</c>.</exception>
        public bool ContainsInterval(IInterval<TBound> interval)
        {
            if (object.ReferenceEquals(interval, null))
                throw new ArgumentNullException("interval");

            if (_root == null)
                return false;
            else
                return _root.ContainsInterval(interval);
        }
        /// <summary>
        /// Determines whether the <see cref="IntervalDictionary&lt;TBound,TValue&gt;"/>
        /// contains an interval that contains the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The key contained in the interval to locate in the <see cref="IntervalDictionary&lt;TBound,TValue&gt;"/>.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="IntervalDictionary&lt;TBound,TValue&gt;"/> contains an
        ///   element with an interval which contains the specified <paramref name="key"/>; otherwise, <c>false</c>.
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="key"/> is <c>null</c>.</exception>
        public bool ContainsKey(TBound key)
        {
            if (object.ReferenceEquals(key, null))
                throw new ArgumentNullException("key");

            if (_root == null)
                return false;
            else
                return _root.ContainsKey(key);
        }

        /// <summary>
        /// Removes the value with the specified <see cref="interval"/> from the <see cref="IntervalDictionary&lt;TBound, TValue&gt;"/>.
        /// </summary>
        /// <param name="interval">The interval of the element to remove.</param>
        /// <returns>
        /// <c>true</c> if the element is successfully found and removed; otherwise, false.
        /// This method returns false if <see cref="interval"/> is not found in
        /// the <see cref="IntervalDictionary&lt;TBound, TValue&gt;"/>
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="interval"/> is <c>null</c>.</exception>
        // TODO: Implement in IntervalNode
        public bool Remove(IInterval<TBound> interval)
        {
            if (object.ReferenceEquals(interval, null))
                throw new ArgumentNullException("interval");

            if (_root == null)
                return false;
            else
            {
                IntervalNode result;
                return _root.TryDelete(interval, out result);
            }
        }
        /// <summary>
        /// Removes the value with the interval which contains the specified <see cref="key"/>
        /// from the <see cref="IntervalDictionary&lt;TBound, TValue&gt;"/>.
        /// </summary>
        /// <param name="key">The key contained in the interval of the element to remove.</param>
        /// <returns>
        /// <c>true</c> if the element is successfully found and removed; otherwise, false.
        /// This method returns false if an interval containing <see cref="key"/> is not found in
        /// the <see cref="IntervalDictionary&lt;TBound, TValue&gt;"/>
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="key"/> is <c>null</c>.</exception>
        // TODO: Implement in IntervalNode
        public bool Remove(TBound key)
        {
            if (object.ReferenceEquals(key, null))
                throw new ArgumentNullException("key");

            if (_root.Interval.Contains(key))
            {
                _root = IntervalNode.RemoveRoot(ref _root);
                return true;
            }
            else
            {
                IntervalNode result;
                return _root.TryDelete(key, out result);
            }

            //throw new NotImplementedException();
        }

        /// <summary>
        /// Attempts to get the value associated with the specified <paramref name="interval"/>
        /// </summary>
        /// <param name="interval">The interval of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified
        /// <see cref="interval"/>, if the <see cref="interval"/> is found; otherwise, the default value
        /// for the type of the value parameter. This parameter is passed uninitialized</param>
        /// <returns>
        /// <c>true</c> if the <see cref="IntervalDictionary&lt;TBound, TValue&gt;"/> contains an element
        /// with the specified <see cref="interval"/>; otherwise, <c>false</c>.
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="interval"/> is <c>null</c>.</exception>
        public bool TryGetValue(IInterval<TBound> interval, out TValue value)
        {
            if (object.ReferenceEquals(interval, null))
                throw new ArgumentNullException("interval");

            if (_root == null)
            {
                value = default(TValue);
                return false;
            }
            else
                return _root.TryFind(interval, out value);
        }
        /// <summary>
        /// Attempts to get the value associated with the interval that contains the specified <paramref name="key"/>
        /// </summary>
        /// <param name="key">The key contained by the interval of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the
        /// interval that contains the specified <see cref="key"/>, if such an interval is found; otherwise,
        /// the default value for the type of the value parameter. This parameter is passed uninitialized</param>
        /// <returns>
        /// <c>true</c> if the <see cref="IntervalDictionary&lt;TBound, TValue&gt;"/> contains an element
        /// with an interval that contains the specified <see cref="key"/>; otherwise, <c>false</c>.
        /// </returns>
        /// 
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="interval"/> is <c>null</c>.</exception>
        public bool TryGetValue(TBound key, out TValue value)
        {
            if (object.ReferenceEquals(key, null))
                throw new ArgumentNullException("key");

            if (_root == null)
            {
                value = default(TValue);
                return false;
            }
            else
                return _root.TryFind(key, out value);
        }

        /// <summary>
        /// Removes all items from the <see cref="IntervalDictionary&lt;TBound, TValue&gt;"/>.
        /// </summary>
        public void Clear()
        {
            _root = null;
        }

        /// <summary>
        /// Gets the number of interval/value pairs contained in the <see cref="IntervalDictionary&lt;TBound, TValue&gt;"/>.
        /// </summary>
        /// <returns>
        /// The number of interval/value pairs contained in the <see cref="IntervalDictionary&lt;TBound, TValue&gt;"/>.
        ///   </returns>
        public int Count
        {
            get
            {
                if (_root == null)
                    return 0;
                else
                    return _root.Count;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="IntervalDictionary&lt;TBound, TValue&gt;"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> structure
        /// for the <see cref="IntervalDictionary&lt;TBound, TValue&gt;"/>.
        /// </returns>
        // TODO: define and return an enumerator type that implements IIntervalDictionaryEnumerator
        public IEnumerator<IntervalValuePair<TBound, TValue>> GetEnumerator()
        {
            return _root.Traverse().GetEnumerator();
        }

        #region Explicitly Implemented Members

        void ICollection<IntervalValuePair<TBound, TValue>>.Add(IntervalValuePair<TBound, TValue> item)
        {
            this.Add(item.Interval, item.Value);
        }

        bool ICollection<IntervalValuePair<TBound, TValue>>.Contains(IntervalValuePair<TBound, TValue> item)
        {
            TValue value;
            if (TryGetValue(item.Interval, out value))
                return item.Value.Equals(value);
            else
                return false;
        }

        void ICollection<IntervalValuePair<TBound, TValue>>.CopyTo(IntervalValuePair<TBound, TValue>[] array, int arrayIndex)
        {
            int i = 0;
            foreach (var pair in _root.Traverse())
                array[arrayIndex + i++] = pair;
        }

        bool ICollection<IntervalValuePair<TBound, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        // TODO: Implement
        bool ICollection<IntervalValuePair<TBound, TValue>>.Remove(IntervalValuePair<TBound, TValue> item)
        {
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        
        #endregion
    }
}
