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
    public class ProductDB
    {
        #region 获取所有有效商品信息
        /// <summary>
        /// 获取所有有效商品信息
        /// </summary>
        /// <param name="size"></param>
        /// <param name="pageIndex"></param>
        /// <param name="dataCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static List<ProductInfo> GetList(int size, int pageIndex, ref int dataCount, ref int pageCount)
        {
            RequestProductBody body = new RequestProductBody() { page_size = size.ToString(), page_no = pageIndex.ToString() };

            var response = DBHelper<ResponseProduct>.GetResponse("GetValidProduct", JsonHelper.ObjectToJson(body));

            if (DBHelper<ResponseProduct>.IsOK(response) && response.body!=null)
            {
                var responseBody = response.body;
                dataCount = responseBody.rec_num;
                pageCount = Utils.GetPageCount(dataCount, size);
                return responseBody.product_list;
            }
            return new List<ProductInfo>();
        }
        #endregion

        #region 获取所有更新的商品信息
        /// <summary>
        /// 获取所有更新的商品信息
        /// </summary>
        /// <param name="size"></param>
        /// <param name="pageIndex"></param>
        /// <param name="dataCount"></param>
        /// <param name="pageCount"></param>
        /// <returns></returns>
        public static List<ProductInfo> GetModifyList(int size, int pageIndex
            , DateTime startDate
            , DateTime endDate
            , ref int dataCount, ref int pageCount)
        {
            List<ProductInfo> list = new List<ProductInfo>();

            DateTime date = startDate;
            if (startDate > endDate)
            {
                startDate = endDate;
                endDate = date;
            }
            RequestModifyProductBody body = new RequestModifyProductBody()
            {
                page_size = size.ToString(),
                page_no = pageIndex.ToString(),
                start_time = startDate.ToString("yyyy-MM-dd") + " 00:00:00",
                end_time = endDate.ToString("yyyy-MM-dd") + " 23:59:59"
            };

            var response = DBHelper<ResponseProduct>.GetResponse("GetModifyProduct", JsonHelper.ObjectToJson(body));

            if (DBHelper<ResponseProduct>.IsOK(response) && response.body != null)
            {
                var responseBody = response.body;
                dataCount = responseBody.rec_num;
                pageCount = Utils.GetPageCount(dataCount, size);
                return responseBody.product_list;
            }

            return new List<ProductInfo>();
        }
        #endregion
    }
}