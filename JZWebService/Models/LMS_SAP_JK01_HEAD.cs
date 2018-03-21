using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JZWebService.Models
{
    [Serializable]
    public class LMS_SAP_JK01_HEAD
    {
        
        public string FRBNR { get; set; }  // WMS收货单号                                 
        public DateTime BUDAT { get; set; } //凭证日期
        public DateTime BLDAT { get; set; } //过账日期
        public string BKTXT { get; set; }  // 抬头文本
        public string ZYWLX { get; set; }  // 业务类型

        public string ZYDDM { get; set; }  // 移动代码
        public string Z1MAT01 { get; set; }  // 移动类型
        public string ZYDBS { get; set; }  // 移动标识
        public string ZGYSBM { get; set; }  // 供应商

        public List<Prices> ITEM { get; set; }// 具体商品

    }
    public class inProduct
    {
        public string EBELN { get; set; }//采购订单号
        public string EBELP { get; set; }//采购订单行项目号
        public string MATNR { get; set; }//商品编码
        public decimal ERFMG { get; set; }//数量

        public string ERFME { get; set; }//单位
        public string WERKS { get; set; }//地点
        public string LGORT { get; set; }//库存地点
        public string CHARG { get; set; }//批次
        public DateTime HSDAT { get; set; }//生产日期
        public DateTime VFDAT { get; set; }//保质期至

        public string LICHA { get; set; }//供应商批号
        public string ZCGLX { get; set; }//采购类型
        public decimal ZJXQ { get; set; }//近效期
        public string ZGYSZH { get; set; }//供应商或债权人账号
        public string SGTXT { get; set; }//行项目文本

    }
}