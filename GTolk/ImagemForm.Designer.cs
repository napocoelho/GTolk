namespace GTolk
{
    partial class ImagemForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureOrigem = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMenosZoom = new System.Windows.Forms.Button();
            this.pictureFinal = new System.Windows.Forms.PictureBox();
            this.btnMaisZoom = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAceitar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.salvarImagemQuadradaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salvarImagemRedondaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureOrigem)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureFinal)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.pictureOrigem);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(253, 296);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Escolha uma imagem:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(78, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Escolher imagem...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // pictureOrigem
            // 
            this.pictureOrigem.Location = new System.Drawing.Point(6, 48);
            this.pictureOrigem.Name = "pictureOrigem";
            this.pictureOrigem.Size = new System.Drawing.Size(240, 240);
            this.pictureOrigem.TabIndex = 0;
            this.pictureOrigem.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(80, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Zoom";
            // 
            // btnMenosZoom
            // 
            this.btnMenosZoom.Location = new System.Drawing.Point(149, 19);
            this.btnMenosZoom.Name = "btnMenosZoom";
            this.btnMenosZoom.Size = new System.Drawing.Size(23, 23);
            this.btnMenosZoom.TabIndex = 17;
            this.btnMenosZoom.Text = "-";
            this.btnMenosZoom.UseVisualStyleBackColor = true;
            this.btnMenosZoom.Click += new System.EventHandler(this.btnMenosZoom_Click_1);
            // 
            // pictureFinal
            // 
            this.pictureFinal.Location = new System.Drawing.Point(6, 50);
            this.pictureFinal.Name = "pictureFinal";
            this.pictureFinal.Size = new System.Drawing.Size(240, 240);
            this.pictureFinal.TabIndex = 15;
            this.pictureFinal.TabStop = false;
            this.pictureFinal.Click += new System.EventHandler(this.pictureFinal_Click);
            this.pictureFinal.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureFinal_MouseClick);
            // 
            // btnMaisZoom
            // 
            this.btnMaisZoom.Location = new System.Drawing.Point(120, 19);
            this.btnMaisZoom.Name = "btnMaisZoom";
            this.btnMaisZoom.Size = new System.Drawing.Size(23, 23);
            this.btnMaisZoom.TabIndex = 16;
            this.btnMaisZoom.Text = "+";
            this.btnMaisZoom.UseVisualStyleBackColor = true;
            this.btnMaisZoom.Click += new System.EventHandler(this.btnMaisZoom_Click_1);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureFinal);
            this.groupBox2.Controls.Add(this.btnMenosZoom);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.btnMaisZoom);
            this.groupBox2.Location = new System.Drawing.Point(271, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(253, 296);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Posicione sua imagem:";
            // 
            // btnAceitar
            // 
            this.btnAceitar.Location = new System.Drawing.Point(368, 314);
            this.btnAceitar.Name = "btnAceitar";
            this.btnAceitar.Size = new System.Drawing.Size(75, 23);
            this.btnAceitar.TabIndex = 22;
            this.btnAceitar.Text = "Aceitar";
            this.btnAceitar.UseVisualStyleBackColor = true;
            this.btnAceitar.Click += new System.EventHandler(this.btnAceitar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(449, 314);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 23;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.salvarImagemQuadradaToolStripMenuItem,
            this.salvarImagemRedondaToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(215, 48);
            // 
            // salvarImagemQuadradaToolStripMenuItem
            // 
            this.salvarImagemQuadradaToolStripMenuItem.Name = "salvarImagemQuadradaToolStripMenuItem";
            this.salvarImagemQuadradaToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.salvarImagemQuadradaToolStripMenuItem.Text = "Salvar imagem quadrada...";
            this.salvarImagemQuadradaToolStripMenuItem.Click += new System.EventHandler(this.salvarImagemQuadradaToolStripMenuItem_Click);
            // 
            // salvarImagemRedondaToolStripMenuItem
            // 
            this.salvarImagemRedondaToolStripMenuItem.Name = "salvarImagemRedondaToolStripMenuItem";
            this.salvarImagemRedondaToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.salvarImagemRedondaToolStripMenuItem.Text = "Salvar imagem redonda...";
            this.salvarImagemRedondaToolStripMenuItem.Click += new System.EventHandler(this.salvarImagemRedondaToolStripMenuItem_Click);
            // 
            // ImagemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 436);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceitar);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ImagemForm";
            this.Text = "Imagem";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureOrigem)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureFinal)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureOrigem;
        private System.Windows.Forms.Button btnMenosZoom;
        private System.Windows.Forms.PictureBox pictureFinal;
        private System.Windows.Forms.Button btnMaisZoom;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAceitar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem salvarImagemQuadradaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salvarImagemRedondaToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}