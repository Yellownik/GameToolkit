using System;
using System.Collections.Generic;

namespace Orbox.Collections
{
    public class ListPool<T>
    {
        private Stack<PoolableList<T>> Pool = new Stack<PoolableList<T>>();

        public PoolableList<T> Rent()
        {
            PoolableList<T> item;

            if (Pool.Count > 0)
            {
                item = Pool.Pop();
            }
            else
            {
                item = new PoolableList<T>(this);
            }

            return item;
        }

        public void Return(PoolableList<T> item)
        {
            item.Clear();
            Pool.Push(item);
        }
    }

    public class PoolableList<T> : ZeroList<T>, IDisposable
    {
        private ListPool<T> Pool;

        public PoolableList(ListPool<T> pool)
        {
            Pool = pool;
        }

        public void Dispose()
        {
            Pool.Return(this);
        }
    }



}