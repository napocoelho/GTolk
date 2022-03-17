using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;

using CoreDll;
using GTolk.Models;


namespace GTolk
{
    public partial class RegistroForm : Form
    {
        private Guid CódigoDeConfirmação { get; set; }

        public RegistroForm()
        {
            InitializeComponent();

            this.CódigoDeConfirmação = Guid.NewGuid();
            this.lblConfirmação.Text = string.Empty;

            this.groupChat.Enabled = false;
            this.groupEmail.Enabled = true;


            lblCapsLock.Text = "Caps Lock";
            lblCapsLockChat.Text = "Caps Lock";
            
            lblCapsLock.Visible = (Control.IsKeyLocked(Keys.CapsLock));
            lblCapsLockChat.Visible = (Control.IsKeyLocked(Keys.CapsLock));

            this.KeyDown += RegistroForm_KeyDown;

            this.KeyPreview = true;
        }

        void RegistroForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.CapsLock)
            {
                lblCapsLock.Visible = (Control.IsKeyLocked(Keys.CapsLock));
                lblCapsLockChat.Visible = (Control.IsKeyLocked(Keys.CapsLock));
            }
        }

        private void btnVerificar_Click(object sender, EventArgs e)
        {
            try
            {
                

                this.CódigoDeConfirmação = Guid.NewGuid();

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(txtEmailEmail.Text);
                mail.To.Add(txtEmailEmail.Text);
                mail.Subject = "Pedido de confirmação do Chat GTolk";
                mail.IsBodyHtml = true;
                mail.Body = "Favor, inserir o código <b>" + this.CódigoDeConfirmação.ToString() + "</b> no formulário de registro do Chat GTolk.";
                

                using (SmtpClient smtp = new SmtpClient(txtSmtp.Text ))
                {
                    int porta ;
                    if (!int.TryParse(txtPorta.Text, out porta))
                        throw new Exception("A porta especificada é inválida! Use apenas números.");

                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // modo de envio
                    smtp.UseDefaultCredentials = false; // vamos utilizar credencias especificas
                    smtp.EnableSsl = chkSsl.Checked; // GMail requer SSL
                    smtp.Port = porta;       // porta para SSL
                    smtp.Timeout = 10000;

                    // seu usuário e senha para autenticação
                    smtp.Credentials = new NetworkCredential(txtEmailEmail.Text, txtSenhaEmail.Text);

                    // envia o e-mail
                    smtp.Send(mail);
                }


                MessageBox.Show("O código de confirmação foi enviado para o seu email!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            try
            {
                //List<Contato> contatos = Contato.GetBy();

                // Verificando se contato já existe:
                Contato jáCadastrado = Contato.GetBy().Where(x => x.Email.Trim() == this.txtEmailEmail.Text.Trim()).FirstOrDefault();


                if (jáCadastrado == null)
                {
                    if (txtApelido.Text.Trim().Length < 4)
                    {
                        throw new Exception("O apelido precisa conter mais de 3 caracteres!");
                    }

                    if (txtSenhaChat.Text.Trim().Length < 6)
                    {
                        throw new Exception("A senha precisa conter 6 ou mais caracteres!");
                    }

                    if (txtSenhaChat.Text.Trim() != txtSenhaChatConfirmação.Text.Trim())
                    {
                        throw new Exception("O campo de senha não bate com a confirmação!");
                    }

                    Contato cadastro = new Contato();
                    cadastro.Guid = Guid.NewGuid();
                    cadastro.Apelido = txtApelido.Text.Trim();
                    cadastro.Senha = txtSenhaChat.Text.Trim();
                    cadastro.Email = txtEmailEmail.Text.Trim();
                    Contato.Save(cadastro);

                    MessageBox.Show("Cadastro realizado com sucesso!");
                    groupChat.Enabled = false;
                    this.Close();
                }
                else
                {
                    jáCadastrado.Apelido = txtApelido.Text.Trim();
                    jáCadastrado.Senha = txtSenhaChat.Text.Trim();
                    Contato.Save(jáCadastrado);

                    MessageBox.Show("Cadastro atualizado com sucesso!");
                    groupChat.Enabled = false;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtEmailEmail_Validated(object sender, EventArgs e)
        {
            string email = txtEmailEmail.Text.Trim().ToLower();
            
            if(email.Contains("@yahoo.com"))
            {
                txtSmtp.Text = "smtp.mail.yahoo.com";
                txtPorta.Text = "587";
                chkSsl.Checked = true;
            }
            else if (email.Contains("@gmail.com") || email.Contains("@gdatasistemas.com") || email.Contains("@gdata.com"))
            {
                txtSmtp.Text = "smtp.gmail.com";
                txtPorta.Text = "587";
                chkSsl.Checked = true;
            }
            else if (email.Contains("@hotmail.com") || email.Contains("@live.com") || email.Contains("@outlook.com"))
            {
                txtSmtp.Text = "smtp.live.com";
                txtPorta.Text = "587";
                chkSsl.Checked = true;
            }
            else
            {
                if (email.Contains("@"))
                {
                    int rightPartCount = email.Length - (email.IndexOf("@") + 1);
                    string rightPart = email.TakeRight(rightPartCount);

                    txtSmtp.Text = "smtp." + rightPart;
                    txtPorta.Text = "25";
                    chkSsl.Checked = false;
                }
                else
                {
                    txtSmtp.Text = "";
                    txtPorta.Text = "";
                    chkSsl.Checked = false;
                }
            }
        }

        private void txtCódigoEmail_TextChanged_1(object sender, EventArgs e)
        {
            if (txtCódigoEmail.Text.Trim() == string.Empty)
            {
                this.lblConfirmação.Text = "";
                this.lblConfirmação.Visible = false;

                this.groupChat.Enabled = false;
                this.groupEmail.Enabled = true;
            }
            else if (this.CódigoDeConfirmação.ToString().Trim() == this.txtCódigoEmail.Text.Trim())
            {
                this.lblConfirmação.Visible = true;
                this.lblConfirmação.Text = "Código válido!";
                this.lblConfirmação.ForeColor = Color.LawnGreen;

                this.groupChat.Enabled = true;
                this.groupEmail.Enabled = false;
                this.txtCódigoEmail.Enabled = false;
                this.LiberarCadastro();
            }
            else
            {
                this.lblConfirmação.Visible = true;
                this.lblConfirmação.Text = "Código inválido!";
                this.lblConfirmação.ForeColor = Color.DarkRed;

                this.groupChat.Enabled = false;
                this.groupEmail.Enabled = true;
            }
        }

        private void LiberarCadastro()
        {
            Contato jáCadastrado = Contato.GetBy().Where(x => x.Email.Trim() == this.txtEmailEmail.Text.Trim()).FirstOrDefault();

            if (jáCadastrado != null)
            {
                txtApelido.Text = jáCadastrado.Apelido.Trim();
                btnRegistrar.Text = "Atualizar";
            }
        }

        private void txtSenhaEmail_TextChanged(object sender, EventArgs e)
        {
            if (Control.IsKeyLocked(Keys.CapsLock))
            {
            }
        }

        private void txtEmailEmail_TextChanged(object sender, EventArgs e)
        {

        }
    }
}