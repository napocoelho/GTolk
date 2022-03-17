using System.Drawing ;
using GTolk.Util;

namespace GTolk.Models
{
    public enum StatusDoContato
    {
        Offline = 0,
        Online = 1,
        Ausente = 2,
        Ocupado = 3
    }

    public static class StatusDoContatoHelper
    {
        public static Color ToColor(this StatusDoContato status)
        {
            return xGetStatusColor(status);
        }

        public static Color xGetStatusColor(StatusDoContato status)
        {
            Color cor;

            if (status == StatusDoContato.Online)
            {
                cor = PaletaDeCores.StatusOnline;
            }
            else if (status == StatusDoContato.Offline)
            {
                cor = PaletaDeCores.StatusOffline;
            }
            else if (status == StatusDoContato.Ausente)
            {
                cor = PaletaDeCores.StatusAusente;
            }
            else
            {
                cor = PaletaDeCores.StatusOcupado;
            }

            return cor;
        }
    }
}