using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDll.Bindables
{
    public class LazyValue
    {
        public Direction Direction { get; set; }
        public object Value { get; set; }
    }
}