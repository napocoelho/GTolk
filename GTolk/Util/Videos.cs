using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace GTolk.Util
{
    public static class Videos
    {
        public static bool IsVideoFile(string fileName)
        {
            string ext = System.IO.Path.GetExtension(fileName).Replace(".", "");

            FieldInfo[] fields = typeof(NReco.VideoConverter.Format).GetFields();

            foreach (FieldInfo field in fields)
            {
                if (ext.ToLower() == field.Name.ToLower())
                {
                    return true;
                }
            }

            return false;
        }
    }
}