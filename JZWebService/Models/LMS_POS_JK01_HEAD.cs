using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace JZWebService.Models
{
    //[Serializable]
    public class LMS_POS_JK01_HEAD
    {
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Fh_lsh { get; set; }//发货流水号,作为销售记录唯一号
        public string Order_No { get; set; }//订单号
        public string Plat_Type { get; set; }//平台类型
        public string Order_Flag { get; set; }//订单方向(1为正单，-1为负单)
        public DateTime Order_Date { get; set; }//订单日期
        public decimal Order_Money { get; set; }//订单金额
        public decimal Trans_Money { get; set; }//运费金额
        public decimal Discount_Money { get; set; }//折扣金额
        public string Store_No { get; set; }//销售单位（网上药店门店号）
        public string User_ID { get; set; }//用户ID（线上用户ID）
        public string Order_Remark { get; set; }//订单备注
        public int Pay_Type { get; set; }//支付方式
        public string DeliverType { get; set; }//运输方式
        public string ReceiveMan { get; set; }//收货人
        public string ReceiveTel { get; set; }//联系电话
        public string ReceiveAddress { get; set; }//收货地址
        public string GoodsCode { get; set; }//商品编码
        public decimal Quantity { get; set; }//商品数量
        public decimal SalePrice { get; set; }//售价
        public int Order_Status { get; set; }//订单状态
        public string Express_Type { get; set; }//快递公司
        public string Express_No { get; set; }//快递单号

        public string Batch_NO { get; set; }//批号
        public string Batch_ID { get; set; }//批次号

       

    }
}