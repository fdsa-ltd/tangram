using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Tangram.Core;

namespace Tangram.Core
{
    public class ScreenManager
    {
        static ScreenManager singlton;
        readonly Dictionary<string, PlugInfo> plugins = new Dictionary<string, PlugInfo>(36, StringComparer.CurrentCultureIgnoreCase);
        readonly Dictionary<string, ITan> tans = new Dictionary<string, ITan>(36, StringComparer.CurrentCultureIgnoreCase);
        readonly Dictionary<string, string> children = new Dictionary<string, string>(36, StringComparer.CurrentCultureIgnoreCase);
        Form mainForm;

        private ScreenManager()
        {

        }
        public void RegisterForm(Form form)
        {
            this.mainForm = form;
            this.ScreenHeight = form.Height;
            this.ScreenWidth = form.Width;
            this.tans.Add(this.mainForm.Text, new DefaultTan(this.mainForm));
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
        int ScreenHeight;
        int ScreenWidth;
        /// <summary>
        /// 打开新窗体
        /// </summary>
        /// <param name="url"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="settings">格式：left=100,top=100,width=100,height=100</param>
        /// <returns></returns>
        public ITan Open(string url, string settings)
        {
            if (tans.Count >= 36)
            {
                //MessageBox.Show("浏览器数据过多", "消息");
                return null;
            }
            //load setttings
            var features = new Features(settings);
            string name = features.Get("name");
            if (string.IsNullOrEmpty(name))
            {
                for (int i = 1; i < 256; i++)
                {
                    var key = "form" + i.ToString();
                    if (!tans.ContainsKey(key))
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

            if (tans.ContainsKey(name))
            {
                //MessageBox.Show("浏览器已经打开", "消息");
                return tans[name];
            }

            try
            {
                features.Set("name", name);
                string type = features.Get("type", "ie");
                if (!this.plugins.ContainsKey(type))
                {
                    //MessageBox.Show("浏览器数据过多", "消息");
                    return null;
                }
                var plugin = this.plugins[type];
                ITan browser;
                switch (plugin.Type)
                {
                    case PlugType.Builtin:
                        browser = new BuiltinTan(features);
                        break;
                    case PlugType.Plugin:
                        browser = new PluginTan(features, this.mainForm);
                        break;
                    case PlugType.Outer:
                        browser = new OuterTan(features);
                        break;
                    default:
                        //MessageBox.Show("浏览器数据过多", "消息");
                        return null;
                }
                browser.OnMessage += FormEventHandler;
                var width = features.GetInt("width", this.ScreenWidth);
                var height = features.GetInt("height", this.ScreenHeight);
                if (width < 10)
                {
                    width = this.ScreenWidth;
                }
                if (height < 10)
                {
                    height = this.ScreenHeight;
                }
                features.Set("width", width);
                features.Set("height", height);

                var left = features.GetInt("left", 0);
                var top = features.GetInt("top", 0);
                features.Set("left", left);
                features.Set("top", top);
                string parent = features.Get("parent", "MainForm");
                features.Set("parent", parent);
                features.Set("fileName", plugin.FileName);
                features.Set("args", string.Join(" ", plugin.Arguments));
                features.Set("type", plugin.Type.ToString());
                features.Set("url", url);
                FileManager.Loger.WritePid(browser.Init());
                tans.Add(name, browser);
                return browser;
            }
            catch (Exception ex)
            {
                FileManager.Loger.WriteLog("error", ex);
                return null;// 
            }
        }
        public string Invoke(FormMessage message)
        {
            if (!tans.ContainsKey(message.To))
            {
                //MessageBox.Show($"窗口{name}不存在", "消息");
                return string.Empty;
            }
            var tan = this.tans[message.To];
            tan.Invoke(message);

            return string.Empty;
        }

        public ITan Find(string title)
        {
            if (tans.ContainsKey(title))
            {
                return tans[title];
            }
            return null;
        }
        private void FormEventHandler(FormMessage message)
        {
            switch (message.Type)
            {
                case FormMessageType.Close:
                    RemoveChildForms(message.From);
                    break;
                default:
                    break;
            }
        }
        private void RemoveChildForms(string form)
        {
            if (tans.ContainsKey(form))
            {
                tans.Remove(form);
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