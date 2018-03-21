using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JZWebService.Models
{
    [Serializable]
    public class SAP_LMS_JK03_HEAD
    {
        /// <summary>
        /// 采购订单号
        /// </summary>
        public string EBELN { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public string BSART { get; set; }
        /// <summary>
        /// 供应商编码（门店）
        /// </summary>
        public string LIFNR { get; set; }
        /// <summary>
        /// 公司代码
        /// </summary>
        public string BUKRS { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public string AEDAT { get; set; }
        /// <summary>
        /// 商品明细
        /// </summary>
        public List<Products> ITEM { get; set; }
    }
    public class Products
    {
        /// <summary>
        /// 行号
        /// </summary>
        public string EBELP { get; set; }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string MATNR { get; set; }
        /// <summary>
        /// 商品品名
        /// </summary>
        public string TXZ01 { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal MENGE { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string MEINS { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal NETPR { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal NETWR { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string CHARG { get; set; }
        /// <summary>
        /// 交货日期
        /// </summary>
        public string EINDT { get; set; }


        #region  该两字段接口中暂未体现
        /// <summary>
        /// 产地
        /// </summary>
        public string OPLACE { get; set; }
        /// <summary>
        /// 生产厂商
        /// </summary>
        public string MANUFACTURER { get; set; }
        #endregion
    }
}