using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JZWebService.Models
{
    [Serializable]
    public class SAP_LMS_JK04_HEAD
    {
        /// <summary>
        /// 价格类型
        /// </summary>
        public string KSCHL { get; set; }
        /// <summary>
        /// 渠道
        /// </summary>
        public string VTWEG { get; set; }
        /// <summary>
        /// 具体商品
        /// </summary>
        public List<Prices> ITEM { get; set; }
    }
    public class Prices
    {
        /// <summary>
        /// 门店编码
        /// </summary>
        public string KUNNR { get; set; }
        /// <summary>
        /// 价格组   50 线上
        /// </summary>
        public string KONDA { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string MATNR { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string KBETR { get; set; }
    }
}