using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDll.Bindables
{
    public class LazyValueBag
    {
        private object _LOCK_;
        private LazyValue Value { get; set; }
        public bool HasValue { get { return this.Value != null; } }

        public LazyValueBag()
        {
            this._LOCK_ = new object();
            this.Value = null;
        }

        public LazyValueBag(object lockObject)
        {
            this._LOCK_ = lockObject;
            this.Value = null;
        }

        public void SetValue(LazyValue value)
        {
            lock (_LOCK_)
            {
                this.Value = value;
            }
        }

        public bool TryGetValue(out LazyValue value)
        {
            lock (_LOCK_)
            {
                if (this.HasValue)
                {
                    value = this.Value;
                    this.Value = null;
                    return true;
                }

                value = null;
                return false;
            }
        }
    }
}