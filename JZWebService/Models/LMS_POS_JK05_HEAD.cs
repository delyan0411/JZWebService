using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace JZWebService.Models
{
    public class LMS_POS_JK05_HEAD
    {
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]

        public string Rk_lsh { get; set; }//入库流水号,作为入库记录唯一号
        public string Rk_no { get; set; }//入库单号
        public string gys_no { get; set; }//供应商编号
        public DateTime rkrq { get; set; }//入库日期
        public string remark { get; set; }//入库单备注
        public string StoreCode { get; set; }//门店编码
        public string GoodsCode { get; set; }//商品编码
        public string BatchID { get; set; }//批次
        public string BatchNo { get; set; }//批号
        public decimal Quantity { get; set; }//商品数量
        public decimal RkPrice { get; set; }//入库价格
        public decimal RkAmount { get; set; }//入库金额
        public DateTime scrq { get; set; }//生产日期
        public string rknote { get; set; }//入库备注

        public string order_no { get; set; }//采购单号
        public int order_num { get; set; }//行项目号
        public DateTime validDate { get; set; }//有效期
        public int rkflag { get; set; }//入库方向

    }


}