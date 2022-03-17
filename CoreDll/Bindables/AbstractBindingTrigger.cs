using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;

namespace CoreDll.Bindables
{
    public abstract class AbstractBindingTrigger
    {
        public event EventHandler ValueChanged;
        public object Value { get; protected set; }

        public object Source { get; protected set; }
        public EventInfo EventInfo { get; protected set; }
        public PropertyInfo PropertyInfo { get; protected set; }

        public AbstractBindingTrigger()
        {
        }

        protected void OnPerformedAction()
        {
            if (this.ValueChanged != null)
            {
                this.ValueChanged(this, new EventArgs());
            }
        }
    }
}