using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices; 
using System.Text;

namespace Tangram.Core
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void EventCallback(EventMessage message);


    public delegate void FormEventCallback(FormMessage message);

    public delegate void GlobalEventCallback(GlobalMessage message);
     
}
