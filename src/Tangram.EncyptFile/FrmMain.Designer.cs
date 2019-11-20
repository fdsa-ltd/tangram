namespace Tangram.Manage
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.btnFileEN = new System.Windows.Forms.Button();
            this.btnFileDE = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnFileEN
            // 
            this.btnFileEN.Location = new System.Drawing.Point(153, 122);
            this.btnFileEN.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnFileEN.Name = "btnFileEN";
            this.btnFileEN.Size = new System.Drawing.Size(237, 97);
            this.btnFileEN.TabIndex = 1;
            this.btnFileEN.Text = "文件加密";
            this.btnFileEN.UseVisualStyleBackColor = true;
            this.btnFileEN.Click += new System.EventHandler(this.btnFileEN_Click);
            // 
            // btnFileDE
            // 
            this.btnFileDE.Location = new System.Drawing.Point(471, 122);
            this.btnFileDE.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnFileDE.Name = "btnFileDE";
            this.btnFileDE.Size = new System.Drawing.Size(237, 97);
            this.btnFileDE.TabIndex = 2;
            this.btnFileDE.Text = "文件解密";
            this.btnFileDE.UseVisualStyleBackColor = true;
            this.btnFileDE.Click += new System.EventHandler(this.btnFileDE_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 435);
            this.Controls.Add(this.btnFileDE);
            this.Controls.Add(this.btnFileEN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "唐图大屏系统—加解密";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnFileEN;
        private System.Windows.Forms.Button btnFileDE;
    }
}