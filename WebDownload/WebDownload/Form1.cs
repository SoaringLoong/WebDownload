using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace WebDownload
{
    public partial class Form1 : Form
    {
        List<string> workList = new List<string>();
        List<string> imageList = new List<string>();
        List<Thread> threadList = new List<Thread>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
                domainUpDownThreadNum.Items.Add(i);

            Thread parser = new Thread(() => ParserLoop());
            threadList.Add(parser);
        }

        private bool isLuan(string txt)
        {
            var bytes = Encoding.UTF8.GetBytes(txt);
            //239 191 189
            for (var i = 0; i < bytes.Length; i++)
            {
                if (i < bytes.Length - 3)
                    if (bytes[i] == 239 && bytes[i + 1] == 191 && bytes[i + 2] == 189)
                    {
                        return true;
                    }
            }
            return false;
        }

        public void ParserLoop()
        {
            var workurl = workList[0];
            try
            {
                WebClient MyWebClient = new WebClient();

                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据

                Byte[] pageData = MyWebClient.DownloadData(workurl); //从指定网站下载数据
                string encodeing = MyWebClient.ResponseHeaders.Get("Content-Type");
                string pageHtml = "";
                if ( -1 == encodeing.IndexOf("charset="))
                {
                    string s_gbk = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句            
                    string s_utf8 = Encoding.UTF8.GetString(pageData);
                    
                    if (!isLuan(s_utf8)) //判断utf8是否有乱码
                    {
                        pageHtml = s_utf8;
                    }
                    else
                    {
                        pageHtml = s_gbk;
                    }
                }
                else
                {
                    int start = encodeing.IndexOf("charset=");
                    encodeing = encodeing.Substring(start + 8);
                    if(encodeing == "GBK")
                    {
                        pageHtml = Encoding.Default.GetString(pageData);
                    }
                    else if( encodeing == "utf-8")
                    {
                        pageHtml = Encoding.UTF8.GetString(pageData);
                    }
                }
                

                pageHtml = pageHtml.Replace("<", "\n<");
                string[] listtemp = Regex.Split(pageHtml, "\n", RegexOptions.IgnoreCase);

                List<string> list = new List<string>();
                foreach (var item in listtemp)
                {
                    var temp = item.Replace("\t", "");
                    temp = temp.Trim();
                    if (temp.Length > 5)
                        list.Add(temp);
                }

                foreach (var item in list)
                {
                    if (-1 != item.IndexOf("<img"))
                        imageList.Add(item);

                    if (-1 != item.IndexOf("<a"))
                        workList.Add(item);
                }

                

                /*using (StreamWriter sw = new StreamWriter("D:\\ouput.html"))//将获取的内容写入文本
                {

                    //sw.Write(pageHtml);

                }*/
            }

            catch (WebException webEx)
            {
                MessageBox.Show(webEx.Message.ToString());
            }
        }

        public void DownloadImageLoop()
        {
            if (!Directory.Exists("D:\\WebDownload"))
                Directory.CreateDirectory("D:\\WebDownload");
            while (true)
            {
                if (imageList.Count == 0)
                {
                    Thread.Sleep(500);
                    continue;
                }

                string downurl = imageList[0];


                int start = downurl.IndexOf(" src=");
                start = downurl.IndexOf("\"", start);
                int end = downurl.IndexOf("\"", start + 1);
                string url = downurl.Substring(start + 1, end - start - 1);
                string filename = url.Substring(7);
                string filepath = "D:\\WebDownload\\" + filename;
                filepath = filepath.Replace("/", "_");
                WebClient mywebclient = new WebClient();
                mywebclient.DownloadFile(url, filepath);
                imageList.RemoveAt(0);
            }
           
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (textBoxStartUrl.Text == "")
                textBoxStartUrl.Text = "http://desk.3987.com/wall/48596.html";
            workList.Clear();
            workList.Add(textBoxStartUrl.Text);
            int num = 0;
            try
            {
                num  = Convert.ToInt32(domainUpDownThreadNum.Text);
            }
            catch(Exception)
            {
                num = 1;
            }
            
            for( int i = 0; i < num; i++)
            {
                Thread twork = new Thread(() => DownloadImageLoop());
                threadList.Add(twork);
            }

            foreach( var item in threadList)
            {
                item.Start();
            }
        }
    }
}
