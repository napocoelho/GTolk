using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CoreDll.Bindables;
using CoreDll.Bindables.Extensions;
using CoreDll.Bindables.Extensions.Forms;

using GTolk.Util;
using GTolk.Controllers;
using GTolk.Models;



namespace GTolk
{
    public partial class ContatosForm : Form
    {
        private object LOCK = new object();

        public ContatosController Controller { get; set; }
        private ConversasForm Chat { get; set; }

        private ContextMenu TrayMenu { get; set; }
        private NotifyIcon TrayIcon { get; set; }
        private Size LastSize { get; set; }


        private bool? _esconder;
        public bool Esconder
        {
            get
            {
                if (!this._esconder.HasValue)
                {
                    this.Esconder = false;
                }

                return this._esconder.Value;
            }
            set
            {
                if (!this._esconder.HasValue || this._esconder.Value != value)
                {
                    this._esconder = value;

                    if (!this.Esconder)
                    {
                        this.WindowState = FormWindowState.Normal;
                        Perfil.PERFIL.SetContatosForm(this);
                        this.Show();
                    }
                    else
                    {
                        Perfil.PERFIL.RegistrarContatos(this);
                        Perfil.SalvarPerfil();
                        this.WindowState = FormWindowState.Minimized;
                    }
                }
            }
        }

        public ContatosForm(Contato usuárioLogado)
        {
            InitializeComponent();

            PaletaDeCores.FundoDeUsuário = Color.FromArgb(255, 112, 128, 144); //SlateGray
            PaletaDeCores.FonteDeUsuário = Color.FromArgb(255, 245, 245, 245); //WhiteSmoke

            PaletaDeCores.FundoDeContato = Color.FromArgb(255, 215, 228, 242);  //GradientInactiveCaption
            PaletaDeCores.FonteDeContato = Color.FromArgb(255, 0, 0, 0);        //Black

            PaletaDeCores.FundoDaListaDeContatosPares = Color.FromArgb(255, 255, 255, 255);     //White
            PaletaDeCores.FundoDaListaDeContatosImpares = Color.FromArgb(255, 245, 245, 245);   //WhiteSmoke
            PaletaDeCores.FundoDeContatoSelecionado = Color.FromArgb(255, 215, 228, 242);   //GradientInactiveCaption

            PaletaDeCores.FundoDeStatus = Color.FromArgb(255, 112, 128, 144);   //SlateGray
            PaletaDeCores.FonteDeStatus = Color.FromArgb(255, 245, 245, 245);   //WhiteSmoke

            PaletaDeCores.StatusOnline = Color.FromArgb(255, 0, 128, 0);   //Green
            PaletaDeCores.StatusOffline = Color.FromArgb(255, 210, 210, 210);   //Gray
            PaletaDeCores.StatusAusente = Color.FromArgb(255, 255, 215, 0);   //Gold
            PaletaDeCores.StatusOcupado = Color.FromArgb(255, 255, 0, 0);   //Red



            Control.CheckForIllegalCrossThreadCalls = false;
            this.Chat = null;

            this.Controller = new ContatosController(usuárioLogado);

            this.Controller.Usuário.PropertyChanged += (object sender, PropertyChangedEventArgs e) =>
                {
                    this.AtualizarInformaçõesDeUsuário();
                };

            this.BackColor = PaletaDeCores.FundoDeUsuário;

            /*
            this.toolStripMenu.Visible = false;
            this.toolStripMenu.Height = 0;
            this.toolStripMenu.Dock = DockStyle.None;
            this.toolStripMenu.Left = 0;
            this.toolStripMenu.GripStyle = ToolStripGripStyle.Hidden;
            this.toolStripMenu.Font = Fontes.Fonte(9F);
            this.toolStripMenu.Margin = new Padding(0);
            this.toolStripMenu.Padding = new Padding(0);
            */

            this.painelDoUsuário.Dock = DockStyle.Top;
            this.painelDoUsuário.BorderStyle = BorderStyle.None;
            this.painelDoUsuário.Top = 0; // toolStripMenu.Bottom;
            this.painelDoUsuário.BackColor = PaletaDeCores.FundoDeUsuário;
            this.painelDoUsuário.Margin = new Padding(0);
            this.painelDoUsuário.Padding = new Padding(0);
            this.painelDoUsuário.Left = 0;
            this.painelDoUsuário.Height = 60;
            this.pictureBoxUsuário.DoubleClick += (object sender, EventArgs e) =>
                {
                    OpçõesForm form = new OpçõesForm(this.Controller.Usuário);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.ShowDialog(this);
                };
            this.painelDoUsuário.MouseClick += (object sender, MouseEventArgs e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    contextMenuStrip1.Show(this.painelDoUsuário, e.Location);
                }
            };

            this.lblNomeUsuário.Left = this.pictureBoxUsuário.Right + 5;
            this.lblNomeUsuário.Font = Fontes.Fonte(12F);
            this.lblNomeUsuário.ForeColor = PaletaDeCores.FonteDeUsuário;

            this.lblDescriçãoUsuário.Left = this.pictureBoxUsuário.Right + 5;
            this.lblDescriçãoUsuário.Font = Fontes.Fonte(8F);
            this.lblDescriçãoUsuário.ForeColor = PaletaDeCores.FonteDeUsuário;
            this.lblDescriçãoUsuário.AutoSize = false;



            this.panelUsuárioStatus.Margin = new Padding(0);
            this.panelUsuárioStatus.Padding = new Padding(0);
            this.panelUsuárioStatus.BringToFront();
            this.panelUsuárioStatus.Top = 0;
            this.panelUsuárioStatus.Width = 5;

            this.ContactControl.AllowDrop = true;
            this.ContactControl.DragDrop += ContactControl_DragDrop;
            this.ContactControl.DragOver += ContactControl_DragOver;

            this.ContactControl.DataSource = this.Controller.Contatos;
            this.ContactControl.SelectionChanged += ContactControl_SelectionChanged;
            this.ContactControl.DoubleClick += ContactControl_DoubleClick;
            this.ContactControl.AutoScroll = false;
            this.ContactControl.VerticalScroll.Enabled = false;
            this.ContactControl.HorizontalScroll.Enabled = false;
            this.ContactControl.BorderStyle = BorderStyle.None;
            this.ContactControl.BackColor = PaletaDeCores.FundoDaListaDeContatosPares;
            this.ContactControl.AlternatedRowBackColor = PaletaDeCores.FundoDaListaDeContatosImpares;
            this.ContactControl.SelectionBackColor = PaletaDeCores.FundoDeContatoSelecionado;
            this.ContactControl.Margin = new Padding(0);
            this.ContactControl.Padding = new Padding(0);
            this.ContactControl.Left = 0;
            this.ContactControl.AutoScroll = false;

            // Configurando o scroll da lista de contatos:
            this.ContactControl.Table.AutoScroll = false;
            this.ContactControl.Table.HorizontalScroll.Maximum = 0;
            this.ContactControl.Table.HorizontalScroll.Visible = false;
            this.ContactControl.Table.HorizontalScroll.Enabled = true;
            this.ContactControl.Table.VerticalScroll.Maximum = 0;
            this.ContactControl.Table.VerticalScroll.Visible = false;
            this.ContactControl.Table.VerticalScroll.Enabled = true;
            this.ContactControl.Table.AutoScroll = true;

            this.panelStatus.Margin = new Padding(0);
            this.panelStatus.Padding = new Padding(0);
            this.panelStatus.Height = 30;
            this.panelStatus.BackColor = PaletaDeCores.FundoDeStatus;
            this.panelStatus.ForeColor = PaletaDeCores.FonteDeStatus;

            this.VerticalScroll.Enabled = false;
            this.HorizontalScroll.Enabled = false;

            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //this.ControlBox = false;

            /*
            Action<object, EventArgs> redimensionou = (object sender, EventArgs e) =>
            {
                this.Redimensionar();
            };
            */

            this.SizeChanged += (object sender, EventArgs e) =>
                {
                    if (this.WindowState == FormWindowState.Minimized)
                    {

                    }
                    else if (this.WindowState == FormWindowState.Maximized)
                    {
                        this.Redimensionar();
                    }
                    else
                    {
                        this.Esconder = false;
                        this.Redimensionar();
                    }
                };

            this.Resize += (object sender, EventArgs e) =>
                {
                    //this.Redimensionar();
                };

            this.Controller.AvisoDeSaídaDoSistema += Controller_AvisoDeSaídaDoSistema;
            this.Controller.ConversasController.AvisoRecebeuNovaMensagem += ConversasController_AvisoRecebeuNovaMensagem;
            

            this.InicializarTrayMode();


            this.DoubleBuffered = true;
            this.KeyPreview = true;

            this.GotFocus += ContatosForm_GotFocus;

            Microsoft.Win32.SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

            this.AtualizarInformaçõesDeUsuário();

            this.painelDoUsuário.BringToFront();

            this.LastSize = this.Size;
            this.Redimensionar(true);
            this.LastSize = this.Size;


            
        }

        void ConversasController_AvisoRecebeuNovaMensagem(object sender, Controllers.Eventos.NovaMensagemEventArgs e)
        {
            lock (LOCK)
            {
                if (!ConversasForm.IsOpen)
                {
                    ConversasForm.AbrirJanelaDeContatos(this.Controller, e.Contato, true);
                    ConversasForm.ExibirMensagem(e);
                }
            }
        }

        

        void ContactControl_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        void ContactControl_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (RowPanel row in ContactControl.Rows)
            {
                if (row.IsMouseOver)
                {
                    this.Controller.ContatoSelecionado = row.Value;
                    //ConversasForm.AbrirJanelaDeContatos(this.Controller, this.Controller.ContatoSelecionado, false);
                    ConversasForm.EnviarArquivoParaContato(this.Controller, this.Controller.ContatoSelecionado, files, false);
                }
            }
            
            //this.EnviarArquivo(files);
        }

        void Controller_AvisoDeSaídaDoSistema(object sender, EventArgs e)
        {
            try
            {
                string thisExePathName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                string creationTimeUtc = System.IO.File.GetCreationTimeUtc(thisExePathName).ToOADate().ToString();
                string strCmdText = string.Format("'{0}' '{1}'", thisExePathName, creationTimeUtc);

                System.Diagnostics.ProcessStartInfo proc = new System.Diagnostics.ProcessStartInfo("Manutenção_GTolk.exe", strCmdText);
                //proc.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                proc.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                System.Diagnostics.Process.Start(proc);
            }
            catch (Exception ex)
            {
                string err = ex.Message.ToString();
            }


            this.FecharAplicativo();
        }

        void SystemEvents_PowerModeChanged(object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
        {
            if (e.Mode == Microsoft.Win32.PowerModes.Resume)
            {
                Contato.SetStatus(this.Controller.Usuário, StatusDoContato.Online);
            }
            else if (e.Mode == Microsoft.Win32.PowerModes.StatusChange)
            {
                Contato.SetStatus(this.Controller.Usuário, StatusDoContato.Ausente);
            }
            else if (e.Mode == Microsoft.Win32.PowerModes.Suspend)
            {
                Contato.SetStatus(this.Controller.Usuário, StatusDoContato.Ausente);
            }
        }






        /*
        /// <summary>
        /// Para otimizar o doublebuffered dos formulários;
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;    // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
        */

        private void AtualizarInformaçõesDeUsuário()
        {
            this.pictureBoxUsuário.Image = Imagens.ScaleImage(this.Controller.Usuário.Imagem, 55, 55); // Imagens.CropToCircle(Imagens.ScaleImage(this.Controller.Usuário.Imagem, 55, 55), 55, true);
            this.lblNomeUsuário.Text = this.Controller.Usuário.Apelido;
            this.lblDescriçãoUsuário.Text = this.Controller.Usuário.Descrição;
            this.panelUsuárioStatus.BackColor = this.Controller.Usuário.Status.ToColor();
        }

        void ContatosForm_GotFocus(object sender, EventArgs e)
        {
            this.ContactControl.Focus();
        }


        void ContactControl_DoubleClick(object sender, EventArgs e)
        {
            if (this.Controller.IsContatoSelecionado())
            {
                //this.Chat = ConversasForm.NotificarChamadas(this.Controller, this.Controller.ContatoSelecionado);
                //this.Chat = ConversasForm.EnviarMensagemParaContato(this.Controller, this.Controller.ContatoSelecionado, null);
                this.Chat = ConversasForm.AbrirJanelaDeContatos(this.Controller, this.Controller.ContatoSelecionado, false);
            }
        }

        private void ContactControl_SelectionChanged(object sender, EventArgs e)
        {
            lock (LOCK)
            {
                if (this.ContactControl == null || this.Controller == null)
                    return;

                if (this.ContactControl.IsSelected)
                {
                    Contato contatoSelecionado = this.ContactControl.SelectedValue;
                    this.Controller.ContatoSelecionado = contatoSelecionado;
                }
                else
                {
                    this.Controller.ContatoSelecionado = null;
                }
            }
        }

        ~ContatosForm()
        {
            this.TrayIcon.Visible = false;
            this.TrayIcon.Dispose();
        }

        private void ContatosForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.Visible == true)
                {
                    Perfil.PERFIL.RegistrarContatos(this);
                    Perfil.SalvarPerfil();
                }

                if (e.CloseReason == CloseReason.UserClosing)
                {
                    e.Cancel = true;
                    this.Esconder = true;
                }
                else
                {
                    try
                    {
                        this.Chat.Close();
                    }
                    catch { }

                    this.FecharAplicativo();
                }
            }
            catch
            {
            }
        }

        private void ContatosForm_Load(object sender, EventArgs e)
        {
            Perfil.PERFIL.SetContatosForm(this);
        }

        private void InicializarTrayMode()
        {
            lock (LOCK)
            {
                this.Esconder = false;

                this.TrayMenu = new ContextMenu();

                //TrayMenu.MenuItems.Add("-");
                this.TrayMenu.MenuItems.Add("Exibir/Esconder",
                                        (sender, e) =>
                                        {
                                            this.Esconder = !this.Esconder;
                                        });

                this.TrayMenu.MenuItems.Add("-");

                this.TrayMenu.MenuItems.Add("Sair",
                                        (sender, e) =>
                                        {
                                            this.TrayIcon.Visible = true;
                                            this.TrayIcon.Dispose();
                                            this.FecharAplicativo();
                                        });


                // Ícone do SystemTray
                this.TrayIcon = new NotifyIcon();
                this.TrayIcon.Text = "GTolk";
                this.TrayIcon.Icon = new Icon(SystemIcons.Application, 256, 256);
                this.TrayIcon.ContextMenu = this.TrayMenu;
                this.TrayIcon.Icon = this.Icon;
                this.TrayIcon.Visible = true;
                this.TrayIcon.MouseClick += (object sender, MouseEventArgs e) =>
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.Esconder = !this.Esconder;
                    }
                };
            }
        }

        private void ContatosForm_KeyDown(object sender, KeyEventArgs e)
        {
            lock (LOCK)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.Esconder = true;
                }
            }
        }

        private void FecharAplicativo()
        {
            lock (LOCK)
            {
                try
                {
                    Contato.SetStatus(this.Controller.Usuário, StatusDoContato.Offline);
                }
                catch
                {
                    try
                    {
                        Contato.SetStatus(this.Controller.Usuário, StatusDoContato.Offline);
                    }
                    catch { }
                }

                this.Close();
                Application.Exit();
            }
        }

        private void ocupadoToolStripMenuItemStatusOcupadoDesocupado_Click(object sender, EventArgs e)
        {
            if (this.Controller.Usuário.Status == StatusDoContato.Ocupado)
            {
                ToolStripMenuItemStatusOcupadoDesocupado.Text = "Ocupado";
                Contato.SetStatus(this.Controller.Usuário, StatusDoContato.Online);
            }
            else
            {
                ToolStripMenuItemStatusOcupadoDesocupado.Text = "Disponível";
                Contato.SetStatus(this.Controller.Usuário, StatusDoContato.Ocupado);
            }

        }

        private void ToolStripMenuItemSair_Click(object sender, EventArgs e)
        {
            this.FecharAplicativo();
        }

        private void ToolStripMenuItemDeslogar_Click(object sender, EventArgs e)
        {
            this.Controller.Usuário.AutoLogon = false;
            Contato.Save(this.Controller.Usuário);

            Perfil.SalvarPerfil();
            this.FecharAplicativo();

            System.Diagnostics.Process.Start(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName).StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
        }

        public void Redimensionar(bool forçarAção = false)
        {
            if (!forçarAção && this.Size == this.LastSize)
            {
                //return;
            }

            this.ResizeRedraw = false;

            this.panelUsuárioStatus.Height = this.painelDoUsuário.Height;
            this.panelUsuárioStatus.Left = this.painelDoUsuário.Right - 5; //this.painelDoUsuário.Width - 25;

            this.ContactControl.Top = this.painelDoUsuário.Bottom;
            this.ContactControl.Width = painelDoUsuário.Width;
            this.ContactControl.Height = this.ContactControl.Height + (this.panelStatus.Top - this.ContactControl.Bottom - 1);

            this.ResizeRedraw = true;

            this.LastSize = this.Size;
        }
    }
}