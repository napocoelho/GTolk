using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using CoreDll.Bindables;

namespace GTolk.Models
{
    public class Conversa : BindableBase
    {
        //public Guid Guid { get { return base.Get<Guid>(); } set { base.Set(value); } }
        public Contato Usuário { get { return base.Get<Contato>(); } set { base.Set(value); } }
        public Contato ConversandoCom { get { return base.Get<Contato>(); } set { base.Set(value); } }
        public DateTime IniciadaEm { get { return base.Get<DateTime>(); } set { base.Set(value); } }
        

        public BindingList<Mensagem> Mensagens { get { return base.Get<BindingList<Mensagem>>(); } set { base.Set(value); } }

        public Mensagem ÚltimaMensagemExibida { get { return base.Get<Mensagem>(); } set { base.Set(value); } }
        //public int MensagensNãoLidas { get { return base.Get<int>(); } set { base.Set(value); } }

        public Conversa(Contato usuário, Contato conversandoCom)
        {
            //this.Guid = Guid.NewGuid();
            this.Usuário = usuário;
            this.ConversandoCom = conversandoCom;
            this.Mensagens = new BindingList<Mensagem>();
            this.ÚltimaMensagemExibida = null;
            //this.MensagensNãoLidas = 0;
        }

        public override int GetHashCode()
        {
            return this.Usuário.GetHashCode() * this.ConversandoCom.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;

            Conversa another = obj as Conversa;

            if (another == null)
                return false;

            if (this.Usuário.Equals(another.Usuário) && this.ConversandoCom.Equals(another.ConversandoCom))
                return true;

            return false;
        }
    }
}