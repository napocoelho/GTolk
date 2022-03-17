using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CoreDll;
using CoreDll.Bindables.Extensions.Forms;
using GTolk.Models;
using GTolk.Controllers;

namespace GTolk
{
    public partial class LoginForm : Form
    {
        public LoginController Controller { get; set; }

        public Contato UsuárioCredenciado { get; private set; }

        public LoginForm()
        {
            InitializeComponent();

            this.Controller = new LoginController();
            this.txtEmail.BindsOnTextChanged(this.Controller, x => x.Email);
            this.txtSenha.BindsOnTextChanged(this.Controller, x => x.Senha);

            string arquivoPerfil = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\gtolk_perfil.xml";

            try
            {

                Perfil.CarregarPerfil(arquivoPerfil);
                System.IO.File.Copy(arquivoPerfil, arquivoPerfil + ".bkp", true);
            }
            catch
            {
                try
                {
                    try
                    {
                        // Se entrar aqui é pq o arquivo [gtolk_perfil.xml] foi corrompido e deve ser excluído.
                        // No próximo passo, o sistema tentará restaurá-lo, mas caso não consiga, o excluirá e o recriará.
                        System.IO.File.Delete(arquivoPerfil);

                        if (System.IO.File.Exists(arquivoPerfil + ".bkp"))
                        {
                            System.IO.File.Copy(arquivoPerfil + ".bkp", arquivoPerfil, true);
                        }

                        Perfil.CarregarPerfil(arquivoPerfil);
                    }
                    catch
                    {
                        throw new Exception("O arquivo [" + arquivoPerfil + "] está corrompido! Tente excluí-lo e inicie o Tolk novamente!");
                    }

                    // Se entrar aqui é pq o arquivo [gtolk_perfil.xml] foi corrompido e deve ser excluído.
                    // No próximo passo, o sistema o recriará!
                    System.IO.File.Delete(arquivoPerfil);
                }
                catch (Exception ex)
                {
                    throw new Exception("O arquivo [" + arquivoPerfil + "] está corrompido! Tente excluí-lo e inicie o Tolk novamente!");
                }
            }

            try
            {
                Contato contato = Contato.GetBy().Where(x => x.Guid == Perfil.PERFIL.GuidÚltimoContatoLogado).FirstOrDefault();

                if (contato != null)
                {
                    this.Controller.Email = contato.Email;
                    this.txtSenha.Select();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            this.UsuárioCredenciado = null;

            this.KeyDown += LoginForm_KeyDown;
            this.KeyPreview = true;
            this.Visible = false;
        }



        private void Entrar()
        {
            try
            {
                this.Visible = false;
                Contato contato = this.Controller.Entrar();

                Perfil.PERFIL.RegistrarUsuário(contato);
                Perfil.SalvarPerfil();

                this.UsuárioCredenciado = contato;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            this.Entrar();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.CapsLock)
            {

            }


            if (e.KeyCode == Keys.Enter)
            {
                this.Entrar();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void lnkRegistrar_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegistroForm frm = new RegistroForm();
            frm.ShowDialog(this);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            Contato contato = Contato.GetBy().Where(x => x.Guid == Perfil.PERFIL.GuidÚltimoContatoLogado).FirstOrDefault();

            if (contato != null && this.Controller.PermitirAutoLogon(contato))
            {
                this.UsuárioCredenciado = contato;
                this.Close();
                return;
            }

            this.Visible = true;
        }
    }
}