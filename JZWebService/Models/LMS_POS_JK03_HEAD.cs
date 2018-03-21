using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace JZWebService.Models
{
    public class LMS_POS_JK03_HEAD
    {
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]

        public string Loss_Lsh { get; set; }//流水号,作为记录唯一号
        public string Stock_Locale { get; set; }//库存地点
        public string Loss_Flag { get; set; }//报损方向（这里值为1）
        public string Loss_No { get; set; }//报损流水号
        public string GoodsCode { get; set; }//商品编码
        public DateTime Loss_Date { get; set; }//报损日期
        public string Batch_NO { get; set; }//报损批号
        public string Batch_ID { get; set; }//报损批次号
        public decimal Quantity { get; set; }//商品数量
        public decimal Amount { get; set; }//报损金额
        public string Loss_Cause { get; set; }//报损原因

        
    }
}