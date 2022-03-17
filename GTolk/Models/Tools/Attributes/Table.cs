using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTolk.Models.Tools.Attributes
{
    [System.AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class Table : System.Attribute
    {
        /// <summary>
        /// Nome da tabela do banco de dados
        /// </summary>
        public string Name { get; set; }

        public Table()
        {
            this.Name = null;
        }

        public Table(string tableName)
        {
            this.Name = tableName;
        }
    }
}