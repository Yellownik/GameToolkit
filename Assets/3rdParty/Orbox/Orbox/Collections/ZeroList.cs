using System;
using System.Collections;
using System.Collections.Generic;

namespace Orbox.Collections
{
    //Alows to use foreach without heap allocations 	
    public class ZeroList<T> : List<T>, IEnumerable<T>
    {
        private int Version = 0;
        private Stack<Iterator> IteratorPool = new Stack<Iterator>();

        public ZeroList(): base()
        {
        }

        public ZeroList(int capacity): base(capacity)
        {
        }

        public ZeroList(IEnumerable<T> collection): base(collection)
        {
        }

        public new Iterator GetEnumerator()
        {
            return GetIterator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetIterator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetIterator();
        }

        public void ReturnToPool(Iterator iterator)
        {            
            IteratorPool.Push(iterator);
        }

        // --- To provide modifications check while an enumeration is in progress ---

        public new T this[int index]
        {
            get
            {
                return base[index];
            }

            set
            {
                base[index] = value;
                Version++;
            }
        }

        public new void Add(T item)
        {
            base.Add(item);
            Version++;
        }

        public new void Clear()
        {
            base.Clear();
            Version++;
        }

        public new void Insert(int index, T item)
        {
            base.Insert(index, item);
            Version++;
        }

        public new void InsertRange(int index, IEnumerable<T> collection)
        {
            base.InsertRange(index, collection);
            Version++;
        }

        public new bool Remove(T item)
        {
            bool result = base.Remove(item);
            Version++;
            return result;
        }

        public new int RemoveAll(Predicate<T> match)
        {
            int result = base.RemoveAll(match);
            Version++;

            return result;
        }

        public new void RemoveAt(int index)
        {
            base.RemoveAt(index);
            Version++;
        }

        public new void RemoveRange(int index, int count)
        {
            base.RemoveRange(index, count);
            Version++;
        }

        public new void Reverse()
        {
            base.Reverse();
            Version++;
        }

        public new void Reverse(int index, int count)
        {
            base.Reverse(index, count);
            Version++;
        }

        public new void Sort()
        {
            base.Sort();
            Version++;
        }

        public new void Sort(IComparer<T> comparer)
        {
            base.Sort(comparer);
            Version++;
        }

        public new void Sort(int index, int count, IComparer<T> comparer)
        {
            base.Sort(index, count, comparer);
            Version++;
        }

        public new void Sort(Comparison<T> comparison)
        {
            base.Sort(comparison);
            Version++; // System.Collections.Generic.List<T> source code does not increment version in this method, but i suppose it need
        }

        // --- private ---

        private Iterator GetIterator()
        {
            if (IteratorPool.Count > 0)
            {
                var iterator = IteratorPool.Pop();
                iterator.Clear();

                return iterator;
            }

            return new Iterator(this);
        }


        [Serializable]
        public class Iterator : IEnumerator<T>
        {
            public T Current { get; private set; }

            private ZeroList<T> List;
            private int Index;
            private int Version;
            

            internal Iterator(ZeroList<T> list)
            {
                this.List = list;
                Index = 0;

                Version = list.Version;
                Current = default(T);
            }

            public void Clear()
            {
                Index = 0;
                Version = List.Version;
                Current = default(T);
            }

            public void Dispose()
            {
                List.ReturnToPool(this);
            }

            public bool MoveNext()
            {
                if (Version == List.Version && ((uint)Index < (uint)List.Count))
                {
                    Current = List[Index];
                    Index++;
                    return true;
                }

                return MoveNextRare();
            }

            Object IEnumerator.Current
            {
                get
                {
                    if (Index == 0 || Index == List.Count + 1)
                    {
                        throw new InvalidOperationException("InvalidOperation_EnumOpCantHappen");
                    }

                    return Current;
                }
            }

            void IEnumerator.Reset()
            {
                if (Version != List.Version)
                {
                    throw new InvalidOperationException("InvalidOperation_EnumFailedVersion");
                }

                Index = 0;
                Current = default(T);
            }

            private bool MoveNextRare()
            {
                if (Version != List.Version)
                {
                    throw new InvalidOperationException("InvalidOperation_EnumFailedVersion");
                }

                Index = List.Count + 1;
                Current = default(T);
                return false;
            }
        }
    }
}
