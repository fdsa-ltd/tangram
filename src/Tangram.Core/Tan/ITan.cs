using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace Tangram.Core
{
    public interface ITan
    {
        IntPtr Handle { get; }
        int Init();
        event FormEventCallback OnMessage;
        void Invoke(FormMessage message);
    }

}
