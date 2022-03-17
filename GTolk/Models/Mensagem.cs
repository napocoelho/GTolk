using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;

using ConnectionManagerDll;
using CoreDll.Bindables;

namespace GTolk.Models
{
    [Serializable]
    public class Mensagem : BindableBase
    {
        public Guid Guid { get { return base.Get<Guid>(); } set { base.Set(value); } }
        public string Texto { get { return base.Get<string>(); } set { base.Set(value); } }
        public Guid GuidRemetente { get { return base.Get<Guid>(); } set { base.Set(value); } }
        public Guid GuidDestinatário { get { return base.Get<Guid>(); } set { base.Set(value); } }
        public DateTime? EnviadaEm { get { return base.Get<DateTime?>(); } set { base.Set(value); } }
        public bool IsArquivo { get { return base.Get<bool>(); } set { base.Set(value); } }
        public string NomeDoArquivo { get { return base.Get<string>(); } set { base.Set(value); } }

        public Mensagem()
        {
            this.Guid = Guid.NewGuid();
            this.Texto = string.Empty;
            //this.GuidRemetente = null;
            //this.GuidDestinatário = null;
            this.EnviadaEm = null;
            this.IsArquivo = false;
            this.NomeDoArquivo = string.Empty;
        }
    }
}