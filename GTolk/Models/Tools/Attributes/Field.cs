using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTolk.Models.Tools.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class Field : System.Attribute
    {
        /// <summary>
        /// Nome do campo do banco de dados
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// O campo do banco de dados aceita nulo
        /// </summary>
        public bool IsNull { get; set; }

        /// <summary>
        /// Tipo do campo do banco de dados
        /// </summary>
        public string DbType { get; set; }
        
        /// <summary>
        /// Largura do campo (seja texto ou numérico)
        /// </summary>
        public int Precision { get; set; }

        /// <summary>
        /// Largura da parte decimal dos campos numéricos
        /// </summary>
        public int Scale { get; set; }

        /// <summary>
        /// Campo conversor onde se obtém uma entrada e ele gerará uma saída personalizada
        /// </summary>
        public Func<object, string> ConvertToSql { get; set; }



        public Field()
        {
            this.Name = null;
            this.DbType = null;
            this.Precision = 0;
            this.Scale = 0;
            this.IsNull = true;

            this.ConvertToSql = null;
        }

        public Field(string fieldName, string dbType = null)
            : this()
        {
            this.Name = fieldName;
            this.DbType = dbType;
        }
    }
}