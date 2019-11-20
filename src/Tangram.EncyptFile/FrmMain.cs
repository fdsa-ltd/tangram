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
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnFileEN_Click(object sender, EventArgs e)
        {
            FrmFileENCode frmFileENCode = new FrmFileENCode();
            frmFileENCode.ShowDialog();
        }

        private void btnFileDE_Click(object sender, EventArgs e)
        {
            FrmFileDECode frmFileDECode = new FrmFileDECode();
            frmFileDECode.ShowDialog();
        }
    }
}
