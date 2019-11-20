namespace Tangram.Manage
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btnGenerateRegisterSoft = new System.Windows.Forms.Button();
            this.lblRegisterCode = new System.Windows.Forms.Label();
            this.txtRegisterCode = new System.Windows.Forms.TextBox();
            this.txtMachineCode = new System.Windows.Forms.TextBox();
            this.lblMachineCode = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGenerateRegisterSoft
            // 
            this.btnGenerateRegisterSoft.Location = new System.Drawing.Point(112, 242);
            this.btnGenerateRegisterSoft.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGenerateRegisterSoft.Name = "btnGenerateRegisterSoft";
            this.btnGenerateRegisterSoft.Size = new System.Drawing.Size(155, 40);
            this.btnGenerateRegisterSoft.TabIndex = 15;
            this.btnGenerateRegisterSoft.Text = "生成授权码";
            this.btnGenerateRegisterSoft.UseVisualStyleBackColor = true;
            this.btnGenerateRegisterSoft.Click += new System.EventHandler(this.btnGenerateRegisterSoft_Click);
            // 
            // lblRegisterCode
            // 
            this.lblRegisterCode.AutoSize = true;
            this.lblRegisterCode.Location = new System.Drawing.Point(126, 325);
            this.lblRegisterCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRegisterCode.Name = "lblRegisterCode";
            this.lblRegisterCode.Size = new System.Drawing.Size(57, 20);
            this.lblRegisterCode.TabIndex = 14;
            this.lblRegisterCode.Text = "注册码";
            // 
            // txtRegisterCode
            // 
            this.txtRegisterCode.Enabled = false;
            this.txtRegisterCode.Location = new System.Drawing.Point(200, 320);
            this.txtRegisterCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtRegisterCode.Name = "txtRegisterCode";
            this.txtRegisterCode.ReadOnly = true;
            this.txtRegisterCode.Size = new System.Drawing.Size(435, 26);
            this.txtRegisterCode.TabIndex = 13;
            // 
            // txtMachineCode
            // 
            this.txtMachineCode.Location = new System.Drawing.Point(200, 58);
            this.txtMachineCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtMachineCode.Name = "txtMachineCode";
            this.txtMachineCode.Size = new System.Drawing.Size(435, 26);
            this.txtMachineCode.TabIndex = 12;
            this.txtMachineCode.MouseEnter += new System.EventHandler(this.TxtMachineCode_MouseEnter);
            // 
            // lblMachineCode
            // 
            this.lblMachineCode.AutoSize = true;
            this.lblMachineCode.Location = new System.Drawing.Point(126, 63);
            this.lblMachineCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMachineCode.Name = "lblMachineCode";
            this.lblMachineCode.Size = new System.Drawing.Size(57, 20);
            this.lblMachineCode.TabIndex = 11;
            this.lblMachineCode.Text = "机器码";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(108, 122);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 20);
            this.label2.TabIndex = 17;
            this.label2.Text = "过期时间";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(200, 116);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(176, 26);
            this.numericUpDown1.TabIndex = 19;
            this.numericUpDown1.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(112, 184);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(155, 40);
            this.button1.TabIndex = 20;
            this.button1.Text = "一个月";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(309, 184);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(155, 40);
            this.button2.TabIndex = 21;
            this.button2.Text = "三个月";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(492, 184);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(155, 40);
            this.button3.TabIndex = 22;
            this.button3.Text = "半年";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(492, 242);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(155, 40);
            this.button4.TabIndex = 23;
            this.button4.Text = "两年";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(309, 242);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(155, 40);
            this.button5.TabIndex = 24;
            this.button5.Text = "一年";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Button5_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 405);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnGenerateRegisterSoft);
            this.Controls.Add(this.lblRegisterCode);
            this.Controls.Add(this.txtRegisterCode);
            this.Controls.Add(this.txtMachineCode);
            this.Controls.Add(this.lblMachineCode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "获取授权码";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerateRegisterSoft;
        private System.Windows.Forms.Label lblRegisterCode;
        private System.Windows.Forms.TextBox txtRegisterCode;
        private System.Windows.Forms.TextBox txtMachineCode;
        private System.Windows.Forms.Label lblMachineCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}