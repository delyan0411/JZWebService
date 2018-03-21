using System;
using System.IO;
using System.Web;
using System.Text;
//using System.Web.UI;
//using System.Drawing;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Core.Common
{
    /// <summary>
    /// 缓存类
    /// Version: 1.0
    /// Author: Atai Lu
    /// Copyright (C) Atai 2012
    /// http://www.hiatai.com/
    /// </summary>
    public class DoCache
    {
        private readonly string _applicationKey = "_AtaiCacheKeys";

        #region 属性
        private long _expires = 600;//默认缓存10分钟;
        private bool _error = false;
        /// <summary>
        /// 是否出错
        /// </summary>
        public bool Error
        {
            get { return _error; }
        }
        private string _errorMsg = "";
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg
        {
            get { return _errorMsg; }
        }
        #endregion
        /// <summary>
        /// DoCache
        /// </summary>
        public DoCache()
        {
        }

        private bool allowCache
        {
            get
            {
                try
                {
                    if (HttpContext.Current == null || HttpContext.Current.Cache == null)
                        return false;
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
        }

        #region GetAtaiCacheKeys
        private SortedList<string, string> GetAtaiCacheKeys()
        {
            object obj = this.GetCache(this._applicationKey, true);
            if (obj == null)
                return new SortedList<string, string>();
            return (SortedList<string, string>)obj;
        }
        private string GetSortedValue(SortedList<string, string> sList, string key)
        {
            if (sList.ContainsKey(key))
                return sList[key];
            return "";
        }
        #endregion

        #region 设置存储所有缓存的Key的缓存
        private void SetAtaiCacheKeys(SortedList<string, string> val)
        {
            if (!allowCache) return;
            //存储7天内创建的所有缓存Key(不包括这个缓存的Key)
            HttpContext.Current.Cache.Insert(this._applicationKey, val, null, DateTime.Now.AddDays(10), TimeSpan.Zero);
        }
        #endregion

        #region 数据缓存函数
        /// <summary>
        /// 数据缓存函数
        /// </summary>
        /// <param name="key">未经过DES.Encode加密的Key</param>
        /// <param name="val">需要缓存的数据</param>
        /// <param name="i">缓存的时间</param>
        public void SetCache(string key, object val, long expires)
        {
            if (!allowCache)
                return;
            if (string.IsNullOrEmpty(key))
                return;
            string _keyTem = key;
            key = Utils.MD5(key);
            HttpContext.Current.Cache.Insert(key, val, null, DateTime.Now.AddSeconds(expires), TimeSpan.Zero);
            //this.SetApplication();
            SortedList<string, string> sList = this.GetAtaiCacheKeys();
            lock (sList)
            {
                if (!sList.ContainsKey(key))
                {
                    sList.Add(key, _keyTem);
                    this.SetAtaiCacheKeys(sList);
                }
            }
        }
        /// <summary>
        /// 数据缓存函数
        /// </summary>
        /// <param name="key">未经过DES.Encode加密的Key</param>
        /// <param name="val"></param>
        public void SetCache(string key, object val)
        {
            this.SetCache(key, val, this._expires);
        }
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="key">未经过DES.Encode加密的Key</param>
        /// <returns></returns>
        public object GetCache(string key)
        {
            return GetCache(key, false);
        }
        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="key">缓存的Key</param>
        /// <param name="isDecryptKey">缓存的Key是否已经被加密(DES.Encode加密过的，此参数为true，否则为false)</param>
        /// <returns></returns>
        public object GetCache(string key, bool isDecryptKey)
        {
            if (!allowCache)
                return null;
            if (string.IsNullOrEmpty(key))
                return new object();
            if (!isDecryptKey) key = Utils.MD5(key);
            
            return HttpContext.Current.Cache[key];
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="key">未经过DES.Encode加密的Key</param>
        /// <returns></returns>
        public void RemoveCache(string key)
        {
            this.RemoveCache(key, false);
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="key">缓存的Key</param>
        /// <param name="isDecryptKey">缓存的Key是否已经被加密(DES.Encode加密过的，此参数为true，否则为false)</param>
        public void RemoveCache(string key, bool isDecryptKey)
        {
            if (!allowCache)
                return;
            if (string.IsNullOrEmpty(key))
                return;
            if (!isDecryptKey) key = Utils.MD5(key);
            if (this.GetCache(key, true) != null)
            {
                HttpContext.Current.Cache.Remove(key);
                //HttpContext.Current.Cache.Insert(key, "", null, DateTime.Now.AddMinutes(-1), TimeSpan.Zero);//清除缓存
                SortedList<string, string> sList = this.GetAtaiCacheKeys();
                if (sList.ContainsKey(key))
                {
                    sList.Remove(key);
                    this.SetAtaiCacheKeys(sList);
                }
            }
        }
        #endregion

        #region GetKeys
        /// <summary>
        /// 返回当前所有缓存的key
        /// </summary>
        /// <param name="isDecryptKey">返回的Key是否为DES.Encode加密过的Key</param>
        /// <returns></returns>
        public List<string> GetKeys(bool isDecryptKey)
        {
            IDictionaryEnumerator enumerator = HttpContext.Current.Cache.GetEnumerator();
            List<string> keys = new List<string>();
            SortedList<string, string> sList = this.GetAtaiCacheKeys();
            while (enumerator.MoveNext())
            {
                string _enumKey = enumerator.Key.ToString();
                string _key = isDecryptKey ? _enumKey : this.GetSortedValue(sList, _enumKey);
                if (!_key.Equals(""))
                    keys.Add(_key);
            }
            return keys;
        }
        #endregion

        #region GetKeys
        /// <summary>
        /// 返回当前所有缓存的key(明文Key,即已被DES.Decode解密过的Key)
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeys()
        {
            return this.GetKeys(false);
        }
        #endregion

        #region RemoveCacheAll
        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void RemoveCacheAll()
        {
            List<string> list = this.GetKeys(true);
            for (int i = 0; i < list.Count; i++)
            {
                this.RemoveCache(list[i], true);
            }
        }
        #endregion

        #region RemoveCacheStartsWith
        /// <summary>
        /// 移除缓存
        /// <param name="startString">键值开头包含的字符串</param>
        /// </summary>
        public void RemoveCacheStartsWith(List<string> startString)
        {
            List<string> list = this.GetKeys(true);
            SortedList<string, string> sList = this.GetAtaiCacheKeys();
            for (int k = 0; k < list.Count; k++)
            {
                string _key = this.GetSortedValue(sList, list[k]);
                for (int i = 0; i < startString.Count; i++)
                {
                    if (_key.StartsWith(startString[i]))
                    {
                        this.RemoveCache(list[k], true);
                    }
                }
            }
        }
        public void RemoveCacheStartsWith(string startString)
        {
            List<string> list = new List<string>();
            list.Add(startString);
            RemoveCacheStartsWith(list);
        }
        #endregion

        #region RemoveCacheEndsWith
        /// <summary>
        /// 移除缓存
        /// <param name="endString">键值结尾包含的字符串</param>
        /// </summary>
        public void RemoveCacheEndsWith(List<string> endString)
        {
            List<string> list = this.GetKeys(true);
            SortedList<string, string> sList = this.GetAtaiCacheKeys();
            for (int k = 0; k < list.Count; k++)
            {
                string _key = this.GetSortedValue(sList, list[k]);
                for (int i = 0; i < endString.Count; i++)
                {
                    if (_key.EndsWith(endString[i]))
                    {
                        this.RemoveCache(list[k], true);
                    }
                }
            }
        }
        public void RemoveCacheEndsWith(string endString)
        {
            List<string> list = new List<string>();
            list.Add(endString);
            RemoveCacheEndsWith(list);
        }
        #endregion

        #region RemoveCacheIndexOf
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="items"></param>
        public void RemoveCacheIndexOf(List<string> items)
        {
            List<string> list = this.GetKeys(true);
            SortedList<string, string> sList = this.GetAtaiCacheKeys();
            for (int k = 0; k < list.Count; k++)
            {
                string _key = this.GetSortedValue(sList, list[k]);
                for (int i = 0; i < items.Count; i++)
                {
                    if (_key.EndsWith(items[i]))
                    {
                        this.RemoveCache(list[k], true);
                    }
                }
            }
        }
        public void RemoveCacheIndexOf(string item)
        {
            List<string> list = new List<string>();
            list.Add(item);
            RemoveCacheIndexOf(list);
        }
        #endregion
    }
}