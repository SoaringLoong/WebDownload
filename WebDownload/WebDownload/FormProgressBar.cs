using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebDownload
{
    public partial class FormProgressBar : Form
    {
        public int ProgressValue
        {
            get { return this.progressBar1.Value; }
            set { progressBar1.Value = value; }
        }

        public int Maximum
        {
            get { return this.progressBar1.Maximum; }
            set { progressBar1.Maximum = value; }
        }

        public FormProgressBar()
        {
            InitializeComponent();
        }
    }
}
