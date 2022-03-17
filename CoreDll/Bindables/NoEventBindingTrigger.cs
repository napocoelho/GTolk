using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDll.Bindables
{
    public class NoEventBindingTrigger : AbstractBindingTrigger
    {
        //public event EventHandler ValueChanged = null;

        public NoEventBindingTrigger(object source, string propertyName)
            : base()
        {
            this.Source = source;
            Type sourceType = source.GetType();
            this.EventInfo = null;
            this.PropertyInfo = sourceType.GetProperty(propertyName);
        }
    }
}