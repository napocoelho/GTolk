using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

namespace CoreDll.Bindables
{
    public class PropertyChangedEventHandlerBindingTrigger<TPropertyChanged> : AbstractBindingTrigger
        where TPropertyChanged : System.ComponentModel.INotifyPropertyChanged
    {
        public PropertyChangedEventHandlerBindingTrigger(TPropertyChanged source, string propertyName)
            : base()
        {
            string eventName = "PropertyChanged";

            this.Source = source;
            Type sourceType = source.GetType();
            this.EventInfo = sourceType.GetEvent(eventName);
            //Type eventType = this.EventInfo.EventHandlerType;
            this.PropertyInfo = sourceType.GetProperty(propertyName);

            if (this.EventInfo.EventHandlerType != typeof(PropertyChangedEventHandler))
            {
                throw new NotSupportedException("O evento não é do tipo PropertyChangedEventHandler!");
            }

            MethodInfo methodInfo = this.GetType().GetMethod("ActionPerformer");
            //Delegate delegateRef = Delegate.CreateDelegate(eventType, methodInfo);
            Delegate delegateRef = Delegate.CreateDelegate(this.EventInfo.EventHandlerType, this, methodInfo);
            this.EventInfo.AddEventHandler(source, delegateRef);
        }

        public void ActionPerformer(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.PropertyInfo.Name)
            {
                //this.Value = (T)this.PropertyInfo.GetValue(this.Source);
                this.Value = this.PropertyInfo.GetValue(this.Source);
                this.OnPerformedAction();
            }
        }
    }
}