using Core.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace OAService
{
    public class HttpUtils
    {
        private const int TimeOut = 15000;

        public static string HttpPost(string url, string requestStr)
        {
            string responseStr = string.Empty;

            if (string.IsNullOrEmpty(url))
            {
                throw new Exception("请求路径为空");
            }
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.Timeout = TimeOut;
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "application/x-www-form-urlencoded;charset=gb2312";

            WebResponse webResponse = null;
            StreamWriter streamWriter = null;
            //Encoding myEncoding = Encoding.Default;
            //Encoding myEncoding = Encoding.GetEncoding("gb2312");
            try
            {
                streamWriter = new StreamWriter(request.GetRequestStream());

                streamWriter.Write(requestStr);
                streamWriter.Close();

                webResponse = request.GetResponse();
                if (webResponse != null)
                {
                    StreamReader streamReader = new StreamReader(webResponse.GetResponseStream());
                    responseStr = streamReader.ReadToEnd();
                    streamReader.Close();
                }
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    Logger.Error(e.InnerException.ToString());
                }
            }
            return responseStr;
        }

        public static string HttpGet(string url)
        {
            string responseStr = string.Empty;

            if (string.IsNullOrEmpty(url))
            {
                throw new Exception("请求路径为空");
            }
            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.Timeout = TimeOut;
            request.ContentType = "application/x-www-form-urlencoded";
            //request.ContentType = "application/x-www-form-urlencoded;charset=gb2312";

            WebResponse webResponse = null;
            // StreamWriter streamWriter = null;
            //Encoding myEncoding = Encoding.Default;
            //Encoding myEncoding = Encoding.GetEncoding("gb2312");
            try
            {
                //streamWriter = new StreamWriter(request.GetRequestStream());

                //streamWriter.Write(requestStr);
                //streamWriter.Close();

                webResponse = request.GetResponse();
                if (webResponse != null)
                {
                    StreamReader streamReader = new StreamReader(webResponse.GetResponseStream());
                    responseStr = streamReader.ReadToEnd();
                    streamReader.Close();
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                if (e.InnerException != null)
                {
                    Logger.Error(e.InnerException.ToString());
                }
            }
            return responseStr;
        }
    }
}
