using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Tangram.Core;
using Tangram.IEBrowser;

namespace Tangram.Master
{
    public partial class FormMain : Form
    {
        private string filePath;
        public FormMain(string filePath)
        {
            this.filePath = filePath;
            InitializeComponent();
        }
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
                    ScreenManager.External.RegisterForm("&" + plugin.Name, plugin);
                }
            }

            ScreenManager.External.RegisterForm("outer", typeof(OuterBrowser));
            ScreenManager.External.RegisterForm("inner", typeof(InnerBrowser));
            if (string.IsNullOrEmpty(this.filePath))
            {
                MessageBox.Show("文件不存在", "消息");
                return;
            }
            if (!File.Exists(this.filePath))
            {
                this.filePath = Path.Combine(Application.StartupPath, this.filePath);
            }
            if (!File.Exists(this.filePath))
            {
                MessageBox.Show("文件不存在", "消息");
                return;
            }
            StringBuilder script = new StringBuilder();
            script.AppendLine("var tg = window.external;");
            if (filePath.ToLower().EndsWith(".tgx"))
            {
                script.AppendLine(FileEncypt.DecryptFile(filePath, "FILE_ENCRYPT_KEY"));
            }
            else
            {
                script.AppendLine(File.ReadAllText(filePath));
            }
            Log.WriteLog("读取文件内容", string.Format("文件内容长度：{0}", script.Length));
            if (script.Length <= 30)
            {
                return;
            }

            ScreenManager.External.Open("about://blank", "name=main");
            ScreenManager.External.Exec("main", script.ToString());
            ScreenManager.External.Invoke("main", "close");
            this.Visible = false;
            this.Hide();
        }
        private void Tsmi_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}