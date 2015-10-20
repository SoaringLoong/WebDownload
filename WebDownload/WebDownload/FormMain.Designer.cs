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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.buttonStart = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBoxPrev = new System.Windows.Forms.PictureBox();
            this.linkLabelUrl = new System.Windows.Forms.LinkLabel();
            this.labelSize = new System.Windows.Forms.Label();
            this.labelSizeText = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.linkLabellocalUrl = new System.Windows.Forms.LinkLabel();
            this.panelPrev = new System.Windows.Forms.Panel();
            this.comboBoxThreadNum = new System.Windows.Forms.ComboBox();
            this.buttonNew = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.labelInfo = new System.Windows.Forms.Label();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.listView1 = new WebDownload.ListViewEx();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPrev)).BeginInit();
            this.panelPrev.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(553, 12);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(97, 23);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 389);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 16);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // pictureBoxPrev
            // 
            this.pictureBoxPrev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxPrev.Location = new System.Drawing.Point(7, 3);
            this.pictureBoxPrev.Name = "pictureBoxPrev";
            this.pictureBoxPrev.Size = new System.Drawing.Size(260, 316);
            this.pictureBoxPrev.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxPrev.TabIndex = 8;
            this.pictureBoxPrev.TabStop = false;
            // 
            // linkLabelUrl
            // 
            this.linkLabelUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelUrl.AutoSize = true;
            this.linkLabelUrl.Location = new System.Drawing.Point(5, 322);
            this.linkLabelUrl.Name = "linkLabelUrl";
            this.linkLabelUrl.Size = new System.Drawing.Size(47, 12);
            this.linkLabelUrl.TabIndex = 9;
            this.linkLabelUrl.TabStop = true;
            this.linkLabelUrl.Text = "workurl";
            this.linkLabelUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelUrl_LinkClicked);
            // 
            // labelSize
            // 
            this.labelSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSize.AutoSize = true;
            this.labelSize.Location = new System.Drawing.Point(5, 362);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(35, 12);
            this.labelSize.TabIndex = 10;
            this.labelSize.Text = "Size:";
            // 
            // labelSizeText
            // 
            this.labelSizeText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSizeText.AutoSize = true;
            this.labelSizeText.Location = new System.Drawing.Point(44, 363);
            this.labelSizeText.Name = "labelSizeText";
            this.labelSizeText.Size = new System.Drawing.Size(23, 12);
            this.labelSizeText.TabIndex = 11;
            this.labelSizeText.Text = "N/N";
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(225, 357);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(42, 23);
            this.buttonSave.TabIndex = 12;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // linkLabellocalUrl
            // 
            this.linkLabellocalUrl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabellocalUrl.AutoSize = true;
            this.linkLabellocalUrl.Location = new System.Drawing.Point(5, 341);
            this.linkLabellocalUrl.Name = "linkLabellocalUrl";
            this.linkLabellocalUrl.Size = new System.Drawing.Size(53, 12);
            this.linkLabellocalUrl.TabIndex = 13;
            this.linkLabellocalUrl.TabStop = true;
            this.linkLabellocalUrl.Text = "localurl";
            this.linkLabellocalUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabellocalUrl_LinkClicked);
            // 
            // panelPrev
            // 
            this.panelPrev.Controls.Add(this.pictureBoxPrev);
            this.panelPrev.Controls.Add(this.linkLabellocalUrl);
            this.panelPrev.Controls.Add(this.linkLabelUrl);
            this.panelPrev.Controls.Add(this.buttonSave);
            this.panelPrev.Controls.Add(this.labelSize);
            this.panelPrev.Controls.Add(this.labelSizeText);
            this.panelPrev.Location = new System.Drawing.Point(660, 12);
            this.panelPrev.Name = "panelPrev";
            this.panelPrev.Size = new System.Drawing.Size(273, 389);
            this.panelPrev.TabIndex = 14;
            // 
            // comboBoxThreadNum
            // 
            this.comboBoxThreadNum.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxThreadNum.FormattingEnabled = true;
            this.comboBoxThreadNum.Location = new System.Drawing.Point(427, 13);
            this.comboBoxThreadNum.Name = "comboBoxThreadNum";
            this.comboBoxThreadNum.Size = new System.Drawing.Size(121, 20);
            this.comboBoxThreadNum.TabIndex = 17;
            // 
            // buttonNew
            // 
            this.buttonNew.Location = new System.Drawing.Point(18, 12);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(97, 23);
            this.buttonNew.TabIndex = 18;
            this.buttonNew.Text = "New";
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Location = new System.Drawing.Point(119, 12);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(97, 23);
            this.buttonOpen.TabIndex = 19;
            this.buttonOpen.Text = "Open";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // labelInfo
            // 
            this.labelInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelInfo.Location = new System.Drawing.Point(238, 392);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelInfo.Size = new System.Drawing.Size(409, 13);
            this.labelInfo.TabIndex = 20;
            this.labelInfo.Text = "info";
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(220, 12);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(97, 23);
            this.buttonEdit.TabIndex = 21;
            this.buttonEdit.Text = "Edit";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // listView1
            // 
            this.listView1.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listView1.HoverSelection = true;
            this.listView1.Location = new System.Drawing.Point(18, 41);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(632, 348);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.Click += new System.EventHandler(this.listView1_Click);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 410);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.buttonNew);
            this.Controls.Add(this.comboBoxThreadNum);
            this.Controls.Add(this.panelPrev);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonStart);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "WebDownload";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Click += new System.EventHandler(this.FormMain_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPrev)).EndInit();
            this.panelPrev.ResumeLayout(false);
            this.panelPrev.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
        private ListViewEx listView1;
        private System.Windows.Forms.PictureBox pictureBoxPrev;
        private System.Windows.Forms.LinkLabel linkLabelUrl;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Label labelSizeText;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.LinkLabel linkLabellocalUrl;
        private System.Windows.Forms.Panel panelPrev;
        private System.Windows.Forms.ComboBox comboBoxThreadNum;
        private System.Windows.Forms.Button buttonNew;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Button buttonEdit;
    }
}

