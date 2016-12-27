using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Droid_Image
{
    public static class GoogleImage
    {
        public static string GetHtmlCode(string topic)
        {
            var rnd = new Random();
            string url = "https://www.google.com/search?q=" + topic + "&tbm=isch";
            string data = "";

            Uri proxy = WebRequest.DefaultWebProxy.GetProxy(new Uri(url));
            var request = (HttpWebRequest)WebRequest.Create(url);
            
            request.Proxy = new WebProxy(Tools4Libraries.Params.WebProxyHost, proxy.Port);
            request.Proxy.Credentials = new NetworkCredential(Tools4Libraries.Params.WebProxyLogin, Tools4Libraries.Params.WebProxyPassword);
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";

            var response = (HttpWebResponse)request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                if (dataStream == null)
                    return "";
                using (var sr = new StreamReader(dataStream))
                {
                    data = sr.ReadToEnd();
                }
            }
            return data;
        }
        public static List<string> GetUrls(string html)
        {
            var urls = new List<string>();
            string[] tab;

            foreach (string item in Regex.Split(html, "imgurl=http"))
            {
                tab = item.Split('&');
                if (tab.Length > 0 && (tab[0].StartsWith("s://") || tab[0].StartsWith("://")))
                {
                    urls.Add("http" + tab[0]);
                }
            }
            return urls;
        }
        public static byte[] GetImage(string url)
        {
            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    Uri proxy = WebRequest.DefaultWebProxy.GetProxy(new Uri(url));
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    request.Proxy = new WebProxy(Tools4Libraries.Params.WebProxyHost, proxy.Port);
                    request.Proxy.Credentials = new NetworkCredential(Tools4Libraries.Params.WebProxyLogin, Tools4Libraries.Params.WebProxyPassword);
                    var response = (HttpWebResponse)request.GetResponse();
                    
                    using (Stream dataStream = response.GetResponseStream())
                    {
                        if (dataStream == null)
                            return null;
                        using (var sr = new BinaryReader(dataStream))
                        {
                            byte[] bytes = sr.ReadBytes(100000000);

                            return bytes;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            return null;
        }
    }
}
