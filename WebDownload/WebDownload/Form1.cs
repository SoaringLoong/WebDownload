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
using System.Windows.Forms;

namespace WebDownload
{
    public partial class Form1 : Form
    {

        List<string> workList = new List<string>();
        List<string> imageList = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {

                WebClient MyWebClient = new WebClient();


                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据

                Byte[] pageData = MyWebClient.DownloadData("http://desk.3987.com/"); //从指定网站下载数据

                string pageHtml = Encoding.Default.GetString(pageData);  //如果获取网站页面采用的是GB2312，则使用这句            

                //string pageHtml = Encoding.UTF8.GetString(pageData); //如果获取网站页面采用的是UTF-8，则使用这句
                pageHtml = pageHtml.Replace("<", "\n<");
                string[] listtemp = Regex.Split(pageHtml, "\n", RegexOptions.IgnoreCase);
                //List<string> list = new List<string>(listtemp);
                List<string> list = new List<string>();
                foreach (var item in listtemp)
                {
                    var temp = item.Replace("\t", "");
                    temp = temp.Trim();
                    if (temp.Length > 5)
                        list.Add(temp);
                }
                List<string> imglist = new List<string>();
                List<string> linklist = new List<string>();
                foreach(var item in list)
                {
                    if (-1 != item.IndexOf("<img"))
                        imglist.Add(item);

                    if (-1 != item.IndexOf("<a"))
                        linklist.Add(item);


                }

                // 开始处理imagelist
                foreach( var item in imglist)
                {
                    int start = item.IndexOf("src=");
                    start = item.IndexOf("\"", start);
                    int end = item.IndexOf("\"", start+1);
                    string url = item.Substring(start+1, end - start-1);
                    string filename = url.Substring(7);
                    string filepath = "D:\\" + filename;
                    filepath = filepath.Replace("/", "_");
                    WebClient mywebclient = new WebClient();
                    mywebclient.DownloadFile(url, filepath);
                }

                using (StreamWriter sw = new StreamWriter("D:\\ouput.html"))//将获取的内容写入文本
                {

                    //sw.Write(pageHtml);

                }
            }

            catch (WebException webEx)
            {

                Console.WriteLine(webEx.Message.ToString());

            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            workList.Clear();
            workList.Add(textBoxStartUrl.Text);
        }
    }
}
