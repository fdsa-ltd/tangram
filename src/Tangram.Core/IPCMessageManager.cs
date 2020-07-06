using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tangram.Core;


namespace Tangram.Core.Event
{
    public class IPCMessageManager
    {
        const int WM_COPYDATA = 0x004A;
        const int WM_DRAWCLIPBOARD = 0x308;
        const int WM_CHANGECBCHAIN = 0x30D;
        private static Dictionary<IntPtr, IntPtr> dict = new Dictionary<IntPtr, IntPtr>();

        public static void AddClipboardViewer(IntPtr formHandle)
        {
            if (!dict.ContainsKey(formHandle))
            {
                dict.Add(formHandle, SetClipboardViewer(formHandle));
            }
        }
        public static void PassMessage(IntPtr handle, int msg, IntPtr wParam, IntPtr lParam)
        {
            if (dict.ContainsKey(handle))
            {
                SendMessage(dict[handle], msg, wParam, lParam);
            }
        }
        public static void RemoveClipboardChain(IntPtr handle)
        {
            if (dict.ContainsKey(handle))
            {
                ChangeClipboardChain(handle, dict[handle]);
            }
        }
        #region WindowsAPI
        /// <summary>
        /// 将CWnd加入一个窗口链，每当剪贴板的内容发生变化时，就会通知这些窗口
        /// </summary>
        /// <param name="hWndNewViewer">句柄</param>
        /// <returns>返回剪贴板观察器链中下一个窗口的句柄</returns>
        [DllImport("User32.dll")]
        private static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        /// <summary>
        /// 从剪贴板链中移出的窗口句柄
        /// </summary>
        /// <param name="hWndRemove">从剪贴板链中移出的窗口句柄</param>
        /// <param name="hWndNewNext">hWndRemove的下一个在剪贴板链中的窗口句柄</param>
        /// <returns>如果成功，非零;否则为0。</returns>
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);



        /// <summary>
        /// 将指定的消息发送到一个或多个窗口
        /// </summary>
        /// <param name="hwnd">其窗口程序将接收消息的窗口的句柄</param>
        /// <param name="wMsg">指定被发送的消息</param>
        /// <param name="wParam">指定附加的消息特定信息</param>
        /// <param name="lParam">指定附加的消息特定信息</param>
        /// <returns>消息处理的结果</returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, ref COPYDATASTRUCT lParam);
        #endregion


        static MemoryMappedFile MMF_WRITER = MemoryMappedFile.CreateOrOpen("MEMORY_MAPPED_FILE", 1024);
        static MemoryMappedFile MMF_READER = MemoryMappedFile.OpenExisting("MEMORY_MAPPED_FILE");

        struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }
        private static string Send(IntPtr hWnd, EventMessage message)
        {
            string data = message.ToString();
            int len = Encoding.Default.GetBytes(data).Length;
            COPYDATASTRUCT cds;
            cds.dwData = (IntPtr)100;
            cds.lpData = data;
            cds.cbData = len + 1;
            SendMessage(hWnd, WM_COPYDATA, 0, ref cds);
            switch (message.Type)
            {
                case MessageType.GlobalFind:
                case MessageType.GlobalOpen:
                case MessageType.GlobalMQ:
                case MessageType.GlobalIO:

                    using (var read = MMF_READER.CreateViewStream())
                    {
                        byte[] buffer = new byte[4];
                        read.Read(buffer, 0, buffer.Length);
                        var length = BitConverter.ToInt32(buffer, 0);
                        if (length <= 0)
                        {
                            return string.Empty;
                        }
                        buffer = new byte[length];
                        read.Read(buffer, 0, buffer.Length);
                        return Encoding.UTF8.GetString(buffer);
                    }
                default:
                    return string.Empty;
            }
        }
        public static void WriteData(string data)
        {
            using (var write = MMF_WRITER.CreateViewStream())
            {
                if (string.IsNullOrEmpty(data))
                {
                    write.Write(new byte[4], 0, 4);
                    return;
                }

                var buffer = BitConverter.GetBytes(data.Length);
                write.Write(buffer, 0, buffer.Length);
                buffer = System.Text.Encoding.UTF8.GetBytes(data);
                write.Write(buffer, 0, buffer.Length);
            }
        }

        public static string Send(IntPtr hWnd, MessageType type, string title, params object[] data)
        {
            return Send(hWnd, new EventMessage()
            {
                From = title,
                Type = type,
                Data = data
            });
        }
        public static GlobalMessage GetFormMessage(Message m)
        {
            var data = string.Empty;
            try
            {
                switch (m.Msg)
                {
                    case WM_DRAWCLIPBOARD:
                        //ClipBoardManager.PassMessage(this.Handle, m.Msg, m.WParam, m.LParam);
                        //显示剪贴板中的文本信息
                        if (Clipboard.ContainsText())
                        {
                            data = Clipboard.GetText();
                        }
                        break;
                    case WM_COPYDATA:
                        COPYDATASTRUCT mystr = new COPYDATASTRUCT();
                        Type mytype = mystr.GetType();
                        mystr = (COPYDATASTRUCT)m.GetLParam(mytype);
                        data = mystr.lpData;
                        break;
                }
            }
            catch (Exception ex)
            {
                FileManager.Loger.WriteLog("系统错误", ex);
            }
            if (string.IsNullOrEmpty(data))
            {
                return null;
            }
            return GlobalMessage.Parse(data);
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="name">信号量名称</param>
        /// <param name="initialCount">初始可请求次</param>
        /// <param name="maximumCount">可请求的最大数</param>
        /// <returns></returns>
        public static Semaphore OpenSemaphore(string name, int initialCount = 0, int maximumCount = 1)
        {
            Semaphore semaphore;
            try
            {
                semaphore = Semaphore.OpenExisting(name);
            }
            catch (Exception ex)
            {
                semaphore = new Semaphore(initialCount, maximumCount, name);
                FileManager.Loger.WriteLog("Exception", ex);
            }
            return semaphore;
        }

        public static void ReleaseSemaphore(Semaphore semaphore, int releaseCount = 1)
        {
            try
            {
                semaphore.Release(releaseCount);
            }
            catch (Exception ex)
            {
                FileManager.Loger.WriteLog("Exception", ex);
            }
        }
    }



    //MemoryMappedFile outputMMF = MemoryMappedFile.CreateOrOpen(ConstUtil.MMMM, 1024);
    //Semaphore semaphore = new Semaphore(0, 100, "output");
    //public void sentToForms(IntPtr form, FormMessage message)
    //{
    //    var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
    //    using (var write = outputMMF.CreateViewStream())
    //    {
    //        byte[] buffer = BitConverter.GetBytes(data.Length);
    //        write.Write(buffer, 0, buffer.Length);
    //        write.Write(data, 0, data.Length);
    //    }
    //    semaphore.Release();
    //}

    //public event FormMessageDelegate OnFormEvent;
    //public void startFormEvent()
    //{
    //    Task.Factory.StartNew(() =>
    //    {
    //        while (true)
    //        {
    //            semaphore.WaitOne();

    //            using (var read = outputMMF.CreateViewStream())
    //            {

    //                byte[] buffer = new byte[4];
    //                read.Read(buffer, 0, buffer.Length);
    //                var length = BitConverter.ToInt32(buffer, 0);
    //                buffer = new byte[length];
    //                read.Read(buffer, 0, buffer.Length);
    //                var data = Encoding.UTF8.GetString(buffer);
    //                FormMessage message = JsonConvert.DeserializeObject<FormMessage>(data);
    //                if (this.OnFormEvent != null)
    //                {
    //                    this.OnFormEvent(message.From.ToString(), message.Data.ToString());
    //                }
    //            }
    //        }
    //    });
    //}

    //Semaphore semaphoreRead = new Semaphore(0, 1, "inputput");

    //MemoryMappedFile inputMMF = MemoryMappedFile.CreateOrOpen("Tangram_Main_Input_Memory_Mapped_File", 1024);
    //public void sendToParent(IntPtr from, FormMessage message)
    //{
    //    try
    //    {
    //        Semaphore mutexWrite = new Semaphore(0, 1, "Tangram_Global");
    //        mutexWrite.WaitOne();

    //        using (var write = inputMMF.CreateViewStream())
    //        {
    //            var data = JsonConvert.SerializeObject(message);
    //            var buffer = BitConverter.GetBytes(data.Length);
    //            write.Write(buffer, 0, buffer.Length);
    //            buffer = System.Text.Encoding.UTF8.GetBytes(data);
    //            write.Write(buffer, 0, buffer.Length);
    //        }
    //        semaphoreRead.Release();
    //    }
    //    catch (Exception ex)
    //    {
    //        MessageBox.Show(ex, "唐图");
    //    }

    //}
    //public delegate void FormMessageDelegate(string from, string data);
    //public event FormMessageDelegate OnGlobalEvent;
    //public void startGlobalEvent()
    //{
    //    Semaphore mutex = new Semaphore(0, 1, "Tangram_Global");
    //    mutex.Release();
    //    Task.Factory.StartNew(() =>
    //    {
    //        while (true)
    //        {
    //            semaphoreRead.WaitOne();
    //            using (var read = inputMMF.CreateViewStream())
    //            {
    //                byte[] buffer = new byte[4];
    //                read.Read(buffer, 0, 4);
    //                var length = BitConverter.ToInt32(buffer, 0);
    //                buffer = new byte[length];
    //                read.Read(buffer, 0, buffer.Length);
    //                var data = Encoding.UTF8.GetString(buffer);
    //                if (this.OnGlobalEvent != null)
    //                {
    //                    this.OnGlobalEvent("", data);
    //                }
    //            }
    //            mutex.Release();
    //        }
    //    });
    //}

}