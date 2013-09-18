using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Konves.Collections
{
    public sealed partial class IntervalDictionary<TBound, TValue>
    {
        //[DebuggerDisplay("[{Interval.LowerBound},{Interval.UpperBound}]: {Value}")]
        // TODO: Add XML Documentation
        private class IntervalNode
        {
            public IntervalNode(IInterval<TBound> interval, TValue value, IntervalNode parent)
            {
                this.Interval = interval;
                this.Value = value;
                this.Parent = parent;
            }

            public readonly IInterval<TBound> Interval;
            public TValue Value;

            public IntervalNode Left;
            public IntervalNode Right;
            public IntervalNode Parent;

            public int Count = 1;

            public int Height
            {
                get
                {
                    if (!_isHeightSynced)
                    {
                        _height = GetHeight();
                        _isHeightSynced = true;
                    }

                    return _height;
                }
            }
            private bool _isHeightSynced = false;
            private int _height = 0;
            private int GetHeight()
            {
                return
                    Math.Max(
                        this.Left == null ? 0 : this.Left.Height + 1,
                        this.Right == null ? 0 : this.Right.Height + 1
                        );
            }

            private int GetBalanceFactor()
            {
                return
                    (this.Left == null ? -1 : this.Left.Height)
                    - (this.Right == null ? -1 : this.Right.Height);
            }

            private BalanceOperation GetBalanceOperation()
            {
                int bf = this.GetBalanceFactor();

                if (bf < -1)
                    if (this.Right.GetBalanceFactor() < 0)
                        return BalanceOperation.RightRight;
                    else
                        return BalanceOperation.RightLeft;
                else if (bf > 1)
                    if (this.Left.GetBalanceFactor() < 0)
                        return BalanceOperation.LeftRight;
                    else
                        return BalanceOperation.LeftLeft;
                else
                    return BalanceOperation.None;
            }        

            public bool TryInsert(IInterval<TBound> interval, TValue value)
            {
                bool result = false;

                if (!this.Interval.Intersects(interval))
                {

                    if (interval.CompareTo(this.Interval) < 0)
                    {
                        if (this.Left == null)
                        {
                            this.Left = new IntervalNode(interval, value, this);
                            result = true;
                        }
                        else
                        {
                            result = this.Left.TryInsert(interval, value);
                        }

                        if (result)
                            IntervalNode.Balance(ref this.Left);
                    }
                    else
                    {
                        if (this.Right == null)
                        {
                            this.Right = new IntervalNode(interval, value, this);
                            result = true;
                        }
                        else
                        {
                            result = this.Right.TryInsert(interval, value);
                        }

                        if (result)
                            IntervalNode.Balance(ref this.Right);
                    }
                }

                if (result)
                {
                    _isHeightSynced = false;
                    Count++;
                }
                
                return result;
            }

            public bool TryDelete(IInterval<TBound> interval, out IntervalNode node)
            {
                throw new NotImplementedException();
            }

            public bool TryDelete(TBound key, out IntervalNode node)
            {
                bool success = true;
                int c = this.Interval.CompareTo(key);

                if (c < 0)
                {
                    if (this.Left.Interval.Contains(key))
                        node = Remove(ref this.Left);
                    else
                        success = this.Left.TryDelete(key, out node);

                    if (this.Left != null)
                        Balance(ref this.Left);
                }
                else if (c > 0)
                {
                    if (this.Right.Interval.Contains(key))
                        node = Remove(ref this.Right);
                    else
                        success = this.Right.TryDelete(key, out node);


                    if (this.Right != null)
                        Balance(ref this.Right);
                }
                else
                {
                    throw new Exception();
                }

                if(success)
                    this.Count--;

                return success;
            }
            
            public bool TryFind(TBound key, out TValue value)
            {
                int result = Interval.CompareTo(key);

                if (result == 0)
                {
                    value = Value;
                    return true;
                }
                else if (result < 0)
                    if (Left == null)
                    {
                        value = default(TValue);
                        return false;
                    }
                    else
                        return Left.TryFind(key, out value);
                else
                    if (Right == null)
                    {
                        value = default(TValue);
                        return false;
                    }
                    else
                        return Right.TryFind(key, out value);
            }

            public bool TryFind(IInterval<TBound> interval, out TValue value)
            {
                if (Interval.Equals(interval))
                {
                    value = this.Value;
                    return true;
                }
                else if (interval.CompareTo(Interval) < 0)
                    if (Left == null)
                    {
                        value = default(TValue);
                        return false;
                    }
                    else
                        return Left.TryFind(interval, out value);
                else
                    if (Right == null)
                    {
                        value = default(TValue);
                        return false;
                    }
                    else
                        return Right.TryFind(interval, out value);
            }

            // TODO: Implement as TrySetValue instead
            public void SetValue(TBound key, TValue value)
            {
                int result = Interval.CompareTo(key);

                if (result == 0)
                    this.Value = value;
                else if (result < 0)
                    if (Left == null)
                        throw new KeyNotFoundException();
                    else
                        Left.SetValue(key, value);
                else
                    if (Right == null)
                        throw new KeyNotFoundException();
                    else
                        Right.SetValue(key, value);
            }

            // TODO: Implement as TrySetValue instead
            public void SetValue(IInterval<TBound> interval, TValue value)
            {
                if (Interval.Equals(interval))
                    this.Value = value;
                else if (interval.CompareTo(Interval) < 0)
                    if (Left == null)
                        throw new KeyNotFoundException();
                    else
                        Left.SetValue(interval, value);
                else
                    if (Right == null)
                        throw new KeyNotFoundException();
                    else
                        Right.SetValue(interval, value);
            }

            public IEnumerable<IntervalValuePair<TBound, TValue>> Traverse()
            {
                if (this.Left != null)
                    foreach (var node in this.Left.Traverse())
                        yield return node;

                yield return new IntervalValuePair<TBound, TValue>(this.Interval, this.Value);

                if (this.Right != null)
                    foreach (var node in this.Right.Traverse())
                        yield return node;
            }

            // TODO: consider moving to a static method or to the IntervalDictionary class
            public IEnumerable<IInterval<TBound>> GetIntervals()
            {
                if (this.Left != null)
                    foreach (var interval in this.Left.GetIntervals())
                        yield return interval;

                yield return this.Interval;

                if (this.Right != null)
                    foreach (var interval in this.Right.GetIntervals())
                        yield return interval;
            }

            // TODO: consider moving to a static method or to the IntervalDictionary class
            public IEnumerable<TValue> GetValues()
            {
                if (this.Left != null)
                    foreach (var value in this.Left.GetValues())
                        yield return value;

                yield return this.Value;

                if (this.Right != null)
                    foreach (var value in this.Right.GetValues())
                        yield return value;
            }

            public bool ContainsKey(TBound key)
            {
                return
                    this.Interval.Contains(key)
                    || (this.Left != null && this.Left.ContainsKey(key))
                    || (this.Right != null && this.Right.ContainsKey(key));
            }

            public bool ContainsInterval(IInterval<TBound> interval)
            {
                return
                    this.Interval.Equals(interval)
                    || (this.Left != null && this.Left.ContainsInterval(interval))
                    || (this.Right != null && this.Right.ContainsInterval(interval));
            }

            public static void RotateLeft(ref IntervalNode root)
            {
                root._isHeightSynced = false;

                var rootParent = root.Parent;
                var pivot = root.Right;
                var temp = pivot.Left;

                pivot.Left = root;
                root.Parent = pivot;
                root.Right = temp;

                if (temp != null)
                {
                    temp.Parent = root;
                }
                if (pivot != null)
                {
                    pivot.Parent = rootParent;
                    pivot._isHeightSynced = false;

                    pivot.Count += 1;
                    pivot.Count += root.Left == null ? 0 : root.Left.Count;

                    root.Count -= 1;
                    root.Count -= pivot.Right == null ? 0 : pivot.Right.Count;
                }

                root = pivot;
            }

            public static void RotateRight(ref IntervalNode root)
            {
                root._isHeightSynced = false;

                var rootParent = root.Parent;
                var pivot = root.Left;
                var temp = pivot.Right;

                pivot.Right = root;
                root.Parent = pivot;
                root.Left = temp;

                if (temp != null)
                {
                    temp.Parent = root;                    
                }
                if (pivot != null)
                {
                    pivot.Parent = rootParent;
                    pivot._isHeightSynced = false;

                    pivot.Count += 1;
                    pivot.Count += root.Right == null ? 0 : root.Right.Count;

                    root.Count -= 1;
                    root.Count -= pivot.Left == null ? 0 : pivot.Left.Count;
                }

                root = pivot;
            }

            public static void Balance(ref IntervalNode root)
            {
                switch (root.GetBalanceOperation())
                {
                    case BalanceOperation.LeftLeft:
                        RotateRight(ref root);
                        break;
                    case BalanceOperation.LeftRight:
                        RotateLeft(ref root.Left);
                        RotateRight(ref root);
                        break;
                    case BalanceOperation.RightLeft:
                        RotateRight(ref root.Right);
                        RotateLeft(ref root);
                        break;
                    case BalanceOperation.RightRight:
                        RotateLeft(ref root);
                        break;
                }
            }

            public static IntervalNode Remove(ref IntervalNode node)
            {
                if (node == null)
                    throw new ArgumentNullException();

                var temp = node;

                var parent = node.Parent;
                node.Parent = null;

                // Leaf
                if (node.Left == null && node.Right == null)
                {
                    if (parent != null)
                        if (object.ReferenceEquals(parent.Left, node))
                            parent.Left = null;
                        else
                            parent.Right = null;
                }
                // Only Left subtree
                else if (node.Left != null && node.Right == null)
                {
                    var child = node.Left;
                    node.Left = null;

                    if (node.Parent != null)
                        if (object.ReferenceEquals(parent.Left, node))
                            parent.Left = child;
                        else
                            parent.Right = child;

                    child.Parent = parent;


                }
                // Only Right subtree
                else if (node.Right != null && node.Left == null)
                {
                    var child = node.Right;
                    node.Right = null;

                    if (parent != null)
                        if (object.ReferenceEquals(parent.Left, node))
                            parent.Left = child;
                        else
                            parent.Right = child;

                    child.Parent = parent;
                }
                // Left and Right subtree
                else if (node.Left != null && node.Right != null)
                {
                    var suc = IntervalNode.RemoveInOrderSuccessor(ref node);
                    
                    suc.Left = node.Left;
                    suc.Right = node.Right;
                    suc.Parent = parent;

                    suc._isHeightSynced = false;

                    node.Left = null;
                    node.Right = null;
                    node.Parent = null;                    
                }

                return temp;
            }

            public static IntervalNode RemoveFirst(ref IntervalNode node)
            {
                var temp = node;
                IntervalNode result;

                if (node.Left == null)
                    result = Remove(ref node);
                else
                {
                    result = RemoveFirst(ref node.Left);
                    Balance(ref temp);
                }

                return result;
            }

            public static IntervalNode RemoveLast(ref IntervalNode node)
            {
                var temp = node;
                IntervalNode result;

                if (node.Right == null)
                    return Remove(ref node);
                else
                {
                    result = RemoveLast(ref node.Right);
                    Balance(ref temp);
                }

                return result;
            }

            public static IntervalNode RemoveInOrderPredecessor(ref IntervalNode node)
            {
                var temp = node;
                IntervalNode result;

                if (node.Left == null)
                {
                    result = null;
                }
                else
                {
                    result = RemoveLast(ref node.Left);
                    Balance(ref temp.Left);
                }

                return result;
            }

            public static IntervalNode RemoveInOrderSuccessor(ref IntervalNode node)
            {
                var temp = node;
                IntervalNode result;

                if (node.Right == null)
                {
                    result = null;
                }
                else
                {
                    result = RemoveFirst(ref node.Right);
                    Balance(ref temp.Right);
                }

                return result;
            }

            public static IntervalNode RemoveRoot(ref IntervalNode rootNode)
            {
                if (rootNode.Left == null && rootNode.Right ==null) // Is leaf
                {
                    return null;
                }
                else if (rootNode.Right == null) // Has left
                {
                    var child = rootNode.Left;

                    rootNode.Left = null;
                    child.Parent = null;

                    return child;
                }
                else if (rootNode.Left == null) // Has right
                {
                    var child = rootNode.Right;

                    rootNode.Right = null;
                    child.Parent = null;

                    return child;
                }
                else // Has left and right
                {
                    throw new NotImplementedException();
                }

            }

            /// <summary>
            /// Replaces node N with node R. Node N is removed from the tree.
            /// </summary>
            /// <param name="N">The N.</param>
            /// <param name="R">The R.</param>
            public static void Replace(ref IntervalNode N, ref IntervalNode R)
            {
                var tempN = N;

                R.Parent = N.Parent;
                R.Left = N.Left;
                R.Right = N.Right;

                tempN.Parent = null;
                tempN.Left = null;
                tempN.Right = null;
            }

            public static IntervalNode GetFirst(IntervalNode node)
            {
                if (node.Left == null)
                    return node;
                else
                    return IntervalNode.GetFirst(node.Left);
            }

            public static IntervalNode GetLast(IntervalNode node)
            {
                if (node.Right == null)
                    return node;
                else
                    return IntervalNode.GetLast(node.Right);
            }

            public override string ToString()
            {
                return string.Format("{0}: {1}", Interval.ToString(), Value);
            }
        }

        public enum BalanceOperation
        {
            None,
            LeftLeft,
            LeftRight,
            RightLeft,
            RightRight
        }
    }
}
