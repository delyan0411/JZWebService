using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Laobai.Model;
using Laobai.Core;
using Laobai.Core.Common.Json;
using Laobai.Core.Common;

namespace Laobai.Core.Data
{
    public class WordDB
    {
        #region 获取关键词列表
        /// <summary>
        /// 获取关键词列表
        /// </summary>
        /// <param name="size"></param>
        /// <param name="pageIndex"></param>
        /// <param name="dataCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static List<WordInfo> GetList(int size, int pageIndex, ref int dataCount, ref int pageCount)
        {
            List<WordInfo> list = new List<WordInfo>();
            RequestWordBody body = new RequestWordBody() { page_size = size.ToString(), page_no = pageIndex.ToString() };

            var response = DBHelper<ResponseWord>.GetResponse("GetWords", JsonHelper.ObjectToJson(body));

            if (DBHelper<ResponseWord>.IsOK(response) && response.body != null)
            {
                var responseBody = response.body;
                dataCount = responseBody.rec_num;
                pageCount = Utils.GetPageCount(dataCount, size);
                return responseBody.word_list;
            }
            return list;
        }
        #endregion

        #region 根据关键词，更新使用频率
        /// <summary>
        /// 根据关键词，更新使用频率
        /// </summary>
        /// <param name="words">关键词列表</param>
        /// <param name="userKey">ip + 设备号</param>
        public static void ModifyWordUseFreq(List<string> words, string userKey)
        {
            if (words.Count < 1)
                return;//没有关键词
            string _cacheKey = "ModifyWordUseFreq$" + userKey + "$";
            List<ModifyWord> list = new List<ModifyWord>();
            foreach (string s in words)
            {
                if (string.IsNullOrEmpty(s))
                    continue;
                list.Add(new ModifyWord(s));
                _cacheKey += s;
            }
            DoCache _cache = new DoCache();
            var data = _cache.GetCache(_cacheKey);
            if (data != null)
            {
                return;//缓存存在的情况,不更新
            }
            RequestModifyWordBody body = new RequestModifyWordBody(){ word_list = list };
            var response = DBHelper<ResponseWord>.GetResponse("ModifyWordUseFreq", JsonHelper.ObjectToJson(body), "", false);
            if (DBHelper<ResponseWord>.IsOK(response))
            {
                _cache.SetCache(_cacheKey, 1, 600);//缓存10分钟
            }
        }
        #endregion
    }
}
