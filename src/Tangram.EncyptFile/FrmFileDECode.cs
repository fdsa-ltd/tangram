using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tangram.Utility;

namespace Tangram.Manage
{
    public partial class FrmFileDECode : Form
    {
        public FrmFileDECode()
        {
            InitializeComponent();
        }

        private void btnDecode_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tsbRarPath.Text))
            {
                FileEncypt.DecryptFile(tsbRarPath.Text, tsbRarPath.Text.TrimEnd('x'), ConstUtil.FILE_ENCRYPT_KEY);
                MessageBox.Show("解密成功!!!");
            }
            else
            {
                MessageBox.Show("请先选择需要解密的文件");
            }
        }

        private void btnSelect1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "tg加密文件|*.tgx|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                tsbRarPath.Text = openFileDialog.FileName;
            }
            openFileDialog.Dispose();
        }
    }
}
