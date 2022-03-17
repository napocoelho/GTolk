using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTolk.Models;

namespace GTolk.Util
{
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public class JavaScriptInterfaces
    {
        public Conversa Conversa { get; private set; }

        public event FileTransferEventHandler SaveAsAction;
        public event FileTransferEventHandler OpenAction;

        public JavaScriptInterfaces(Conversa conversa)
        {
            this.Conversa = conversa;
        }

        public void SaveAs(string guidRemetente, string fileName)
        {
            this.OnSaveAsAction(guidRemetente, fileName);
        }

        private void OnSaveAsAction(string guidRemetente, string fileName)
        {
            if (this.SaveAsAction != null)
            {
                this.SaveAsAction(new FileTransferEventArgs(this.Conversa, guidRemetente, fileName));
            }
        }


        public void Open(string guidRemetente, string fileName)
        {
            this.OnOpen(guidRemetente, fileName);
        }

        private void OnOpen(string guidRemetente, string fileName)
        {
            if (this.OpenAction != null)
            {
                this.OpenAction(new FileTransferEventArgs(this.Conversa, guidRemetente, fileName));
            }
        }


    }
}