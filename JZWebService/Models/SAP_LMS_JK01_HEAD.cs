using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JZWebService.Models
{
    [Serializable]
    public class SAP_LMS_JK01_HEAD
    {
        /// <summary>
        /// 物料编号
        /// </summary>
        public string MATNR { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string MAKTM { get; set; }
        /// <summary>
        /// 物料组
        /// </summary>
        public string MATKL { get; set; }
        /// <summary>
        /// 通用名 
        /// </summary>
        public string ZTYMI { get; set; }
        /// <summary>
        /// 品牌
        /// </summary>
        public string ZPPAI { get; set; }
        /// <summary>
        /// 批准文号
        /// </summary>
        public string ZPZWH { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string ZGUGE { get; set; }
        /// <summary>
        /// 生产企业名称
        /// </summary>
        public string NAME1 { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public string ZJXSL { get; set; }
        /// <summary>
        /// 低价限价
        /// </summary>
        public string ZDJXJ { get; set; }
        /// <summary>
        /// 中标价 
        /// </summary>
        public string ZZBJA { get; set; }
        /// <summary>
        /// 医保支付价格
        /// </summary>
        public string ZYBZF { get; set; }
        /// <summary>
        /// 企业自主定价
        /// </summary>
        public string ZQYZZ { get; set; }
        /// <summary>
        /// 仓储分类
        /// </summary>
        public string ZCCFE { get; set; }
        /// <summary>
        /// 是否药品
        /// </summary>
        public string ZSFYP { get; set; }
        /// <summary>
        /// 是否医疗器械
        /// </summary>
        public string ZSFYL { get; set; }
        /// <summary>
        /// 是否食品
        /// </summary>
        public string ZSFSP { get; set; }
        /// <summary>
        /// 是否保健食品
        /// </summary>
        public string ZSFBJ { get; set; }

    }
}