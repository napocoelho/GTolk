using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Concurrent;

using CoreDll.Bindables;
using CoreDll;
using GTolk.Models;

namespace GTolk.Controllers
{
    public class ContatosController : BindableBase
    {
        public object _LOCK_ = new object();
        public Contato Usuário { get { return base.Get<Contato>(); } set { base.Set(value); } }
        public SyncBindingList<Contato> Contatos { get { lock (_LOCK_) { return base.Get<SyncBindingList<Contato>>(); } } set { base.Set(value); } }

        /// <summary>
        /// ConversasController é instanciada e configurada internamente por ContatosController.
        /// ContatosController possui e comunica com o ConversasController através de eventos ou diretamente.
        /// </summary>
        public ConversasController ConversasController { get { return base.Get<ConversasController>(); } private set { base.Set(value); } } // <--- importantíssimo;

        public Contato ContatoSelecionado { get { return base.Get<Contato>(); } set { base.Set(value); } }

        private ConcurrentQueue<Contato> ContatosAlterados { get { return base.Get<ConcurrentQueue<Contato>>(); } set { base.Set(value); } }

        
        public BackgroundWorker ContatosWorker { get { return base.Get<BackgroundWorker>(); } set { base.Set(value); } }

        public ContatosController(Contato usuárioLogado)
        {
            this.ContatosAlterados = new ConcurrentQueue<Contato>();

            this.Usuário = usuárioLogado;
            this.Contatos = new SyncBindingList<Contato>();

            this.ContatoSelecionado = null;

            this.ConversasController = new Controllers.ConversasController(this);   // <--- importantíssimo;




            Contato.SetStatus(this.Usuário, StatusDoContato.Online);


            //this.AvisoDeAlteraçãoDeContatos += ContatosController_AvisoDeAlteraçãoDeContatos;
            //this.AvisoDeChegadaDeMensagens += ContatosController_AvisoDeChegadaDeMensagens;


            this.ContatosWorker = new BackgroundWorker();
            this.ContatosWorker.WorkerReportsProgress = true;
            this.ContatosWorker.WorkerSupportsCancellation = true;



            if (!this.Usuário.IsAdmin)
            {
                BackgroundWorker manutençãoWorker = new BackgroundWorker();
                manutençãoWorker.WorkerReportsProgress = true;
                manutençãoWorker.WorkerSupportsCancellation = true;

                manutençãoWorker.ProgressChanged += (object sender, ProgressChangedEventArgs e) =>
                    {
                        lock (_LOCK_)
                        {
                            this.OnAvisoDeManutenção();
                        }
                    };


                manutençãoWorker.DoWork += (object sender, DoWorkEventArgs e) =>
                    {
                        while (true)
                        {
                            try
                            {
                                object obj = Conexão.ExecuteScalar("select count(*) from Tlk_Manutenção");
                                int count = (int)obj;

                                if (count > 0)
                                {
                                    manutençãoWorker.ReportProgress(0);
                                }
                            }
                            catch (Exception ex)
                            {
                                string err = ex.Message;
                            }

                            Thread.Sleep(3000);
                        }
                    };

                manutençãoWorker.RunWorkerAsync();
            }

            // Configuração do BackgroundWorker de CONTATOS;
            this.ContatosWorker.ProgressChanged += (object sender, ProgressChangedEventArgs e) =>
                {
                    this.AtualizarContatos();
                    this.OnAvisoDeAlteraçãoDeContatos();

                    Thread.Sleep(50);

                    while (this.ConversasController.MensagensWorker.IsBusy)
                    {
                        Thread.Sleep(50);
                    }

                    this.ConversasController.MensagensWorker.RunWorkerAsync();
                };

            this.ContatosWorker.DoWork += (object sender, DoWorkEventArgs e) =>
                {
                    while (true)
                    {
                        try
                        {
                            if (this.ContatosWorker.CancellationPending)
                            {
                                e.Cancel = true;

                                if (this.ContatosAlterados.Count > 0)
                                {
                                    this.ContatosWorker.ReportProgress(0);
                                }
                            }

                            this.VerificarContatos(false);  // o false é muito importante;

                            if (this.ContatosAlterados.Count > 0)
                            {
                                this.ContatosWorker.ReportProgress(0);  // 
                            }
                        }
                        catch (System.Data.SqlClient.SqlException sqlEx)
                        {
                            string teste = sqlEx.Message;
                            try
                            {
                                Conexão.Reconnect();
                            }
                            catch { }
                        }
                        catch (Exception ex)
                        {
                            string err = ex.Message;
                        }

                        Thread.Sleep(3000);
                    }
                };

            this.ContatosWorker.RunWorkerAsync();
        }


        private void AtualizarContatos()
        {
            Contato contato = null;

            while (this.ContatosAlterados.TryDequeue(out contato))
            {
                Contato contatoAtivo = this.Contatos.Where(c => c.Guid == contato.Guid).FirstOrDefault();

                if (contatoAtivo == null)   // se for contato novo;
                {
                    this.Contatos.Add(contato);
                }
                else   // se for contato antigo que foi alterado;                
                {
                    CoreDll.Reflection.ObjectCopier.CopyPropertiesTo(contato, contatoAtivo);
                }
            }
        }


        

        /*
        private void ContatosController_AvisoDeAlteraçãoDeContatos(object sender, EventArgs e)
        {
            Contato  contato = null;
            
            while (this.ContatosAlterados.TryDequeue(out contato))
            {
                Contato contatoAtivo = this.Contatos.Where(c => c.Guid == contato.Guid).FirstOrDefault();

                if (contatoAtivo == null)   // se for contato novo;
                {
                    this.Contatos.Add(contato);
                }
                else   // se for contato antigo que foi alterado;                
                {
                    CoreDll.Reflection.ObjectCopier.CopyPropertiesTo(contato, contatoAtivo);
                }
            }
        }
        */
        


        ~ContatosController()
        {
            if (this.Usuário != null)
            {
                try
                {
                    if (!Conexão.IsConnected )
                    {
                        Conexão.Reconnect();
                    }

                    Contato.SetStatus(this.Usuário, StatusDoContato.Offline);
                }
                catch
                {
                    try
                    {
                        if (ConnectionManagerDll.ConnectionManager.GetConnection != null)
                        {
                            Contato.SetStatus(this.Usuário, StatusDoContato.Offline);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public bool IsContatoSelecionado()
        {
            return (this.ContatoSelecionado != null);
        }

        

        public void VerificarContatos(bool acionarEventos = true)
        {
            IList<Contato> contatosAtualizados = Contato.GetContatos(this.Usuário).OrderBy(x => x.Status).ToList();
            

            // Apenas atualiza os contatos que já foram obtidos (não os substitui, ah não ser que tenha algum novo):
            foreach (Contato contato in contatosAtualizados)
            {   
                Contato contatoAtivo = this.Contatos.Where(c => c.Guid == contato.Guid).FirstOrDefault();

                if (contatoAtivo == null || contatoAtivo.Timestamp != contato.Timestamp)
                {
                    this.ContatosAlterados.Enqueue(contato);
                }

            }

            if (acionarEventos && this.ContatosAlterados.Count > 0)
            {
                this.OnAvisoDeAlteraçãoDeContatos();
            }
        }



        #region Eventos


        public event EventHandler AvisoDeAlteraçãoDeContatos;

        private void OnAvisoDeAlteraçãoDeContatos()
        {

            if (this.AvisoDeAlteraçãoDeContatos != null)
            {
                this.AvisoDeAlteraçãoDeContatos(this, new EventArgs());
            }
        }

        public event EventHandler AvisoDeSaídaDoSistema;

        private void OnAvisoDeManutenção()
        {

            if (this.AvisoDeSaídaDoSistema != null)
            {
                this.AvisoDeSaídaDoSistema(this, new EventArgs());
            }
        }
        

        #endregion Eventos
    }
}