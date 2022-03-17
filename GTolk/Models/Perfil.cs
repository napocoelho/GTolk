using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Drawing;

using CoreDll.Bindables;

namespace GTolk.Models
{
    [Serializable]
    public class Perfil : BindableBase
    {
        private static object LOCK = new object();
        private static Perfil perfil = null;
        public static string FILEPATH = null;

        public static Perfil PERFIL
        {
            get { lock (LOCK) { return perfil; } }
            set { lock (LOCK) { perfil = value; } }
        }

        public Guid GuidÚltimoContatoLogado { get; set; }
        public List<PerfilDeUsuário> Perfis { get; set; }

        public Perfil()
        {
            this.Perfis = new List<PerfilDeUsuário>();
        }

        private PerfilDeUsuário EncontrarÚltimoPerfilDeUsuárioLogado()
        {
            lock (LOCK)
            {
                if (this.GuidÚltimoContatoLogado == null)
                {
                    return null;
                }

                foreach (PerfilDeUsuário item in this.Perfis)
                {
                    if (item.GuidContato == this.GuidÚltimoContatoLogado)
                    {
                        return item;
                    }
                }

                return null;
            }
        }

        public void RegistrarTodos(Contato usuário, ContatosForm contatosForm, ConversasForm chatForm)
        {
            lock (LOCK)
            {
                this.RegistrarUsuário(usuário);
                this.RegistrarContatos(contatosForm);
                this.RegistrarConversas(chatForm);
            }
        }

        public void RegistrarUsuário(Contato usuário)
        {
            lock (LOCK)
            {
                PerfilDeUsuário perfilEncontrado = null;

                foreach (PerfilDeUsuário item in this.Perfis)
                {
                    if (item.GuidContato == usuário.Guid)
                    {
                        perfilEncontrado = item;
                        break;
                    }
                }

                // Cria novo perfil se ainda não existir:
                if (perfilEncontrado == null)
                {
                    perfilEncontrado = new PerfilDeUsuário();
                    this.Perfis.Add(perfilEncontrado);
                }

                this.GuidÚltimoContatoLogado = usuário.Guid;
                perfilEncontrado.GuidContato = usuário.Guid;
            }
        }

        public void RegistrarContatos(ContatosForm contatosForm)
        {
            lock (LOCK)
            {
                if (contatosForm.WindowState == System.Windows.Forms.FormWindowState.Normal)
                {
                    if (contatosForm != null && contatosForm.Visible && contatosForm.Width > 10 && contatosForm.Height > 10 && contatosForm.Location.X > 0 && contatosForm.Location.Y > 0)
                    {
                        PerfilDeUsuário perfilEncontrado = this.EncontrarÚltimoPerfilDeUsuárioLogado();
                        perfilEncontrado.PosiçãoInicialDosContatos = contatosForm.Location;
                        perfilEncontrado.TamanhoInicialDosContatos = new Point(contatosForm.Width, contatosForm.Height);
                    }
                }
            }
        }

        public void RegistrarConversas(ConversasForm chatForm)
        {
            lock (LOCK)
            {
                if (chatForm.WindowState == System.Windows.Forms.FormWindowState.Normal)
                {
                    if (chatForm != null && chatForm.Visible && chatForm.Width > 10 && chatForm.Height > 10 && chatForm.Location.X > 0 && chatForm.Location.Y > 0)
                    {
                        PerfilDeUsuário perfilEncontrado = this.EncontrarÚltimoPerfilDeUsuárioLogado();
                        perfilEncontrado.PosiçãoInicialDasConversas = new Point(chatForm.Location.X, chatForm.Location.Y);
                        perfilEncontrado.TamanhoInicialDasConversas = new Point(chatForm.Width, chatForm.Height);
                    }
                }
            }
        }

        public void SetContatosForm(ContatosForm form)
        {
            lock (LOCK)
            {
                PerfilDeUsuário perfilDeUsuário = this.EncontrarÚltimoPerfilDeUsuárioLogado();

                if (perfilDeUsuário.TamanhoInicialDosContatos.HasValue)
                {
                    form.Width = perfilDeUsuário.TamanhoInicialDosContatos.Value.X;
                    form.Height = perfilDeUsuário.TamanhoInicialDosContatos.Value.Y;
                }

                if (perfilDeUsuário.PosiçãoInicialDosContatos.HasValue)
                {
                    form.Location = perfilDeUsuário.PosiçãoInicialDosContatos.Value;
                }
            }
        }

        public void SetChatForm(ConversasForm form)
        {
            lock (LOCK)
            {
                PerfilDeUsuário perfilDeUsuário = this.EncontrarÚltimoPerfilDeUsuárioLogado();

                if (perfilDeUsuário.TamanhoInicialDasConversas.HasValue)
                {
                    form.Width = perfilDeUsuário.TamanhoInicialDasConversas.Value.X;
                    form.Height = perfilDeUsuário.TamanhoInicialDasConversas.Value.Y;
                }

                if (perfilDeUsuário.PosiçãoInicialDasConversas.HasValue)
                {
                    form.Location = perfilDeUsuário.PosiçãoInicialDasConversas.Value;
                }
            }
        }


        public static void CarregarPerfil(string filePath)
        {
            lock (LOCK)
            {
                FILEPATH = filePath;

                if (!File.Exists(FILEPATH))
                {
                    PERFIL = new Perfil();
                    SalvarPerfil();
                }
                else
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Perfil));

                    using (TextReader reader = new StreamReader(FILEPATH, Encoding.UTF8))
                    {
                        PERFIL = serializer.Deserialize(reader) as Perfil;
                    }
                }
            }
        }

        public static void SalvarPerfil()
        {
            lock (LOCK)
            {
                if (File.Exists(FILEPATH))
                {
                    File.Delete(FILEPATH);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(Perfil));

                using (TextWriter writer = new StreamWriter(FILEPATH, false, Encoding.UTF8))
                {
                    serializer.Serialize(writer, PERFIL);
                }
            }
        }
    }

    [Serializable]
    public class PerfilDeUsuário : BindableBase
    {
        public Guid? GuidContato { get; set; }
        public Point? PosiçãoInicialDosContatos { get; set; }
        public Point? PosiçãoInicialDasConversas { get; set; }
        public Point? TamanhoInicialDosContatos { get; set; }
        public Point? TamanhoInicialDasConversas { get; set; }

        public PerfilDeUsuário()
        {
        }
    }
}