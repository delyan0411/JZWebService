using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace JZWebService.Util
{
    public class CacheHelper
    {
        /// <summary>
        /// 创建缓存
        /// </summary>
        /// <param name="key">缓存的Key</param>
        /// <param name="value">缓存的数据</param>
        /// <param name="cacheDependency">依赖项，一般为null</param>
        /// <param name="dateTime">缓存过期时间</param>
        /// <param name="timeSpan">设置缓存是不使用过期还是到时间就过期</param>
        /// <param name="cacheItemPriority">缓存优先级</param>
        /// <param name="cacheItemRemovedCallback">回调方法，一般为null</param>
        /// <returns></returns>
        public bool CreateCache(string key, object value, CacheDependency cacheDependency, DateTime dateTime, TimeSpan timeSpan,
            CacheItemPriority cacheItemPriority, CacheItemRemovedCallback cacheItemRemovedCallback)
        {
            if (string.IsNullOrEmpty(key) || value == null)
            {
                return false;
            }
            HttpRuntime.Cache.Insert(key, value, cacheDependency, dateTime, timeSpan, cacheItemPriority, cacheItemRemovedCallback);
            return true;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetCache(string key)
        {
            return string.IsNullOrEmpty(key) ? null : HttpRuntime.Cache.Get(key);
        }


        /// <summary>
        /// 移除所有缓存
        /// </summary>
        /// <returns></returns>
        public bool RemoveAll()
        {
            IDictionaryEnumerator iDictionaryEnumerator = HttpRuntime.Cache.GetEnumerator();
            while (iDictionaryEnumerator.MoveNext())
            {
                HttpRuntime.Cache.Remove(Convert.ToString(iDictionaryEnumerator.Key));
            }
            return true;
        }
    }
}