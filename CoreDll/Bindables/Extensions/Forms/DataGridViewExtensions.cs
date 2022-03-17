using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;

namespace CoreDll.Bindables.Extensions.Forms
{
    public static class DataGridViewExtensions
    {
        public static DataGridViewDecorator DataSourceConfig(this System.Windows.Forms.DataGridView grid)
        {
            return new DataGridViewDecorator(grid);
        }

        public static DataGridViewDecorator DataSourceConfig(this System.Windows.Forms.DataGridView grid, object dataSource)
        {
            return new DataGridViewDecorator(grid, dataSource);
        }


        public static void BindsOnMultiSelectionChanged<TSource, TSourceMember>(this DataGridView grid, TSource dataSource, Expression<Func<TSource, ICollection<TSourceMember>>> dataSourceMember)
            where TSource : INotifyPropertyChanged
        {
            string propertyName = CoreDll.LinqExpressions.ExprenssionHelper.MemberName(dataSourceMember);

            bool block = false;

            grid.SelectionChanged += (object sender, EventArgs e) =>
            {
                if (!block)
                {
                    block = true;
                    try
                    {
                        PropertyInfo info = typeof(TSourceMember).GetProperty(propertyName);
                        ICollection<TSourceMember> lista = (ICollection<TSourceMember>)info.GetValue(dataSource);
                        lista.Clear();

                        foreach (DataGridViewRow row in grid.SelectedRows)
                        {
                            TSourceMember value = default(TSourceMember);
                            lista.Add(value);
                        }
                    }
                    catch
                    { }

                    block = false;
                }
            };

            dataSource.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
            {
                if (e.PropertyName == propertyName)
                {
                    try
                    {
                        if (!block)
                        {
                            block = true;
                            grid.ClearSelection();

                            PropertyInfo info = typeof(TSourceMember).GetProperty(propertyName);
                            ICollection<TSourceMember> lista = (ICollection<TSourceMember>)info.GetValue(dataSource);

                            foreach (TSourceMember value in lista)
                            {
                                foreach (DataGridViewRow row in grid.Rows)
                                {
                                    if (object.ReferenceEquals(row.DataBoundItem, value))
                                    {
                                        row.Selected = true;
                                    }
                                }
                            }
                        }
                    }
                    catch { }

                    block = false;
                }
            };
        }


        public static void BindsOnSingleSelectionChanged<TSource, TSourceMember>(this DataGridView grid, TSource dataSource, Expression<Func<TSource, TSourceMember>> dataSourceMember)
            where TSource : INotifyPropertyChanged
        {
            string propertyName = CoreDll.LinqExpressions.ExprenssionHelper.MemberName(dataSourceMember);

            bool block = false;

            grid.SelectionChanged += (object sender, EventArgs e) =>
            {
                if (!block)
                {
                    block = true;

                    try
                    {
                        foreach (DataGridViewRow row in grid.SelectedRows)
                        {
                            TSourceMember value = default(TSourceMember);

                            try
                            {
                                value = (TSourceMember)row.DataBoundItem;
                            }
                            catch
                            { }

                            try
                            {
                                PropertyInfo info = typeof(TSource).GetProperty(propertyName);
                                info.SetValue(dataSource, value);
                            }
                            catch
                            { }

                            break; // obtém apenas o 1o valor. Se tiver mais, não deveria, pois é pra um único item selecionado.
                        }
                    }
                    catch { }

                    block = false;
                }
            };

            dataSource.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
                {
                    if (e.PropertyName == propertyName)
                    {
                        if (!block)
                        {
                            block = true;

                            try
                            {
                                grid.ClearSelection();

                                PropertyInfo info = typeof(TSourceMember).GetProperty(propertyName);
                                TSourceMember value = (TSourceMember)info.GetValue(dataSource);

                                foreach (DataGridViewRow row in grid.Rows)
                                {
                                    if (object.ReferenceEquals(row.DataBoundItem, value))
                                    {
                                        row.Selected = true;
                                        break;
                                    }
                                }
                            }
                            catch { }

                            block = false;
                        }
                    }

                };
        }
    }
}