using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDll.Bindables
{
    public class BindingPeer<TLeftProperty, TRightProperty> : IBindingPeer
    {
        private object _LOCK_;
        private bool _LOOPING_PREVENT_ = false;

        public UpdatingWay Way { get; set; }

        public LazyValueBag LastChangedValue { get; protected set; }

        public Func<TRightProperty, TLeftProperty> ConvertToLeft { get; protected set; }
        public Func<TLeftProperty, TRightProperty> ConvertToRight { get; protected set; }

        public AbstractBindingTrigger LeftTrigger { get; protected set; }
        public AbstractBindingTrigger RightTrigger { get; protected set; }

        public BindingPeer(AbstractBindingTrigger leftTrigger, AbstractBindingTrigger rightTrigger,
                           Func<TRightProperty, TLeftProperty> conversionToLeft, Func<TLeftProperty, TRightProperty> conversionToRight,
                           UpdatingWay updateWay = UpdatingWay.Both)
        {
            this.LeftTrigger = leftTrigger;
            this.RightTrigger = rightTrigger;

            this.ConvertToLeft = conversionToLeft;
            this.ConvertToRight = conversionToRight;

            if (LeftTrigger != null)
            {
                this.LeftTrigger.ValueChanged += LeftValueChanged;
            }

            if (RightTrigger != null)
            {
                this.RightTrigger.ValueChanged += RightValueChanged;
            }

            this.Way = updateWay;

            this._LOCK_ = new object(); //--> objeto para fazer os locks (garantir consistência);
            this.LastChangedValue = new LazyValueBag(_LOCK_);

            // começar com o primeiro valor:
            LeftValueChanged(null, null);
        }

        private void LeftValueChanged(object sender, EventArgs e)
        {
            if (_LOOPING_PREVENT_)
                return;

            if (this.ConvertToRight == null)
                return;

            object value = this.LeftTrigger.PropertyInfo.GetValue(this.LeftTrigger.Source);

            LazyValue lazyValue = new LazyValue();
            lazyValue.Value = this.ConvertToRight((TLeftProperty)value);
            lazyValue.Direction = Direction.ToRight;
            this.LastChangedValue.SetValue(lazyValue);

            if (this.Way == UpdatingWay.Both || this.Way == UpdatingWay.LeftToRight)
            {
                this.UpdatePeers();
            }
        }

        private void RightValueChanged(object sender, EventArgs e)
        {
            if (_LOOPING_PREVENT_)
                return;

            if (this.ConvertToLeft == null)
                return;

            object value = this.RightTrigger.PropertyInfo.GetValue(this.RightTrigger.Source);

            LazyValue lazyValue = new LazyValue();
            lazyValue.Value = this.ConvertToLeft((TRightProperty)value);
            lazyValue.Direction = Direction.ToLeft;
            this.LastChangedValue.SetValue(lazyValue);

            if (this.Way == UpdatingWay.Both || this.Way == UpdatingWay.RightToLeft)
            {
                this.UpdatePeers();
            }
        }

        public void UpdatePeers()
        {
            lock (_LOCK_)
            {
                LazyValue value;

                if (this.LastChangedValue.TryGetValue(out value))
                {
                    _LOOPING_PREVENT_ = true;

                    if (value.Direction == Direction.ToLeft)
                    {
                        this.LeftTrigger.PropertyInfo.SetValue(this.LeftTrigger.Source, value.Value);
                    }

                    if (value.Direction == Direction.ToRight)
                    {
                        this.RightTrigger.PropertyInfo.SetValue(this.RightTrigger.Source, value.Value);
                    }

                    _LOOPING_PREVENT_ = false;
                }
            }
        }
    }
}
