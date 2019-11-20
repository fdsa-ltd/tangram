using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace Tangram.Core
{
    public interface IFormBrowser
    {
        string Text { get; set; }
        IntPtr Handle { get; }
        void Init(string name, string url, Features features);

        event EventCallback OnMessage;

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="parent"></param>
        void show(string parent);
        /// <summary>
        /// 关闭窗口
        /// </summary>
        void close();
        /// <summary>
        /// 最小化隐藏窗口
        /// </summary>
        void hide();
        /// <summary>
        /// 设置窗口在屏幕上的绝对位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void site(int x, int y);
        /// <summary>
        /// 设置窗口的大小
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void size(int width, int height);
        /// <summary>
        /// 刷新窗口的内容
        /// </summary>
        /// <param name="url">为空时刷新原始url内容</param>
        void refresh(string url);
        /// <summary>
        /// 窗口执行脚本
        /// </summary>
        /// <param name="script"></param>
        void exec(string script);
        /// <summary>
        /// 窗体的状态
        /// </summary>
        /// <param name="status"></param>
        void mode(int status);

    }
}
