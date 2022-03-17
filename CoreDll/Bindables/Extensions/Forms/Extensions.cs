using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CoreDll.Bindables.Extensions;

namespace CoreDll.Bindables.Extensions.Forms
{
    public static class Extensions
    {
        public static IBindingPeer BindsOnTextChanged<TSource>(this ToolStripStatusLabel control, TSource dataSource, Expression<Func<TSource, string>> dataSourceMember, UpdatingWay updatingWay = UpdatingWay.Both) where TSource : System.ComponentModel.INotifyPropertyChanged //, new()
        {
            throw new NotImplementedException();
            //return BindsToSource<ToolStripStatusLabel, TSource, string>(control, c => c.Text, "TextChanged", dataSource, dataSourceMember, updatingWay);
        }

        public static IBindingPeer BindsOnTextChanged<TSource>(this TextBox control, TSource dataSource, Expression<Func<TSource, string>> dataSourceMember, UpdatingWay updatingWay = UpdatingWay.Both) where TSource : System.ComponentModel.INotifyPropertyChanged //, new()
        {
            return DataBindingExtensions.BindsToSource<TextBox, TSource, string>(control, c => c.Text, "TextChanged", dataSource, dataSourceMember, updatingWay);
        }

        public static IBindingPeer BindsOnTextChanged<TSource, TPropertyRight>(this TextBox control, TSource dataSource, Expression<Func<TSource, TPropertyRight>> dataSourceMember, Func<TPropertyRight, string> conversionToLeft, Func<string, TPropertyRight> conversionToRight, UpdatingWay updatingWay = UpdatingWay.Both) where TSource : System.ComponentModel.INotifyPropertyChanged //, new()
        {
            return DataBindingExtensions.BindsToSource<TextBox, TSource, string, TPropertyRight>(control, c => c.Text, "TextChanged", dataSource, dataSourceMember, conversionToLeft, conversionToRight, updatingWay);
        }

        public static IBindingPeer BindsOnTextChanged<TSource>(this Label control, TSource dataSource, Expression<Func<TSource, string>> dataSourceMember, UpdatingWay updatingWay = UpdatingWay.Both) where TSource : System.ComponentModel.INotifyPropertyChanged //, new()
        {
            return DataBindingExtensions.BindsToSource<Label, TSource, string>(control, c => c.Text, "TextChanged", dataSource, dataSourceMember, updatingWay);
        }

        public static IBindingPeer BindsOnCheckedChanged<TSource>(this CheckBox control, TSource dataSource, Expression<Func<TSource, bool>> dataSourceMember, UpdatingWay updatingWay = UpdatingWay.Both) where TSource : System.ComponentModel.INotifyPropertyChanged, new()
        {
            return DataBindingExtensions.BindsToSource<CheckBox, TSource, bool>(control, c => c.Checked, "CheckedChanged", dataSource, dataSourceMember, updatingWay);
        }

        public static IBindingPeer BindsOnCheckedChanged<TSource>(this RadioButton control, TSource dataSource, Expression<Func<TSource, bool>> dataSourceMember, UpdatingWay updatingWay = UpdatingWay.Both) where TSource : System.ComponentModel.INotifyPropertyChanged, new()
        {
            return DataBindingExtensions.BindsToSource<RadioButton, TSource, bool>(control, c => c.Checked, "CheckedChanged", dataSource, dataSourceMember, updatingWay);
        }

        public static IBindingPeer BindsOnValueChanged<TSource>(this DateTimePicker control, TSource dataSource, Expression<Func<TSource, DateTime>> dataSourceMember, UpdatingWay updatingWay = UpdatingWay.Both) where TSource : System.ComponentModel.INotifyPropertyChanged, new()
        {
            return DataBindingExtensions.BindsToSource<DateTimePicker, TSource, DateTime>(control, c => c.Value, "ValueChanged", dataSource, dataSourceMember, updatingWay);
        }

        public static IBindingPeer BindsOnValueChanged<TSource>(this ComboBox control, TSource dataSource, Expression<Func<TSource, object>> dataSourceMember, UpdatingWay updatingWay = UpdatingWay.Both) where TSource : System.ComponentModel.INotifyPropertyChanged, new()
        {
            return DataBindingExtensions.BindsToSource<ComboBox, TSource, object>(control, c => c.SelectedValue, "SelectedValueChanged", dataSource, dataSourceMember, updatingWay);
        }

        public static IBindingPeer BindsOnValueChanged<TSource, TSourceValue>(this ComboBox control, TSource dataSource, Expression<Func<TSource, TSourceValue>> dataSourceMember, UpdatingWay updatingWay = UpdatingWay.Both) where TSource : System.ComponentModel.INotifyPropertyChanged, new()
        {
            Func<object, TSourceValue> toSource = (value) => { return (TSourceValue)value; };
            Func<TSourceValue, object> toControl = (value) => { return value; };
            return DataBindingExtensions.BindsToSource<ComboBox, TSource, object, TSourceValue>(control, c => c.SelectedValue, "SelectedValueChanged", dataSource, dataSourceMember, toControl, toSource, updatingWay);
        }
    }
}