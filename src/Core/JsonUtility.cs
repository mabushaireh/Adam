using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace i2fam.Core
{
    public static class JsonUtility
    {
        public static string GetJsonString(string url)
        {
            using (WebClient wc = new WebClient())
            {
                var json = wc.DownloadString(url);
                return json;
            }
        }
    }
}
