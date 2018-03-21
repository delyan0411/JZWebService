using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace JZWebService.Models
{
    public class SRM_SAP_JK01_HEAD
    {
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MATNR { get; set; }//商品编码
        public string MAKTX { get; set; }//物料名称
        public string ZTYMI { get; set; }//通用名称
        public string Spec { get; set; }//商品规格
        public string ZSCQY { get; set; }//生产厂家
        public string ZPZWH { get; set; }//批准文号
        public string ZTXMA { get; set; }//条形码
        public string Status { get; set; }//状态
        
    }
}