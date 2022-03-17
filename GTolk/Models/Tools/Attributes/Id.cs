using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTolk.Models.Tools.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Id : Field
    {
        public Id()
            : base()
        {
            this.Name = null;
        }

        public Id(string fieldName)
            : this()
        {
            this.Name = fieldName;
        }
    }
}