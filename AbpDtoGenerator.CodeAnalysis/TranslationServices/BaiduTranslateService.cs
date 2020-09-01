using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace AbpDtoGenerator.CodeAnalysis.TranslationServices
{
    public static class BaiduTranslateService
    {
        public static TransRoot GetTranslate(string text, BaiduTrranslateEnum source, BaiduTrranslateEnum target)
        {
            string text2 = "20200221000386710";
            string str = "6DXjXjhzXX6PdF07jwrP";
            string str2 = source.ToString();
            string str3 = target.ToString();
            string str4 = new Random().Next(100000).ToString();
            string str5 = BaiduTranslateService.EncryptString(text2 + text + str4 + str);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("http://api.fanyi.baidu.com/api/trans/vip/translate?" + "q=" + HttpUtility.UrlEncode(text) + "&from=" + str2 + "&to=" + str3 + "&appid=" + text2 + "&salt=" + str4 + "&sign=" + str5);
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "text/html;charset=UTF-8";
            httpWebRequest.UserAgent = null;
            httpWebRequest.Timeout = 6000;
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            if (httpWebResponse.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }
            Stream responseStream = httpWebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            string text3 = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();
            TransRoot transRoot = JsonConvert.DeserializeObject<TransRoot>(text3);
            if (string.IsNullOrEmpty(transRoot.to))
            {
                throw new Exception("翻译错误！百度api错误信息：" + text3.ToString());
            }
            return transRoot;
        }

        private static string EncryptString(string str)
        {
            HashAlgorithm hashAlgorithm = MD5.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] array = hashAlgorithm.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in array)
            {
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }
}
