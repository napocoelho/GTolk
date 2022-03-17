using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace CoreDll.Bindables
{
    public static class DataBindingRegistry
    {
        public static Dictionary<Form, DataBindingContext> Contexts = new Dictionary<Form, DataBindingContext>();

        public static void Register(Form form, IBindingPeer bind)
        {
            if (form != null && Contexts.ContainsKey(form))
            {
                DataBindingContext context = Contexts[form];
                context.Bindings.Add(bind);
            }
        }

        public static void UpdatePeers()
        {
            foreach (KeyValuePair<Form, DataBindingContext> pair in Contexts)
            {
                foreach (IBindingPeer peer in pair.Value.Bindings)
                {
                    peer.UpdatePeers();
                }
            }
        }
    }
}
