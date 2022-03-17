using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.Concurrent;
using System.Drawing;

using CoreDll.Bindables;
using CoreDll;
using GTolk.Models;
using GTolk.Util;

namespace GTolk.Controllers
{
    public class OpçõesController : BindableBase
    {
        public Contato Usuário { get { return base.Get<Contato>(); } private set { base.Set(value); } }

        public string Apelido { get { return base.Get<string>(); } private set { base.Set(value); } }
        public string Descrição { get { return base.Get<string>(); } private set { base.Set(value); } }

        public Image Imagem50 { get { return base.Get<Image>(); } private set { base.Set(value); } }
        public Image Imagem75 { get { return base.Get<Image>(); } private set { base.Set(value); } }
        public Image Imagem100 { get { return base.Get<Image>(); } private set { base.Set(value); } }


        public OpçõesController(Contato usuárioLogado)
        {
            this.Usuário = Contato.GetByGuid(usuárioLogado.Guid);

            this.AtualizarImagens();
            this.Apelido = this.Usuário.Apelido;
            this.Descrição = this.Usuário.Descrição;

            this.Usuário.PropertyChanged += Usuário_PropertyChanged;
        }

        void Usuário_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.AtualizarImagens();
        }

        public void AtualizarImagens()
        {
            this.Imagem50 = Imagens.ScaleImage(this.Usuário.Imagem, 50, 50);
            this.Imagem75 = Imagens.ScaleImage(this.Usuário.Imagem, 75, 75);
            this.Imagem100 = Imagens.ScaleImage(this.Usuário.Imagem, 100, 100);
        }

        public void Salvar()
        {
            this.Usuário.Apelido = this.Apelido;
            this.Usuário.Descrição = this.Descrição;
            Contato.Save(this.Usuário);
        }
    }
}