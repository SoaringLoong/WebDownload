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
        public Wizard()
        {
            InitializeComponent();
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
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".conf";
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
    }

    
}
