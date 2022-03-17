using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;


namespace CoreDll.Bindables.Extensions.Forms
{
    public class DataGridViewDecorator
    {
        public System.Windows.Forms.DataGridView Grid { get; private set; }
        private bool IsStretchLastColumn { get; set; }
        //private bool IsAutoFill { get; set; }


        public DataGridViewDecorator(System.Windows.Forms.DataGridView gridToBind)
        {
            this.Grid = gridToBind;
        }

        public DataGridViewDecorator(System.Windows.Forms.DataGridView gridToBind, object dataSource = null)
        {
            this.Grid = gridToBind;

            /*
            this.Grid.DataSourceChanged += (object sender, EventArgs e) =>
            {
                this.StretchLastColumn(this.IsStretchLastColumn);
            };
            */

            if (dataSource != null)
            {
                this.Grid.Columns.Clear();
                this.Grid.AutoGenerateColumns = false;  // *** se não desativar esta propriedade, o controle não deixará criar manualmente;
                this.Grid.AutoSize = true;
                this.Grid.DataSource = dataSource;
                this.Grid.RowHeadersVisible = false;
                //this.AutoFillColumns(this.IsAutoFill);
            }
        }

        /// <summary>
        /// Aquela primeira coluna inútil que não serve pra nada e que aparece quando se cria todo maldito DataGridView.
        /// Por padrão, ao utilizar [DataGridViewDecorator], este parâmetro será alterado para FALSE.
        /// </summary>    
        public DataGridViewDecorator RowHeadersVisible(bool value)
        {
            this.Grid.RowHeadersVisible = value;
            return this;
        }

        public DataGridViewDecorator DataSource(object dataSource)
        {
            this.Grid.AutoGenerateColumns = false;
            this.Grid.AutoSize = true;
            this.Grid.DataSource = dataSource;
            this.Grid.RowHeadersVisible = false;

            return this;
        }

        /// <summary>
        /// Nome das propriedades que serão exibidas.
        /// Se uma determinada coluna não existir, uma nova será criada.
        /// </summary>
        public DataGridViewDecorator SetColumns(params string[] columnNames)
        {
            this.Grid.Columns.Clear();
            this.Grid.AutoGenerateColumns = columnNames.Count() == 0;
            this.Grid.AutoSize = true;

            int idx = 0;

            foreach (string columnName in columnNames)
            {
                System.Windows.Forms.DataGridViewColumn column = null;

                if (this.Grid.Columns.Count > idx)
                {
                    column = this.Grid.Columns[idx];
                    column.DataPropertyName = columnName;
                }
                else
                {
                    column = new System.Windows.Forms.DataGridViewTextBoxColumn();
                    column.DataPropertyName = columnName;
                    column.Name = columnName;
                    this.Grid.Columns.Add(column);
                }

                idx++;
            }

            return this;
        }

        /// <summary>
        /// Título das propriedades que serão exibidas no Header do DataGridView. Ordem da esquerda para a direita. 
        /// Se uma determinada coluna não existir, uma nova será criada.
        /// </summary>
        public DataGridViewDecorator SetTitles(params string[] titleNames)
        {
            int idx = 0;

            foreach (string titleName in titleNames)
            {
                System.Windows.Forms.DataGridViewColumn column = null;

                if (this.Grid.Columns.Count > idx)
                {
                    column = this.Grid.Columns[idx];
                    column.Name = titleName;
                }
                else
                {
                    column = new System.Windows.Forms.DataGridViewTextBoxColumn();
                    column.DataPropertyName = titleName;
                    column.Name = titleName;
                    this.Grid.Columns.Add(column);
                }

                idx++;
            }

            return this;
        }

        /// <summary>
        /// Altera o nome de uma coluna existente ou cria uma nova.
        /// </summary>
        public DataGridViewDecorator SetColumnTitle(string propertyName, string titleName)
        {
            bool isColumnFound = false;
            int idx = 0;

            for (; idx < this.Grid.Columns.Count; idx++)
            {
                if (this.Grid.Columns[idx].DataPropertyName == propertyName)
                {
                    this.Grid.Columns[idx].Name = titleName;
                    isColumnFound = true;
                    break;
                }
            }

            if (isColumnFound)
            {
                System.Windows.Forms.DataGridViewColumn column = new System.Windows.Forms.DataGridViewTextBoxColumn();
                column.DataPropertyName = titleName;
                column.Name = titleName;
                this.Grid.Columns.Add(column);
            }

            return this;
        }

        /// <summary>
        /// Extende a última coluna até o final à direita.
        /// </summary>
        public DataGridViewDecorator StretchLastColumn(bool value = false)
        {
            this.IsStretchLastColumn = value;

            if (this.IsStretchLastColumn)
            {
                if (this.Grid != null && this.Grid.DataSource != null)
                {
                    int lastColumnIndex = this.Grid.Columns.Count - 1;

                    if (lastColumnIndex >= 0)
                    {
                        System.Windows.Forms.DataGridViewColumn lastColumn = this.Grid.Columns[lastColumnIndex];
                        lastColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                    }
                }
            }

            return this;
        }

        /*
        public DataGridViewDecorator AutoFillColumns(bool value = true)
        {
            this.IsAutoFill = value;

            if (this.IsAutoFill)
            {

                foreach (DataGridViewColumn column in this.Grid.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }


                IBindingList source = this.Grid.DataSource as IBindingList;

                if (source != null)
                {
                    try
                    {
                        source.ListChanged -= source_ListChanged;
                        source.ListChanged -= source_ListChanged;
                    }
                    catch { }

                    source.ListChanged += source_ListChanged;
                }
            }
            else
            {
                IBindingList source = this.Grid.DataSource as IBindingList;

                if (source != null)
                {
                    try
                    {
                        source.ListChanged -= source_ListChanged;
                        source.ListChanged -= source_ListChanged;
                    }
                    catch { }
                }

                foreach (DataGridViewColumn column in this.Grid.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                }
            }

            return this;
        }

        
        private void AutoFillColumn(string propertyName)
        {
            if (this.Grid != null && this.Grid.DataSource != null)
            {
                System.Windows.Forms.DataGridViewColumn column = this.Grid.Columns[propertyName];
                column.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        private void source_ListChanged(object sender, ListChangedEventArgs e)
        {
            try
            {
                this.AutoFillColumn(e.PropertyDescriptor.Name);
            }
            catch { }
        }
        */

    }
}