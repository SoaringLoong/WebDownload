using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebDownload
{
    public partial class FormPreview : Form
    {
        public FormPreview(string url)
        {
            InitializeComponent();

            try
            {
                WebClient mywebclient = new WebClient();
                Byte[] imgdata = mywebclient.DownloadData(url);
                MemoryStream ms = new MemoryStream(imgdata);
                Image img = Image.FromStream(ms);
                this.Width = img.Width + this.Width - pictureBox1.Width;
                this.Height = img.Height + this.Height - pictureBox1.Height;
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox1.Image = img;
            }
            catch(Exception)
            {

            }
        }
    }
}
