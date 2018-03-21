using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

using Microsoft.VisualBasic;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace Core.Common
{
    /// <summary>
    /// 工具类 1.0
    /// Version: 1.0
    /// Author: Atai Lu
    /// </summary>
    [Serializable]
    public class Utils
    {
        #region 返回字符串真实长度(一个汉字为两个字符)
        /// <summary>
        /// 返回字符串真实长度(一个汉字为两个字符)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }
        #endregion

        #region 用正则判断是否匹配特定字符
        /// <summary>
        /// 用正则判断是否匹配特定字符(默认单行模式)
        /// </summary>
        /// <param name="val">待验证的字符串</param>
        /// <param name="reg">正则表达式</param>
        /// <param name="opts">匹配模式</param>
        /// <returns></returns>
        public static bool IsMatch(string val, string reg, RegexOptions opts = RegexOptions.Singleline)
        {
            if (string.IsNullOrEmpty(val))
                return false;
            if (opts == RegexOptions.None)
                return new Regex(@reg).IsMatch(val);

            return new Regex(@reg, opts).IsMatch(val);
        }
        #endregion

        #region 是否包含中文字符
        /// <summary>
        /// 是否包含中文字符
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool HasCnChar(string val)
        {
            return IsMatch(val, "[\u4e00-\u9fa5\uf900-\ufa2d]+", RegexOptions.Multiline);
        }
        #endregion

        #region 是否整数(int)
        /// <summary>
        /// 是否整数(int)
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsInt(string val)
        {
            return IsMatch(val, "^-?\\d{1,16}$");
        }
        #endregion

        #region 是否整数(long)
        /// <summary>
        /// 是否整数(long)
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsLong(string val)
        {
            return IsMatch(val, "^-?\\d{1,32}$");
        }
        #endregion

        #region 是否数字
        /// <summary>
        /// 是否数字(不包括科学计数法的数字)
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsNumber(string val)
        {
            return IsMatch(val, "^-?\\d{1,32}(\\.\\d{1,32})?$");
        }
        #endregion

        #region 判断是否以逗号隔开的数字
        /// <summary>
        /// 判断是否以逗号隔开的数字
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsIdList(string val)
        {
            return IsMatch(val, "^\\d+(,\\d+)*$");
        }
        #endregion

        #region 判断是否邮箱地址
        /// <summary>
        /// 判断是否邮箱地址
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsEmail(string val)
        {
            if (val.Length > 100)
                return false;
            return IsMatch(val, "^\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$");
        }
        #endregion

        #region 判断是否手机号码
        /// <summary>
        /// 判断是否手机号码
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsMobile(String val)
        {
            return IsMatch(val, "^1[0-9]{10}$");
        }
        #endregion

        #region IsGuid
        /// <summary>
        /// 是否Guid
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsGuid(string val)
        {
            return IsMatch(val, @"^[a-zA-Z\d]{8}-[a-zA-Z\d]{4}-[a-zA-Z\d]{4}-[a-zA-Z\d]{4}-[a-zA-Z\d]{12}$");
        }
        /// <summary>
        /// 是否Guid
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsGuid(object val)
        {
            return IsGuid(val.ToString());
        }
        /// <summary>
        /// 是否用逗号隔开的guid型字串
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsGuidList(string val)
        {
            if (val.Length < 1)
                return false;
            string[] arr = val.Split(",".ToCharArray());
            for (int i = 0; i < arr.Length; i++)
            {
                if (!IsGuid(arr[i]))
                    return false;
            }
            return true;
        }
        #endregion
        
        #region IsGuidInit
        /// <summary>
        /// 是否 00000000-0000-0000-0000-000000000000
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool IsGuidInit(string val)
        {
            return val.Equals("00000000-0000-0000-0000-000000000000");
        }
        #endregion

        #region 判断密码格式是否正确(非空字符串，6-20位)
        /// <summary>
        /// 判断密码格式是否正确(非空字符串，6-20位)
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsPassword(string val)
        {
            return IsMatch(val, "^[^\\s]{6,20}$");
        }
        #endregion

        #region 判断是否yyyy-MM-dd HH:mm::ss格式的日期
        /// <summary>
        /// 判断是否yyyy-MM-dd HH:mm::ss格式的日期
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsDateTime(String val)
        {
            return IsMatch(val, "^\\d{1,4}-\\d{1,2}-\\d{1,2} \\d{1,2}:\\d{1,2}:\\d{1,2}(\\.\\d{1,3})?$");
        }
        #endregion

        #region 判断是否IP V4
        /// <summary>
        /// 判断是否IP V4
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsIPV4(string ipAddress)
        {
            try
            {
                IPAddress address = IPAddress.Parse(ipAddress);
                return AddressFamily.InterNetwork.Equals(address.AddressFamily);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 判断是否IPV6
        /// <summary>
        /// 判断是否IPV6
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public static bool IsIPV6(string ipAddress)
        {
            try
            {
                IPAddress address = IPAddress.Parse(ipAddress);
                return AddressFamily.InterNetworkV6.Equals(address.AddressFamily);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 判断是否IP地址
        /// <summary>
        /// 判断是否IP地址
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsIP(string val)
        {
            if (IsIPV4(val) || IsIPV6(val))
                return true;
            return false;
        }
        #endregion

        #region 隐藏IP最后一位
        /// <summary>
        /// 隐藏IP最后一位
        /// </summary>
        public static string HiddenIP(string ip)
        {
            if (IsIPV4(ip))
            {
                string[] arr = ip.Split(".".ToCharArray());
                return arr[0] + "." + arr[1] + "." + arr[2] + ".***";
            }
            return "";
        }
        #endregion

        #region IPV4ToLong
        /// <summary>
        /// IPV4ToLong
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static long IPV4ToLong(string val)
        {
            if (!IsIPV4(val))
                return -1;
            //long result = 0;
            string[] arr = val.Split(".".ToCharArray());
            return 255L * 255L * 255L * long.Parse(arr[0])
                + 255L * 255L * long.Parse(arr[1])
                + 255L * long.Parse(arr[2])
                + long.Parse(arr[3]);
        }
        #endregion

        #region 字符串转Long
        /// <summary>
        /// 字符串转Long
        /// </summary>
        /// <param name="val"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static long StrToLong(string val, int def = 0)
        {
            long retVal = def;
            try
            {
                if (Utils.IsNumber(val))
                {
                    try
                    {
                        retVal = long.Parse(val);
                    }
                    catch (Exception)
                    {
                        val = val.Substring(0, val.IndexOf("."));
                        retVal = long.Parse(val);
                    }
                }
            }
            catch (Exception)
            {

            }
            return retVal;
        }
        #endregion

        #region 字符串转int
        /// <summary>
        /// 字符串转int
        /// </summary>
        /// <param name="val"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static int StrToInt(string val, int def = 0)
        {
            int retVal = def;
            try
            {
                if (Utils.IsNumber(val))
                {
                    try
                    {
                        retVal = int.Parse(val);
                    }
                    catch (Exception)
                    {
                        val = val.Substring(0, val.IndexOf("."));
                        retVal = int.Parse(val);
                    }
                }
            }
            catch (Exception)
            {

            }
            return retVal;
        }
        #endregion

        #region 字符串转double
        /// <summary>
        /// 字符串转double
        /// </summary>
        /// <param name="val"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static double StrToDouble(string val, int def = 0)
        {
            double retVal = def;
            try
            {
                if (Utils.IsNumber(val))
                {
                    retVal = double.Parse(val);
                }
            }
            catch (Exception)
            {

            }
            return retVal;
        }
        #endregion

        #region 字符串转float
        /// <summary>
        /// 字符串转float
        /// </summary>
        /// <param name="val"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static float StrToFloat(string val, int def = 0)
        {
            float retVal = def;
            try
            {
                if (Utils.IsNumber(val))
                {
                    retVal = float.Parse(val);
                }
            }
            catch (Exception)
            {

            }
            return retVal;
        }
        #endregion

        #region 字符串转decimal
        /// <summary>
        /// 字符串转decimal
        /// </summary>
        /// <param name="val"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static decimal StrToDecimal(string val, int def = 0)
        {
            decimal retVal = def;
            try
            {
                if (Utils.IsNumber(val))
                {
                    retVal = decimal.Parse(val);
                }
            }
            catch (Exception)
            {

            }
            return retVal;
        }
        #endregion

        #region 字符串转DateTime
        /// <summary>
        /// 字符串转DateTime
        /// </summary>
        /// <param name="val"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static DateTime StrToDateTime(string val, DateTime def)
        {
            DateTime retVal = def;
            try
            {
                if (Utils.IsDateTime(val))
                {
                    if (val.IndexOf(".") > 0)
                        val = val.Substring(0, val.IndexOf("."));
                    retVal = DateTime.Parse(val);
                }
            }
            catch (Exception)
            {

            }
            return retVal;
        }
        public static DateTime StrToDateTime(string val)
        {
            return StrToDateTime(val, DateTime.Parse("1900-01-01"));
        }
        #endregion

        #region 返回相差的时间
        /// <summary>
        /// 返回time2-time1相差的秒数
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static long DateDiffSeconds(DateTime time1, DateTime time2)
        {
            TimeSpan ts = time2 - time1;
            if (ts.TotalSeconds > long.MaxValue)
            {
                return long.MaxValue;
            }
            else if (ts.TotalSeconds < long.MinValue)
            {
                return long.MinValue;
            }
            return (long)ts.TotalSeconds;
        }

        /// <summary>
        /// 返回time2-time1相差的分钟数
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static long DateDiffMinutes(DateTime time1, DateTime time2)
        {
            TimeSpan ts = time2 - time1;
            if (ts.TotalMinutes > long.MaxValue)
            {
                return long.MaxValue;
            }
            else if (ts.TotalMinutes < long.MinValue)
            {
                return long.MinValue;
            }
            return (long)ts.TotalMinutes;
        }

        /// <summary>
        /// 返回time2-time1相差的小时数
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static long DateDiffHours(DateTime time1, DateTime time2)
        {
            TimeSpan ts = time2 - time1;
            if (ts.TotalHours > long.MaxValue)
            {
                return long.MaxValue;
            }
            else if (ts.TotalHours < long.MinValue)
            {
                return long.MinValue;
            }
            return (long)ts.TotalHours;
        }
        /// <summary>
        /// 返回time2-time1相差的天数
        /// </summary>
        /// <param name="time1"></param>
        /// <param name="time2"></param>
        /// <returns></returns>
        public static int DateDiffDays(DateTime time1, DateTime time2)
        {
            TimeSpan ts = time2 - time1;
            if (ts.TotalDays > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalDays < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalDays;
        }
        #endregion
        //
        #region 获取指定时间所在周的星期日的日期，注意：星期日为每周的第一天
        /// <summary>
        /// 获取指定时间所在周的星期日的日期，注意：星期日为每周的第一天
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime GetSunday(DateTime dt)
        {
            //0:星期日 1:星期一 2:星期二 3:星期三
            //4:星期四 5:星期五 6:星期六
            return dt.AddDays(-dt.DayOfWeek.GetHashCode());//
        }
        #endregion
        //
        #region 获取指定日期的星期几
        /// <summary>
        /// 获取指定日期的星期几
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>星期日、星期一、...、星期六</returns>
        public string GetWeekDayString(DateTime dt)
        {
            char[] arr = "日一二三四五六".ToCharArray();
            return "星期" + arr[dt.DayOfWeek.GetHashCode()];
        }
        #endregion

        #region 得到当前时间的时间刻度
        /// <summary>
        /// 得到当前时间的时间刻度
        /// </summary>
        public static long GetNowTicks
        {
            get
            {
                Random r = new Random();
                return DateTime.Now.Ticks + +(long)r.Next(0, 5000);
            }
        }
        #endregion

        #region 得到当前时间-2012年1月1日的时间刻度
        /// <summary>
        /// 得到当前时间-2012年1月1日的时间刻度
        /// </summary>
        public static long Ticks
        {
            get
            {
                Random r = new Random();
                return DateTime.Now.Ticks - DateTime.Parse("2012-01-01 00:00:00").Ticks + (long)r.Next(0, 5000);
            }
        }
        #endregion
        //
        #region yyyy-MM-dd HH:mm:ss格式的当前时间字串
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss格式的当前时间字串
        /// </summary>
        public static string GetNowString
        {
            get
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        #endregion

        #region 获取时间戳
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long GetTimeStamp(DateTime dateTime)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(dateTime - startTime).TotalSeconds;
        }
        // 时间戳转为C#格式时间
        public static DateTime StampToDateTime(string timeStamp)
        {
            DateTime dateTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);

            return dateTime.Add(toNow);
        }
        #endregion

        #region MD5函数
        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5(string str)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }
        #endregion
        //
        #region SHA256函数
        /// <summary>
        /// SHA256函数
        /// </summary>
        /// /// <param name="str">原始字符串</param>
        /// <returns>SHA256结果</returns>
        public static string SHA256(string str)
        {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] Result = Sha256.ComputeHash(SHA256Data);
            return Convert.ToBase64String(Result);//返回长度为44字节的字符串
        }
        #endregion

        #region SHA1加密
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SHA1(string source)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(source);

            var data = System.Security.Cryptography.SHA1.Create().ComputeHash(buffer);
            var sb = new StringBuilder();
            foreach (var t in data)
                sb.Append(t.ToString("X2"));
            return sb.ToString();
        }
        #endregion

        #region Html编码
        /// <summary>
        /// Html编码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string HtmlEncode(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            return HttpUtility.HtmlEncode(input);
        }
        #endregion

        #region Html解码
        /// <summary>
        /// Html解码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string HtmlDecode(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";
            return HttpUtility.HtmlDecode(input);
        }
        #endregion

        #region 创建/删除文件夹
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path">待创建的目录名(物理路径)</param>
        public static void CreateDir(string path)
        {
            if (!Directory.Exists(path))
            {
                //不存在,创建目录
                Directory.CreateDirectory(path);
            }
        }
        //
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="str">物理路径</param>
        public static void DeleteDir(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }
        }
        #endregion
        //
        #region 判断文件是否存在
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path">物理路径</param>
        /// <returns></returns>
        public static bool FileExists(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            string folder = path.Substring(0, path.LastIndexOf("\\"));
            if (!Directory.Exists(folder))
                return false;//目录不存在
            return File.Exists(path);
        }
        #endregion
        //
        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">物理路径</param>
        public static void DeleteFile(string path)
        {
            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);

        }
        #endregion

        #region 格式化字节数字符串
        /// <summary>
        /// 格式化字节数字符串(转换体积单位)
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FormatBytesStr(long bytes)
        {
            if (bytes > 1073741824)
            {
                return ((double)(bytes / 1073741824)).ToString("0") + "G";
            }
            if (bytes > 1048576)
            {
                return ((double)(bytes / 1048576)).ToString("0") + "M";
            }
            if (bytes > 1024)
            {
                return ((double)(bytes / 1024)).ToString("0") + "K";
            }
            return bytes.ToString().Trim() + "Bytes";
        }
        /// <summary>
        /// 单位K
        /// </summary>
        /// <param name="bytes">单位K</param>
        /// <returns></returns>
        public static string FormatKBytesStr(long bytes)
        {
            if (bytes > 1048576)
            {
                return ((double)(bytes / 1048576)).ToString("0") + "G";
            }
            if (bytes > 1024)
            {
                return ((double)(bytes / 1024)).ToString("0") + "M";
            }
            return bytes.ToString().Trim() + "K";
        }
        #endregion
        //
        #region 分割字符串
        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (strContent.IndexOf(strSplit) < 0)
            {
                string[] tmp = { strContent };
                return tmp;
            }
            if (strContent == null || strContent == "")
            {
                string[] tmp = { strContent };
                return tmp;
            }
            RegexOptions options;
            if (Environment.Version.Major == 1)
                options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline;
            else
                options = RegexOptions.IgnoreCase | RegexOptions.Singleline;
            if (@strSplit == @"{$page$}")
                @strSplit = @"\{\$page\$\}";
            return Regex.Split(strContent, @strSplit, options);
        }
        #endregion
        //
        #region 获取随机数
        public static double GetRandom
        {
            get
            {
                Random r = new Random();
                return r.NextDouble() * 1000d;
            }
        }
        #endregion
        //
        #region 产生验证码
        /// <summary>
        /// 产生验证码
        /// </summary>
        /// <param name="len">长度</param>
        /// <returns>验证码</returns>
        public static string GetRandomString(int len)
        {
            int number;
            StringBuilder checkCode = new StringBuilder();

            Random random = new Random();

            for (int i = 0; i < len; i++)
            {
                number = random.Next();

                if (number % 2 == 0)
                {
                    checkCode.Append((char)('0' + (char)(number % 10)));
                }
                else
                {
                    checkCode.Append((char)('A' + (char)(number % 26)));
                }

            }

            return checkCode.ToString();
        }
        #endregion
        //
        #region 得到由当前时间的年、月、日、时、分、秒、毫秒组成的字符串
        /// <summary>
        /// 得到由当前时间的年、月、日、时、分、秒、毫秒组成的字符串
        /// </summary>
        public static string GetDateTimeString
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMddHHmmssff");
            }
        }
        #endregion

        #region 清除html代码
        /// <summary>
        /// 清除html代码
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string ClearHtml(string strHtml, bool clearSpace = false)
        {
            if (strHtml != "")
            {
                strHtml = Regex.Replace(strHtml, @"<script[^>]*>[\s\S]*?</script>", "", GetRegexOptions);//清除所有script标签
                strHtml = Regex.Replace(strHtml, @"<style[^>]*>[\s\S]*?</style>", "", GetRegexOptions);//清除所有style标签

                strHtml = Regex.Replace(strHtml, @"<object[^>]*>[\s\S]*?</object>", "", GetRegexOptions);//清除所有object标签
                strHtml = Regex.Replace(strHtml, @"<embed[^>]*>[\s\S]*?</embed>", "", GetRegexOptions);//清除所有style标签
                strHtml = Regex.Replace(strHtml, @"<\/?[^>]*>", "", GetRegexOptions);//清除所有HTML标签
            }//end if
            if (clearSpace)
            {
                strHtml = Regex.Replace(strHtml, "\\s", "", GetRegexOptions);
                //strHtml = strHtml.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&amp;", "&");
                strHtml = strHtml.Replace("&nbsp;", "");
                //strHtml = HttpContext.Current.Server.HtmlEncode(strHtml);
            }
            return strHtml.Replace("'", "‘");
        }
        #endregion

        #region 格式化内容(替换回车符)
        public static string FormatText(string strHtml, string c = "p")
        {
            if (!string.IsNullOrEmpty(strHtml))
            {
                strHtml = Regex.Replace(strHtml, @"<\/?[^>]*>", "", GetRegexOptions);//清除所有HTML标签
                string[] arr = strHtml.Split('\n');
                StringBuilder sb = new StringBuilder();
                foreach (string val in arr)
                {
                    if (c == "p") sb.AppendLine("<p>" + val.Trim() + "</p>");
                    else sb.AppendLine(val.Trim() + "<br/>");
                }
                strHtml = sb.ToString().Trim();
            }
            return strHtml;
        }
        #endregion

        #region 获得Assembly的信息
        /// <summary>
        /// 获得Assembly版本号
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyVersion()
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            FileVersionInfo myFileVersion = FileVersionInfo.GetVersionInfo(myAssembly.Location);
            return string.Format("{0}.{1}.{2}", myFileVersion.FileMajorPart, myFileVersion.FileMinorPart, myFileVersion.FileBuildPart);
        }

        /// <summary>
        /// 获得Assembly产品名称
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyProductName()
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            FileVersionInfo myFileVersion = FileVersionInfo.GetVersionInfo(myAssembly.Location);
            return myFileVersion.ProductName;
        }

        /// <summary>
        /// 获得Assembly产品版权
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyCopyright()
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            FileVersionInfo myFileVersion = FileVersionInfo.GetVersionInfo(myAssembly.Location);
            return myFileVersion.LegalCopyright;
        }
        #endregion

        #region 写文本
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="filePath">物理路径</param>
        /// <param name="val">文本内容</param>
        public static void WriteText(string filePath, string val)
        {
            //filePath = Utils.GetMapPath(filePath);
            //Utils.coutln(filePath);
            try
            {
                StreamWriter writer = null;
                //
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                writer = File.CreateText(filePath);
                writer.WriteLine(val);
                writer.Close();
                writer.Dispose();
            }
            catch (Exception) { }
        }
        #endregion
        //
        #region 向指定文本追加内容
        /// <summary>
        /// 向指定文本追加内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="val"></param>
        public static void AppenText(string filePath, string val)
        {
            try
            {
                StreamWriter writer = null;
                if (File.Exists(filePath))
                {
                    writer = File.AppendText(filePath);
                }
                else
                {
                    writer = File.CreateText(filePath);
                }
                writer.WriteLine(val);
                writer.Close();
                writer.Dispose();
            }
            catch (Exception) { }
        }
        #endregion
        //
        #region 读取文件
        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="filePath">物理路径</param>
        public static string LoadFile(string filePath, string charset)
        {
            string result = "";
            if (charset == "") charset = "utf-8";
            Encoding encoding = Encoding.GetEncoding(charset);
            try
            {
                if (!File.Exists(filePath))
                {
                    result = "文件不存在...";
                }
                else
                {
                    StreamReader objReader = new StreamReader(filePath, encoding);
                    result = objReader.ReadToEnd();
                    objReader.Close();
                    objReader.Dispose();
                }
            }
            catch (Exception) { }
            return result;
        }
        #endregion

        #region 清空文件夹
        /// <summary>
        /// 清空文件夹
        /// </summary>
        public static void ClearFolder(string path)
        {
            //string dir = this.IndexPath;
            foreach (string d in System.IO.Directory.GetFileSystemEntries(path))
            {
                if (File.Exists(d))
                {
                    FileInfo fi = new FileInfo(d);
                    if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                        fi.Attributes = FileAttributes.Normal;
                    File.Delete(d);//直接删除其中的文件  
                }
                else
                {
                    DirectoryInfo d1 = new DirectoryInfo(d);
                    if (d1.GetFiles().Length != 0)
                    {
                        ClearFolder(d1.FullName);////递归删除子文件夹
                    }
                    System.IO.Directory.Delete(d);
                }
            }
        }
        #endregion

        //
        #region GetRegexOptions
        public static RegexOptions GetRegexOptions
        {
            get
            {
                RegexOptions options;
                if (Environment.Version.Major == 1)
                    options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline;
                else
                    options = RegexOptions.IgnoreCase | RegexOptions.Singleline;
                return options;
            }
        }
        #endregion

        #region 将字符串编码为Base64字符串
        /// <summary>
        /// 将字符串编码为Base64字符串
        /// </summary>
        /// <param name="value">要编码的字符串</param>
        public static string Base64Encode(string value)
        {
            byte[] barray;
            barray = Encoding.Default.GetBytes(value);
            return Convert.ToBase64String(barray);
        }
        #endregion
        //
        #region 解析name1=value1&name2=value2...
        public static Dictionary<string, string> ParseParame(string val)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            if (val == null || val.Trim().Equals("") || val.IndexOf("=") <= 0)
                return list;
            val = val.Replace("&amp;", "§§§§§§");
            string[] arr = val.Split("&".ToCharArray());
            for (int i = 0; i < arr.Length; i++)
            {
                string[] arr2 = arr[i].Split("=".ToCharArray());
                if (arr2.Length == 1)
                {
                    if (!list.ContainsKey(arr2[0]))
                        list.Add(arr2[0], "");
                }
                else if (arr2.Length > 1)
                {
                    if (!list.ContainsKey(arr2[0]))
                        list.Add(arr2[0], arr[i].Substring(arr[i].IndexOf("=") + 1).Replace("§§§§§§", "&"));
                    else
                        list[arr2[0]] = arr[i].Substring(arr[i].IndexOf("=") + 1).Replace("§§§§§§", "&");
                }
            }
            return list;
        }
        #endregion

        #region GetDictionaryValue
        public static string GetDictionaryValue(Dictionary<string, string> list, string key)
        {
            if (list.ContainsKey(key))
                return list[key];
            return "";
        }
        #endregion

        #region 将UNIX时间戳转换成系统时间
        public static DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }
        #endregion

        #region 将c# DateTime时间格式转换为Unix时间戳格式
        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>
        public static long ConvertDateTimeLong(DateTime time)
        {
            return GetTimeStamp(time);
        }
        #endregion

        #region ToUnicode
        /// <summary>
        /// 将字符串转换成Unicode编码
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string ToUnicode(string val)
        {
            string result = "";
            char[] chs = val.ToCharArray();
            foreach (char c in chs)
            {
                string s = char.ConvertToUtf32(c.ToString(), 0).ToString("x");
                string _s = "";
                for (int i = s.Length; i < 4; i++)
                    _s += "0";
                result += @"\u" + _s + s;
            }
            return result;
        }
        #endregion

        #region Unicode解码
        public static string DeUnicode(string val)
        {
            StringBuilder strResult = new StringBuilder();
            if (!string.IsNullOrEmpty(val))
            {
                string[] arr = val.Replace("\\", "").Split('u');
                try
                {
                    for (int i = 1; i < arr.Length; i++)
                    {
                        int charCode = Convert.ToInt32(arr[i], 16);
                        strResult.Append((char)charCode);
                    }
                }
                catch (FormatException)
                {
                    return Regex.Unescape(val);
                }
            }
            return strResult.ToString();
        }
        #endregion

    }
}
