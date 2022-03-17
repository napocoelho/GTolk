using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using ConnectionManagerDll;

namespace GTolk.Models
{
    public class Manutenção
    {
        public DateTime Inicio { get; set; }

        public static Manutenção CreateNew()
        {
            string sql = "select Inicio from Tlk_Manutenção";

            Conexão.BeginTransaction();
            Conexão.ExecuteNonQuery("insert into Tlk_Manutenção (Inicio) select GetDate()");
            object obj = Conexão.ExecuteScalar(sql);
            Conexão.CommitTransaction();

            Manutenção value = new Manutenção();
            value.Inicio = DateTime.Parse(obj.ToString());
            return value;
        }

        public static int DeleteAll()
        {
            int count = Conexão.ExecuteNonQuery("delete from Tlk_Manutenção");
            return count;
        }

        public static List<Manutenção> GetBy()
        {
            List<Manutenção> list = new List<Manutenção>();
            string sql = "select Inicio from Tlk_Manutenção";
            DataTable tbl = Conexão.ExecuteDataTable(sql);

            foreach (DataRow row in tbl.Rows)
            {
                Manutenção value = new Manutenção();
                value.Inicio = DateTime.Parse(row["Inicio"].ToString());
                list.Add(value);
            }

            return list;
        }


    }
}