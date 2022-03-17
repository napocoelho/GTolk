namespace GTolkServer
{
    partial class Form1
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
            this.btnVerificarIp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnVerificarIp
            // 
            this.btnVerificarIp.Location = new System.Drawing.Point(114, 146);
            this.btnVerificarIp.Name = "btnVerificarIp";
            this.btnVerificarIp.Size = new System.Drawing.Size(281, 23);
            this.btnVerificarIp.TabIndex = 0;
            this.btnVerificarIp.Text = "Verificar IP";
            this.btnVerificarIp.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 371);
            this.Controls.Add(this.btnVerificarIp);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnVerificarIp;
    }
}

