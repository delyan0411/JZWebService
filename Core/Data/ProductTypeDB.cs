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
    public class ProductTypeDB
    {
        #region 获取分类列表
        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <param name="is_visible"></param>
        /// <returns></returns>
        public static List<ProductTypeInfo> GetList(int is_visible)
        {
            List<ProductTypeInfo> list = new List<ProductTypeInfo>();
            RequestProductTypeBody body = new RequestProductTypeBody() { is_visible = is_visible.ToString() };
            var response = DBHelper<ResponseProductType>.GetResponse("GetTypeListAll"
                , JsonHelper.ObjectToJson(body));

            if (DBHelper<ResponseProductType>.IsOK(response) && response.body != null)
            {
                var responseBody = response.body;
                return responseBody.type_list;
            }
            return list;
        }
        #endregion

        #region 根据父分类ID获取分类列表
        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static List<ProductTypeInfo> GetList(List<ProductTypeInfo> list, int parentId)
        {
            var data = list.FindAll(em => em.parent_type_id.Equals(parentId.ToString()));
            return data == null ? (new List<ProductTypeInfo>()) : data;
        }
        #endregion

        #region 获取单个分类
        /// <summary>
        /// 获取单个分类
        /// </summary>
        /// <param name="list"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public static ProductTypeInfo GetInfo(List<ProductTypeInfo> list, int typeId)
        {
            var item = list.Find(em => em.product_type_id.Equals(typeId.ToString()));
            return item == null ? new ProductTypeInfo() : item;
        }
        #endregion

        #region 根据父分类ID获取分类列表
        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <param name="list"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public static ProductTypeInfo GetRootType(List<ProductTypeInfo> list, int parentId)
        {
            if (parentId > 0)
            {
                var type = GetInfo(list, parentId);
                var val = Utils.StrToInt(type.parent_type_id);
                if (val == 0)
                {
                    return type;
                }
                return GetRootType(list, val);
            }
            return new ProductTypeInfo();
        }
        #endregion
    }
}
