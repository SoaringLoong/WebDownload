using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDownload
{
    [Serializable]
    public class DownloadConfig
    {
        public string workUrl;
        public string baseUrl;
        public bool useCache;
        public List<string> pageSplit = new List<string>();
        public string savepath;
        public string cachepath;
        public string UrlKeyWords;
        public string TitleKeyWords;
    }
}
