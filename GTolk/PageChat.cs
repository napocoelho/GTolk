using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using GTolk.Models;

namespace GTolk
{
    public partial class PageChat : UserControl
    {
        private object LOCK = new object();

        public TextBox TextBox { get { return this.txtMensagem; } }
        public WebBrowser WebBrowser { get { return this.browser; } }
        public SplitContainer SplitContainer { get { return this.splitContainer; } }
        public Crownwood.Magic.Controls.TabPage TabPageRelated { get; private set; }
        public Conversa Conversa { get; private set; }

        public bool IsActivated { get; private set; }
        public int MensagensNãoLidas { get; private set; }

        public PageChat(Crownwood.Magic.Controls.TabControl bindsToTabControl, Crownwood.Magic.Controls.TabPage bindsToTabPage, Conversa conversa)
        {
            this.MensagensNãoLidas = 0;
            this.IsActivated = false;

            InitializeComponent();

            browser.Margin = new System.Windows.Forms.Padding(0);
            browser.Padding = new System.Windows.Forms.Padding(0);
            
            txtMensagem.Margin = new System.Windows.Forms.Padding(0);
            txtMensagem.Padding = new System.Windows.Forms.Padding(0);
            txtMensagem.AllowDrop = false;
            //txtMensagem.DragDrop += txtMensagem_DragDrop;
            //txtMensagem.DragOver += txtMensagem_DragOver;
            //txtMensagem.DragLeave += txtMensagem_DragLeave;

            this.Enabled = true;
            this.SplitContainer.Panel2MinSize = 30;
            //this.SplitContainer.Panel2.Height = 100;

            this.Conversa = conversa;
            
            this.TabPageRelated = bindsToTabPage;
            Crownwood.Magic.Controls.TabControl tabControl = bindsToTabControl;
            tabControl.TabIndexChanged += tabControl_TabIndexChanged;
            this.VisualizouMensagens();
        }
        
        public void RecebeuMensagem()
        {
            if (!this.IsActivated)
            {
                this.MensagensNãoLidas++;
                this.TabPageRelated.Title = "*" + this.Conversa.ConversandoCom.Apelido + " (" + this.MensagensNãoLidas + ")";
            }
        }

        public void VisualizouMensagens()
        {
            this.TabPageRelated.Title = this.Conversa.ConversandoCom.Apelido;
        }

        void tabControl_TabIndexChanged(object sender, EventArgs e)
        {
            Crownwood.Magic.Controls.TabControl tabControl = sender as Crownwood.Magic.Controls.TabControl;

            if (object.ReferenceEquals(tabControl.SelectedTab, this.TabPageRelated))
            {
                this.VisualizouMensagens();
            }
        }

        private void PageChat_Enter(object sender, EventArgs e)
        {
            this.OnEnter();
        }

        private void PageChat_Leave(object sender, EventArgs e)
        {
            this.OnLeave();
        }

        private void OnEnter()
        {
            this.IsActivated = true;
            this.VisualizouMensagens();
        }

        private void OnLeave()
        {
            this.IsActivated = false;
        }
    }
}