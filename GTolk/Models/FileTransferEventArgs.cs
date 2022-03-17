using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTolk.Models
{
    public class FileTransferEventArgs : EventArgs
    {
        public Conversa Conversa { get; set; }
        public string FileName { get; set; }
        public string GuidRemetente { get; set; }

        public FileTransferEventArgs(Conversa conversa, string guidRemetente, string fileName)
        {
            this.GuidRemetente = guidRemetente;
            this.FileName = fileName;
            this.Conversa = conversa;
        }
    }
}