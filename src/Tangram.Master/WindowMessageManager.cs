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
using Tangram.Core;

namespace Tangram.Master
{
    public class WindowMessageManager
    {
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, ref COPYDATASTRUCT lParam);
        static MemoryMappedFile MMF_WRITER = MemoryMappedFile.CreateOrOpen("MEMORY_MAPPED_FILE", 1024);
        static MemoryMappedFile MMF_READER = MemoryMappedFile.OpenExisting("MEMORY_MAPPED_FILE");
        const int WM_COPYDATA = 0x004A;
        struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }
        private static string Send(IntPtr hWnd, WindowMessage message)
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
                case WindowMessageType.GlobalFind:
                case WindowMessageType.GlobalOpen:
                case WindowMessageType.GlobalMQ:
                case WindowMessageType.GlobalIO:

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

        public static string Send(IntPtr hWnd, WindowMessageType type, string title, params object[] data)
        {
            return Send(hWnd, new WindowMessage()
            {
                To = title,
                Type = type,
                Data = data
            });
        }
        public static WindowMessage GetFormMessage(Message m)
        {
            if (m == null)
            {
                return null;
            }
            if (WM_COPYDATA != m.Msg)
            {
                return null;
            }
            try
            {
                COPYDATASTRUCT mystr = new COPYDATASTRUCT();
                Type mytype = mystr.GetType();
                mystr = (COPYDATASTRUCT)m.GetLParam(mytype);
                var data = mystr.lpData;
                return WindowMessage.Parse(data);
            }
            catch (Exception ex)
            {
                Log.WriteLog("系统错误", ex.Message);
                return null;
            }
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
                Log.WriteLog("Exception", ex.Message);
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
                Log.WriteLog("Exception", ex.Message);
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
    //        MessageBox.Show(ex.Message, "唐图");
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