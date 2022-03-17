using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace CoreDll.Bindables
{
    public class EventHandlerBindingTrigger<TControl> : AbstractBindingTrigger
        where TControl : Control//, new()
    {
        public EventHandlerBindingTrigger(string eventName, TControl source, string propertyName)
            : base()
        {
            this.Source = source;
            Type sourceType = source.GetType();
            this.EventInfo = sourceType.GetEvent(eventName);
            this.PropertyInfo = sourceType.GetProperty(propertyName);

            if (this.EventInfo.EventHandlerType != typeof(EventHandler))
            {
                throw new NotSupportedException("O evento não é do tipo EventHandler!");
            }

            MethodInfo methodInfo = this.GetType().GetMethod("ActionPerformer");
            //Delegate delegateRef = Delegate.CreateDelegate(eventType, methodInfo);
            Delegate delegateRef = Delegate.CreateDelegate(this.EventInfo.EventHandlerType, this, methodInfo);
            this.EventInfo.AddEventHandler(source, delegateRef);
        }

        //public delegate void ActionPerformerDelegate(object sender, EventArgs e);

        public void ActionPerformer(object sender, EventArgs e)
        {
            //this.Value = (T)this.PropertyInfo.GetValue(this.Source);
            this.Value = this.PropertyInfo.GetValue(this.Source);
            this.OnPerformedAction();
        }
    }
}