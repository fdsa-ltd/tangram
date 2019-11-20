using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace Tangram.Core
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void EventCallback(string form, CallbackType type);

    public delegate object GloalMessageCallback(GlobalMessage message);

    public delegate object FormMessageCallback(FormMessage message);
    public enum CallbackType
    {
        None,
        Error,
        Close,
    }
}
