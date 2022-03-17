using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

using GTolk.Util;
using GTolk.Models;
using GTolk.Controllers;

using CoreDll;
using CoreDll.Bindables;
using CoreDll.Bindables.Extensions;
using CoreDll.Bindables.Extensions.Forms;

namespace GTolk
{
    public partial class OpçõesForm : Form
    {
        //private OpçõesController Controller { get; set; }
        //private Contato Usuário { get; set; }
        private Contato UsuárioOriginal { get; set; }

        private Image Imagem { get; set; }

        public OpçõesForm(Contato usuárioLogado)
        {
            InitializeComponent();


            this.UsuárioOriginal = usuárioLogado;

            this.Imagem = this.UsuárioOriginal.Imagem;
            this.txtApelido.Text = this.UsuárioOriginal.Apelido;
            this.txtDescrição.Text = this.UsuárioOriginal.Descrição;
            this.chkLogarAutomaticamente.Checked = this.UsuárioOriginal.AutoLogon;
            this.chkIniciarComWindows.Checked = this.GetIniciarComWindows();

            this.SetImage(this.UsuárioOriginal.Imagem);


            this.KeyDown += OpçõesForm_KeyDown;
            /*
            this.txtApelido.BindsOnTextChanged(this.Controller, x => x.Apelido);
            this.txtDescrição.BindsOnTextChanged(this.Controller, x => x.Descrição);

            this.Controller.BindsToNonSource<OpçõesController, PictureBox, Image>(src => src.Imagem50, this.pictureBox50, ctrl => ctrl.Image);
            this.Controller.BindsToNonSource<OpçõesController, PictureBox, Image>(src => src.Imagem75, this.pictureBox75, ctrl => ctrl.Image);
            this.Controller.BindsToNonSource<OpçõesController, PictureBox, Image>(src => src.Imagem100, this.pictureBox100, ctrl => ctrl.Image);

            this.Controller.AtualizarImagens();
            */

            this.btnIniciarManutenção.Visible = usuárioLogado.IsAdmin;
            this.btnFinalizarManutenção.Visible = usuárioLogado.IsAdmin;
        }

        void OpçõesForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void lnkAlterarImagem_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ImagemForm form = new ImagemForm();
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ShowDialog(this);

            if (ImagemForm.ImagemEditada != null)
            {
                this.SetImage(ImagemForm.ImagemEditada);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                this.UsuárioOriginal.Apelido = txtApelido.Text.Trim().TakeLeft(20);
                this.UsuárioOriginal.Descrição = txtDescrição.Text.Trim().TakeLeft(40);
                this.UsuárioOriginal.Imagem = this.Imagem;
                this.UsuárioOriginal.AutoLogon = chkLogarAutomaticamente.Checked;

                this.SetIniciarComWindows(this.chkIniciarComWindows.Checked);



                Contato.Save(this.UsuárioOriginal);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SetImage(Image imagem)
        {
            this.Imagem = imagem;
            this.pictureBox50.Image = Imagens.ScaleImage(imagem, 50, 50);
            this.pictureBox75.Image = Imagens.ScaleImage(imagem, 75, 75);
            this.pictureBox100.Image = Imagens.ScaleImage(imagem, 100, 100);
        }

        private void btnIniciarManutenção_Click(object sender, EventArgs e)
        {
            try
            {
                Manutenção value = Manutenção.CreateNew();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFinalizarManutenção_Click(object sender, EventArgs e)
        {
            try
            {
                Manutenção.DeleteAll();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        public bool GetIniciarComWindows()
        {
            string fileName = Path.GetFileName(Application.ExecutablePath);

            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false);
            bool isMarked = (registryKey.GetValue(fileName) != null);

            return isMarked;
        }

        /// <summary>
        /// Iniciar ao logar no Windows.
        /// </summary>
        public void SetIniciarComWindows(bool value)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            string fileName = Path.GetFileName(Application.ExecutablePath);
            string pathName = Application.ExecutablePath + " \\sleep";

            if (value)
            {
                registryKey.SetValue(fileName, pathName);
            }
            else
            {
                if (registryKey.GetValue(fileName) != null)
                {
                    registryKey.DeleteValue(fileName);
                }
            }
        }


    }
}