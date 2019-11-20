using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using Tangram.Core;

namespace Tangram.Core
{
    public class ScreenManager
    {
        static ScreenManager singlton;
        public readonly Dictionary<string, PlugInfo> plugins = new Dictionary<string, PlugInfo>(36, StringComparer.CurrentCultureIgnoreCase);
        readonly Dictionary<string, Type> types = new Dictionary<string, Type>(36, StringComparer.CurrentCultureIgnoreCase);
        readonly Dictionary<string, IFormBrowser> browsers = new Dictionary<string, IFormBrowser>(36, StringComparer.CurrentCultureIgnoreCase);
        readonly Dictionary<string, string> children = new Dictionary<string, string>(36, StringComparer.CurrentCultureIgnoreCase);
        readonly WebMessageServer socketsChatServer;
        private ScreenManager()
        {
            this.socketsChatServer = new WebMessageServer();
        }
        public void RegisterForm(string name, Type formType)
        {
            if (types.ContainsKey(name))
            {
                types[name] = formType;
            }
            else
            {
                types.Add(name, formType);
            }
        }

        public void RegisterForm(string name, PlugInfo formType)
        {
            if (plugins.ContainsKey(name))
            {
                plugins[name] = formType;
            }
            else
            {
                plugins.Add(name, formType);
            }
        }

        public static ScreenManager External
        {
            get
            {
                if (singlton == null)
                {
                    singlton = new ScreenManager();
                }
                return singlton;
            }
        }

        public void SendToAsync(string payload, string name)
        {
            this.socketsChatServer.SendToAsync(payload, name);
        }
        public int ScreenHeight { get; private set; }
        public int ScreenWidth { get; private set; }

        /// <summary>
        /// 打开新窗体
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="features">格式：left=100,top=100,width=100,height=100</param>
        /// <returns></returns>
        public IFormBrowser Open(string url, string features)
        {
            if (browsers.Count >= 36)
            {
                //MessageBox.Show("浏览器数据过多", "消息");
                return null;
            }
            //load setttings
            var dict = new Features(features);
            string name = dict.Get("name");
            if (browsers.ContainsKey(name))
            {
                //MessageBox.Show("浏览器已经打开", "消息");
                return browsers[name];
            }

            if (string.IsNullOrEmpty(name))
            {
                for (int i = browsers.Count; i < 256; i++)
                {
                    var key = "form" + i.ToString();
                    if (!browsers.ContainsKey(key))
                    {
                        name = key;
                        break;
                    }
                }
            }
            if (string.IsNullOrEmpty(name))
            {
                //MessageBox.Show("浏览器数据过多", "消息");
                return null;
            }
            try
            {
                IFormBrowser browser;
                string type = dict.Get("type", "inner");
                if (!plugins.ContainsKey(type))
                {
                    browser = (IFormBrowser)Activator.CreateInstance(types[type]);
                }
                else
                {

                    browser = (IFormBrowser)Activator.CreateInstance(types["outer"]);
                }
                browser.OnMessage += FormEventHandler;
                browser.Init (name, url, dict);
              
                var width = dict.GetInt("width", 0);
                var height = dict.GetInt("height", 0);
                if (width < 10)
                {
                    width = this.ScreenWidth;
                }
                if (height < 10)
                {
                    height = this.ScreenHeight;
                }
                browser.size(width, height);
                var left = dict.GetInt("left", 0);
                var top = dict.GetInt("top", 0);
                browser.site(left, top);
                string parent = dict.Get("parent", "MainForm");
                browser.show(parent);
                browsers.Add(name, browser);
                return browser;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "出错");
                //Log.WriteLog(string.Format("打开新窗体出错，具体如下：url：{0}\r\n name：{1}\r\n type：{2}", url, name, type), ex.ToString());
                return null;// 
            }
        }
        public string Invoke(string name, string method, params string[] args)
        {
            if (!browsers.ContainsKey(name))
            {
                //MessageBox.Show($"窗口{name}不存在", "消息");
                return string.Empty;
            }

            switch (method)
            {
                case "show":
                    string parent = args[0].ToString();
                    if (children.ContainsKey(parent))
                    {
                        if (!children[parent].Contains(name))
                        {
                            children[parent] = string.Join(";", children[parent], name);
                        }
                    }
                    else
                    {
                        children.Add(parent, name);
                    }
                    browsers[name].show(parent);
                    break;
                case "close":
                    browsers[name].close();
                    break;
                case "hide":
                    browsers[name].hide();
                    break;
                case "position":
                    browsers[name].site(StringUtil.ParseByDefault(args[0].ToString(), 0), StringUtil.ParseByDefault(args[1].ToString(), 0));
                    break;
                case "size":
                    browsers[name].size(StringUtil.ParseByDefault(args[0].ToString(), 600), StringUtil.ParseByDefault(args[1].ToString(), 600));
                    break;
                case "refresh":
                    browsers[name].refresh(args[0].ToString());
                    break;
                case "mode":
                    browsers[name].mode(StringUtil.ParseByDefault(args[0].ToString(), 0));
                    break;
                default:
                    break;
            }

            return string.Empty;
        }
        public string Exec(string name, string script)
        {
            if (!browsers.ContainsKey(name))
            {
                return string.Format("窗口{0}不存在", name);
            }
            browsers[name].exec(script);
            return string.Empty;
        }
        public IFormBrowser Find(string title)
        {
            if (browsers.ContainsKey(title))
            {
                return browsers[title];
            }
            return null;
        }
        private void FormEventHandler(string form, CallbackType type)
        {
            switch (type)
            {
                case CallbackType.Close:
                    RemoveChildForms(form);
                    break;
                default:
                    break;
            }
        }
        private void RemoveChildForms(string form)
        {
            if (browsers.ContainsKey(form))
            {
                browsers.Remove(form);
            }
            if (children.ContainsKey(form))
            {
                foreach (var item in children[form].Split(';'))
                {
                    RemoveChildForms(item);
                }
                children.Remove(form);
            }
        }
    }
}