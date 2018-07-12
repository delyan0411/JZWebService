using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace JZWebService.Util
{
    public class Logger
    {
        public static string LOGDIR = System.Configuration.ConfigurationManager.AppSettings["LOGDIR"].ToString();

        static string ExistPath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        static void Write(string dir, string filename, string msg)
        {
            StreamWriter writer = null;
            try
            {                
                string path = ExistPath(LOGDIR);
                string fullname = path + "\\" + "Log_" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                writer = new StreamWriter(fullname, true, Encoding.UTF8);
                writer.WriteLine(msg);
            }
            catch (Exception)
            {
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                    writer.Dispose();
                }
            }
        }

        public static void Log(params string[] msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("=======================" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "=============================\t\r\n");
            sb.Append(string.Join("\r\n", msg));
            sb.Append("\r\n");
            Write("Log", "", sb.ToString());
        }

        public static void Error(params string[] msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("=======================" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "=============================\t\r\n");
            sb.Append(string.Join("\r\n", msg));
            sb.Append("\r\n");
            Write("Error", "", sb.ToString());
        }

        public static void Debug(params string[] msg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("=======================" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "=============================\t\r\n");
            sb.Append(string.Join("\r\n", msg));
            sb.Append("\r\n");
            Write("Debug", "", sb.ToString());
        }
    }
}
