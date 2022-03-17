using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Concurrent;
using System.Threading;

using CoreDll;
using CoreDll.Bindables;
using GTolk.Models;
using GTolk.Controllers.Eventos;

namespace GTolk.Controllers
{
    public class ConversasController : BindableBase
    {
        //private static ChatController _INSTANCE_ = null;
        private static object _LOCK_ = new object();
        private object LOCK_RECEIVING_MESSAGES = new object();

        public Contato Usuário { get { return base.Get<Contato>(); } set { base.Set(value); } }
        public BindingList<Conversa> Conversas { get { return base.Get<BindingList<Conversa>>(); } set { base.Set(value); } }
        public ContatosController ContatosController { get { return base.Get<ContatosController>(); } private set { base.Set(value); } }
        public ConcurrentQueue<Envelope> CaixaDeEntrada { get { return base.Get<ConcurrentQueue<Envelope>>(); } set { base.Set(value); } }

        

        public BackgroundWorker MensagensWorker { get { return base.Get<BackgroundWorker>(); } set { base.Set(value); } }

        public Conversa ConversaSelecionada 
        { 
            get { return base.Get<Conversa>(); } 
            set 
            {
                Conversa conversaTemp = this.ConversaSelecionada;
                if (base.Set(value)) { OnConversaSelecionadaChanged(); this.ConversaSelecionadaAnteriormente = conversaTemp; }
            } 
        }

        public Conversa ConversaSelecionadaAnteriormente { get { return base.Get<Conversa>(); } private set { base.Set(value); } }

        public string TítuloDoChat { get { return base.Get<string>(); } set { base.Set(value); } }

        public ConversasController(ContatosController contatosController)
        {
            this.ContatosController = contatosController;
            this.Usuário = this.ContatosController.Usuário;
            this.Conversas = new BindingList<Conversa>();
            this.TítuloDoChat = "GTolk - " + this.Usuário.Apelido + " (" + this.Usuário.Email + ")";

            this.ConversaSelecionada = null;
            this.ConversaSelecionadaAnteriormente = null;
            this.CaixaDeEntrada = new ConcurrentQueue<Envelope>();



            this.MensagensWorker = new BackgroundWorker();
            this.MensagensWorker.WorkerReportsProgress = true;
            this.MensagensWorker.WorkerSupportsCancellation = true;


            // Configuração do BackgroundWorker de MENSAGENS
            this.MensagensWorker.ProgressChanged += (object sender, ProgressChangedEventArgs e) =>
            {
                this.NotificarRecebimentoDeNovasMensagens();
            };

            this.MensagensWorker.DoWork += (object sender, DoWorkEventArgs e) =>
            {
                //Thread.Sleep(2000);

                while (true)
                {
                    try
                    {
                        if (this.MensagensWorker.CancellationPending)
                        {
                            e.Cancel = true;

                            if (this.CaixaDeEntrada.Count > 0)
                            {
                                this.MensagensWorker.ReportProgress(0);  // 
                            }
                        }

                        this.ReceberNovasMensagens();

                        if (this.CaixaDeEntrada.Count > 0)
                        {
                            this.MensagensWorker.ReportProgress(0);  // 
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

                    Thread.Sleep(2000);
                }
            };
        }

               
        
        public void ReceberNovasMensagens()
        {
            lock (LOCK_RECEIVING_MESSAGES)
            {
                try
                {
                    List<Envelope> envelopes = Envelope.ReceiveNews(this.Usuário);

                    if (envelopes.Count > 0)
                    {
                        foreach (Envelope envelope in envelopes)
                        {
                            this.CaixaDeEntrada.Enqueue(envelope);
                        }
                    }
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                }
            }
        }

        public void NotificarRecebimentoDeNovasMensagens()
        {
            Envelope envelope = null;

            lock (_LOCK_)
            {

                while (this.CaixaDeEntrada.TryDequeue(out envelope))
                {
                    Contato destinatário = null;

                    if (envelope.GuidDestinatário == this.Usuário.Guid)
                    {
                        destinatário = this.ContatosController.Contatos.Where(x => x.Guid == envelope.GuidRemetente).FirstOrDefault();
                    }
                    else
                    {
                        destinatário = this.ContatosController.Contatos.Where(x => x.Guid == envelope.GuidDestinatário).FirstOrDefault();
                    }

                    if (destinatário != null)
                    {
                        Mensagem mensagem = envelope.ToMensagem();
                        //ConversasForm.NotificarChamadas(this.Controller, destinatario, mensagem);
                        //ConversasForm.EnviarMensagemParaContato(this.ContatosController.Controller, this.Controller.ContatoSelecionado, mensagem, true);

                        Conversa conversa = null;

                        conversa = this.GetOrCreateNewConversa(destinatário);
                        conversa.Mensagens.Add(mensagem);
                        this.OnAvisoRecebeuNovaMensagem(conversa, destinatário, mensagem);
                    }
                }
            }
        }
        
        private Conversa GetConversa(Mensagem mensagem)
        {
            Conversa conversa = this.Conversas.Where(c =>
            {
                return c.Usuário.Equals(this.Usuário) && c.ConversandoCom.Guid == mensagem.GuidRemetente
                        || c.Usuário.Equals(this.Usuário) && c.ConversandoCom.Guid == mensagem.GuidDestinatário;
            }).FirstOrDefault();

            return conversa;
        }

        
        

        public void SelecionarConversa(Contato destinatário)
        {
            lock (_LOCK_)
            {
                Conversa conversa = this.GetOrCreateNewConversa(destinatário);
                this.ConversaSelecionada = conversa;
            }
        }

        public void FinalizarConversaSelecionada()
        {
            if (this.IsConversaSelecionada())
            {
                this.Conversas.Remove(this.ConversaSelecionada);
                this.ConversaSelecionada = this.ConversaSelecionadaAnteriormente;
            }
        }

        /// <summary>
        /// Retorna uma conversa existente com o contato alvo ou cria uma nova.
        /// </summary>
        /// <param name="contato">Instância do contato com o qual se deseja criar a conversa.</param>
        /// <returns>Retorna a conversa com o contato especificado no parâmetro</returns>
        private Conversa GetOrCreateNewConversa(Contato contato)
        {
            lock (_LOCK_)
            {
                Conversa conversa = this.Conversas.Where(x => x.ConversandoCom.Equals(contato) && x.Usuário.Equals(this.Usuário)).FirstOrDefault(); // garante que não haverá conversas duplicadas na lista;

                // se conversa não existir, adiciona:
                if (conversa == null)                
                {
                    conversa = new Conversa(this.Usuário, contato);
                    conversa.IniciadaEm = Conexão.GetServerDateTime();
                    this.Conversas.Add(conversa); 
                }

                return conversa;
            }
        }

        public void EnviarMensagem(Contato destinatário, string mensagemTexto, string nomeDoArquivo = null)
        {
            Conversa conversa = this.GetOrCreateNewConversa(destinatário);
            this.EnviarMensagem(conversa, mensagemTexto, nomeDoArquivo);
        }

        public void EnviarMensagem(Contato destinatário, Mensagem mensagem)
        {
            Conversa conversa = this.GetOrCreateNewConversa(destinatário);
            this.EnviarMensagem(conversa, mensagem);
        }

        public void EnviarMensagem(Conversa conversa, string mensagemTexto, string nomeDoArquivo = null)
        {
            Mensagem novaMensagem = new Mensagem();
            novaMensagem.GuidRemetente = conversa.Usuário.Guid;
            novaMensagem.GuidDestinatário = conversa.ConversandoCom.Guid;
            novaMensagem.Texto = mensagemTexto;

            if (nomeDoArquivo != null)
            {
                novaMensagem.IsArquivo = true;
                novaMensagem.NomeDoArquivo = nomeDoArquivo;
            }

            this.EnviarMensagem(conversa, novaMensagem);
        }

        public void EnviarMensagem(Conversa conversa, Mensagem mensagem)
        {
            Envelope.Send(mensagem);

            this.ReceberNovasMensagens();
            this.NotificarRecebimentoDeNovasMensagens();
        }

        public void SelecionarConversaPosterior()
        {
            if (!this.IsConversaSelecionada())
            {
                this.ConversaSelecionada = this.Conversas.FirstOrDefault();
            }
            else
            {
                for (int idx = 0; idx < this.Conversas.Count; idx++)
                {
                    if (this.Conversas[idx].Equals(this.ConversaSelecionada))
                    {
                        int idxPosterior = (idx + 1 < this.Conversas.Count) ? idx + 1 : 0;
                        this.ConversaSelecionada = this.Conversas[idxPosterior];
                        break;
                    }
                }
            }
        }

        public void SelecionarConversaAnterior()
        {
            if (!this.IsConversaSelecionada())
            {
                this.ConversaSelecionada = this.Conversas.FirstOrDefault();
            }
            else
            {
                for (int idx = 0; idx < this.Conversas.Count; idx++)
                {
                    if (this.Conversas[idx].Equals(this.ConversaSelecionada))
                    {
                        int idxAnterior = (idx - 1 >= 0) ? idx - 1 : this.Conversas.Count - 1;
                        idxAnterior = (idxAnterior) < 0 ? 0 : idxAnterior;
                        this.ConversaSelecionada = this.Conversas[idxAnterior];
                        break;
                    }
                }
            }
        }

        public bool IsConversaSelecionada()
        {
            return (this.ConversaSelecionada != null);
        }

        






        


        


        #region Eventos

        public event EventHandler ConversaSelecionadaChanged;

        private void OnConversaSelecionadaChanged()
        {
            if (this.ConversaSelecionadaChanged != null)
            {
                this.ConversaSelecionadaChanged(this, new EventArgs());
            }
        }

        
        public event NovaMensagemEventHandler AvisoRecebeuNovaMensagem;

        private void OnAvisoRecebeuNovaMensagem(Conversa conversa, Contato contato, Mensagem mensagem)
        {
            if (this.AvisoRecebeuNovaMensagem != null)
            {
                this.AvisoRecebeuNovaMensagem(this, new NovaMensagemEventArgs(conversa, contato, mensagem));
            }
        }
        

        #endregion Eventos
    }
}