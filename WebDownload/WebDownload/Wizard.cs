using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebDownload
{
    public partial class Wizard : Form
    {
        public string savepath = "";
        public bool EditMode = false;
        public Wizard()
        {
            InitializeComponent();
        }

        public Wizard(string path)
        {
            EditMode = true;
            savepath = path;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DownloadConfig conf = new DownloadConfig();
            conf.workUrl = textBoxStartUrl.Text;
            conf.baseUrl = textBoxBaseUrl.Text;
            conf.useCache = checkBoxUseCache.Checked;
            foreach (var item in listBox1.Items)
            {
                conf.pageSplit.Add(item.ToString());
            }
            conf.savepath = "";
            conf.cachepath = "cache\\" + conf.workUrl;
            conf.cachepath = conf.cachepath.Replace(":", "_");
            conf.cachepath = conf.cachepath.Replace("/", "_");
            conf.TitleKeyWords = textBoxTitleKeyWords.Text;
            conf.UrlKeyWords = textBoxURLKeyWords.Text;
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = "conf";
            save.Filter = "config files(*.conf)|*.confi|All files(*.*)|*.*";
            save.AddExtension = true;
            if( DialogResult.OK == save.ShowDialog())
            {
                savepath = save.FileName;
                BinaryFormatter bf = new BinaryFormatter();
                FileStream stream = new FileStream(savepath, FileMode.Create, FileAccess.Write);
                bf.Serialize(stream, conf);
                stream.Close();
            }


            
        }

        private void textBoxStartUrl_TextChanged(object sender, EventArgs e)
        {
            if (textBoxStartUrl.Text.Length <= 8)
            {
                textBoxBaseUrl.Text = textBoxStartUrl.Text;
            }
            else if (textBoxStartUrl.Text.IndexOf('/', 8) == -1)
            {
                textBoxBaseUrl.Text = textBoxStartUrl.Text;
            }
            else
            {
                textBoxBaseUrl.Text = textBoxStartUrl.Text.Substring(0, textBoxStartUrl.Text.IndexOf('/', 8));
            }
        }

        private void Wizard_Load(object sender, EventArgs e)
        {

        }
    }

    
}
