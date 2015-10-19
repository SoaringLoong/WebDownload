namespace WebDownload
{
    partial class Wizard
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
            this.checkBoxUseCache = new System.Windows.Forms.CheckBox();
            this.textBoxBaseUrl = new System.Windows.Forms.TextBox();
            this.textBoxStartUrl = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.domainUpDown1 = new System.Windows.Forms.DomainUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBoxTitleKeyWords = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxURLKeyWords = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxUseCache
            // 
            this.checkBoxUseCache.AutoSize = true;
            this.checkBoxUseCache.Checked = true;
            this.checkBoxUseCache.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUseCache.Location = new System.Drawing.Point(435, 36);
            this.checkBoxUseCache.Name = "checkBoxUseCache";
            this.checkBoxUseCache.Size = new System.Drawing.Size(96, 16);
            this.checkBoxUseCache.TabIndex = 19;
            this.checkBoxUseCache.Text = "使用缓存队列";
            this.checkBoxUseCache.UseVisualStyleBackColor = true;
            // 
            // textBoxBaseUrl
            // 
            this.textBoxBaseUrl.Location = new System.Drawing.Point(66, 59);
            this.textBoxBaseUrl.Name = "textBoxBaseUrl";
            this.textBoxBaseUrl.Size = new System.Drawing.Size(325, 21);
            this.textBoxBaseUrl.TabIndex = 18;
            this.textBoxBaseUrl.Text = "http://d.3987.com";
            // 
            // textBoxStartUrl
            // 
            this.textBoxStartUrl.Location = new System.Drawing.Point(66, 36);
            this.textBoxStartUrl.Name = "textBoxStartUrl";
            this.textBoxStartUrl.Size = new System.Drawing.Size(325, 21);
            this.textBoxStartUrl.TabIndex = 17;
            this.textBoxStartUrl.Text = "http://desk.3987.com/wall/48596.html";
            this.textBoxStartUrl.TextChanged += new System.EventHandler(this.textBoxStartUrl_TextChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(103, 286);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(325, 21);
            this.textBox1.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(113, 322);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 22;
            this.label1.Text = "忽略列表";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(431, 286);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 23;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Items.AddRange(new object[] {
            "第[0-9]*页",
            "第[\\u4e00-\\u9fa5]*页",
            "第[^\\x00-\\xff]*页",
            "[（][0-9]*[）]",
            "[\\\\(][0-9]*[\\\\)]"});
            this.listBox1.Location = new System.Drawing.Point(193, 322);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(235, 148);
            this.listBox1.TabIndex = 24;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(168, 476);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 25;
            this.button2.Text = "Save";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button3.Location = new System.Drawing.Point(353, 476);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 26;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // domainUpDown1
            // 
            this.domainUpDown1.Location = new System.Drawing.Point(146, 17);
            this.domainUpDown1.Name = "domainUpDown1";
            this.domainUpDown1.Size = new System.Drawing.Size(120, 21);
            this.domainUpDown1.TabIndex = 27;
            this.domainUpDown1.Text = "domainUpDown1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 12);
            this.label2.TabIndex = 28;
            this.label2.Text = "从开始地址遍历层数";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.domainUpDown1);
            this.groupBox1.Location = new System.Drawing.Point(143, 222);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 47);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(25, 0);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 30;
            this.checkBox1.Text = "遍历层级";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBoxTitleKeyWords
            // 
            this.textBoxTitleKeyWords.Location = new System.Drawing.Point(141, 148);
            this.textBoxTitleKeyWords.Name = "textBoxTitleKeyWords";
            this.textBoxTitleKeyWords.Size = new System.Drawing.Size(325, 21);
            this.textBoxTitleKeyWords.TabIndex = 30;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 151);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 31;
            this.label3.Text = "Title关键字";
            // 
            // textBoxURLKeyWords
            // 
            this.textBoxURLKeyWords.Location = new System.Drawing.Point(141, 109);
            this.textBoxURLKeyWords.Name = "textBoxURLKeyWords";
            this.textBoxURLKeyWords.Size = new System.Drawing.Size(325, 21);
            this.textBoxURLKeyWords.TabIndex = 32;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(64, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 33;
            this.label4.Text = "URL关键字";
            // 
            // Wizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 511);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxURLKeyWords);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxTitleKeyWords);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.checkBoxUseCache);
            this.Controls.Add(this.textBoxBaseUrl);
            this.Controls.Add(this.textBoxStartUrl);
            this.Name = "Wizard";
            this.Text = "Wizard";
            this.Load += new System.EventHandler(this.Wizard_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxUseCache;
        private System.Windows.Forms.TextBox textBoxBaseUrl;
        private System.Windows.Forms.TextBox textBoxStartUrl;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DomainUpDown domainUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBoxTitleKeyWords;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxURLKeyWords;
        private System.Windows.Forms.Label label4;
    }
}