using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konves.Collections
{
    public interface IBound<T> where T : IComparable<T>, IEquatable<T>
    {
        /// <summary>
        /// Gets or sets the value of the bound.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        T Value { get; }
        /// <summary>
        /// Gets or sets the type of the bound.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        BoundType Type { get; }
    }

    

    public enum BoundType
    {
        Inclusive = 1,
        Exclusive = -1
    }
}
