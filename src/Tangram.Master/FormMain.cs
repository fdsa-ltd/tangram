using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Tangram.Core;
using Tangram.Core.Event;


namespace Tangram.Master
{
    public partial class FormMain : Form
    {
        private string filePath;
        public FormMain(string filePath)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.OnMessage = new GlobalEventCallback(this.Global_EventCallback);
            this.filePath = filePath;
            InitializeComponent();
        }

        private readonly GlobalEventCallback OnMessage;
        private void MainForm_Load(object sender, EventArgs e)
        {
            var pluginPath = Path.Combine(Application.StartupPath, "Plugins");
            if (!Directory.Exists(pluginPath))
            {
                Directory.CreateDirectory(pluginPath);
            }
            var staticPath = Path.Combine(Application.StartupPath, "static");
            if (!Directory.Exists(staticPath))
            {
                Directory.CreateDirectory(staticPath);
            }
            foreach (var item in Directory.GetFiles(pluginPath, "*.json"))
            {
                var plugin = PlugInfo.GetPlugInfo(File.ReadAllText(item));
                if (plugin != null)
                {
                    if (!File.Exists(plugin.FileName))
                    {
                        plugin.FileName = Path.Combine(Application.StartupPath, plugin.FileName);
                    }
                    ScreenManager.External.RegisterForm(plugin.Name, plugin);
                }
            }
            ScreenManager.External.RegisterForm(this);
            IPCMessageManager.AddClipboardViewer(this.Handle);
            RPCMessageManager.External.RegisterCallback(this.OnMessage);
            if (string.IsNullOrEmpty(this.filePath))
            {
                this.filePath = Path.Combine(Application.StartupPath, "app.tg");
            }
            if (!File.Exists(this.filePath))
            {
                this.filePath = Path.Combine(Application.StartupPath, this.filePath);
            }
            if (!File.Exists(this.filePath))
            {
                MessageBox.Show("文件不存在", "消息");
                Application.Exit();
                return;
            }

            foreach (var item in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrEmpty(item))
                {
                    continue;
                }

                if (item.StartsWith("//"))
                {
                    continue;
                }
                var value = item.Trim();
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }
                var i = value.IndexOf("->");
                var url = value.Substring(0, i).Trim();
                var settings = value.Substring(i + 2).Trim();
                ScreenManager.External.Open(url, settings);
            }
        }
        private void Tsmi_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Global_EventCallback(GlobalMessage message)
        {
            switch (message.Type)
            {

                case GlobalMessageType.Invoke:
                    var type = Enum.Parse<FormMessageType>(message.Data.GetString(0), true);
                    FormMessage formMessage = new FormMessage()
                    {
                        From = message.From,
                        To = message.To,
                        Type = type,
                        Data = message.Data.GetStrings(1).ToArray(),
                    };
                    ScreenManager.External.Invoke(formMessage);
                    break;
                case GlobalMessageType.Open:
                    ScreenManager.External.Open(message.Data.GetString(0), message.Data.GetString(1));
                    break;

                case GlobalMessageType.Find:
                    ScreenManager.External.Find(message.Data.GetString(0));
                    break;

                default:
                    break;
            }



        }
        protected override void WndProc(ref Message m)
        {
            var message = IPCMessageManager.GetFormMessage(m);

            if (message != null)
            {
                var msg = new GlobalMessage() { From = message.Data.GetString(0), To = message.Data.GetString(1), Data = message.Data.GetStrings(2).ToArray() };
                this.OnMessage(msg);
            }
            base.WndProc(ref m);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}