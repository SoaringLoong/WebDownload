namespace WebDownload
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            this.textBoxStartUrl = new System.Windows.Forms.TextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.domainUpDownThreadNum = new System.Windows.Forms.DomainUpDown();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxBaseUrl = new System.Windows.Forms.TextBox();
            this.listView1 = new WebDownload.ListViewEx();
            this.url = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonColl = new System.Windows.Forms.Button();
            this.buttonEx = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxStartUrl
            // 
            this.textBoxStartUrl.Location = new System.Drawing.Point(65, 12);
            this.textBoxStartUrl.Name = "textBoxStartUrl";
            this.textBoxStartUrl.Size = new System.Drawing.Size(325, 21);
            this.textBoxStartUrl.TabIndex = 0;
            this.textBoxStartUrl.Text = "http://desk.3987.com/wall/48596.html";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(600, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(97, 23);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // domainUpDownThreadNum
            // 
            this.domainUpDownThreadNum.Location = new System.Drawing.Point(474, 12);
            this.domainUpDownThreadNum.Name = "domainUpDownThreadNum";
            this.domainUpDownThreadNum.Size = new System.Drawing.Size(120, 21);
            this.domainUpDownThreadNum.TabIndex = 3;
            this.domainUpDownThreadNum.Text = "1";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 389);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // textBoxBaseUrl
            // 
            this.textBoxBaseUrl.Location = new System.Drawing.Point(65, 35);
            this.textBoxBaseUrl.Name = "textBoxBaseUrl";
            this.textBoxBaseUrl.Size = new System.Drawing.Size(325, 21);
            this.textBoxBaseUrl.TabIndex = 5;
            this.textBoxBaseUrl.Text = "http://d.3987.com";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.url});
            this.listView1.Location = new System.Drawing.Point(65, 62);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(632, 327);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.Click += new System.EventHandler(this.listView1_Click);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // url
            // 
            this.url.Text = "url";
            this.url.Width = 300;
            // 
            // buttonColl
            // 
            this.buttonColl.Location = new System.Drawing.Point(11, 71);
            this.buttonColl.Name = "buttonColl";
            this.buttonColl.Size = new System.Drawing.Size(42, 23);
            this.buttonColl.TabIndex = 7;
            this.buttonColl.Text = "Coll";
            this.buttonColl.UseVisualStyleBackColor = true;
            this.buttonColl.Click += new System.EventHandler(this.buttonColl_Click);
            // 
            // buttonEx
            // 
            this.buttonEx.Location = new System.Drawing.Point(14, 100);
            this.buttonEx.Name = "buttonEx";
            this.buttonEx.Size = new System.Drawing.Size(42, 23);
            this.buttonEx.TabIndex = 8;
            this.buttonEx.Text = "Ex";
            this.buttonEx.UseVisualStyleBackColor = true;
            this.buttonEx.Click += new System.EventHandler(this.buttonEx_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(745, 410);
            this.Controls.Add(this.buttonEx);
            this.Controls.Add(this.buttonColl);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.textBoxBaseUrl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.domainUpDownThreadNum);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.textBoxStartUrl);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxStartUrl;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.DomainUpDown domainUpDownThreadNum;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxBaseUrl;
        private ListViewEx listView1;
        private System.Windows.Forms.ColumnHeader url;
        private System.Windows.Forms.Button buttonColl;
        private System.Windows.Forms.Button buttonEx;
    }
}

