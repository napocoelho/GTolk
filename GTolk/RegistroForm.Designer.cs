namespace GTolk
{
    partial class RegistroForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegistroForm));
            this.txtSenhaEmail = new System.Windows.Forms.TextBox();
            this.txtEmailEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupEmail = new System.Windows.Forms.GroupBox();
            this.lblCapsLock = new System.Windows.Forms.Label();
            this.groupConfiguraçõesAdicionais = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.chkSsl = new System.Windows.Forms.CheckBox();
            this.txtPorta = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSmtp = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnVerificar = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupChat = new System.Windows.Forms.GroupBox();
            this.lblCapsLockChat = new System.Windows.Forms.Label();
            this.btnRegistrar = new System.Windows.Forms.Button();
            this.txtApelido = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSenhaChatConfirmação = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSenhaChat = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupConfirmação = new System.Windows.Forms.GroupBox();
            this.lblConfirmação = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCódigoEmail = new System.Windows.Forms.TextBox();
            this.groupEmail.SuspendLayout();
            this.groupConfiguraçõesAdicionais.SuspendLayout();
            this.groupChat.SuspendLayout();
            this.groupConfirmação.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSenhaEmail
            // 
            this.txtSenhaEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSenhaEmail.Location = new System.Drawing.Point(67, 129);
            this.txtSenhaEmail.MaxLength = 20;
            this.txtSenhaEmail.Name = "txtSenhaEmail";
            this.txtSenhaEmail.PasswordChar = '*';
            this.txtSenhaEmail.Size = new System.Drawing.Size(126, 20);
            this.txtSenhaEmail.TabIndex = 1;
            this.txtSenhaEmail.UseSystemPasswordChar = true;
            this.txtSenhaEmail.TextChanged += new System.EventHandler(this.txtSenhaEmail_TextChanged);
            // 
            // txtEmailEmail
            // 
            this.txtEmailEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmailEmail.Location = new System.Drawing.Point(67, 103);
            this.txtEmailEmail.Name = "txtEmailEmail";
            this.txtEmailEmail.Size = new System.Drawing.Size(182, 20);
            this.txtEmailEmail.TabIndex = 0;
            this.txtEmailEmail.TextChanged += new System.EventHandler(this.txtEmailEmail_TextChanged);
            this.txtEmailEmail.Validated += new System.EventHandler(this.txtEmailEmail_Validated);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Email";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Senha";
            // 
            // groupEmail
            // 
            this.groupEmail.Controls.Add(this.lblCapsLock);
            this.groupEmail.Controls.Add(this.groupConfiguraçõesAdicionais);
            this.groupEmail.Controls.Add(this.btnVerificar);
            this.groupEmail.Controls.Add(this.label3);
            this.groupEmail.Controls.Add(this.txtEmailEmail);
            this.groupEmail.Controls.Add(this.txtSenhaEmail);
            this.groupEmail.Controls.Add(this.label1);
            this.groupEmail.Controls.Add(this.label2);
            this.groupEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupEmail.Location = new System.Drawing.Point(12, 12);
            this.groupEmail.Name = "groupEmail";
            this.groupEmail.Size = new System.Drawing.Size(258, 295);
            this.groupEmail.TabIndex = 4;
            this.groupEmail.TabStop = false;
            this.groupEmail.Text = "Conta de Email:";
            // 
            // lblCapsLock
            // 
            this.lblCapsLock.AutoSize = true;
            this.lblCapsLock.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCapsLock.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblCapsLock.Location = new System.Drawing.Point(199, 133);
            this.lblCapsLock.Name = "lblCapsLock";
            this.lblCapsLock.Size = new System.Drawing.Size(49, 12);
            this.lblCapsLock.TabIndex = 11;
            this.lblCapsLock.Text = "Caps Lock";
            // 
            // groupConfiguraçõesAdicionais
            // 
            this.groupConfiguraçõesAdicionais.Controls.Add(this.label10);
            this.groupConfiguraçõesAdicionais.Controls.Add(this.chkSsl);
            this.groupConfiguraçõesAdicionais.Controls.Add(this.txtPorta);
            this.groupConfiguraçõesAdicionais.Controls.Add(this.label9);
            this.groupConfiguraçõesAdicionais.Controls.Add(this.txtSmtp);
            this.groupConfiguraçõesAdicionais.Controls.Add(this.label8);
            this.groupConfiguraçõesAdicionais.Location = new System.Drawing.Point(9, 155);
            this.groupConfiguraçõesAdicionais.Name = "groupConfiguraçõesAdicionais";
            this.groupConfiguraçõesAdicionais.Size = new System.Drawing.Size(240, 101);
            this.groupConfiguraçõesAdicionais.TabIndex = 10;
            this.groupConfiguraçõesAdicionais.TabStop = false;
            this.groupConfiguraçõesAdicionais.Text = "Configurações adicionais:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label10.Location = new System.Drawing.Point(7, 77);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(150, 13);
            this.label10.TabIndex = 8;
            this.label10.Text = "* Portas comuns: 25, 465, 587";
            // 
            // chkSsl
            // 
            this.chkSsl.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSsl.Location = new System.Drawing.Point(120, 49);
            this.chkSsl.Name = "chkSsl";
            this.chkSsl.Size = new System.Drawing.Size(46, 18);
            this.chkSsl.TabIndex = 4;
            this.chkSsl.Text = "SSL";
            this.chkSsl.UseVisualStyleBackColor = true;
            // 
            // txtPorta
            // 
            this.txtPorta.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPorta.Location = new System.Drawing.Point(58, 48);
            this.txtPorta.MaxLength = 5;
            this.txtPorta.Name = "txtPorta";
            this.txtPorta.Size = new System.Drawing.Size(52, 20);
            this.txtPorta.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(7, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Porta";
            // 
            // txtSmtp
            // 
            this.txtSmtp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSmtp.Location = new System.Drawing.Point(58, 22);
            this.txtSmtp.MaxLength = 512;
            this.txtSmtp.Name = "txtSmtp";
            this.txtSmtp.Size = new System.Drawing.Size(176, 20);
            this.txtSmtp.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(7, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(37, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "SMTP";
            // 
            // btnVerificar
            // 
            this.btnVerificar.Location = new System.Drawing.Point(174, 262);
            this.btnVerificar.Name = "btnVerificar";
            this.btnVerificar.Size = new System.Drawing.Size(75, 23);
            this.btnVerificar.TabIndex = 5;
            this.btnVerificar.Text = "Verificar...";
            this.btnVerificar.UseVisualStyleBackColor = true;
            this.btnVerificar.Click += new System.EventHandler(this.btnVerificar_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(243, 69);
            this.label3.TabIndex = 4;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // groupChat
            // 
            this.groupChat.Controls.Add(this.lblCapsLockChat);
            this.groupChat.Controls.Add(this.btnRegistrar);
            this.groupChat.Controls.Add(this.txtApelido);
            this.groupChat.Controls.Add(this.label6);
            this.groupChat.Controls.Add(this.txtSenhaChatConfirmação);
            this.groupChat.Controls.Add(this.label5);
            this.groupChat.Controls.Add(this.txtSenhaChat);
            this.groupChat.Controls.Add(this.label4);
            this.groupChat.Enabled = false;
            this.groupChat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupChat.Location = new System.Drawing.Point(11, 392);
            this.groupChat.Name = "groupChat";
            this.groupChat.Size = new System.Drawing.Size(259, 137);
            this.groupChat.TabIndex = 5;
            this.groupChat.TabStop = false;
            this.groupChat.Text = "Conta do Chat:";
            // 
            // lblCapsLockChat
            // 
            this.lblCapsLockChat.AutoSize = true;
            this.lblCapsLockChat.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCapsLockChat.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblCapsLockChat.Location = new System.Drawing.Point(99, 112);
            this.lblCapsLockChat.Name = "lblCapsLockChat";
            this.lblCapsLockChat.Size = new System.Drawing.Size(49, 12);
            this.lblCapsLockChat.TabIndex = 12;
            this.lblCapsLockChat.Text = "Caps Lock";
            // 
            // btnRegistrar
            // 
            this.btnRegistrar.Location = new System.Drawing.Point(175, 106);
            this.btnRegistrar.Name = "btnRegistrar";
            this.btnRegistrar.Size = new System.Drawing.Size(75, 23);
            this.btnRegistrar.TabIndex = 9;
            this.btnRegistrar.Text = "Registrar";
            this.btnRegistrar.UseVisualStyleBackColor = true;
            this.btnRegistrar.Click += new System.EventHandler(this.btnRegistrar_Click);
            // 
            // txtApelido
            // 
            this.txtApelido.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApelido.Location = new System.Drawing.Point(102, 28);
            this.txtApelido.MaxLength = 12;
            this.txtApelido.Name = "txtApelido";
            this.txtApelido.Size = new System.Drawing.Size(147, 20);
            this.txtApelido.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "Apelido";
            // 
            // txtSenhaChatConfirmação
            // 
            this.txtSenhaChatConfirmação.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSenhaChatConfirmação.Location = new System.Drawing.Point(102, 80);
            this.txtSenhaChatConfirmação.MaxLength = 16;
            this.txtSenhaChatConfirmação.Name = "txtSenhaChatConfirmação";
            this.txtSenhaChatConfirmação.PasswordChar = '*';
            this.txtSenhaChatConfirmação.Size = new System.Drawing.Size(147, 20);
            this.txtSenhaChatConfirmação.TabIndex = 8;
            this.txtSenhaChatConfirmação.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Confirmação";
            // 
            // txtSenhaChat
            // 
            this.txtSenhaChat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSenhaChat.Location = new System.Drawing.Point(102, 54);
            this.txtSenhaChat.MaxLength = 16;
            this.txtSenhaChat.Name = "txtSenhaChat";
            this.txtSenhaChat.PasswordChar = '*';
            this.txtSenhaChat.Size = new System.Drawing.Size(147, 20);
            this.txtSenhaChat.TabIndex = 7;
            this.txtSenhaChat.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Senha";
            // 
            // groupConfirmação
            // 
            this.groupConfirmação.Controls.Add(this.lblConfirmação);
            this.groupConfirmação.Controls.Add(this.label7);
            this.groupConfirmação.Controls.Add(this.txtCódigoEmail);
            this.groupConfirmação.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupConfirmação.Location = new System.Drawing.Point(12, 313);
            this.groupConfirmação.Name = "groupConfirmação";
            this.groupConfirmação.Size = new System.Drawing.Size(258, 73);
            this.groupConfirmação.TabIndex = 6;
            this.groupConfirmação.TabStop = false;
            this.groupConfirmação.Text = "Confirmação:";
            // 
            // lblConfirmação
            // 
            this.lblConfirmação.AutoSize = true;
            this.lblConfirmação.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfirmação.Location = new System.Drawing.Point(128, 48);
            this.lblConfirmação.Name = "lblConfirmação";
            this.lblConfirmação.Size = new System.Drawing.Size(60, 13);
            this.lblConfirmação.TabIndex = 12;
            this.lblConfirmação.Text = "Confirmado";
            this.lblConfirmação.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Código";
            // 
            // txtCódigoEmail
            // 
            this.txtCódigoEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCódigoEmail.Location = new System.Drawing.Point(69, 25);
            this.txtCódigoEmail.Name = "txtCódigoEmail";
            this.txtCódigoEmail.Size = new System.Drawing.Size(182, 20);
            this.txtCódigoEmail.TabIndex = 10;
            this.txtCódigoEmail.TextChanged += new System.EventHandler(this.txtCódigoEmail_TextChanged_1);
            // 
            // RegistroForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(281, 535);
            this.Controls.Add(this.groupConfirmação);
            this.Controls.Add(this.groupChat);
            this.Controls.Add(this.groupEmail);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RegistroForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registrar";
            this.groupEmail.ResumeLayout(false);
            this.groupEmail.PerformLayout();
            this.groupConfiguraçõesAdicionais.ResumeLayout(false);
            this.groupConfiguraçõesAdicionais.PerformLayout();
            this.groupChat.ResumeLayout(false);
            this.groupChat.PerformLayout();
            this.groupConfirmação.ResumeLayout(false);
            this.groupConfirmação.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtSenhaEmail;
        private System.Windows.Forms.TextBox txtEmailEmail;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupEmail;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupChat;
        private System.Windows.Forms.TextBox txtSenhaChatConfirmação;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSenhaChat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnVerificar;
        private System.Windows.Forms.TextBox txtApelido;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnRegistrar;
        private System.Windows.Forms.GroupBox groupConfiguraçõesAdicionais;
        private System.Windows.Forms.CheckBox chkSsl;
        private System.Windows.Forms.TextBox txtPorta;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSmtp;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupConfirmação;
        private System.Windows.Forms.Label lblConfirmação;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCódigoEmail;
        private System.Windows.Forms.Label lblCapsLock;
        private System.Windows.Forms.Label lblCapsLockChat;
    }
}