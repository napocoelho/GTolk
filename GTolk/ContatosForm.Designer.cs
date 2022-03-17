namespace GTolk
{
    partial class ContatosForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContatosForm));
            this.pictureBoxUsuário = new System.Windows.Forms.PictureBox();
            this.painelDoUsuário = new System.Windows.Forms.Panel();
            this.lblDescriçãoUsuário = new System.Windows.Forms.Label();
            this.panelUsuárioStatus = new System.Windows.Forms.Panel();
            this.lblNomeUsuário = new System.Windows.Forms.Label();
            this.panelStatus = new System.Windows.Forms.Panel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemStatusOcupadoDesocupado = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemDeslogar = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemSair = new System.Windows.Forms.ToolStripMenuItem();
            this.ContactControl = new GTolk.ContactListControl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUsuário)).BeginInit();
            this.painelDoUsuário.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBoxUsuário
            // 
            this.pictureBoxUsuário.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxUsuário.Name = "pictureBoxUsuário";
            this.pictureBoxUsuário.Size = new System.Drawing.Size(60, 60);
            this.pictureBoxUsuário.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxUsuário.TabIndex = 7;
            this.pictureBoxUsuário.TabStop = false;
            // 
            // painelDoUsuário
            // 
            this.painelDoUsuário.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.painelDoUsuário.Controls.Add(this.lblDescriçãoUsuário);
            this.painelDoUsuário.Controls.Add(this.panelUsuárioStatus);
            this.painelDoUsuário.Controls.Add(this.lblNomeUsuário);
            this.painelDoUsuário.Controls.Add(this.pictureBoxUsuário);
            this.painelDoUsuário.Dock = System.Windows.Forms.DockStyle.Top;
            this.painelDoUsuário.Location = new System.Drawing.Point(0, 0);
            this.painelDoUsuário.Name = "painelDoUsuário";
            this.painelDoUsuário.Size = new System.Drawing.Size(208, 60);
            this.painelDoUsuário.TabIndex = 9;
            // 
            // lblDescriçãoUsuário
            // 
            this.lblDescriçãoUsuário.Location = new System.Drawing.Point(66, 31);
            this.lblDescriçãoUsuário.Name = "lblDescriçãoUsuário";
            this.lblDescriçãoUsuário.Size = new System.Drawing.Size(130, 27);
            this.lblDescriçãoUsuário.TabIndex = 10;
            this.lblDescriçãoUsuário.Text = "label1";
            // 
            // panelUsuárioStatus
            // 
            this.panelUsuárioStatus.Location = new System.Drawing.Point(202, 0);
            this.panelUsuárioStatus.Name = "panelUsuárioStatus";
            this.panelUsuárioStatus.Size = new System.Drawing.Size(5, 60);
            this.panelUsuárioStatus.TabIndex = 9;
            // 
            // lblNomeUsuário
            // 
            this.lblNomeUsuário.AutoSize = true;
            this.lblNomeUsuário.Location = new System.Drawing.Point(66, 3);
            this.lblNomeUsuário.Name = "lblNomeUsuário";
            this.lblNomeUsuário.Size = new System.Drawing.Size(35, 13);
            this.lblNomeUsuário.TabIndex = 8;
            this.lblNomeUsuário.Text = "label1";
            // 
            // panelStatus
            // 
            this.panelStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelStatus.Location = new System.Drawing.Point(0, 476);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(208, 30);
            this.panelStatus.TabIndex = 10;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripSeparator1,
            this.ToolStripMenuItemDeslogar,
            this.ToolStripMenuItemSair});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(121, 76);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemStatusOcupadoDesocupado});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(120, 22);
            this.toolStripMenuItem1.Text = "Status";
            // 
            // ToolStripMenuItemStatusOcupadoDesocupado
            // 
            this.ToolStripMenuItemStatusOcupadoDesocupado.Name = "ToolStripMenuItemStatusOcupadoDesocupado";
            this.ToolStripMenuItemStatusOcupadoDesocupado.Size = new System.Drawing.Size(123, 22);
            this.ToolStripMenuItemStatusOcupadoDesocupado.Text = "Ocupado";
            this.ToolStripMenuItemStatusOcupadoDesocupado.Click += new System.EventHandler(this.ocupadoToolStripMenuItemStatusOcupadoDesocupado_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(117, 6);
            // 
            // ToolStripMenuItemDeslogar
            // 
            this.ToolStripMenuItemDeslogar.Name = "ToolStripMenuItemDeslogar";
            this.ToolStripMenuItemDeslogar.Size = new System.Drawing.Size(120, 22);
            this.ToolStripMenuItemDeslogar.Text = "Deslogar";
            this.ToolStripMenuItemDeslogar.Click += new System.EventHandler(this.ToolStripMenuItemDeslogar_Click);
            // 
            // ToolStripMenuItemSair
            // 
            this.ToolStripMenuItemSair.Name = "ToolStripMenuItemSair";
            this.ToolStripMenuItemSair.Size = new System.Drawing.Size(120, 22);
            this.ToolStripMenuItemSair.Text = "Sair";
            this.ToolStripMenuItemSair.Click += new System.EventHandler(this.ToolStripMenuItemSair_Click);
            // 
            // ContactControl
            // 
            this.ContactControl.AlternatedRowBackColor = System.Drawing.Color.WhiteSmoke;
            this.ContactControl.AutoScroll = true;
            this.ContactControl.BackColor = System.Drawing.Color.White;
            this.ContactControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ContactControl.DataSource = null;
            this.ContactControl.Location = new System.Drawing.Point(0, 66);
            this.ContactControl.Name = "ContactControl";
            this.ContactControl.RowBackColor = System.Drawing.Color.White;
            this.ContactControl.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.ContactControl.Size = new System.Drawing.Size(208, 404);
            this.ContactControl.TabIndex = 8;
            // 
            // ContatosForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(208, 506);
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.painelDoUsuário);
            this.Controls.Add(this.ContactControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ContatosForm";
            this.Text = "GTolk";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ContatosForm_FormClosing);
            this.Load += new System.EventHandler(this.ContatosForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ContatosForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxUsuário)).EndInit();
            this.painelDoUsuário.ResumeLayout(false);
            this.painelDoUsuário.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxUsuário;
        private ContactListControl ContactControl;
        private System.Windows.Forms.Panel painelDoUsuário;
        private System.Windows.Forms.Label lblNomeUsuário;
        private System.Windows.Forms.Panel panelUsuárioStatus;
        private System.Windows.Forms.Panel panelStatus;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemStatusOcupadoDesocupado;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemSair;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDeslogar;
        private System.Windows.Forms.Label lblDescriçãoUsuário;
    }
}

