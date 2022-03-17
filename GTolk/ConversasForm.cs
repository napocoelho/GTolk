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

using CoreDll;
using CoreDll.Bindables;
using CoreDll.Bindables.Extensions;
using GTolk.Util;
using GTolk.Controllers;
using GTolk.Models;




namespace GTolk
{
    public partial class ConversasForm : Form
    {
        private static object LOCK = new object();
        private static ConversasForm _INSTANCE_ = null;
        public static bool IsOpen { get; private set; }
        //private static ContatosForm _OWNER_ = null;
        public ConversasController Controller { get; set; }
        public bool IsActivated { get; private set; }



        private bool showWithoutActivation = false;
        protected override bool ShowWithoutActivation { get { return this.showWithoutActivation; } }


        //public Crownwood.Magic.Controls.TabPage LastSelectedTabPage { get; set; }


        private ConversasForm(ContatosController contatosController) //, Contato contatoSelecionado)
        {
            this.IsActivated = false;
            this.SetShowWithoutActivation(false);

            //this.Owner = owner;
            InitializeComponent();

            //this.tabControlNew.TabPages.Clear();

            ConversasForm.IsOpen = true;

            //this.Controller = ChatController.GetInstance(usuárioLogado);
            this.Controller = contatosController.ConversasController; // new ConversasController(contatosController);
            
            
            this.Controller.Conversas.ListChanged += Conversas_ListChanged;
            this.Controller.ConversaSelecionadaChanged += Controller_ConversaSelecionadaChanged;
            this.Controller.AvisoRecebeuNovaMensagem += Controller_AvisoRecebeuNovaMensagem;
            //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            // Databindings;
            //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            CoreDll.Bindables.Extensions.DataBindingExtensions.BindsToNonSource<ConversasController, ConversasForm, string>(this.Controller, src => src.TítuloDoChat, this, ctrl => ctrl.Text);

            //this.KeyDown += ConversasForm_KeyDown;

            this.KeyPreview = true;

            //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            this.Font = Fontes.Fonte(8F);

            //this.GotFocus += ConversasForm_GotFocus;
        }

        ~ConversasForm()
        {
            try
            {
                this.Controller.Conversas.ListChanged -= Conversas_ListChanged;
                this.Controller.ConversaSelecionadaChanged -= Controller_ConversaSelecionadaChanged;
                this.Controller.AvisoRecebeuNovaMensagem -= Controller_AvisoRecebeuNovaMensagem;
                this.Controller.Conversas.Clear();
            }
            catch { }
        }

        public void SetShowWithoutActivation(bool value)
        {
            this.showWithoutActivation = value;
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

        public void SelecionarConversa()
        {
            // Verifica se o TabPage já existe;
            foreach (Crownwood.Magic.Controls.TabPage tab in this.tabControlNew.TabPages.ToList<Crownwood.Magic.Controls.TabPage>())
            {
                if (tab.Tag == null)
                {
                    this.tabControlNew.TabPages.Remove(tab);
                    continue;
                }

                Conversa conversaPesquisada = tab.Tag as Conversa;

                // Evita que seja criado 2 TabPages para a mesma conversa:
                if (conversaPesquisada != null && conversaPesquisada.Equals(this.Controller.ConversaSelecionada))
                {
                    this.tabControlNew.SelectedTab = tab;
                    this.tabControlNew.Focus();
                    tab.Focus();
                    PageChat chat = tab.Controls["PageChat"] as PageChat;
                    chat.TextBox.Focus();
                    //chat.TextBox.Select();
                    break;
                }
            }
        }

        /// <summary>
        /// Verifica se existe um TabPage para a Conversa especificada. Se o TabPage correspondente não existir, cria um.
        /// </summary>
        /// <returns>Retorna o TabPage correspondente</returns>
        private Crownwood.Magic.Controls.TabPage CriarConversa(Conversa conversa)
        {
            lock (LOCK)
            {

                string htmlInicial = null;

                if (conversa == null)
                    return null;

                // Abrindo arquivo .html;
                if (System.IO.File.Exists(@"base.html"))
                {
                    using (System.IO.StreamReader reader = System.IO.File.OpenText(@"base.html"))
                    {
                        htmlInicial = reader.ReadToEnd();
                    }
                }
                else
                {
                    throw new Exception("O arquivo 'base.html' não foi encontrado!");
                }

                this.AllowDrop = true;
                this.DragOver += event_DragOver;
                this.DragDrop += event_DragDrop;

                this.tabControlNew.Font = Fontes.Fonte(9F);

                // Se não existir o TabPage, cria um;
                Crownwood.Magic.Controls.TabPage page = new Crownwood.Magic.Controls.TabPage();
                page.Font = Fontes.Fonte(9F);
                page.Tag = conversa;

                //page.Text = "[" + conversa.ConversandoCom.Email + "]";
                //page.Title = "[" + conversa.ConversandoCom.Email + "]";


                PageChat chat = new PageChat(this.tabControlNew, page, conversa);
                chat.Dock = DockStyle.Fill;
                chat.Name = "PageChat";

                WebBrowser browser = chat.WebBrowser;
                browser.Name = "WebBrowser";
                browser.TabStop = false;
                browser.Dock = DockStyle.Fill;
                browser.AllowNavigation = false;
                browser.AllowWebBrowserDrop = false;
                browser.WebBrowserShortcutsEnabled = false;
                browser.IsWebBrowserContextMenuEnabled = false;
                browser.ContextMenuStrip = popUpMenuBrowser;

                JavaScriptInterfaces javaScriptInterface = new JavaScriptInterfaces(chat.Conversa);
                javaScriptInterface.SaveAsAction += javaScriptInterface_SaveAsAction;
                javaScriptInterface.OpenAction += javaScriptInterface_OpenAction;
                browser.ObjectForScripting = javaScriptInterface;

                browser.PreviewKeyDown += browser_PreviewKeyDown;

                //-------------------
                SHDocVw.IWebBrowser2 browserCom = browser.ActiveXInstance as SHDocVw.IWebBrowser2;

                if (browserCom != null)
                {
                    mshtml.HTMLDocument doc = browserCom.Document as mshtml.HTMLDocument;

                    if (doc != null)
                    {
                        mshtml.HTMLDocumentEvents2_Event iEvent = doc as mshtml.HTMLDocumentEvents2_Event;

                        if (iEvent != null)
                        {
                            iEvent.onkeydown += iEvent_onkeydown;

                        }
                    }
                }
                //-------------------




                //panelTop.Controls.Add(browser);


                // Snipet para quando for abrir o html pelo código:
                browser.Navigate("about:blank");
                browser.Document.OpenNew(false);
                browser.Document.Write(htmlInicial);
                browser.Refresh();

                //browser.Document.Window.Scroll += Window_Scroll;

                TextBox txtBox = chat.TextBox;
                txtBox.Name = "TextBox";
                txtBox.TabStop = true;
                txtBox.Multiline = true;
                txtBox.Font = Fontes.Fonte(10F);
                txtBox.AllowDrop = true;
                txtBox.DragOver += event_DragOver;
                txtBox.DragDrop += event_DragDrop;
                txtBox.ContextMenuStrip = popUpMenuText;

                //txtBox.Focus();


                //txtBox.Dock = DockStyle.Bottom;
                //txtBox.Height = txtBox.Height * 2;
                //txtBox.AllowDrop = false;
                //panelBottom.Controls.Add(txtBox);


                page.Controls.Add(chat);
                this.tabControlNew.TabPages.Add(page);

                //this.tabControlNew.Focus();
                //page.Focus();

                Application.DoEvents();


                this.HtmlIniciarChat(browser, conversa);



                Conversa conversaHistórica = FiltrarHistórico(chat, DateTime.Today);
                this.HtmlExibirHistórico(browser, conversaHistórica);

                return page;
            }
        }

        void javaScriptInterface_OpenAction(FileTransferEventArgs args)
        {
            try
            {
                string filePath = args.GuidRemetente + "\\" + args.FileName;


                if (File.Exists(filePath))
                {
                    System.Diagnostics.Process.Start(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void javaScriptInterface_SaveAsAction(FileTransferEventArgs args)
        {
            try
            {
                string filePath = args.GuidRemetente + "\\" + args.FileName;


                if (File.Exists(filePath))
                {
                    saveFileDialog1.Title = "Salvar arquivo como...";
                    saveFileDialog1.DefaultExt = "";
                    saveFileDialog1.CheckFileExists = false;
                    saveFileDialog1.CheckPathExists = true;
                    //saveFileDialog1.Filter = "*.*";
                    saveFileDialog1.FileName = args.FileName;

                    if (saveFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (File.Exists(filePath))
                        {
                            File.Copy(filePath, saveFileDialog1.FileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        void browser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C)
            {
                Crownwood.Magic.Controls.TabPage tabPage = this.tabControlNew.SelectedTab;
                PageChat chat = tabPage.Controls["PageChat"] as PageChat;
                this.CopiarParaClipboardTextoSelecionadoDeWebBrowser(chat.WebBrowser);
            }
        }

        void iEvent_onkeydown(mshtml.IHTMLEventObj pEvtObj)
        {
        }

        void event_DragOver(object sender, DragEventArgs e)
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

        void event_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            this.EnviarArquivo(files);
        }


        public void EnviarArquivo(string[] files)
        {
            lock (LOCK)
            {
                if (files != null && files.Length > 0)
                {
                    string userDirName = this.Controller.Usuário.Guid.ToString();

                    if (!Directory.Exists(userDirName))
                    {
                        Directory.CreateDirectory(userDirName);
                    }

                    foreach (string file in files)
                    {
                        if (File.Exists(file))
                        {
                            string newFilePath = Application.StartupPath + "\\" + this.Controller.Usuário.Guid.ToString() + "\\" + Path.GetFileName(file);
                            int fileDuplicatedNumber = 0;

                            while (File.Exists(newFilePath))
                            {
                                fileDuplicatedNumber++;
                                newFilePath = Application.StartupPath + "\\" + this.Controller.Usuário.Guid.ToString() + "\\" + Path.GetFileNameWithoutExtension(file) + " " + fileDuplicatedNumber + Path.GetExtension(file);
                            }


                            //string dirExts = Application.StartupPath + "\\" + "exts";
                            string dirExts = "exts";
                            string iconExtName = Path.GetExtension(file).Replace(".", "") + ".png";
                            string iconPathName = "";
                            bool isImageFile = false;
                            bool isVideoFile = false;

                            File.Copy(file, newFilePath, true);
                            PageChat chat = this.tabControlNew.SelectedTab.Controls["PageChat"] as PageChat;



                            if (Imagens.IsRecognisedImageFile(newFilePath)) // se trata de um arquivo imagem (.jpg, .bmp, .gif, etc...);
                            {
                                isImageFile = true;
                                iconPathName = newFilePath;
                            }
                            else if (Videos.IsVideoFile(newFilePath))
                            {
                                string thumbNailFilePath = Application.StartupPath + "\\" + this.Controller.Usuário.Guid.ToString() + "\\" + Path.GetFileNameWithoutExtension(newFilePath) + ".jpg";
                                (new NReco.VideoConverter.FFMpegConverter()).GetVideoThumbnail(newFilePath, thumbNailFilePath, 1);
                                iconPathName = thumbNailFilePath;
                                isVideoFile = true;
                            }
                            else if (!File.Exists(dirExts + "\\" + iconExtName))    // não se trata de um arquivo imagem;
                            {
                                isImageFile = false;

                                try
                                {
                                    if (!Directory.Exists(dirExts))
                                    {
                                        Directory.CreateDirectory(dirExts);
                                    }

                                    Image imagem = ExtractLargeIconFromFile.ShellEx.GetBitmapFromFilePath(newFilePath, ExtractLargeIconFromFile.ShellEx.IconSizeEnum.LargeIcon48);

                                    if (imagem != null)
                                    {
                                        if (File.Exists(dirExts + "\\" + iconExtName))
                                        {
                                            File.Delete(dirExts + "\\" + iconExtName);
                                        }

                                        imagem.Save(dirExts + "\\" + iconExtName, System.Drawing.Imaging.ImageFormat.Png);
                                    }
                                    else
                                    {
                                        iconExtName = "no_icon.png";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    iconExtName = "no_icon.png";
                                }

                                iconPathName = Application.StartupPath + "\\" + dirExts + "\\" + iconExtName;
                            }
                            else
                            {
                                isImageFile = false;
                                iconPathName = Application.StartupPath + "\\" + dirExts + "\\" + iconExtName;
                            }



                            string imgWidth = " width =\"80\" ";
                            imgWidth = (isImageFile ? " width =\"90%\" " : imgWidth);
                            imgWidth = (isVideoFile ? " width =\"150\" " : imgWidth);

                            string msg = "<img src=\"{0}\" " + imgWidth + " /><br />" +
                                         "{2}<br />" +
                                         "<a href=\"{1}\" download=\"{2}\" OnClick=\"window.external.SaveAs('{3}', '{2}')\">Salvar como</a> " +
                                         " | " +
                                         "<a href=\"{1}\" download=\"{2}\" OnClick=\"window.external.Open('{3}', '{2}')\">Abrir</a><br /><br />";

                            msg = msg.FormatTo(iconPathName,
                                               newFilePath,
                                               Path.GetFileName(newFilePath),
                                               this.Controller.Usuário.Guid.ToString());

                            this.EnviarMensagem(chat.Conversa, msg);
                        }
                    }
                }
            }
        }

        private void Controller_AvisoRecebeuNovaMensagem(object sender, Controllers.Eventos.NovaMensagemEventArgs e)
        {
            this.ExibirMensagem(e.Contato, e.Conversa, e.Mensagem);
        }

        public static void ExibirMensagem(Controllers.Eventos.NovaMensagemEventArgs e)
        {
            ConversasForm form = ConversasForm._INSTANCE_;

            if (form != null)
            {
                form.ExibirMensagem(e.Contato, e.Conversa, e.Mensagem);
            }
        }

        private void ExibirMensagem(Contato contato, Conversa conversa, Mensagem mensagem)
        {
            lock (LOCK)
            {
                Crownwood.Magic.Controls.TabPage tabPage = this.tabControlNew.TabPages.ToList<Crownwood.Magic.Controls.TabPage>().Where(item => item.Tag.Equals(conversa)).FirstOrDefault();

                
                if (tabPage != null)
                {
                    //Conversa conversa = e.Conversa;
                    //Mensagem mensagem = e.Mensagem;
                    //bool isPrimeiraMensagem = (e.Conversa.ÚltimaMensagemExibida == null);

                    PageChat chat = tabPage.Controls["PageChat"] as PageChat;
                    WebBrowser browser = chat.WebBrowser;
                    //browser.DocumentText += e.Mensagem.Texto + "<br />";

                    HtmlElement boxMensagens = null;
                    HtmlElement newElementInicial = null;
                    HtmlElement newElement = null;
                    HtmlElement newElementTitulo = null;



                    bool agruparPorDia = (conversa.ÚltimaMensagemExibida == null || conversa.ÚltimaMensagemExibida.EnviadaEm.Value.ToShortDateString() != mensagem.EnviadaEm.Value.ToShortDateString());

                    bool reagrupar = (conversa.ÚltimaMensagemExibida == null
                                        || conversa.ÚltimaMensagemExibida.GuidRemetente != mensagem.GuidRemetente
                                        || conversa.ÚltimaMensagemExibida.EnviadaEm.Value.ToShortTimeString() != mensagem.EnviadaEm.Value.ToShortTimeString()
                                        || agruparPorDia);

                    string cssId = mensagem.Guid.ToString();
                    string cssClass = "";

                    if (conversa.Usuário.Guid == mensagem.GuidRemetente)
                    {
                        cssClass = "Remetente";
                    }
                    else
                    {
                        cssClass = "Destinatário";
                        chat.RecebeuMensagem();
                    }

                    TryAgain.Run(() =>
                    {
                        boxMensagens = browser.Document.GetElementById("IdMensagens");
                    });


                    if (agruparPorDia)
                    {
                        string textoDia = "";
                        DateTime today = Conexão.GetServerDateTime();

                        DateTime dataDaMensagem = new DateTime(mensagem.EnviadaEm.Value.Year, mensagem.EnviadaEm.Value.Month, mensagem.EnviadaEm.Value.Day);
                        DateTime dataDeHoje = new DateTime(today.Year, today.Month, today.Day);
                        //DateTime dataDeHoje = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);


                        if (dataDaMensagem.Equals(dataDeHoje))
                        {
                            textoDia = "Hoje";
                        }
                        else if (dataDaMensagem.Equals(dataDeHoje.AddDays(-1)))
                        {
                            textoDia = "Ontem";
                        }
                        else
                        {
                            textoDia = dataDaMensagem.ToLongDateString();
                        }

                        TryAgain.Run(() =>
                        {
                            newElementInicial = browser.Document.CreateElement("conversa");
                            newElementInicial.InnerHtml = "<br /><p class='TextoDeCabeçalho'>" + textoDia + "</p>";
                            boxMensagens.AppendChild(newElementInicial);
                        });

                    }

                    if (reagrupar)
                    {
                        //string br = "";
                        //if (e.Mensagem.GuidRemetente == this.Controller.Usuário.Guid)
                        {
                            //br = "<br/>";
                        }

                        string horárioEnviado = mensagem.EnviadaEm.Value.ToShortTimeString();
                        Contato remetente = (mensagem.GuidRemetente == conversa.Usuário.Guid) ? conversa.Usuário : conversa.ConversandoCom;

                        // Cabeçalho do grupo de mensagens:
                        TryAgain.Run(() =>
                        {
                            newElementTitulo = browser.Document.CreateElement("conversa");
                            newElementTitulo.InnerHtml = "<br /><p class='TítuloDo" + cssClass + "'>::" + remetente.Apelido + "<br/></p>";
                            boxMensagens.AppendChild(newElementTitulo);
                        });



                        // Rodapé do grupo de mensagens:
                        TryAgain.Run(() =>
                        {
                            newElementTitulo = browser.Document.CreateElement("conversa");
                            newElementTitulo.InnerHtml = "<p id='" + horárioEnviado + "' class='RodapeDo" + cssClass + "'>" + horárioEnviado + "</p>";
                            boxMensagens.AppendChild(newElementTitulo);
                        });
                    }

                    TryAgain.Run(() =>
                    {
                        newElement = browser.Document.CreateElement("conversa");
                        newElement.InnerHtml = "<p id='" + cssId + "' class='MensagemDo" + cssClass + "'>" + mensagem.Texto + "</p>"; //<br/>";
                        boxMensagens.Children[boxMensagens.Children.Count - 1].InsertAdjacentElement(HtmlElementInsertionOrientation.BeforeBegin, newElement);
                    });


                    //

                    //HtmlElement elTextoRodapé = browser.Document.GetElementById("IdTextoRodapé");
                    //elTextoRodapé.InnerHtml = "Última mensagem: " + e.Mensagem.EnviadaEm.Value.ToLongTimeString();


                    // Posicionando o scroll:
                    TryAgain.Run(() =>
                    {
                        browser.Document.GetElementById("IdScroll").ScrollIntoView(false); // configura a posição do scroll (barra de rolagem);
                        conversa.ÚltimaMensagemExibida = mensagem;
                    });




                    //File.WriteAllText(@"c:\aaahtml.html", browser.Document.Body.OuterHtml);
                }
            }

            this.StartFlashWindow();
        }

        private void HtmlExibirHistórico(WebBrowser browser, Conversa conversa)
        {
            HtmlElement boxMensagensHistoricas = browser.Document.GetElementById("IdMensagensHistoricas");
            boxMensagensHistoricas.InnerHtml = "";



            foreach (Mensagem mensagem in conversa.Mensagens)
            {
                bool agruparPorDia = (conversa.ÚltimaMensagemExibida == null || conversa.ÚltimaMensagemExibida.EnviadaEm.Value.ToShortDateString() != mensagem.EnviadaEm.Value.ToShortDateString());

                bool reagrupar = (conversa.ÚltimaMensagemExibida == null
                                    || conversa.ÚltimaMensagemExibida.GuidRemetente != mensagem.GuidRemetente
                                    || conversa.ÚltimaMensagemExibida.EnviadaEm.Value.ToShortTimeString() != mensagem.EnviadaEm.Value.ToShortTimeString()
                                    || agruparPorDia);

                string cssId = mensagem.Guid.ToString();
                string cssClass = "";

                if (conversa.Usuário.Guid == mensagem.GuidRemetente)
                {
                    cssClass = "Remetente";
                }
                else
                {
                    cssClass = "Destinatário";
                }


                if (agruparPorDia)
                {
                    string textoDia = "";
                    DateTime dataDaMensagem = new DateTime(mensagem.EnviadaEm.Value.Year, mensagem.EnviadaEm.Value.Month, mensagem.EnviadaEm.Value.Day);
                    DateTime dataDeHoje = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);


                    if (dataDaMensagem.Equals(dataDeHoje))
                    {
                        textoDia = "Hoje";
                    }
                    else if (dataDaMensagem.Equals(dataDeHoje.AddDays(-1)))
                    {
                        textoDia = "Ontem";
                    }
                    else
                    {
                        textoDia = dataDaMensagem.ToLongDateString();
                    }


                    HtmlElement newElementInicial = browser.Document.CreateElement("conversa");
                    newElementInicial.InnerHtml = "<br /><p class='TextoDeCabeçalho'>" + textoDia + " (Histórico)</p>";
                    boxMensagensHistoricas.AppendChild(newElementInicial);
                }

                if (reagrupar)
                {
                    //string br = "";
                    //if (e.Mensagem.GuidRemetente == this.Controller.Usuário.Guid)
                    {
                        //br = "<br/>";
                    }

                    string horárioEnviado = mensagem.EnviadaEm.Value.ToShortTimeString();
                    Contato remetente = (mensagem.GuidRemetente == conversa.Usuário.Guid) ? conversa.Usuário : conversa.ConversandoCom;

                    HtmlElement newElementTitulo = browser.Document.CreateElement("conversa");
                    newElementTitulo.InnerHtml = "<br /><p class='TítuloDo" + cssClass + "'>::" + remetente.Apelido + "<br/></p>";
                    boxMensagensHistoricas.AppendChild(newElementTitulo);

                    newElementTitulo = browser.Document.CreateElement("conversa");
                    newElementTitulo.InnerHtml = "<p id='" + horárioEnviado + "' class='RodapeDo" + cssClass + "'>" + horárioEnviado + "</p>";
                    boxMensagensHistoricas.AppendChild(newElementTitulo);
                }


                HtmlElement newElement = browser.Document.CreateElement("conversa");
                //boxMensagensHistoricas.AppendChild(newElement);

                newElement.InnerHtml = "<p id='" + cssId + "' class='MensagemDo" + cssClass + "'>" + mensagem.Texto + "</p>";
                boxMensagensHistoricas.Children[boxMensagensHistoricas.Children.Count - 1].InsertAdjacentElement(HtmlElementInsertionOrientation.BeforeBegin, newElement);

                /*
                newElement.InnerHtml = "<div id='" + cssId + "' class='MensagemDo" + cssClass + "'>" +
                                            "<div class='ColunaDeMensagem'>" + mensagem.Texto + "</div>" +
                                            "<div class='ColunaDeData'>" + mensagem.EnviadaEm.Value.ToShortTimeString() + "</div>" +
                                       "</div>";
                */
                //<p>" + mensagem.EnviadaEm.Value.ToShortDateString() + " " + mensagem.EnviadaEm.Value.ToShortTimeString() + "</p>
                conversa.ÚltimaMensagemExibida = mensagem;
            }
        }

        private void HtmlIniciarChat(WebBrowser browser, Conversa conversa)
        {

        }

        public void EnviarMensagem(Conversa conversa, string mensagemTexto, string nomeDoArquivo = null)
        {
            int tentativasDeReconexão = 5;
            bool enviouMensagem = false;

            if (conversa == null)
            {
                return;
            }

            lock (LOCK)
            {
                while (!enviouMensagem)
                {
                    if (tentativasDeReconexão == 0)
                    {
                        break;
                    }

                    try
                    {
                        this.Controller.EnviarMensagem(conversa, mensagemTexto, nomeDoArquivo);
                        enviouMensagem = true;
                    }
                    catch (System.Data.SqlClient.SqlException sqlEx)
                    {
                        tentativasDeReconexão--;
                        System.Threading.Thread.Sleep(3000);
                        Conexão.Reconnect();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                if (!enviouMensagem)
                {
                    throw new Exception("Houve uma falha na conexão e a mensagem não pôde ser enviada!");
                }
            }
        }

        public void StopFlashWindow()
        {
            lock (LOCK)
            {
                Util.FlashWindowHelper.StopBlinking();
            }
        }

        //public void StartFlashWindow(List<Mensagem> mensagens)
        public void StartFlashWindow()
        {
            lock (LOCK)
            {
                // Este recurso será mantido em outra thread por não ser uma prioridade e por ser ThreadSafe;
                // A janela só deverá piscar quando a mensagem tiver sido enviada por outro Contato;
                if (!this.IsActivated)
                {
                    Task.Run(() =>
                    {
                        Util.FlashWindowHelper.StartBlinking(this);
                        return;

                        /*
                        foreach (Mensagem mensagem in mensagens)
                        {
                            if (mensagem.GuidDestinatário == this.Controller.Usuário.Guid)
                            {
                                Util.FlashWindowHelper.StartBlinking(this);
                                break;
                            }
                        }
                        */
                    });
                }
            }
        }

        





        /// <summary>
        /// Singleton para chamar o chat!
        /// </summary>
        /// <param name="contatosController"></param>
        /// <param name="destinatário"></param>
        /// <returns></returns>
        public static ConversasForm AbrirJanelaDeContatos(ContatosController contatosController, Contato destinatário, bool abrirJanelaMinimizada = false)
        {
            lock (LOCK)
            {
                if (ConversasForm._INSTANCE_ == null || ConversasForm._INSTANCE_.IsDisposed)
                {
                    ConversasForm._INSTANCE_ = new ConversasForm(contatosController);
                }

                //ConversasForm._INSTANCE_.NotificarChamadas(destinatário, mensagens);


                ConversasForm form = ConversasForm._INSTANCE_;  // apenas um aliás para facilitar a escrita do código;

                form.SelecionarConversa(destinatário);

                if (!abrirJanelaMinimizada)
                {
                    form.SetShowWithoutActivation(false);

                    if (form.Visible)
                    {
                        form.BringToFront();
                        form.Focus();
                        form.Show();
                    }
                    else
                    {
                        form.StartPosition = FormStartPosition.CenterScreen;
                        form.Visible = true;
                        form.Show();
                        form.BringToFront();
                        form.Focus();
                    }

                    if (form.WindowState == FormWindowState.Minimized)
                    {
                        form.WindowState = FormWindowState.Normal;
                    }

                    form.SetShowWithoutActivation(true);
                }
                else
                {
                    if (!form.IsDisposed)
                    {
                        form.SetShowWithoutActivation(true);
                        form.Visible = true;
                    }
                }

                return ConversasForm._INSTANCE_;
            }
        }

        public void SelecionarConversa(Contato destinatário)
        {
            this.Controller.SelecionarConversa(destinatário);
        }


        /// <summary>
        /// Abre janela de chat e envia mensagens.
        /// </summary>
        /// <param name="contatosController"></param>
        /// <param name="contato"></param>
        /// <param name="mensagens"></param>
        /// <returns></returns>
        public static ConversasForm EnviarMensagemParaContato(ContatosController contatosController, Contato destinatário, Mensagem mensagem, bool abrirJanelaMinimizada = true)
        {
            lock (LOCK)
            {
                ConversasForm form = AbrirJanelaDeContatos(contatosController, destinatário, abrirJanelaMinimizada);

                contatosController.ConversasController.EnviarMensagem(destinatário, mensagem);

                //form.NotificarChamadas(destinatário, mensagens);


                /*
                if (abrirJanelaMinimizada)
                {
                    form.StartFlashWindow();
                }
                */

                return form;
            }
        }


        public static ConversasForm EnviarArquivoParaContato(ContatosController contatosController, Contato destinatário, string[] files, bool abrirJanelaMinimizada = false)
        {
            lock (LOCK)
            {
                ConversasForm form = AbrirJanelaDeContatos(contatosController, destinatário, abrirJanelaMinimizada);
                form.EnviarArquivo(files);

                return form;
            }

        }

        void Controller_ConversaSelecionadaChanged(object sender, EventArgs e)
        {
            if (this.Controller.IsConversaSelecionada())
            {
                this.SelecionarConversa();
            }
            else
            {
                this.Close();
            }
        }

        void Conversas_ListChanged(object sender, ListChangedEventArgs e)
        {
            lock (LOCK)
            {
                if (e.ListChangedType == ListChangedType.ItemAdded)
                {
                    Conversa conversa = this.Controller.Conversas[e.NewIndex];
                    this.CriarConversa(conversa);
                }
                else if (e.ListChangedType == ListChangedType.ItemDeleted)
                {
                    foreach (Crownwood.Magic.Controls.TabPage tab in this.tabControlNew.TabPages.ToList<Crownwood.Magic.Controls.TabPage>())
                    {
                        Conversa conversaRemovida = tab.Tag as Conversa;

                        if (conversaRemovida == null || !this.Controller.Conversas.Contains(conversaRemovida))
                        {
                            this.tabControlNew.TabPages.Remove(tab);
                        }
                    }
                }
            }
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            Perfil.PERFIL.SetChatForm(this);
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Perfil.PERFIL.RegistrarConversas(this);
                Perfil.SalvarPerfil();
            }
            catch { }
        }

        private void ChatForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ConversasForm.IsOpen = false;

            try
            {
                this.Controller.Conversas.ListChanged -= Conversas_ListChanged;
                this.Controller.ConversaSelecionadaChanged -= Controller_ConversaSelecionadaChanged;
                this.Controller.AvisoRecebeuNovaMensagem -= Controller_AvisoRecebeuNovaMensagem;
                this.Controller.Conversas.Clear();
            }
            catch { }

            ConversasForm._INSTANCE_ = null;
            this.Dispose(true);
        }

        private void tabControlNew_SelectionChanged(object sender, EventArgs e)
        {
            if (this.tabControlNew.SelectedTab == null)
            {
                this.Close();
                return;
            }

            this.Controller.ConversaSelecionada = this.tabControlNew.SelectedTab.Tag as Conversa;
        }

        private void tabControlNew_ClosePressed(object sender, EventArgs e)
        {
            if (this.tabControlNew.TabPages.Count <= 1)
            {
                this.Close();
                return;
            }

            this.Controller.FinalizarConversaSelecionada();
        }

        private void ChatForm_KeyDown(object sender, KeyEventArgs e)
        {
            lock (LOCK)
            {
                if (e.Control)
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        //ApertouCtrlEnter();
                    }
                    else if (e.KeyCode == Keys.PageDown)
                    {
                        this.Controller.SelecionarConversaAnterior();
                    }
                    else if (e.KeyCode == Keys.PageUp)
                    {
                        this.Controller.SelecionarConversaPosterior();
                    }
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    ApertouEnter();
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    this.WindowState = FormWindowState.Minimized;
                }
            }
        }

        private void ChatForm_Activated(object sender, EventArgs e)
        {
            this.IsActivated = true;
            this.StopFlashWindow();
            this.KeyPreview = true;

            Crownwood.Magic.Controls.TabPage tabPage = this.tabControlNew.SelectedTab;

            PageChat chat = tabPage.Controls["PageChat"] as PageChat;
            chat.Focus();
            chat.TextBox.Focus();
        }

        private void ChatForm_Deactivate(object sender, EventArgs e)
        {
            this.IsActivated = false;
        }

        public void ApertouCtrlEnter()
        {
            Crownwood.Magic.Controls.TabPage tabPage = this.tabControlNew.SelectedTab;

            Conversa conversa = tabPage.Tag as Conversa;

            if (conversa != null)
            {
                PageChat chat = tabPage.Controls["PageChat"] as PageChat;

                try
                {
                    chat.TextBox.Enabled = false;
                    chat.TextBox.Text = chat.TextBox.Text + "\r\n";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    chat.TextBox.Focus();
                }
            }
        }

        public void ApertouEnter()
        {
            this.ResizeRedraw = false;
            Crownwood.Magic.Controls.TabPage tabPage = this.tabControlNew.SelectedTab;

            Conversa conversa = tabPage.Tag as Conversa;
            PageChat chat = tabPage.Controls["PageChat"] as PageChat;
            TextBox textBox = chat.TextBox;

            if (textBox.Text.Trim() == string.Empty)
            {
                return;
            }

            if (conversa != null)
            {
                try
                {
                    textBox.Enabled = false;
                    string message = textBox.Text;


                    message = message.Replace("\r\n", "<br/>");
                    this.EnviarMensagem(conversa, message);

                    textBox.ResetText();
                    textBox.Multiline = false;
                    textBox.Multiline = true;
                    textBox.Enabled = true;
                    textBox.Focus();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {

                }
            }
            this.ResizeRedraw = true;
        }


        /*
        private string AplicarLink(string message)
        {
            if (message.Contains("http://") || message.Contains("https://"))
            {
                int leftIdx = 0;
                string[] links = { "http://", "https://" };
                int rightIdx = 0;
                bool hasNext = false;

                do
                {
                    int lnkIdx = -1;

                    foreach (string link in links)
                    {
                        int idx = message.IndexOf(link, leftIdx);

                        if (idx != -1 && idx < lnkIdx)
                        {
                            lnkIdx = idx;
                        }
                    }

                    hasNext = (lnkIdx != -1);


                } while (hasNext);

            }
        }
        */


        private void SairDoSistema()
        {

            // {{{saída_programada}}}
            Perfil.SalvarPerfil();
            Application.Exit();
        }

        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Crownwood.Magic.Controls.TabPage tabPage = this.tabControlNew.SelectedTab;
            PageChat chat = tabPage.Controls["PageChat"] as PageChat;
            this.CopiarParaClipboardTextoSelecionadoDeWebBrowser(chat.WebBrowser);
        }

        private void CopiarParaClipboardTextoSelecionadoDeWebBrowser(WebBrowser browser)
        {
            mshtml.IHTMLDocument2 doc = browser.Document.DomDocument as mshtml.IHTMLDocument2;

            Clipboard.Clear();

            if (doc != null)
            {
                mshtml.IHTMLSelectionObject currentSelection = doc.selection as mshtml.IHTMLSelectionObject;

                if (currentSelection != null)
                {
                    mshtml.IHTMLTxtRange range = currentSelection.createRange() as mshtml.IHTMLTxtRange;

                    if (range != null)
                    {
                        Clipboard.SetText(range.text);
                    }
                }
            }
        }

        private void umDiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (LOCK)
            {
                Crownwood.Magic.Controls.TabPage page = this.tabControlNew.SelectedTab as Crownwood.Magic.Controls.TabPage;
                PageChat chat = page.Controls["PageChat"] as PageChat;

                Conversa conversa = FiltrarHistórico(chat, DateTime.Today.AddDays(-1));
                this.HtmlExibirHistórico(chat.WebBrowser, conversa);
            }
        }

        private void umaSemanaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (LOCK)
            {
                Crownwood.Magic.Controls.TabPage page = this.tabControlNew.SelectedTab as Crownwood.Magic.Controls.TabPage;
                PageChat chat = page.Controls["PageChat"] as PageChat;

                Conversa conversa = FiltrarHistórico(chat, DateTime.Today.AddDays(-7));
                this.HtmlExibirHistórico(chat.WebBrowser, conversa);
            }
        }

        private void umMêsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (LOCK)
            {
                Crownwood.Magic.Controls.TabPage page = this.tabControlNew.SelectedTab as Crownwood.Magic.Controls.TabPage;
                PageChat chat = page.Controls["PageChat"] as PageChat;

                Conversa conversa = FiltrarHistórico(chat, DateTime.Today.AddDays(-30));
                this.HtmlExibirHistórico(chat.WebBrowser, conversa);
            }
        }

        private void seisMesesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (LOCK)
            {
                Crownwood.Magic.Controls.TabPage page = this.tabControlNew.SelectedTab as Crownwood.Magic.Controls.TabPage;
                PageChat chat = page.Controls["PageChat"] as PageChat;

                Conversa conversa = FiltrarHistórico(chat, DateTime.Today.AddMonths(-6));
                this.HtmlExibirHistórico(chat.WebBrowser, conversa);
            }
        }

        private void tudoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lock (LOCK)
            {
                Crownwood.Magic.Controls.TabPage page = this.tabControlNew.SelectedTab as Crownwood.Magic.Controls.TabPage;
                PageChat chat = page.Controls["PageChat"] as PageChat;

                Conversa conversa = FiltrarHistórico(chat, DateTime.Today.AddYears(-100));
                this.HtmlExibirHistórico(chat.WebBrowser, conversa);
            }
        }

        private Conversa FiltrarHistórico(PageChat chat, DateTime desde)
        {
            Mensagem primeiraMensagem = chat.Conversa.Mensagens.FirstOrDefault();
            DateTime mensagemMaisAntiga = (primeiraMensagem == null) ? Conexão.GetServerDateTime() : primeiraMensagem.EnviadaEm.Value;
            string dataDe = desde.ToShortDateString();
            string dataAté = mensagemMaisAntiga.ToShortDateString() + " " + mensagemMaisAntiga.ToLongTimeString();

            string whereExpression = "((GuidRemetente = '{0}' and GuidDestinatario = '{1}' and RemetenteLeu = 1) OR (GuidRemetente = '{1}' and GuidDestinatario = '{0}' and DestinatarioLeu = 1)) and EnviadoEm >= '{2}' and EnviadoEm < '{3}'";
            whereExpression = whereExpression.FormatTo(chat.Conversa.Usuário.Guid.ToString(), chat.Conversa.ConversandoCom.Guid.ToString(), dataDe, dataAté);

            Conversa conversa = new Conversa(chat.Conversa.Usuário, chat.Conversa.ConversandoCom);

            foreach (Envelope envelope in Envelope.GetBy(whereExpression))
            {
                conversa.Mensagens.Add(envelope.ToMensagem());
            }

            return conversa;
        }
    }
}