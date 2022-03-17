using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTolk.Models.Tools.Attributes
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NonField : System.Attribute
    {

    }
}