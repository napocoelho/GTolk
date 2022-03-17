using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

using ConnectionManagerDll;
using GTolk.Util;
using CoreDll;
using CoreDll.Bindables;
using CoreDll.Cryptography;
using CoreDll.Reflection;

namespace GTolk.Models
{
    public class Contato : BindableBase
    {
        public Guid Guid { get { return base.GetSync<Guid>(); } set { base.SetSync(value); } }

        public string Email { get { return base.GetSync<string>(); } set { base.SetSync(value); } }
        public string Senha { get { return base.GetSync<string>(); } set { base.SetSync(value); } }
        public bool AutoLogon { get { return base.GetSync<bool>(); } set { base.SetSync(value); } }
        public string Apelido { get { return base.GetSync<string>(); } set { base.SetSync(value); } }
        public string Descrição { get { return base.GetSync<string>(); } set { base.SetSync(value); } }
        public StatusDoContato Status { get { return base.GetSync<StatusDoContato>(); } set { base.SetSync(value); } }
        public bool Inativo { get { return base.GetSync<bool>(); } set { base.SetSync(value); } }
        public DateTime CriadoEm { get { return base.GetSync<DateTime>(); } set { base.SetSync(value); } }
        public DateTime VerificouEm { get { return base.GetSync<DateTime>(); } set { base.SetSync(value); } }
        public Image Imagem { get { return base.GetSync<Image>(); } set { base.SetSync(value); } }
        public long Timestamp { get { return base.GetSync<long>(); } set { base.SetSync(value); } }
        public string Ip { get { return base.GetSync<string>(); } set { base.SetSync(value); } }

        private byte[] InformacaoCodificada { get { return base.Get<byte[]>(); } set { base.Set(value); } }

        public bool IsAdmin { get { return (this.Email == "napocoelho@gmail.com"); } }
        
        
        public Contato()
        {
            this.Guid = Guid.NewGuid();
            this.InformacaoCodificada = null;
            this.Timestamp = 0;
            this.Descrição = string.Empty;
            this.AutoLogon = false;

            //this.Imagem = Image.FromFile(@"C:\Whats.ico");
            //this.Imagem = new System.Windows.Forms.DataGridViewImageColumn();
            //this.Imagem = Image.FromFile(@"C:\Whats.ico");
            //this.Imagem = Image.FromFile(@"C:\Users\napoleao.junio\Pictures\wtf_3b7f15_3047722.gif");
            //this.Imagem = Image.FromFile(@"sem_imagem.png");
            //
        }

        private ContatoCodificado Decodificar()
        {
            byte[] decodedBytes = CryptographyHelper.DecodeBytes(this.InformacaoCodificada, "palavra_ultra_secreta");
            ContatoCodificado contatoPart = SerializerHelper.DeserializeBytes<ContatoCodificado>(decodedBytes);

            if (this.Guid != contatoPart.Guid || this.Email != contatoPart.Email)
            {
                throw new Exception("Houve uma tentativa de burlar o sistema de segurança! As informações do usuário " + this.Apelido + " foram alteradas manualmente.");
            }

            this.Guid = contatoPart.Guid;
            this.Email = contatoPart.Email;
            this.Senha = contatoPart.Senha;
            this.AutoLogon = contatoPart.AutoLogon;
            

            return contatoPart;
        }


        private byte[] Codificar()
        {
            ContatoCodificado contatoPart = new ContatoCodificado();
            contatoPart.Guid = this.Guid;
            contatoPart.Email = this.Email;
            contatoPart.Senha = this.Senha;
            contatoPart.AutoLogon = this.AutoLogon;
            
            

            byte[] coded = SerializerHelper.SerializeBytes(contatoPart);
            this.InformacaoCodificada  = CryptographyHelper.CodeBytes(coded, "palavra_ultra_secreta");

            return this.InformacaoCodificada;
        }

        public static bool SetStatus(Contato usuário, StatusDoContato status)
        {
            string sql = string.Format("update Tlk_Contatos set Status = {0} where Guid = '{1}'", status.GetHashCode(), usuário.Guid.ToString());

            if (Conexão.ExecuteNonQuery(sql) > 0)
            {
                usuário.Status = status;
                return true;
            }
            return false;
        }

        public static bool SetVerificadoEm(Contato usuário)
        {
            string sql = string.Format("update Tlk_Contatos set VerificouEm = GetDate() where Guid = '{1}'", usuário.Guid.ToString());

            if (Conexão.ExecuteNonQuery(sql) > 0)
            {
                return true;
            }
            return false;
        }

        public static List<Contato> GetContatos(Contato usuário)
        {
            string where = string.Format("Guid NOT IN ( '{0}' )", usuário.Guid);
            return Contato.GetBy(where);
        }

        public static List<Contato> GetBy(string whereExpression = null)
        {
            List<Contato> lista = new List<Contato>();

            whereExpression = (whereExpression == null ? string.Empty : " where " + whereExpression);
            string sql = "select Guid, Email, InformacaoCodificada, Imagem, Apelido, Descricao, Status, Inativo, CriadoEm, VerificouEm, Timestamp from Tlk_Contatos" + whereExpression;
            DataTable table = Conexão.ExecuteDataTable(sql);

            foreach (DataRow row in table.Rows)
            {
                Contato item = new Contato();
                item.Guid = Guid.Parse(row["Guid"].ToString());
                item.Email = row["Email"].ToString();
                item.Apelido = row["Apelido"].ToString();

                item.Status = row["Status"].ToEnum<StatusDoContato>();

                item.Inativo = (bool)row["Inativo"];
                item.CriadoEm = DateTime.Parse(row["CriadoEm"].ToString());
                item.VerificouEm = DateTime.Parse(row["VerificouEm"].ToString());
                item.Timestamp = BitConverter.ToInt64((byte[])row["Timestamp"], 0);

                item.InformacaoCodificada = Convert.FromBase64String(row["InformacaoCodificada"].ToString());

                item.Imagem = Imagens.ByteArrayToImage(Convert.FromBase64String(row["Imagem"].ToString()));
                
                if (item.Imagem == null)
                {
                    item.Imagem = Image.FromFile("sem_imagem.png");
                }

                item.Descrição = row["Descricao"].ToString();
                
                item.Decodificar();
                lista.Add(item);
            }

            return lista;
        }

        public static Contato GetByGuid(Guid guid)
        {
            return Contato.GetBy("Guid = '{0}'".FormatTo(guid.ToString())).FirstOrDefault();
        }

        public static bool Save(Contato value)
        {
            string sql = null;
            
            value.Codificar();

            if (value.Imagem == null)
            {
                value.Imagem = Image.FromFile(@"sem_imagem.png");
            }

            sql = " update Tlk_Contatos set Guid = '{0}', Email = '{1}', InformacaoCodificada = '{2}', Apelido = '{3}', Status = {4}, Inativo = {5}, Imagem = '{6}', Descricao = '{7}' where Guid = '{0}'";
            sql = sql.FormatTo(value.Guid, value.Email, Convert.ToBase64String(value.InformacaoCodificada), value.Apelido.Trim(), value.Status.GetHashCode(),
                                (value.Inativo ? 1 : 0), Convert.ToBase64String(Imagens.ImageToByteArray(value.Imagem)), value.Descrição.Trim());
            int count = Conexão.ExecuteNonQuery(sql);

            if (count == 0)
            {
                sql = " insert into Tlk_Contatos ( Guid, Email, InformacaoCodificada, Apelido, Status, Inativo, Imagem, Descricao ) values ( '{0}', '{1}', '{2}', '{3}', {4}, {5}, '{6}', '{7}' ) ";
                sql = sql.FormatTo(value.Guid, value.Email, Convert.ToBase64String(value.InformacaoCodificada), value.Apelido.Trim(), value.Status.GetHashCode(),
                                    (value.Inativo ? 1 : 0), Convert.ToBase64String(Imagens.ImageToByteArray(value.Imagem)), value.Descrição.Trim());
                count = Conexão.ExecuteNonQuery(sql);
            }
            
            return (count > 0);
        }

        public override int GetHashCode()
        {
            return this.Guid.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;

            Contato another = obj as Contato;

            if (another == null)
                return false;

            if (this.Guid.Equals(another.Guid))
                return true;

            return false;
        }
    }

    [Serializable]
    public class ContatoCodificado
    {
        public Guid Guid { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public bool AutoLogon { get; set; }

        public ContatoCodificado() 
        {
            this.Email = "";
            this.Senha = "";
            this.AutoLogon = false;
        }
    }
}