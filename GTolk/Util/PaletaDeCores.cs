using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace GTolk.Util
{
    public static class PaletaDeCores
    {
        public static Color FundoDeUsuário { get; set; }
        public static Color FonteDeUsuário { get; set; }

        public static Color FundoDeContato { get; set; }
        public static Color FonteDeContato { get; set; }

        public static Color FundoDeStatus { get; set; }
        public static Color FonteDeStatus { get; set; }
        
        public static Color FundoDaListaDeContatosImpares { get; set; }
        public static Color FundoDaListaDeContatosPares{ get; set; }
        public static Color FundoDeContatoSelecionado{ get; set; }

        public static Color StatusOnline { get; set; }
        public static Color StatusOffline { get; set; }
        public static Color StatusOcupado { get; set; }
        public static Color StatusAusente { get; set; }
    }
}
