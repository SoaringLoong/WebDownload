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
using System.Collections;

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

        SerializableDictionary<string, ListViewGroup> listgroupmap = new SerializableDictionary<string, ListViewGroup>();

        string baseurl = "";

        bool AppRunning = false;

        Point PrevLoc = new Point();
        Size PrevSize = new Size();

        List<string> addToViewList = new List<string>();

        DownloadConfig config = null;

        public FormMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int num = Environment.ProcessorCount + 2;
            for (int i = 1; i < 10; i++)
                comboBoxThreadNum.Items.Add(i);

            comboBoxThreadNum.Text = num.ToString();

            sw = new StreamWriter(fs, Encoding.Default);

            listView1.Groups.Clear();
            listView1.Items.Clear();

            listView1.View = View.Details;

            listView1.SetGroupState(ListViewGroupState.Collapsible);

            ShowPrevArea(false);

            labelInfo.Text = "No open";
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
            WriteLog("Parser Thread is starting. id is:" + Thread.CurrentThread.ManagedThreadId);
            while (true)
            {
                if (!AppRunning)
                    break;

                if (workList.Count <= 0)
                {
                    Thread.Sleep(500);
                    continue;
                }
                string workurl = "";
                lock(workList)
                {
                    if (workList.Count <= 0)
                    {
                        Thread.Sleep(500);
                        continue;
                    }
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

                    string pageHtml = "";
                    // 尝试下载数据
                    try
                    {
                        Byte[] pageData = MyWebClient.DownloadData(workurl); //从指定网站下载数据
                        string encodeing = MyWebClient.ResponseHeaders.Get("Content-Type");

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
                    }
                    catch(System.NotSupportedException e)
                    {
                        WriteLog("Download page fiald, no support url.error message is:" + e.ToString() + ".WorkUrl is:" + workurl);
                    }
                    catch(System.Net.WebException e)
                    {
                        WriteLog("Download page fiald, the url is not find.error message is:" + e.ToString() + ".WorkUrl is:" + workurl);
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

                        if (string.IsNullOrEmpty(url))
                            continue;

                        // 支持全局相对路径
                        if (url[0] == '/')
                        {
                            url = orgbaseurl + url;
                        }
                        // 过滤跳转标记和javascript
                        else if( url[0] == '#' || url.IndexOf("javascript") != -1)
                        {
                            continue;
                        }
                        // 没有http认为是当前相对路径
                        else if( -1 == url.IndexOf("http") )
                        {
                            url = workurl.Substring(0, workurl.LastIndexOf("/")+1) + url;
                        }
                        else
                        {
                            url = item.Substring(start + 1, end - start - 1);
                        }

                        if (!bParserIMG && url.IndexOf(baseurl) == -1 && url.IndexOf(orgbaseurl) == -1 )
                        {
                            //WriteLog("The url is not current site:" + url);
                            continue;
                        }

                        // some a tag's url is a image. check it.
                        if( bParserIMG == false )
                        {
                            string temp = url.Substring(url.Length - 4).ToLower();
                            if (temp == "jpeg" || temp == ".jpg" || temp == ".png" || temp == ".bmp" || temp == ".gif")
                            {
                                url = url.Substring(url.LastIndexOf("://") - 4);
                                bParserIMG = true;
                            }
                        }
                        

                        if( bParserIMG )
                        {
                            if( !imageList.ContainsKey(url))
                            {
                                string samiltitle = title;
                                // 处理title
                                foreach( var split in config.pageSplit)
                                {
                                    Regex rg = new Regex(split);
                                    var res = rg.Match(title);
                                    if( res.Value != "" )
                                    {
                                        samiltitle = Regex.Replace(title, split, "");
                                        break;
                                    }
                                }


                                // 准备信息
                                DownloadInfo info = new DownloadInfo();
                                info.imageUrl = url;
                                info.baseUrl = orgbaseurl;
                                info.title = title;
                                info.samilTitle = samiltitle;
                                info.workUrl = workurl;
                                imageList.Add(url, info);
                                downloadList.Enqueue(url);

                                lock(addToViewList)
                                {
                                    addToViewList.Add(url);
                                }                              
                            }
                        }
                        else
                        {
                            //lock(linkList)
                            {
                                if (!linkList.ContainsKey(url))
                                {
                                    linkList.Add(url, null);
                                    lock(workList)
                                    {
                                        workList.Enqueue(url);
                                    }
                                }
                            }
                            
                        }
                        
                    }
                }
                catch (Exception e)
                {
                    WriteLog("Parser fialed:" + e.ToString());
                }
            }
            WriteLog("Parser Thread is end. id is:" + Thread.CurrentThread.ManagedThreadId);
        }

        public void DownloadImageLoop()
        {
            WriteLog("Download Thread is starting. id is:" + Thread.CurrentThread.ManagedThreadId);
            if (!Directory.Exists("D:\\WebDownload"))
                Directory.CreateDirectory("D:\\WebDownload");

            string downurl = "";
            DownloadInfo downinfo = null;
            int trytag = 0;
            while (true)
            {
                if (!AppRunning)
                    break;
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

                    var filepath = GetImageSavePath(downinfo.imageUrl);
                    // 如果文件已经存在,则认为已经下载过.直接跳过
                    if (File.Exists(filepath))
                        continue;

                    WebClient mywebclient = new WebClient();
                    Byte[] imgdata = mywebclient.DownloadData(downinfo.imageUrl);
                    MemoryStream ms = new MemoryStream(imgdata);
                    Image img = Image.FromStream(ms);
                    downinfo.imgSize = img.Size;
                    if (img.Width > 400 && img.Height > 400)
                    {
                        var savefolder = GetImageSaveFolder(downinfo.imageUrl);
                        if (!Directory.Exists(savefolder))
                            Directory.CreateDirectory(savefolder);
                        img.Save(filepath);
                    }
                }
                catch (Exception e)
                {
                    WriteLog("Download image failed:" + e.Message.ToString());
                }
            }

            WriteLog("Download Thread is end. id is:" + Thread.CurrentThread.ManagedThreadId);
        }

        public string GetImageSavePath( string imgurl )
        {
            var downinfo = imageList[imgurl];

            string filename = downinfo.title + downinfo.imageUrl.Substring(7);
            string filepath = GetImageSaveFolder(imgurl) + filename;
            filepath = filepath.Replace("/", "_");
            return filepath;
        }

        public string GetImageSaveFolder( string imgurl )
        {
            var downinfo = imageList[imgurl];

            string savepath = "D:\\WebDownload\\" + downinfo.baseUrl.Substring(7) + "\\" + downinfo.samilTitle + "\\";
            string filename = downinfo.imageUrl.Substring(7);
            savepath = savepath.Replace("/", "_");
            return savepath;
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

        public void StartWork()
        {
            AppRunning = true;

            WriteLog("Start work");

            buttonStart.Text = "Stop";


            linkList.Clear();
            workList.Clear();


            if (config.useCache)
            {
                try
                {
                    if (File.Exists("cache\\imagelist.bin"))
                        imageList = imageList.Deserializer("cache\\imagelist.bin");

                    if (File.Exists("cache\\linklist.bin"))
                        linkList = linkList.Deserializer("cache\\linklist.bin");


                    if (File.Exists("cache\\downloadlist.bin"))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        FileStream stream = new FileStream("cache\\downloadlist.bin", FileMode.Open, FileAccess.Read);
                        downloadList = (Queue<string>)formatter.Deserialize(stream);
                        stream.Close();
                    }

                    if (File.Exists("cache\\worklist.bin"))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        FileStream stream = new FileStream("cache\\worklist.bin", FileMode.Open, FileAccess.Read);
                        workList = (Queue<string>)formatter.Deserialize(stream);
                        stream.Close();
                    }

                    addToViewList.AddRange(imageList.Keys);
                }
                catch (Exception ex)
                {
                    WriteLog("反序列化失败,错误消息:" + ex.ToString() + "\r\n删除所有序列化文件并清空队列.");
                    imageList.Clear();
                    linkList.Clear();
                    downloadList.Clear();
                    workList.Clear();
                }
            }
            else
            {
                if (Directory.Exists("cache"))
                    Directory.Delete("cache", true);
            }


            if (workList.Count == 0) workList.Enqueue(config.workUrl);
            if (linkList.Count == 0) linkList.Add(config.workUrl, null);

            baseurl = config.baseUrl;

            int num = 0;
            try
            {
                num = Convert.ToInt32(comboBoxThreadNum.Text);
            }
            catch (Exception)
            {
                num = 1;
            }

            for (int i = 0; i < num; i++)
            {
                Thread twork = new Thread(() => DownloadImageLoop());
                twork.Name = "Download Thread";
                threadList.Add(twork);
                Thread parser = new Thread(() => ParserLoop());
                parser.Name = "Parser Thread";
                threadList.Add(parser);
            }

            foreach (var item in threadList)
            {
                if (item.ThreadState != ThreadState.Running)
                    item.Start();
            }
        }

        public void StopWork()
        {
            AppRunning = false;
            
            WriteLog("Stop work");

            buttonStart.Text = "Start";
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (buttonStart.Text == "Start")
                StartWork();

            else
                StopWork();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = "download list:" + downloadList.Count + ";;;work list:" + workList.Count;

            try
            {
                // Add to view
                List<string> addList = new List<string>();
                lock (addToViewList)
                {
                    if (addToViewList.Count > 0)
                    {
                        addList.AddRange(addToViewList);
                        addToViewList.Clear();
                    }
                }
                if (addList.Count > 0)
                {
                    foreach (var item in addList)
                    {
                        var info = imageList[item];
                        if (!listgroupmap.ContainsKey(info.samilTitle))
                        {
                            ListViewGroup group = new ListViewGroup(info.samilTitle);

                            listView1.Groups.Add(group);
                            listgroupmap.Add(info.samilTitle, group);

                            listView1.SetGroupState(ListViewGroupState.Collapsible | ListViewGroupState.Collapsed, group);
                        }
                        // Create items and add them to myListView.
                        ListViewItem additem = new ListViewItem(new string[] { info.imageUrl }, 0, listgroupmap[info.samilTitle]);
                        listView1.Items.Add(additem);
                    }
                }
            }
            catch(Exception ex)
            {
                WriteLog("in TimeLoop happend error:" + ex.ToString());
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            

        }

        private void listView1_Click(object sender, EventArgs e)
        {
//             int selectCount = listView1.SelectedItems.Count; //SelectedItems.Count就是：取得值，表示SelectedItems集合的物件数目。 
//             if (selectCount > 0)//若selectCount大於0，说明用户有选中某列。
//             {
//                 
//             }
            try
            {
                pictureBoxPrev.Image = null;
                labelSizeText.Text = "";
                ShowPrevArea(true);

                var info = imageList[listView1.FocusedItem.Text];
                linkLabelUrl.Text = info.workUrl;
                linkLabelUrl.Links[0].LinkData = info.workUrl;
                var path = GetImageSavePath(info.imageUrl);
                if (File.Exists(path))
                {
                    // File is download
                    pictureBoxPrev.LoadAsync(path);
                    buttonSave.Hide();
                    linkLabellocalUrl.Text = path;
                    linkLabellocalUrl.Links[0].LinkData = path;
                }
                else
                {
                    pictureBoxPrev.Load(info.imageUrl);
                    buttonSave.Show();
                }
            Retry:
                if (info.imgSize.Width == 0 || info.imgSize.Height == 0)
                {
                    info.imgSize = pictureBoxPrev.Image.Size;
                    goto Retry;
                }
                labelSizeText.Text = info.imgSize.ToString();
            }
            catch(Exception ex)
            {
                WriteLog("error in listview click, error message:" + ex.ToString());
            }

            
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            FormPreview pre = new FormPreview(listView1.FocusedItem.Text);
            pre.ShowDialog();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var path = GetImageSavePath(linkLabelUrl.Text);
            pictureBoxPrev.Image.Save(path);
            linkLabellocalUrl.Text = path;
        }

        private void linkLabelUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());    
        }

        private void linkLabellocalUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());    
        }

        private void listView1_Leave(object sender, EventArgs e)
        {
            ShowPrevArea(false);
        }

        private void FormMain_Click(object sender, EventArgs e)
        {
            ShowPrevArea(false);
        }

        public void ShowPrevArea(bool bShow = false )
        {
            if( bShow)
            {
                if (panelPrev.Visible)
                    return;
                panelPrev.Location = PrevLoc;
                panelPrev.Size = PrevSize;
                panelPrev.Show();
                panelPrev.Enabled = true;
                this.Width += panelPrev.Width;
            }
            else
            {
                if (!panelPrev.Visible)
                    return;
                PrevLoc = panelPrev.Location;
                PrevSize = panelPrev.Size;
                panelPrev.Hide();
                panelPrev.Enabled = false;
                this.Width -= panelPrev.Width;
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            AppRunning = false;
            bool bIsRunning = false;
            do
            {
                foreach (var item in threadList)
                {
                    if (item.IsAlive)
                    {
                        bIsRunning = true;
                        continue;
                    }
                }
                bIsRunning = false;
            } while (bIsRunning);

            if( config != null && config.useCache )
            {
                if (!Directory.Exists("cache"))
                    Directory.CreateDirectory("cache");

                imageList.Serializer("cache\\imagelist.bin");
                linkList.Serializer("cache\\linklist.bin");

                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream("cache\\downloadlist.bin", FileMode.Create, FileAccess.Write);
                formatter.Serialize(stream, downloadList);
                stream.Close();
                stream = new FileStream("cache\\worklist.bin", FileMode.Create, FileAccess.Write);
                formatter.Serialize(stream, workList);
                stream.Close();
            }
            

            sw.Close();
            fs.Close();
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            Wizard wiz = new Wizard();
            if( DialogResult.OK == wiz.ShowDialog())
            {
                OpenConfig(wiz.savepath);
            }
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if( DialogResult.OK == ofd.ShowDialog())
            {
                OpenConfig(ofd.FileName);
            }
        }

        public void OpenConfig(string path)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            config = (DownloadConfig)formatter.Deserialize(stream);
            stream.Close();

            labelInfo.Text = config.workUrl;
        }
    }

    public class DownloadInfo
    {
        public string imageUrl;
        public string title;
        public string samilTitle;
        public string baseUrl;
        public string workUrl;
        public Size imgSize;
    }
}
