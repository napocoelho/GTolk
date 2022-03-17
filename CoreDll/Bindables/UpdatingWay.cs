using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreDll.Bindables
{
    /// <summary>
    /// The way or direction the action should be performed.
    /// </summary>
    public enum UpdatingWay
    {
        None = 1,
        Both = 2,
        LeftToRight = 3,
        RightToLeft = 4
    }
}
