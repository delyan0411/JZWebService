using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Core.Common
{
    public class JsonHelper
    {
        public static string ObjectToJson<T>(object obj)
        {
            string jsonString = string.Empty;

            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T)) ;
            MemoryStream stream = new MemoryStream();

            try
            {
                js.WriteObject(stream,obj);
                jsonString = Encoding.UTF8.GetString(stream.ToArray());
                stream.Close();
                //替换Json的Date字符串
                //string p = @"\\/Date\((\d+)\+\d+\)\\/";
                //MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
                //Regex reg = new Regex(p);
                //jsonString = reg.Replace(jsonString, matchEvaluator);
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
            }
            return jsonString;
        }


        public static T JsonToObject<T>(string json) where T : class
        {
            try
            {
                byte[] bt = Encoding.UTF8.GetBytes(json.Trim());
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
                MemoryStream stream = new MemoryStream(bt);
                T obj = (T)js.ReadObject(stream);
                return obj;
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                Logger.Error(json);
                return default(T);
            }
        }

        /// <summary>
        /// 将时间字符串转为Json时间
        /// </summary>
        private static string ConvertDateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }

        /// <summary>
        /// 将Json序列化的时间由/Date(1294499956278+0800)转为字符串
        /// </summary>
        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }

        public static string ObjectToJson(object obj)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                return js.Serialize(obj);
            }
            catch (Exception e)
            {
                //Logger.Error(e.ToString());
                return string.Empty;
            }
        }

        public static object JsonToObject(string json)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            try
            {
                return js.DeserializeObject(json);
            }
            catch (Exception e)
            {
                //Logger.Error(e.ToString());
                return null;
            }
        }
    }
}
