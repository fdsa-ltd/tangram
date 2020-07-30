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
        /// <summary>
        /// 控制句柄
        /// </summary>
        IntPtr Handle { get; }
        int InitProcess();
        /// <summary>
        /// 内置
        /// </summary>
        event FormEventCallback OnMessage;
        void Invoke(FormMessage message);
    }

}
