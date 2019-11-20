using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Tangram.Core
{
    public class ClipBoardManager
    {
        public const int WM_DRAWCLIPBOARD = 0x308;
        public const int WM_CHANGECBCHAIN = 0x30D;
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


        #endregion
    }
}
