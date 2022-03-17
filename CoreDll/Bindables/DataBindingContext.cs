using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoreDll.Bindables
{
    public class DataBindingContext
    {
        public List<IBindingPeer> Bindings { get; private set; }
        public Form FormBound { get; private set; }

        public DataBindingContext()
        {
            this.Bindings = new List<IBindingPeer>();
            this.FormBound = null;
        }

        public DataBindingContext(Form bindToForm)
        {
            this.FormBound = bindToForm;
            this.Bindings = new List<IBindingPeer>();

            if (this.FormBound != null)
            {
                DataBindingRegistry.Contexts[this.FormBound] = this;
            }
        }

        public void UpdatePeers()
        {
            foreach (IBindingPeer peer in this.Bindings)
            {
                peer.UpdatePeers();
            }
        }
    }
}
