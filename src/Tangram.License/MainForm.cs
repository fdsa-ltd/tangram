using Tangram.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tangram.Manage
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnGenerateRegisterSoft_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMachineCode.Text))
            {
                MessageBox.Show("请把信息填写完整！");
                return;
            }
            var hours = this.numericUpDown1.Value * 24;
            txtRegisterCode.Text = SoftRegister.CreatSerialNumber(txtMachineCode.Text, (int)hours);
            Clipboard.SetText(this.txtRegisterCode.Text);
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMachineCode.Text))
            {
                MessageBox.Show("请把信息填写完整！");
                return;
            }
            var hours = 30 * 24;
            txtRegisterCode.Text = SoftRegister.CreatSerialNumber(txtMachineCode.Text, (int)hours);
            Clipboard.SetText(this.txtRegisterCode.Text);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMachineCode.Text))
            {
                MessageBox.Show("请把信息填写完整！");
                return;
            }
            var hours = 90 * 24;
            txtRegisterCode.Text = SoftRegister.CreatSerialNumber(txtMachineCode.Text, (int)hours);
            Clipboard.SetText(this.txtRegisterCode.Text);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMachineCode.Text))
            {
                MessageBox.Show("请把信息填写完整！");
                return;
            }
            var hours = 30 * 6 * 24;
            txtRegisterCode.Text = SoftRegister.CreatSerialNumber(txtMachineCode.Text, (int)hours);
            Clipboard.SetText(this.txtRegisterCode.Text);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMachineCode.Text))
            {
                MessageBox.Show("请把信息填写完整！");
                return;
            }
            var hours = 30 * 12 * 24;
            txtRegisterCode.Text = SoftRegister.CreatSerialNumber(txtMachineCode.Text, (int)hours);
            Clipboard.SetText(this.txtRegisterCode.Text);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMachineCode.Text))
            {
                MessageBox.Show("请把信息填写完整！");
                return;
            }
            var hours = 30 * 24 * 24;
            txtRegisterCode.Text = SoftRegister.CreatSerialNumber(txtMachineCode.Text, (int)hours);
            Clipboard.SetText(this.txtRegisterCode.Text);
        }

        private void TxtMachineCode_MouseEnter(object sender, EventArgs e)
        {
            var input = Clipboard.GetText();
            if (!string.IsNullOrEmpty(input))
            {
                this.txtMachineCode.Text = input;
            }
        }
    }
}
