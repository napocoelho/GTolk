using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTolk.Models;

namespace GTolk.Controllers.Eventos
{
    public class NovaMensagemEventArgs : EventArgs
    {
        public Conversa Conversa { get; set; }
        public Contato Contato { get; set; }
        public Mensagem Mensagem { get; set; }
        
        public NovaMensagemEventArgs(Conversa conversa, Contato contato, Mensagem mensagem)
            : base()
        {
            this.Conversa = conversa;
            this.Contato = contato;
            this.Mensagem = mensagem;
        }
    }
}