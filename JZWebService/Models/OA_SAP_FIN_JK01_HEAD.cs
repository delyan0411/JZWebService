using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace JZWebService.Models
{
    public class OA_SAP_FIN_JK01_HEAD
    {
        ////会计科目
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string KTOPL{ get; set; }//账目表
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SAKNR{ get; set; }//G/L科目号
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string KTOKS{ get; set; }//总账科目组
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string GLACCOUNT_TYPE{ get; set; }//总账科目类型
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string TXT20_ML{ get; set; }//标准的文本
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string TXT50_ML{ get; set; }//文本
        
    }
}