using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GTolk.Util;
using GTolk.Models;
using CoreDll;
using CoreDll.Bindables;


namespace GTolk
{
    public partial class ContactListControl : BindableBaseForUserControls
    {
        private object LOCK = new object();
        private bool SegurouClick { get { return base.Get<bool>(); } set { if (base.Set(value)) { this.UpdateAllRowsColor(); } } }

        public BindingList<Contato> DataSource
        {
            get { return base.Get<BindingList<Contato>>(); }
            set
            {
                lock (LOCK)
                {
                    BindingList<Contato> antigoContatos = this.DataSource;

                    if (base.Set(value))
                    {
                        this.SetContatos(antigoContatos);
                        this.UpdateAllRowsColor();
                    }
                }
            }
        }

        public FlowLayoutPanel Table { get { return base.Get<FlowLayoutPanel>(); } private set { base.Set(value); } }

        public int SelectedIndex { get { return base.Get<int>(); } private set { base.Set(value); } }
        public Contato SelectedValue { get { return base.Get<Contato>(); } private set { base.Set(value); } }
        private RowPanel SelectedRow { get { return base.Get<RowPanel>(); } set { base.Set(value); } }

        public bool IsSelected { get { return base.Get<bool>(); } private set { base.Set(value); } }


        public Color SelectionBackColor { get { return base.Get<Color>(); } set { if (base.Set(value)) { this.UpdateAllRowsColor(); } } }
        public Color RowBackColor { get { return base.Get<Color>(); } set { if (base.Set(value)) { this.UpdateAllRowsColor(); } } }
        public Color AlternatedRowBackColor { get { return base.Get<Color>(); } set { if (base.Set(value)) { this.UpdateAllRowsColor(); } } }


        public RowPanel[] Rows
        {
            get
            {
                List<RowPanel> items = new List<RowPanel>();
                foreach (Control item in this.Table.Controls)
                {
                    RowPanel row = item as RowPanel;
                    if (row != null)
                    {
                        items.Add(row);
                    }
                }

                //return items.ToArray();
                return this.Table.Controls.ToList<RowPanel>().ToArray();
            }
        }

        #region Eventos

        public event EventHandler SelectionChanged;
        private void OnSelectionChanged()
        {
            if (this.SelectionChanged != null)
            {
                this.SelectionChanged(this, new EventArgs());
            }
        }

        #endregion Eventos

        public ContactListControl()
        {
            InitializeComponent();

             

            this.Table = new FlowLayoutPanel();
            this.Controls.Add(this.Table);
            this.Table.Margin = new Padding(0);
            this.Table.Padding = new Padding(0);
            this.Table.Dock = DockStyle.Fill;
            //this.Table.FlowDirection = FlowDirection.TopDown;

            this.Table.Name = "Table";
            //this.Table.VerticalScroll.Enabled = true;
            //this.Table.VerticalScroll.Visible = true;
            //this.Table.HorizontalScroll.Visible = false;
            this.Table.Margin = new Padding(0);
            this.Table.Padding = new Padding(0);

            this.DataSource = null;
            this.Table.Controls.Clear();
            this.DoubleBuffered = true;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            this.BackColor = Color.FromKnownColor(KnownColor.White);
            this.SelectionBackColor = Color.FromKnownColor(KnownColor.MenuHighlight);
            this.RowBackColor = Color.FromKnownColor(KnownColor.White);
            this.AlternatedRowBackColor = Color.FromKnownColor(KnownColor.WhiteSmoke);


            this.SelectIndex();

            this.Resize += ContactListControl_Resize;
            this.GotFocus += ContactListControl_GotFocus;

            this.UpdateAllRowsColor();
            this.Redimensionar();
        }

        void ContactListControl_GotFocus(object sender, EventArgs e)
        {
            this.Table.Focus();
        }

        private void ContactListControl_Resize(object sender, EventArgs e)
        {
            this.Redimensionar();
        }

        private void Redimensionar()
        {
            lock (LOCK)
            {
                this.ResizeRedraw = false;

                foreach (Control ctrl in this.Table.Controls)
                {
                    RowPanel rowPanel = ctrl as RowPanel;
                    Redimensionar(rowPanel);
                }

                this.ResizeRedraw = true;
            }
        }

        private void Redimensionar(RowPanel rowPanel)
        {
            lock (LOCK)
            {
                if (rowPanel != null)
                {
                    rowPanel.Width = this.Width;

                    Panel picPanel = rowPanel.Controls[0] as Panel;
                    Panel textPanel = rowPanel.Controls[1] as Panel;
                    Panel statusPanel = rowPanel.Controls[2] as Panel;

                    statusPanel.Left = rowPanel.Width - 5; // textPanel.Right + 2;

                    textPanel.Width = statusPanel.Left - textPanel.Left;

                    rowPanel.BoxApelido.Width = textPanel.Width - 5;
                    rowPanel.BoxDescrição.Width = textPanel.Width - 5;
                }
            }
        }
        
        private void SetContatos(BindingList<Contato> velhoContatosSource)
        {
            lock (LOCK)
            {
                if (velhoContatosSource != null)
                {
                    try
                    {
                        velhoContatosSource.ListChanged -= Contatos_ListChanged;
                        velhoContatosSource.ListChanged -= Contatos_ListChanged;
                    }
                    catch { }
                }


                if (this.DataSource != null)
                {
                    this.DataSource.ListChanged += Contatos_ListChanged;

                    this.Table.AutoScroll = true;


                    //this.Table.RowCount = 0;
                    //this.Table.ColumnCount = 1;
                    this.Table.Dock = DockStyle.Fill;
                    this.Table.Controls.Clear();

                    this.AutoScroll = true;

                    for (int idx = 0; idx < this.DataSource.Count; idx++)
                    {
                        RowPanel newRow = this.AddRowPanel(idx);
                        this.UpdateRowsColor(newRow);
                    }
                }
                else
                {
                    this.Table.Controls.Clear();
                }
            }
        }

        public void SelectIndex(int index = -1)
        {
            lock (LOCK)
            {
                index = index < 0 ? -1 : index;


                if (this.SelectedIndex != index)
                {
                    int? oldIndex = (this.IsSelected ? this.SelectedIndex : (int?)null);
                    RowPanel row = null;

                    if (index < 0 || this.DataSource == null || index >= this.DataSource.Count())
                    {
                        this.SelectedIndex = -1;
                        this.SelectedValue = null;
                        this.SelectedRow = null;
                        this.IsSelected = false;
                    }
                    else
                    {
                        row = this.Table.Controls[index] as RowPanel;

                        this.SelectedIndex = index;
                        this.SelectedValue = row.Value;
                        this.SelectedRow = row;
                        this.IsSelected = true;

                        this.Table.ScrollControlIntoView(this.SelectedRow);
                    }


                    if (oldIndex.HasValue)
                    {
                        this.UpdateRowsColor(oldIndex.Value);
                    }

                    if (row != null)
                    {
                        this.UpdateRowsColor(row);
                    }

                    this.OnSelectionChanged();
                }
            }
        }


        private void UpdateRowsColor(int index)
        {
            RowPanel row = this.Table.Controls[index] as RowPanel;
            this.UpdateRowsColor(row);
        }

        private void UpdateRowsColor(RowPanel row)
        {
            bool isSelected = (row.Index == this.SelectedIndex);

            if (isSelected)
            {
                row.BackColor = this.SelectionBackColor;
            }
            else
            {
                row.BackColor = ((row.Index % 2) == 0) ? this.RowBackColor : this.AlternatedRowBackColor;
            }
        }

        private void UpdateAllRowsColor()
        {
            lock (LOCK)
            {
                this.ResizeRedraw = false;

                foreach (Control item in this.Table.Controls)
                {
                    RowPanel row = item as RowPanel;
                    this.UpdateRowsColor(row);
                }

                this.ResizeRedraw = true;
            }
        }

        private void Contatos_ListChanged(object sender, ListChangedEventArgs e)
        {
            lock (LOCK)
            {
                if (e.ListChangedType == ListChangedType.ItemAdded)
                {
                    this.AddRowPanel(e.NewIndex);
                }
                else if (e.ListChangedType == ListChangedType.ItemChanged)
                {
                    RowPanel row = this.Table.Controls[e.NewIndex] as RowPanel;
                    row.Value = this.DataSource[e.NewIndex];
                    this.UpdateRowValues(e.NewIndex);

                }
                else if (e.ListChangedType == ListChangedType.ItemDeleted)
                {
                    // Não vai fazer nada mesmo. Provavelmente, nunca acontecerá;
                    //this.RemoveRow(e.OldIndex);
                }
                else if (e.ListChangedType == ListChangedType.Reset)
                {
                    BindingList<Contato> bkpContatos = this.DataSource;
                    this.DataSource = null;
                    this.DataSource = bkpContatos;
                }
            }
        }

        /*
        private void RemoveRow(int index)
        {
            lock (LOCK)
            {
                if (this.SelectedIndex == index)
                {
                    this.SelectIndex(-1);
                }

                table.Controls[index].Click -= ContactListControl_Click;
                table.Controls.RemoveAt(index);
            }
        }
        */

        private RowPanel AddRowPanel(int index)
        {

            lock (LOCK)
            {
                Contato contato = this.DataSource[index];

                //this.BorderStyle = System.Windows.Forms.BorderStyle.None;


                // Painel principal, que representa uma linha:
                RowPanel rowPanel = new RowPanel();
                this.Table.Controls.Add(rowPanel);
                this.Table.SetFlowBreak(rowPanel, true);
                rowPanel.Index = index;
                rowPanel.Value = contato;

                this.UpdateRowsColor(rowPanel);

                rowPanel.Dock = DockStyle.None;

                rowPanel.Click += ContactListControl_Click;
                rowPanel.DoubleClick += (object sender, EventArgs e) => { this.OnDoubleClick(e); };
                rowPanel.MouseEnter += (object sender, EventArgs e) => { rowPanel.MouseHasEntered(); };
                rowPanel.MouseLeave += (object sender, EventArgs e) => { rowPanel.MouseHasLeaved(); };
                

                rowPanel.Left = 1;
                rowPanel.Height = 60;
                rowPanel.Width = this.Width - 2;
                rowPanel.Margin = new Padding(0);
                rowPanel.Padding = new Padding(0);

                


                // Imagem:
                Panel picPanel = new Panel();
                picPanel.Width = 60;
                picPanel.Dock = DockStyle.Left;
                picPanel.Margin = new Padding(0);
                picPanel.Padding = new Padding(0);

                picPanel.Click += ContactListControl_Click;
                picPanel.DoubleClick += (object sender, EventArgs e) => { this.OnDoubleClick(e); };
                picPanel.MouseEnter += (object sender, EventArgs e) => { rowPanel.MouseHasEntered(); };
                picPanel.MouseLeave += (object sender, EventArgs e) => { rowPanel.MouseHasLeaved(); };


                rowPanel.Controls.Add(picPanel);


                PictureBox pic = new PictureBox();
                pic.Name = "PictureBox";
                //pic.Image = Util.Imagens.ScaleImage(contato.Imagem, 55, 55);
                pic.Dock = DockStyle.Fill;

                pic.Click += ContactListControl_Click;
                pic.DoubleClick += (object sender, EventArgs e) => { this.OnDoubleClick(e); };
                pic.MouseEnter += (object sender, EventArgs e) => { rowPanel.MouseHasEntered(); };
                pic.MouseLeave += (object sender, EventArgs e) => { rowPanel.MouseHasLeaved(); };

                pic.SizeMode = PictureBoxSizeMode.CenterImage;
                pic.BorderStyle = System.Windows.Forms.BorderStyle.None;
                picPanel.Controls.Add(pic);



                // Textos:
                Panel textPanel = new Panel();
                textPanel.Name = "textPanel";

                textPanel.Click += ContactListControl_Click;
                textPanel.DoubleClick += (object sender, EventArgs e) => { this.OnDoubleClick(e); };
                textPanel.MouseEnter += (object sender, EventArgs e) => { rowPanel.MouseHasEntered(); };
                textPanel.MouseLeave += (object sender, EventArgs e) => { rowPanel.MouseHasLeaved(); };

                textPanel.Margin = new Padding(0);
                textPanel.Padding = new Padding(0);
                textPanel.Left = picPanel.Right + 1;
                textPanel.Width = 50;

                rowPanel.Controls.Add(textPanel);



                Label lblApelido = new Label();
                lblApelido.Name = "Label";
                //lblApelido.Text = contato.Apelido.TakeLeft(19);
                lblApelido.Font = Fontes.Fonte(12F);
                lblApelido.TextAlign = ContentAlignment.MiddleLeft;
                lblApelido.Width = textPanel.Width - 5;
                lblApelido.AutoSize = false;
                lblApelido.Dock = DockStyle.Top;

                lblApelido.Click += ContactListControl_Click;
                lblApelido.DoubleClick += (object sender, EventArgs e) => { this.OnDoubleClick(e); };
                lblApelido.MouseEnter += (object sender, EventArgs e) => { rowPanel.MouseHasEntered(); };
                lblApelido.MouseLeave += (object sender, EventArgs e) => { rowPanel.MouseHasLeaved(); };

                textPanel.Controls.Add(lblApelido);

                Label lblDescrição = new Label();
                lblDescrição.Enabled = false;
                lblDescrição.Name = "Label";
                //lblDescrição.Text = contato.Descrição.TakeLeft(45);
                lblDescrição.Font = Fontes.Fonte(8F);

                lblDescrição.Height = textPanel.Height - lblApelido.Bottom - 2;
                lblDescrição.Width = textPanel.Width - 5;
                lblDescrição.AutoSize = false;
                lblDescrição.Dock = DockStyle.Bottom;

                lblDescrição.Click += ContactListControl_Click;
                lblDescrição.DoubleClick += (object sender, EventArgs e) => { this.OnDoubleClick(e); };
                lblDescrição.MouseEnter += (object sender, EventArgs e) => { rowPanel.MouseHasEntered(); };
                lblDescrição.MouseLeave += (object sender, EventArgs e) => { rowPanel.MouseHasLeaved();};

                textPanel.Controls.Add(lblDescrição);


                // Status Panel:
                Panel statusPanel = new Panel();
                statusPanel.Name = "statusPanel";
                statusPanel.Margin = new Padding(0);
                statusPanel.Padding = new Padding(0);
                statusPanel.Top = 0;
                statusPanel.Width = 5;
                statusPanel.Height = rowPanel.Height;
                statusPanel.Left = rowPanel.Width - 5; // textPanel.Right + 2;
                statusPanel.BringToFront();
                //statusPanel.BackColor = contato.Status.ToColor();
                statusPanel.Visible = true;
                statusPanel.BorderStyle = System.Windows.Forms.BorderStyle.None;

                statusPanel.MouseEnter += (object sender, EventArgs e) => { rowPanel.MouseHasEntered(); };
                statusPanel.MouseLeave += (object sender, EventArgs e) => { rowPanel.MouseHasLeaved();};

                rowPanel.Controls.Add(statusPanel);

                textPanel.Width = statusPanel.Left - textPanel.Left;

                // Adicionando valores:
                rowPanel.BoxApelido = lblApelido;
                rowPanel.BoxDescrição = lblDescrição;
                rowPanel.BoxImagem = pic;
                rowPanel.BoxStatus = statusPanel;

                rowPanel.MouseEnter += (object sender, EventArgs e) => { rowPanel.MouseHasEntered(); };
                rowPanel.MouseLeave += (object sender, EventArgs e) => { rowPanel.MouseHasLeaved(); };

                // Atualizando valores:
                this.UpdateRowValues(rowPanel.Index);
                this.Redimensionar(rowPanel);

                return rowPanel;
            }
        }

        /*
        void rowPanel_MouseLeave(object sender, EventArgs e)
        {
            RowPanel row = sender as RowPanel;

            if (row != null)
            {
                row.MouseHasLeaved();
            }
        }

        void rowPanel_MouseEnter(object sender, EventArgs e)
        {
            RowPanel row = sender as RowPanel;

            if (row != null)
            {
                row.MouseHasEntered();
            }
        }
        */

        private void UpdateRowValues(int index)
        {
            lock (LOCK)
            {
                RowPanel row = this.Table.Controls[index] as RowPanel;
                row.BoxApelido.Text = row.Value.Apelido.Trim().TakeLeft(19);
                row.BoxDescrição.Text = row.Value.Descrição.Trim().TakeLeft(45);
                row.BoxImagem.Image = Util.Imagens.ScaleImage(row.Value.Imagem, 55, 55);
                row.BoxStatus.BackColor = row.Value.Status.ToColor();
            }
        }

        private void ContactListControl_Click(object sender, EventArgs e)
        {
            lock (LOCK)
            {
                Control control = sender as Control;

                RowPanel row = FindRowPanel(control);

                if (row != null)
                {
                    this.SelectIndex(row.Index);
                }
                else
                {
                    this.SelectIndex();
                }
            }

            base.OnClick(e);
        }

        private RowPanel FindRowPanel(Control selectedControl)
        {
            lock (LOCK)
            {
                RowPanel found = selectedControl as RowPanel;

                if (found == null)
                {
                    if (selectedControl.Parent != null)
                    {
                        found = FindRowPanel(selectedControl.Parent);
                    }
                }

                return found;
            }
        }
    }
}