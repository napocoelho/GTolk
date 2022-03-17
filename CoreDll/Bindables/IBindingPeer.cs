using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDll.Bindables
{
    public interface IBindingPeer
    {
        UpdatingWay Way { get; set; }

        LazyValueBag LastChangedValue { get; }


        void UpdatePeers();
    }
}