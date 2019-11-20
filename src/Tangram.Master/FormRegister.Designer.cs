namespace Tangram.Master
{
    partial class FormRegister
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRegister));
            this.btnRegisterSoft = new System.Windows.Forms.Button();
            this.lblRegisterCode = new System.Windows.Forms.Label();
            this.txtRegisterCode = new System.Windows.Forms.TextBox();
            this.txtMachineCode = new System.Windows.Forms.TextBox();
            this.lblMachineCode = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnRegisterSoft
            // 
            this.btnRegisterSoft.Location = new System.Drawing.Point(140, 297);
            this.btnRegisterSoft.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRegisterSoft.Name = "btnRegisterSoft";
            this.btnRegisterSoft.Size = new System.Drawing.Size(112, 38);
            this.btnRegisterSoft.TabIndex = 9;
            this.btnRegisterSoft.Text = "注册";
            this.btnRegisterSoft.UseVisualStyleBackColor = true;
            this.btnRegisterSoft.Click += new System.EventHandler(this.btnRegisterSoft_Click);
            // 
            // lblRegisterCode
            // 
            this.lblRegisterCode.AutoSize = true;
            this.lblRegisterCode.Location = new System.Drawing.Point(66, 177);
            this.lblRegisterCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRegisterCode.Name = "lblRegisterCode";
            this.lblRegisterCode.Size = new System.Drawing.Size(57, 20);
            this.lblRegisterCode.TabIndex = 8;
            this.lblRegisterCode.Text = "注册码";
            // 
            // txtRegisterCode
            // 
            this.txtRegisterCode.Location = new System.Drawing.Point(140, 172);
            this.txtRegisterCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtRegisterCode.Multiline = true;
            this.txtRegisterCode.Name = "txtRegisterCode";
            this.txtRegisterCode.Size = new System.Drawing.Size(343, 101);
            this.txtRegisterCode.TabIndex = 7;
            // 
            // txtMachineCode
            // 
            this.txtMachineCode.Location = new System.Drawing.Point(140, 87);
            this.txtMachineCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMachineCode.Name = "txtMachineCode";
            this.txtMachineCode.Size = new System.Drawing.Size(343, 26);
            this.txtMachineCode.TabIndex = 6;
            // 
            // lblMachineCode
            // 
            this.lblMachineCode.AutoSize = true;
            this.lblMachineCode.Location = new System.Drawing.Point(66, 92);
            this.lblMachineCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMachineCode.Name = "lblMachineCode";
            this.lblMachineCode.Size = new System.Drawing.Size(57, 20);
            this.lblMachineCode.TabIndex = 5;
            this.lblMachineCode.Text = "机器码";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 42);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(345, 20);
            this.label1.TabIndex = 10;
            this.label1.Text = "友情提示：请将机器码复制给管理员获取注册码";
            // 
            // FormRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 395);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRegisterSoft);
            this.Controls.Add(this.lblRegisterCode);
            this.Controls.Add(this.txtRegisterCode);
            this.Controls.Add(this.txtMachineCode);
            this.Controls.Add(this.lblMachineCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormRegister";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "唐图大屏系统注册";
            this.Load += new System.EventHandler(this.Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRegisterSoft;
        private System.Windows.Forms.Label lblRegisterCode;
        private System.Windows.Forms.TextBox txtRegisterCode;
        private System.Windows.Forms.TextBox txtMachineCode;
        private System.Windows.Forms.Label lblMachineCode;
        private System.Windows.Forms.Label label1;
    }
}