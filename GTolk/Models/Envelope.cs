using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;

using ConnectionManagerDll;
using CoreDll;
using CoreDll.Bindables;
using CoreDll.Cryptography;
using CoreDll.Reflection;


namespace GTolk.Models
{
    public class Envelope : BindableBase 
    {
        private static object LOCK_GLOBAL = new object();

        public Guid Guid { get { return base.Get<Guid>(); } set { base.Set(value); } }
        public byte[] Mensagem { get { return base.Get<byte[]>(); } set { base.Set(value); } }
        public DateTime? EnviadoEm { get { return base.Get<DateTime?>(); } set { base.Set(value); } }
        public Guid GuidDestinatário { get { return base.Get<Guid>(); } set { base.Set(value); } }
        public Guid GuidRemetente { get { return base.Get<Guid>(); } set { base.Set(value); } }

        public bool RemetenteLeu { get { return base.Get<bool>(); } set { base.Set(value); } }
        public bool DestinatarioLeu { get { return base.Get<bool>(); } set { base.Set(value); } }
        

        public Envelope()
        { }

        private Mensagem toMensagem = null;
        public Mensagem ToMensagem()
        {
            if (this.toMensagem == null)
            {
                byte[] decodedBytes = CryptographyHelper.DecodeBytes(this.Mensagem, "palavra_ultra_secreta");
                this.toMensagem = SerializerHelper.DeserializeBytes<Mensagem>(decodedBytes);
                this.toMensagem.EnviadaEm = this.EnviadoEm;

                if (!this.Guid.Equals(this.toMensagem.Guid))
                {
                    this.toMensagem = null;
                    throw new Exception("A mensagem é inconsistente e não confere com o envelope!");
                }

                if (this.GuidDestinatário != this.toMensagem.GuidDestinatário
                    || this.GuidRemetente != this.toMensagem.GuidRemetente)
                {
                    this.toMensagem = null;
                    throw new Exception("Houve uma tentativa de burlar o sistema de segurança! Um usuário não autorizado tentou acessar a mensagem de outro(s) usuário(s).");
                }
            }

            return this.toMensagem;
        }

        public static Envelope Send(Mensagem mensagem)
        {
            lock (LOCK_GLOBAL)
            {
                Envelope envelope = null;

                envelope = new Envelope();
                envelope.Guid = mensagem.Guid;

                byte[] mensagemCodificada = SerializerHelper.SerializeBytes(mensagem);
                mensagemCodificada = CryptographyHelper.CodeBytes(mensagemCodificada, "palavra_ultra_secreta");

                envelope.Mensagem = mensagemCodificada;
                envelope.GuidRemetente = mensagem.GuidRemetente;
                envelope.GuidDestinatário = mensagem.GuidDestinatário;

                //byte[] decByte3 = Convert.FromBase64String(s3);
                //string text = Convert.ToBase64String(sequenceToCode);


                string blob = Convert.ToBase64String(envelope.Mensagem);
                string sqlSent = string.Format("insert into Tlk_Envelopes ( Guid, Mensagem, GuidRemetente, GuidDestinatario ) values ( '{0}', '{1}', '{2}', '{3}' );", envelope.Guid.ToString(), blob, envelope.GuidRemetente, envelope.GuidDestinatário);
                string sqlRequest = string.Format("select EnviadoEm from Tlk_Envelopes where Guid = '{0}';", envelope.Guid.ToString());

                DateTime dt = DateTime.Parse(Conexão.ExecuteScalar(sqlSent + sqlRequest).ToString());
                envelope.EnviadoEm = dt;

                return envelope;
            }
        }

        public static List<Envelope> ReceiveNews(Contato usuárioLogado)
        {
            lock (LOCK_GLOBAL)
            {
                
                //RemetenteLeu = 1 AND GuidDestinatario = 1 
                string sqlWhere = string.Format("(GuidDestinatario = '{0}' AND DestinatarioLeu = 0) OR (GuidRemetente = '{0}' AND RemetenteLeu = 0) ", usuárioLogado.Guid);
                List<Envelope> novosEnvelopes = Envelope.GetBy(sqlWhere);

                foreach (Envelope envelope in novosEnvelopes.ToList())
                {
                    try
                    {
                        Mensagem mensagem = envelope.ToMensagem();

                        if (envelope.GuidDestinatário != mensagem.GuidDestinatário || envelope.GuidRemetente != mensagem.GuidRemetente)
                        {
                            Envelope.RemoveByGuid(envelope.Guid);
                            novosEnvelopes.Remove(envelope);
                        }
                    }
                    catch
                    {
                        Envelope.RemoveByGuid(envelope.Guid);
                        novosEnvelopes.Remove(envelope);
                    }
                }

                if (novosEnvelopes.Count > 0)
                {
                    {
                        List<Guid> lidosPeloDestinatário = novosEnvelopes.Where(x => x.GuidDestinatário == usuárioLogado.Guid).Select(x => x.Guid).ToList<Guid>();
                        if (lidosPeloDestinatário.Count > 0)
                        {
                            string inSql = lidosPeloDestinatário.JoinWith(", ", x => "'" + x.ToString() + "'");
                            string sql = string.Format("update Tlk_Envelopes set DestinatarioLeu = 1 where Guid IN ( {0} )", inSql);
                            Conexão.ExecuteNonQuery(sql);
                        }
                    }

                    {
                        List<Guid> lidosPeloRemetente = novosEnvelopes.Where(x => x.GuidRemetente == usuárioLogado.Guid).Select(x => x.Guid).ToList<Guid>();
                        if (lidosPeloRemetente.Count > 0)
                        {
                            string inSql = lidosPeloRemetente.JoinWith(", ", x => "'" + x.ToString() + "'");
                            string sql = string.Format("update Tlk_Envelopes set RemetenteLeu = 1 where Guid IN ( {0} )", inSql);
                            Conexão.ExecuteNonQuery(sql);
                        }
                    }
                }
                return novosEnvelopes;
            }
        }

        public static List<Envelope> ReceiveOlds(Contato usuárioLogado, Contato contato)
        {
            lock (LOCK_GLOBAL)
            {
                string sqlWhere = string.Format("(RemetenteLeu = 1 OR DestinatarioLeu = 1) AND GuidDestinatario IN ('{0}', '{1}') AND GuidRemetente IN ('{0}', '{1}')", usuárioLogado.Guid, contato.Guid);
                
                List<Envelope> lista = Envelope.GetBy(sqlWhere);

                foreach (Envelope item in lista.ToList())
                {
                    if (item.GuidDestinatário != item.ToMensagem().GuidDestinatário
                        || item.GuidRemetente != item.ToMensagem().GuidRemetente)
                    {
                        lista.Remove(item);
                    }
                }
                return lista;
            }
        }

        public static List<Envelope> GetBy(string whereExpression = null)
        {
            List<Envelope> lista = new List<Envelope>();

            whereExpression = (whereExpression == null ? string.Empty : " where " + whereExpression);
            string sql = "select Guid, Mensagem, EnviadoEm, GuidRemetente, GuidDestinatario, RemetenteLeu, DestinatarioLeu from Tlk_Envelopes" + whereExpression + " order by EnviadoEm";
            DataTable table = Conexão.ExecuteDataTable(sql);

            foreach (DataRow row in table.Rows)
            {
                Envelope item = new Envelope();
                //item.Id = Guid.Parse(row["Id"].ToString());
                item.Guid = Guid.Parse(row["Guid"].ToString());

                item.Mensagem = Convert.FromBase64String(row["Mensagem"].ToString());
                
                item.EnviadoEm = DateTime.Parse(row["EnviadoEm"].ToString());
                item.GuidRemetente = Guid.Parse(row["GuidRemetente"].ToString());
                item.GuidDestinatário = Guid.Parse(row["GuidDestinatario"].ToString());
                item.RemetenteLeu = (bool)row["RemetenteLeu"];
                item.DestinatarioLeu = (bool)row["DestinatarioLeu"];
                lista.Add(item);
            }

            return lista;
        }

        public static int RemoveBy(string whereExpression = null)
        {
            whereExpression = (whereExpression == null ? string.Empty : " where " + whereExpression);

            string sql = "delete from Tlk_Envelopes" + whereExpression;
            int count = Conexão.ExecuteNonQuery(sql);
            return count;
        }

        public static int RemoveByGuid(params Guid[] guidArgs)
        {
            return RemoveByGuid(guidArgs.ToList());
        }

        public static int RemoveByGuid(List< Guid> guids)
        {
            string join = guids.JoinWith(", ", x => "'" + x + "'");
            string where = string.Format("Guid IN ( {0} )", join);
            return RemoveBy(where);
        }

        public static Envelope GetById(Guid id)
        {
            string where = string.Format("Guid = '{0}'", id.ToString());
            Envelope item = Envelope.GetBy(where).FirstOrDefault();
            return item;
        }

        public override int GetHashCode()
        {
            return this.Guid.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;

            Envelope another = obj as Envelope;

            if (another == null)
                return false;

            if (this.Guid.Equals(another.Guid))
                return true;

            return false;
        }
    }
}