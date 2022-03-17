namespace GTolk
{
    partial class ConversasForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConversasForm));
            this.tabControlNew = new Crownwood.Magic.Controls.TabControl();
            this.popUpMenuText = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.popUpMenuBrowser = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copiarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.históricoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exibirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.umDiaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.umaSemanaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.umMêsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.seisMesesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tudoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.popUpMenuBrowser.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlNew
            // 
            this.tabControlNew.BoldSelectedPage = true;
            this.tabControlNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlNew.HideTabsMode = Crownwood.Magic.Controls.TabControl.HideTabsModes.ShowAlways;
            this.tabControlNew.Location = new System.Drawing.Point(0, 0);
            this.tabControlNew.Name = "tabControlNew";
            this.tabControlNew.PositionTop = true;
            this.tabControlNew.ShowArrows = true;
            this.tabControlNew.ShowClose = true;
            this.tabControlNew.Size = new System.Drawing.Size(654, 342);
            this.tabControlNew.TabIndex = 0;
            this.tabControlNew.TabStop = false;
            this.tabControlNew.ClosePressed += new System.EventHandler(this.tabControlNew_ClosePressed);
            this.tabControlNew.SelectionChanged += new System.EventHandler(this.tabControlNew_SelectionChanged);
            // 
            // popUpMenuText
            // 
            this.popUpMenuText.Name = "popUpMenu";
            this.popUpMenuText.Size = new System.Drawing.Size(61, 4);
            this.popUpMenuText.Text = "popUpMenuText";
            // 
            // popUpMenuBrowser
            // 
            this.popUpMenuBrowser.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copiarToolStripMenuItem,
            this.toolStripSeparator1,
            this.históricoToolStripMenuItem});
            this.popUpMenuBrowser.Name = "popUpMenuBrowser";
            this.popUpMenuBrowser.Size = new System.Drawing.Size(156, 54);
            // 
            // copiarToolStripMenuItem
            // 
            this.copiarToolStripMenuItem.Name = "copiarToolStripMenuItem";
            this.copiarToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.copiarToolStripMenuItem.Text = "&Copiar [Ctrl+C]";
            this.copiarToolStripMenuItem.Click += new System.EventHandler(this.copiarToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(152, 6);
            // 
            // históricoToolStripMenuItem
            // 
            this.históricoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exibirToolStripMenuItem,
            this.umDiaToolStripMenuItem,
            this.umaSemanaToolStripMenuItem,
            this.umMêsToolStripMenuItem,
            this.seisMesesToolStripMenuItem,
            this.tudoToolStripMenuItem});
            this.históricoToolStripMenuItem.Name = "históricoToolStripMenuItem";
            this.históricoToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.históricoToolStripMenuItem.Text = "Histórico";
            // 
            // exibirToolStripMenuItem
            // 
            this.exibirToolStripMenuItem.BackColor = System.Drawing.SystemColors.ControlLight;
            this.exibirToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.exibirToolStripMenuItem.Enabled = false;
            this.exibirToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Underline);
            this.exibirToolStripMenuItem.Name = "exibirToolStripMenuItem";
            this.exibirToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.exibirToolStripMenuItem.Text = "Mostrar:";
            // 
            // umDiaToolStripMenuItem
            // 
            this.umDiaToolStripMenuItem.Name = "umDiaToolStripMenuItem";
            this.umDiaToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.umDiaToolStripMenuItem.Text = "Um dia";
            this.umDiaToolStripMenuItem.Click += new System.EventHandler(this.umDiaToolStripMenuItem_Click);
            // 
            // umaSemanaToolStripMenuItem
            // 
            this.umaSemanaToolStripMenuItem.Name = "umaSemanaToolStripMenuItem";
            this.umaSemanaToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.umaSemanaToolStripMenuItem.Text = "Uma semana";
            this.umaSemanaToolStripMenuItem.Click += new System.EventHandler(this.umaSemanaToolStripMenuItem_Click);
            // 
            // umMêsToolStripMenuItem
            // 
            this.umMêsToolStripMenuItem.Name = "umMêsToolStripMenuItem";
            this.umMêsToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.umMêsToolStripMenuItem.Text = "Um mês";
            this.umMêsToolStripMenuItem.Click += new System.EventHandler(this.umMêsToolStripMenuItem_Click);
            // 
            // seisMesesToolStripMenuItem
            // 
            this.seisMesesToolStripMenuItem.Name = "seisMesesToolStripMenuItem";
            this.seisMesesToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.seisMesesToolStripMenuItem.Text = "Seis meses";
            this.seisMesesToolStripMenuItem.Click += new System.EventHandler(this.seisMesesToolStripMenuItem_Click);
            // 
            // tudoToolStripMenuItem
            // 
            this.tudoToolStripMenuItem.Name = "tudoToolStripMenuItem";
            this.tudoToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.tudoToolStripMenuItem.Text = "Tudo";
            this.tudoToolStripMenuItem.Click += new System.EventHandler(this.tudoToolStripMenuItem_Click);
            // 
            // ConversasForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 342);
            this.Controls.Add(this.tabControlNew);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConversasForm";
            this.Text = "Chat";
            this.Activated += new System.EventHandler(this.ChatForm_Activated);
            this.Deactivate += new System.EventHandler(this.ChatForm_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ChatForm_FormClosed);
            this.Load += new System.EventHandler(this.ChatForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ChatForm_KeyDown);
            this.popUpMenuBrowser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Crownwood.Magic.Controls.TabControl tabControlNew;
        private System.Windows.Forms.ContextMenuStrip popUpMenuText;
        private System.Windows.Forms.ContextMenuStrip popUpMenuBrowser;
        private System.Windows.Forms.ToolStripMenuItem copiarToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem históricoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem umDiaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem umaSemanaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem umMêsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exibirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem seisMesesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tudoToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}