using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;

namespace CoreDll
{
    public class SyncBindingList<T> : BindingList<T>
    {
        private object LOCK = new object();
        private SynchronizationContext ctx = SynchronizationContext.Current;


        public new T this[int index]
        {
            get
            {
                lock (LOCK)
                {
                    return base[index];
                }
            }
            set
            {
                lock (LOCK)
                {
                    base[index] = value;
                }
            }
        }

        public new void Add(T item)
        {
            lock (LOCK)
            {
                base.Add(item);
            }
        }

        public new T AddNew()
        {
            lock (LOCK)
            {
                return base.AddNew();
            }
        }

        public new void CancelNew(int itemIndex)
        {
            lock (LOCK)
            {
                base.CancelNew(itemIndex);
            }
        }

        public new void Clear()
        {
            lock (LOCK)
            {
                base.Clear();
            }
        }

        public new bool Contains(T item)
        {
            lock (LOCK)
            {
                return base.Contains(item);
            }
        }

        public new void CopyTo(T[] array, int index)
        {
            lock (LOCK)
            {
                base.CopyTo(array, index);
            }
        }
        
        public new int Count
        {
            get { lock (LOCK) { return base.Count; } }
        }

        public new void EndNew(int itemIndex)
        {
            lock (LOCK)
            {
                base.EndNew(itemIndex);
            }
        }

        public new IEnumerator<T> GetEnumerator()
        {
            lock (LOCK)
            {
                return base.GetEnumerator();
            }
        }

        public new int IndexOf(T item)
        {
            lock (LOCK)
            {
                return base.IndexOf(item);
            }
        }

        public new void Insert(int index, T item)
        {
            lock (LOCK)
            {
                base.Insert(index, item);
            }
        }

        public new bool Remove(T item)
        {
            lock (LOCK)
            {
                return base.Remove(item);
            }
        }

        public new void  RemoveAt(int index)
        {
            lock (LOCK)
            {
                base.RemoveAt(index);
            }
        }

        public new void ResetBindings()
        {
            lock (LOCK)
            {
                base.ResetBindings();
            }
        }

        public new void ResetItem(int position)
        {
            lock (LOCK)
            {
                base.ResetItem(position);
            }
        }


        protected override object AddNewCore()
        {
            lock(this)
            {
                return base.AddNewCore();
            }
        }

        protected override void SetItem(int index, T item)
        {
            lock (LOCK)
            {
                base.SetItem(index, item);
            }
        }

        protected override void RemoveItem(int index)
        {
            lock (LOCK)
            {
                base.RemoveItem(index);
            }
        }

        protected override void ClearItems()
        {
            lock (LOCK)
            {
                base.ClearItems();
            }
        }
        



        protected override void OnAddingNew(AddingNewEventArgs e)
        {

            if (ctx == null)
            {
                base.OnAddingNew(e);
            }
            else
            {
                ctx.Send(delegate { base.OnAddingNew(e); }, null);
            }
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {

            if (ctx == null)
            {
                base.OnListChanged(e);
            }
            else
            {
                ctx.Send(delegate { base.OnListChanged(e); }, null);
            }
        }
    }
}