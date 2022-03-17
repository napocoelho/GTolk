using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GTolk.Util
{
    public static class Fontes
    {
        private static string fontePadrão = "Segoe UI"; //Tahoma, Geneva, Verdana, sans-serif";
        public static string FontePadrão { get { return fontePadrão; } set { fontePadrão = value; } }
        
        public static Font Fonte(float tamanho)
        {
            return new Font(new FontFamily(FontePadrão), tamanho);
        }

        public static Font Fonte(float tamanho, FontStyle style)
        {
            return new Font(new FontFamily(FontePadrão), tamanho, style);
        }
    }
}