using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace JZWebService.Models
{
    public class SRM_SAP_JK02_HEAD
    {
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LIFNR { get; set; }//供应商编码
        public string NAME1{ get; set; }//供应商名称
        public string STRAS{ get; set; }//供应商地址
        public string ZEMM_FZRE{ get; set; }//供应商联系人
        public string TELCODE { get; set; }//联系电话
        public string lxdw { get; set; }//来向单位
        public string Sup_Type { get; set; }//单位类别
    }
}