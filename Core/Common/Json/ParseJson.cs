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

namespace Core.Common
{
    public class ParseJson
    {
        public ParseJson()
        {
        }

        #region SearchCloseCharIndex
        /// <summary>
        /// 寻找配对的字符
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="openChar"></param>
        /// <param name="closeChar"></param>
        /// <returns></returns>
        private int SearchCloseCharIndex(string source,char openChar, char closeChar)
        {
            int rVal = -1;
            if (string.IsNullOrEmpty(source))
                return rVal;
            if (openChar == closeChar && (openChar == '\'' || openChar=='\"'))
                source = source.Replace("\\\'", "☆☆").Replace("\\\"", "★★");

            char[] charList = source.ToCharArray();
            int openCharCount = 0;
            int closeCharCount = 0;
            
            for (int i=0;i< charList.Length;i++)
            {
                char c = charList[i];
                if (openChar != closeChar)
                {
                    #region 起始字符 != 结束字符的情况
                    if (openChar == c)
                    {
                        openCharCount++;
                    }
                    else if (closeChar == c || i+1 == charList.Length)
                    {
                        closeCharCount++;
                    }

                    if (openCharCount > 0 && openCharCount == closeCharCount)
                    {
                        rVal = i;
                        break;
                    }
                    #endregion
                }
                else if (openChar == closeChar && openChar==c)
                {
                    #region 起始字符 == 结束字符的情况
                    openCharCount++;
                    if (openCharCount == 2)
                    {
                        rVal = i;
                        break;
                    }
                    #endregion
                }
            }

            return rVal;
        }
        #endregion

        #region ParseJsonArray
        /// <summary>
        /// 将Json数组分解成单个Json
        /// </summary>
        private List<string> ParseJsonArray(string jsonArrayString)
        {
            List<string> jsonList = new List<string>();
            if (string.IsNullOrEmpty(jsonArrayString))
                return jsonList;

            jsonArrayString = jsonArrayString.Trim();
            int startIdx = jsonArrayString.IndexOf("{");
            if (startIdx < 0)
                return jsonList;

            int endIdx = this.SearchCloseCharIndex(jsonArrayString, '{', '}');
            if (endIdx < 0 || endIdx <= startIdx)
                return jsonList;

            string item = jsonArrayString.Substring(startIdx + 1, endIdx - startIdx);
            jsonList.Add(item);
            if (endIdx + 1 < jsonArrayString.Length)
            {
                jsonArrayString = jsonArrayString.Substring(endIdx + 1);
                List<string> list = this.ParseJsonArray(jsonArrayString);
                foreach (string json in list)
                    jsonList.Add(json);
            }
            return jsonList;
        }
        #endregion

        #region ParseJsonItem
        /// <summary>
        /// 解析Json的属性
        /// </summary>
        private SortedDictionary<string, string> ParseJsonItem(string jsonString)
        {
            SortedDictionary<string, string> jsonItems = new SortedDictionary<string, string>();
            if (string.IsNullOrEmpty(jsonString))
                return jsonItems;
            jsonString = jsonString.Trim();
            bool isCutStart = false;
            if (jsonString.StartsWith("{"))
            {
                jsonString = jsonString.Substring(1).Trim();
                isCutStart = true;
            }
            if (jsonString.EndsWith("}") && isCutStart)
            {
                jsonString = jsonString.Substring(0, jsonString.Length - 1).Trim();
            }
            char[] charList = jsonString.ToCharArray();
            if (charList.Length < 1)
                return jsonItems;//空字符串,退出
            int startIdx = 0;
            if (charList[0] == '\'' || charList[0] == '\"')
                startIdx = 1;//第一个字符为单撇号或双引号
            int endIdx = jsonString.IndexOf(":");
            if (endIdx <= startIdx)
                return jsonItems;//没有找到冒号或冒号的位置等于起始位置,退出

            string key = jsonString.Substring(startIdx, endIdx - startIdx).Trim();
            if (key.EndsWith("'") || key.EndsWith("\""))
            {
                key = key.Substring(0, key.Length - 1).Trim();
            }
            string val = "";
            //startIdx = endIdx + 1;
            if (endIdx + 1 < jsonString.Length)
            {
                #region 获取值
                jsonString = jsonString.Substring(endIdx + 1).Trim();
                if (jsonString.Length < 1)
                    return jsonItems;//如果截取后只剩下空字串,退出
                charList = jsonString.ToCharArray();
                startIdx = 0;
                switch (charList[0])
                {
                    case '\''://值为单撇号括起来
                    case '\"'://值为双引号括起来
                        startIdx = 1;
                        endIdx = this.SearchCloseCharIndex(jsonString, charList[0], charList[0]);
                        break;
                    case '['://值为数组的情况
                        startIdx = 0;
                        endIdx = this.SearchCloseCharIndex(jsonString, '[', ']') + 1;
                        break;
                    case '{'://值为Json的情况
                        startIdx = 0;
                        endIdx = this.SearchCloseCharIndex(jsonString, '{', '}') + 1;
                        break;
                    default://其它情况(数字)
                        startIdx = 0;
                        endIdx = jsonString.IndexOf(",");
                        if(endIdx<0)
                            endIdx = jsonString.IndexOf("}");
                        if (endIdx < 0)
                            endIdx = jsonString.Length;
                        break;
                }
                if (endIdx > startIdx && endIdx < jsonString.Length)
                {
                    val = jsonString.Substring(startIdx, endIdx - startIdx).Trim().Replace("\\\"", "\"").Replace("\\\'", "\'");
                }
                else if (endIdx > startIdx && endIdx == jsonString.Length)
                {
                    val = jsonString.Substring(startIdx).Trim().Replace("\\\"", "\"").Replace("\\\'", "\'");
                }
                #endregion
            }
            if (!string.IsNullOrEmpty(key) && !jsonItems.ContainsKey(key))
            {
                jsonItems.Add(key, val);
            }
            if (endIdx + 1 < jsonString.Length)
            {
                jsonString = jsonString.Substring(endIdx + 1).Trim();
                if (jsonString.StartsWith(","))
                {
                    jsonString = jsonString.Substring(1).Trim();//如果截取后,是逗号开头的，把逗号截掉
                }
                if (jsonString.Length < 1)
                    return jsonItems;//如果截取后只剩下空字串,退出
                SortedDictionary<string, string> items = this.ParseJsonItem(jsonString);
                foreach (string itemKey in items.Keys)
                {
                    if (!string.IsNullOrEmpty(itemKey) && !jsonItems.ContainsKey(itemKey))
                    {
                        jsonItems.Add(itemKey, items[itemKey]);
                    }
                }
            }

            return jsonItems;
        }
        #endregion

        #region ParseArray
        /// <summary>
        /// 解析json数组
        /// </summary>
        /// <param name="jsonArray"></param>
        /// <returns></returns>
        public List<SortedDictionary<string, string>> ParseArray(string jsonArray)
        {
            List<SortedDictionary<string, string>> list = new List<SortedDictionary<string, string>>();
            List<string> jsonList = this.ParseJsonArray(jsonArray);
            foreach (string json in jsonList)
            {
                list.Add(this.ParseJsonItem(json));
            }
            return list;
        }
        #endregion

        #region ParseArray
        /// <summary>
        /// 解析json数组
        /// </summary>
        /// <param name="jsonList"></param>
        /// <returns></returns>
        public List<SortedDictionary<string, string>> ParseArray(List<string> jsonList)
        {
            List<SortedDictionary<string, string>> list = new List<SortedDictionary<string, string>>();
            foreach (string json in jsonList)
            {
                list.Add(this.ParseJsonItem(json));
            }
            return list;
        }
        #endregion

        #region Parse
        /// <summary>
        /// 解析单个Json
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public SortedDictionary<string, string> Parse(string json)
        {
            return this.ParseJsonItem(json);
        }
        #endregion

        #region GetJsonValue
        public string GetJsonValue(SortedDictionary<string, string> list, string key)
        {
            if (list.ContainsKey(key))
                return list[key];
            return "";
        }
        #endregion
    }
}
