using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using Tangram.Core;

namespace Tangram.Master
{
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
        }
        private void Form_Load(object sender, EventArgs e)
        {
            txtMachineCode.Text = SoftRegister.GetMachineCode();
            txtMachineCode.ReadOnly = true;
        }

        private void btnRegisterSoft_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtRegisterCode.Text))
            {
                string SerialNumber = txtRegisterCode.Text;
                SoftRegister.WriteSetting("SerialNumber", SerialNumber);
                if (SoftRegister.IsRegister())
                {
                    MessageBox.Show("注册成功，请重启软件！");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("请输入正确的注册码！");
                }

            }
            else
            {
                MessageBox.Show("请填写注册码，如果没有请联系管理员获取！");
            }

        }


    }
}
