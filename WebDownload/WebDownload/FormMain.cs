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
using System.Runtime.Serialization.Formatters.Binary;

namespace WebDownload
{
    public partial class FormMain : Form
    {
        // Save the all link list, in parser thread, will parser the super link and add the link to this list.
        // and then, the parser thread will parser this list. to find all link in site.
        //List<string> linkList = new List<string>();
        SerializableDictionary<string, object> linkList = new SerializableDictionary<string, object>();

        // Save the all image url, when add new url, will be contains the url is not downloaded, 
        // if no, add to list and download list. 
        SerializableDictionary<string, DownloadInfo> imageList = new SerializableDictionary<string, DownloadInfo>();

        // The download image list. 
        Queue<string> downloadList = new Queue<string>();

        // The parser url list. 
        Queue<string> workList = new Queue<string>();

        // The thread list, the first is parser thread, other is download thread.
        List<Thread> threadList = new List<Thread>();

        FileStream fs = new FileStream("D:\\WebDownload.log", FileMode.Append);
        StreamWriter sw = null;

        Dictionary<string, ListViewGroup> listgroupmap = new Dictionary<string, ListViewGroup>();

        string baseurl = "";

        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;

            if (File.Exists("imagelist.bin"))
                imageList = imageList.Deserializer("imagelist.bin");


            if(File.Exists("downloadlist.bin"))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream("downloadlist.bin", FileMode.Open, FileAccess.Read);
                downloadList = (Queue<string>)formatter.Deserialize(stream);
                stream.Close();

                stream = new FileStream("worklist.bin", FileMode.Open, FileAccess.Read);
                workList = (Queue<string>)formatter.Deserialize(stream);
                stream.Close();
            }
            

            for (int i = 0; i < 100; i++)
                domainUpDownThreadNum.Items.Add(i);

            sw = new StreamWriter(fs, Encoding.Default);

            listView1.Groups.Clear();
            listView1.Items.Clear();

            listView1.View = View.Details;

            listView1.SetGroupState(ListViewGroupState.Collapsible);
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
            while (true)
            {
                if (workList.Count <= 0)
                {
                    Thread.Sleep(500);
                    continue;
                }
                string workurl = "";
                lock(workList)
                {
                    workurl = workList.Dequeue();
                }
                
                // 全局相对路径,解析当前url的根路径
                string orgbaseurl = "";
                if( workurl.IndexOf('/', 8) == -1 )
                {
                    orgbaseurl = workurl;
                }
                else
                {
                    orgbaseurl = workurl.Substring(0, workurl.IndexOf('/', 8));
                }

                try
                {
                    int end = -1;
                    int start = -1;
                    WebClient MyWebClient = new WebClient();

                    MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据

                    Byte[] pageData = MyWebClient.DownloadData(workurl); //从指定网站下载数据
                    string encodeing = MyWebClient.ResponseHeaders.Get("Content-Type");
                    string pageHtml = "";
                    if (-1 == encodeing.IndexOf("charset="))
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
                        start = encodeing.IndexOf("charset=");
                        encodeing = encodeing.Substring(start + 8);
                        if (encodeing == "GBK" || encodeing == "gbk")
                        {
                            pageHtml = Encoding.Default.GetString(pageData);
                        }
                        else if (encodeing == "utf-8")
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
                            list.Add(temp.ToLower());
                    }

                    string title = "no title";
                    

                    foreach (var item in list)
                    {
                        string splitstring = "";

                        bool bParserIMG = false;
                        if (-1 != item.IndexOf("<img"))
                        {
                            splitstring = " src=";
                            bParserIMG = true;
                        }
                        else if (-1 != item.IndexOf("<a"))
                        {
                            splitstring = " href=";
                            bParserIMG = false;
                        }
                        else if (-1 != item.IndexOf("<title"))
                        {
                            start = item.IndexOf("<title>") + 7;
                            end = item.IndexOf(@"</title>");
                            if (end == -1)
                                end = item.Length;
                            title = item.Substring(start, end - start);
                            continue;
                        }
                        else
                        {
                            continue;
                        }


                        // parser the image url
                        
                        start = item.IndexOf(splitstring);
                        if (-1 == start)
                            continue;

                        char tag = item[start + splitstring.Length];
                        string url = "";
                        
                        switch (tag)
                        {
                            case '\"':
                                start = item.IndexOf("\"", start);
                                end = item.IndexOf("\"", start + 1);
                                url = item.Substring(start + 1, end - start - 1);
                                break;
                            case '\'':
                                start = item.IndexOf("\'", start);
                                end = item.IndexOf("\'", start + 1);
                                url = item.Substring(start + 1, end - start - 1);
                                break;
                            default:
                                start += splitstring.Length;
                                end = item.IndexOf(">");
                                break;
                        }

                        if (url[0] == '/')
                        {
                            url = orgbaseurl + url;
                        }
                        else
                        {
                            url = item.Substring(start + 1, end - start - 1);
                        }


                        // if cannot find http for HTTP , the item is not a url.
                        if (-1 == url.IndexOf("http"))
                        {
                            WriteLog("The url is not good for url:" + url);
                            continue;
                        }


                        if (url.IndexOf(baseurl) == -1 && url.IndexOf(orgbaseurl) == -1)
                        {
                            WriteLog("The url is not current site:" + url);
                            continue;
                        }


                        if( bParserIMG )
                        {
                            if( !imageList.ContainsKey(url))
                            {
                                DownloadInfo info = new DownloadInfo();
                                info.imageUrl = url;
                                info.baseUrl = orgbaseurl;
                                info.title = title;
                                info.workUrl = workurl;
                                imageList.Add(url, info);
                                downloadList.Enqueue(url);

                                if (!listgroupmap.ContainsKey(title))
                                {
//                                     ListViewGroup group1 = new ListViewGroup(title);

                                    var res = listView1.Groups.Add(title, title);
                                    listgroupmap.Add(title, res);

                                    listView1.SetGroupState(ListViewGroupState.Collapsible | ListViewGroupState.Collapsed);
                                }
                                // Create items and add them to myListView.
                                ListViewItem item0 = new ListViewItem(new string[] { url }, 0, listgroupmap[title]);
                                listView1.Items.Add(item0);
                            }
                        }
                        else
                        {
                            if (!linkList.ContainsKey(url))
                            {
                                linkList.Add(url,null);
                                workList.Enqueue(url);
                            }
                        }
                        
                    }
                }
                catch (Exception e)
                {
                    WriteLog("Parser fialed:" + e.Message.ToString());
                }
            }

        }

        public void DownloadImageLoop()
        {
            if (!Directory.Exists("D:\\WebDownload"))
                Directory.CreateDirectory("D:\\WebDownload");

            string downurl = "";
            DownloadInfo downinfo = null;
            int trytag = 0;
            while (true)
            {
                try
                {
                    if (workList.Count <= 0 && downloadList.Count <= 0)
                    {
                        if (trytag <= 5)
                        {
                            trytag++;
                            Thread.Sleep(500);
                            continue;
                        }
                        else
                            break;
                    }
                    lock (downloadList)
                    {
                        if (downloadList.Count == 0)
                        {
                            Thread.Sleep(500);
                            continue;
                        }

                        downurl = downloadList.Dequeue();
                        downinfo = imageList[downurl];
                    }
                    string savepath = "D:\\WebDownload\\" + downinfo.baseUrl.Substring(7) +"\\" + downinfo.title + "\\";
                    string filename = downinfo.imageUrl.Substring(7);
                    string filepath = savepath + filename;
                    filepath = filepath.Replace("/", "_");
                    savepath = savepath.Replace("/", "_");
                    
                    WebClient mywebclient = new WebClient();
                    Byte[] imgdata = mywebclient.DownloadData(downinfo.imageUrl);
                    MemoryStream ms = new MemoryStream(imgdata);
                    Image img = Image.FromStream(ms);
                    //mywebclient.DownloadFile(downurl, filepath);
                    //Image img = Image.FromFile(filepath);
                    if (img.Width > 400 && img.Height > 400)
                    {
                        if (!Directory.Exists(savepath))
                            Directory.CreateDirectory(savepath);
                        img.Save(filepath);
                    }
                }
                catch (Exception e)
                {
                    WriteLog("Download image failed:" + e.Message.ToString());
                }
            }

        }

        public void WriteLog(string log)
        {
            try
            {
                sw.WriteLine(log);
            }
            catch (Exception)
            { }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            linkList.Clear();
            linkList.Add(textBoxStartUrl.Text,null);
            workList.Clear();
            workList.Enqueue(textBoxStartUrl.Text);

            baseurl = textBoxBaseUrl.Text;

            int num = 0;
            try
            {
                num = Convert.ToInt32(domainUpDownThreadNum.Text);
            }
            catch (Exception)
            {
                num = 1;
            }

            for (int i = 0; i < num; i++)
            {
                Thread twork = new Thread(() => DownloadImageLoop());
                threadList.Add(twork);
                Thread parser = new Thread(() => ParserLoop());
                threadList.Add(parser);
            }

            foreach (var item in threadList)
            {
                if (item.ThreadState != ThreadState.Running)
                    item.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = "download list:" + downloadList.Count + ";;;work list:" + workList.Count;
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            imageList.Serializer("imagelist.bin");

            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream("downloadlist.bin", FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, downloadList);
            stream.Close();
            stream = new FileStream("worklist.bin", FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, workList);
            stream.Close(); 
            
            foreach (var item in threadList)
            {
                try
                {
                    item.Abort();
                }
                catch (Exception)
                { }
            }
            sw.Close();
            fs.Close();

        }

        private void listView1_Click(object sender, EventArgs e)
        {
            int selectCount = listView1.SelectedItems.Count; //SelectedItems.Count就是：取得值，表示SelectedItems集合的物件数目。 
            if (selectCount > 0)//若selectCount大於0，说明用户有选中某列。
            {
                
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            FormPreview pre = new FormPreview(listView1.FocusedItem.Text);
            pre.ShowDialog();
        }

        private void buttonColl_Click(object sender, EventArgs e)
        {
            listView1.SetGroupState(ListViewGroupState.Collapsible | ListViewGroupState.Collapsed);
        }

        private void buttonEx_Click(object sender, EventArgs e)
        {
            listView1.SetGroupState(ListViewGroupState.Collapsible | ListViewGroupState.Normal);
        }
    }

    public class DownloadInfo
    {
        public string imageUrl;
        public string title;
        public string baseUrl;
        public string workUrl;
    }
}
