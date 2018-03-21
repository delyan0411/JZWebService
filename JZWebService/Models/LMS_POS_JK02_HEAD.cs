using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace JZWebService.Models
{
    public class LMS_POS_JK02_HEAD
    {
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]

        public string Adjust_Lsh { get; set; }//流水号,作为调整记录唯一号
        public string Adjust_No{ get; set; }//调整单号
        public string Stock_Locale { get; set; }//库存地点
        public string Adjust_Flag { get; set; }//调整方向（1为盘盈，-1盘亏）
        public string GoodsCode { get; set; }//商品编码
        public DateTime Adjust_Date { get; set; }//调整日期
        public string Batch_NO { get; set; }//调整批号
        public string Batch_ID { get; set; }//调整批次号
        public decimal Quantity { get; set; }//商品数量
        public decimal Amount { get; set; }//调整金额
        public string Adjust_Cause { get; set; }//调整原因

        
    }
}