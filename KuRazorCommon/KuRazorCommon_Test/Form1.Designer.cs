namespace Tuple的用法
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.Read = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(53, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(127, 52);
            this.button1.TabIndex = 0;
            this.button1.Text = "动态生成DLL文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Read
            // 
            this.Read.Location = new System.Drawing.Point(260, 29);
            this.Read.Name = "Read";
            this.Read.Size = new System.Drawing.Size(180, 52);
            this.Read.TabIndex = 1;
            this.Read.Text = "读取DLL";
            this.Read.UseVisualStyleBackColor = true;
            this.Read.Click += new System.EventHandler(this.Read_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(172, 116);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(180, 52);
            this.button2.TabIndex = 2;
            this.button2.Text = "Go";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(523, 180);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Read);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button Read;
        private System.Windows.Forms.Button button2;
    }
}

