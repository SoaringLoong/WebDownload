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


        SerializableDictionary<string, ListViewGroup> listgroupmap = new SerializableDictionary<string, ListViewGroup>();

        string baseurl = "";

        bool AppRunning = false;
        bool bIsStartWork = false;

        Point PrevLoc = new Point();
        Size PrevSize = new Size();

        List<string> addToViewList = new List<string>();

        DownloadConfig config = null;

        string CurrentConfigPath = "";

        public FormMain()
        {
            InitializeComponent();

            AppRunning = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int num = Environment.ProcessorCount + 2;
            for (int i = 1; i < 10; i++)
                comboBoxThreadNum.Items.Add(i);

            comboBoxThreadNum.Text = num.ToString();

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

        /// <summary>
        /// 获取网页源代码方法
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="charSet">指定编码，如果为空，则自动判断</param>
        /// <param name="out_str">网页源代码</param>
        public string GetHtml(string url, string charSet)
        {
            string strResult = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                string defenconding = "utf-8";
                string headencoding = "";
                
                //声明一个HttpWebRequest请求
                request.Timeout = 3000000;
                //设置连接超时时间
                request.Headers.Set("Pragma", "no-cache");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if( response.ContentType != null )
                {
                    int index = response.ContentType.IndexOf("charset=");
                    if (-1 != index)
                    {
                        headencoding = response.ContentType.Substring(index + 8);
                    }
                }
                
                if (response.ToString() != "")
                {
                    Stream streamReceive = response.GetResponseStream();
                    string strencoding = defenconding;
                    // ContentType 指定的编码
                    if (!string.IsNullOrWhiteSpace(headencoding))
                        strencoding = headencoding;

                    Encoding encoding = Encoding.GetEncoding(strencoding);
                    StreamReader streamReader = new StreamReader(streamReceive, encoding);
                    strResult = streamReader.ReadToEnd();

                    // 如果ContentType没有指定编码,则检查Meta的编码
                    if(string.IsNullOrWhiteSpace(headencoding))
                    {
                        Match charSetMatch = Regex.Match(strResult, "<meta([^>]*)charset=(\")?(.*)?\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                        string webCharSet = charSetMatch.Groups[3].Value.Trim().ToLower();
                        if (!string.IsNullOrWhiteSpace(webCharSet))
                            strencoding = webCharSet;

                        if (strencoding != defenconding)
                        {
                            encoding = Encoding.GetEncoding(strencoding);//乱码处理
                            streamReader = new StreamReader(streamReceive, encoding);
                            strResult = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                WriteLog("Download page fiald, the url is not find.error message is:" + exp.ToString() + ".WorkUrl is:" + url);
            }
            return strResult;
        }

        public void ParserLoop()
        {
            WriteLog("Parser Thread is starting. id is:" + Thread.CurrentThread.ManagedThreadId);
            while (true)
            {
                if (!AppRunning)
                    break;

                if(!bIsStartWork)
                {
                    Thread.CurrentThread.Suspend();
                }

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

                  
                    pageHtml = GetHtml(workurl,null);

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

                    string title = "no title";
                    

                    foreach (var item in list)
                    {
                        string splitstring = "";

                        bool bParserIMG = false;
                        if (-1 != item.IndexOf("<img", StringComparison.OrdinalIgnoreCase))
                        {
                            splitstring = "src=";
                            bParserIMG = true;
                        }
                        else if (-1 != item.IndexOf("<a", StringComparison.OrdinalIgnoreCase))
                        {
                            splitstring = "href=";
                            bParserIMG = false;
                        }
                        else if (-1 != item.IndexOf("<title", StringComparison.OrdinalIgnoreCase))
                        {
                            start = item.IndexOf("<title>", StringComparison.OrdinalIgnoreCase) + 7;
                            end = item.IndexOf(@"</title>", StringComparison.OrdinalIgnoreCase);
                            if (end == -1)
                                end = item.Length;
                            title = item.Substring(start, end - start);
                            continue;
                        }
                        else
                        {
                            continue;
                        }


                        // parser the url


                        if (-1 == item.IndexOf(splitstring, StringComparison.OrdinalIgnoreCase))
                            continue;

                        string url = "";

                        string[] resu = item.Split(' ');
                        foreach( var subitem in resu)
                        {
                            if (-1 != subitem.IndexOf(splitstring, StringComparison.OrdinalIgnoreCase))
                            {
                                url = subitem.Substring(subitem.IndexOf("=") + 1);
                                break;
                            }
                        }

                        url = GetRealURL(url, orgbaseurl, workurl);

                        if (!bParserIMG && url.IndexOf(baseurl, StringComparison.OrdinalIgnoreCase) == -1)//&& url.IndexOf(orgbaseurl) == -1 )
                        {
                            //WriteLog("The url is not current site:" + url);
                            continue;
                        }

                        string workpage = config.workUrl.Substring(0, config.workUrl.LastIndexOf('.'));
                        if (!bParserIMG && config.OnlyCurPage && url.IndexOf(workpage) == -1)
                        {
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
								// 去除多页时通常使用的分隔
                                samiltitle = samiltitle.Replace("-", "").Trim();

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
                                if (-1 == url.IndexOf(config.UrlKeyWords))
                                    continue;
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

                if (!bIsStartWork)
                {
                    Thread.CurrentThread.Suspend();
                }

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

                        try
                        {
                            img.Save(filepath);
                        }
                        catch(Exception exp)
                        {
                            WriteLog("Save Image error:" + exp.Message.ToString()+"\r\nThe url is :"+downinfo.workUrl+"\r\nThe save path is:"
                                + filepath);

                            lock(downloadList)
                            {
                                downloadList.Enqueue(downurl);
                            }
                        }
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

            string filename = downinfo.imageUrl.Substring(7);
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
                using( FileStream fs = new FileStream("D:\\WebDownload.log", FileMode.Append))
                {
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    log = string.Format("[{0}]:{1}", DateTime.Now.ToString(), log);
                    sw.WriteLine(log);
                    sw.Close();
                    fs.Close();
                }
            }
            catch (Exception)
            { }
        }

        public void StartWork()
        {
            bIsStartWork = true;

            WriteLog("Start work");

            buttonStart.Text = "Stop";


            linkList.Clear();
            workList.Clear();

            if (config.useCache && Directory.Exists(config.cachepath))
            {
                try
                {
                    if (File.Exists(config.cachepath + "\\imagelist.bin"))
                        imageList = imageList.Deserializer(config.cachepath + "\\imagelist.bin");

                    if (File.Exists(config.cachepath + "\\linklist.bin"))
                        linkList = linkList.Deserializer(config.cachepath + "\\linklist.bin");


                    if (File.Exists(config.cachepath + "\\downloadlist.bin"))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        FileStream stream = new FileStream(config.cachepath + "\\downloadlist.bin", FileMode.Open, FileAccess.Read);
                        downloadList = (Queue<string>)formatter.Deserialize(stream);
                        stream.Close();
                    }

                    if (File.Exists(config.cachepath + "\\worklist.bin"))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        FileStream stream = new FileStream(config.cachepath + "\\worklist.bin", FileMode.Open, FileAccess.Read);
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
                switch(item.ThreadState)
                {
                    case ThreadState.Unstarted:
                        item.Start();
                        break;
                    case ThreadState.Suspended:
                        item.Resume();
                        break;
                    default:

                        break;
                }

            }
        }

        public void StopWork()
        {
            bIsStartWork = false;
            
            WriteLog("Stop work");

            buttonStart.Text = "Start";

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

            threadList.Clear();

            if (config != null && config.useCache)
            {
                if (!Directory.Exists(config.cachepath))
                    Directory.CreateDirectory(config.cachepath);

                imageList.Serializer(config.cachepath + "\\imagelist.bin");
                linkList.Serializer(config.cachepath + "\\linklist.bin");

                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(config.cachepath + "\\downloadlist.bin", FileMode.Create, FileAccess.Write);
                formatter.Serialize(stream, downloadList);
                stream.Close();
                stream = new FileStream(config.cachepath + "\\worklist.bin", FileMode.Create, FileAccess.Write);
                formatter.Serialize(stream, workList);
                stream.Close();
            }
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

        private void listView1_Click(object sender, EventArgs e)
        {
            try
            {

                AppRunning = false;


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
            ofd.Filter = "config files(*.conf)|*.conf|All files(*.*)|*.*";
            if( DialogResult.OK == ofd.ShowDialog())
            {
                OpenConfig(ofd.FileName);
            }
        }

        public void OpenConfig(string path)
        {
            if (bIsStartWork)
                StopWork();

            linkList.Clear();
            imageList.Clear();
            workList.Clear();
            downloadList.Clear();
            listgroupmap.Clear();

            listView1.Groups.Clear();
            listView1.Items.Clear();

            listView1.View = View.Details;

            listView1.SetGroupState(ListViewGroupState.Collapsible);

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            config = (DownloadConfig)formatter.Deserialize(stream);
            stream.Close();

            labelInfo.Text = config.workUrl;

            CurrentConfigPath = path;

            FileInfo fi = new FileInfo(path);
            this.Text = fi.Name + " - WebDownload";
        }

        public string GetRealURL( string text, string baseurl, string cururl)
        {
            string url = text;
            char tag = url[0];
            int start, end;

            switch (tag)
            {
                case '\"':
                    start = url.IndexOf("\"", 0);
                    end = url.IndexOf("\"", start + 1);
                    url = url.Substring(start + 1, end - start - 1);
                    break;
                case '\'':
                    start = url.IndexOf("\'", 0);
                    end = url.IndexOf("\'", start + 1);
                    url = url.Substring(start + 1, end - start - 1);
                    break;
            }

            if (string.IsNullOrEmpty(url))
                return "";

            // 支持全局相对路径
            if (url[0] == '/')
            {
                url = baseurl + url;
            }
            // 过滤跳转标记和javascript
            else if (url[0] == '#' || url.IndexOf("javascript") != -1)
            {
                return "";
            }
            // 没有http认为是当前相对路径
            else if (-1 == url.IndexOf("http"))
            {
                url = cururl.Substring(0, cururl.LastIndexOf("/") + 1) + url;
            }

            return url;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if( string.IsNullOrWhiteSpace(CurrentConfigPath ))
            {
                buttonOpen_Click(sender, e);
            }
            Wizard wiz = new Wizard(CurrentConfigPath);
            if( DialogResult.OK == wiz.ShowDialog())
            {
                OpenConfig(CurrentConfigPath);
            }
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
