using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using Core.Common;
using Core.Model;


namespace Core
{
    public class Settings
    {
        /// <summary>
        /// 所有配置、日志、索引存储的根目录
        /// </summary>
        public static string RootPath
        {
            get{ return System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString(); }
        }

        #region 获取目录
        private static string FormatRootPath()
        {
            string path = RootPath.Replace("/", "\\");
            if (!path.EndsWith("\\")) path += "\\";

            return path;
        }
        /// <summary>
        /// 配置目录
        /// </summary>
        public static string SettingsPath
        {
            get
            {
                return FormatRootPath() + "Settings\\";
            }
        }
        /// <summary>
        /// 日志目录
        /// </summary>
        public static string LogsPath
        {
            get
            {
                return FormatRootPath() + "Logs\\";
            }
        }
        #endregion

        #region 读取规则目录下的文件
        /// <summary>
        /// 读取规则目录下的文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        private static T ParseSettings<T>(string fileName)
        {
            try
            {

                string path = SettingsPath + fileName;
                //T obj = (T)js.ReadObject(stream);
                string cacheKey = "Settings$" + path;
                DoCache _cache = new DoCache();
                if (_cache.GetCache(cacheKey) != null)
                {
                    return (T)_cache.GetCache(cacheKey);
                }
                string json = Utils.LoadFile(path, "utf-8");
                byte[] bt = Encoding.UTF8.GetBytes(json.Trim());
                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(T));
                MemoryStream stream = new MemoryStream(bt);
                T obj = (T)js.ReadObject(stream);
                _cache.SetCache(cacheKey, obj, 3600);
                return obj;
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
                //Console.WriteLine(SettingsPath);
                //throw(e);
            }
            return default(T);
        }
        #endregion

        #region 获取OA同步规则
        /// <summary>
        /// 获取OA同步规则
        /// </summary>
        public static RuleInfo GetRuleList
        {
            get
            {
                return ParseSettings<RuleInfo>("rulelist.json");
            }
        }
        #endregion
    }
}
