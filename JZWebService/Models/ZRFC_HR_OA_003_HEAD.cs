using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace JZWebService.Models
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "")]
    public class ZRFC_HR_OA_003_HEAD
    {
        ///<summary>姓名</summary>
        private string m_NACHN;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string NACHN
        {
            get { return m_NACHN; }
            set { m_NACHN = value; }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        ///<summary>员工编号</summary>
        private string m_PERNR;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PERNR
        {
            get { return m_PERNR; }
            set { m_PERNR = value; }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        ///<summary>员工组</summary>
        private string m_PERSG;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PERSG
        {
            get { return m_PERSG; }
            set { m_PERSG = value; }
        }
        ///<summary>员工组文本</summary>
        private string m_PTEXT;
         [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PTEXT
        {
            get { return m_PTEXT; }
            set { m_PTEXT = value; }
        }

        ///<summary>员工子组</summary>
        private string m_PERSK;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PERSK
        {
            get { return m_PERSK; }
            set { m_PERSK = value; }
        }
        ///<summary>员工子组文本</summary>
        private string m_PTEXT1;
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PTEXT1
        {
            get { return m_PTEXT1; }
            set { m_PTEXT1 = value; }
        }

        ///<summary>性别</summary>
        private string m_GESCH;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string GESCH
        {
            get { return m_GESCH; }
            set { m_GESCH = value; }
        }
        ///<summary>电子邮件</summary>
        private string m_USRID_LONG;
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string USRID_LONG
        {
            get { return m_USRID_LONG; }
            set { m_USRID_LONG = value; }
        }
        ///<summary>手机</summary>
        private string m_USRID;
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string USRID
        {
            get { return m_USRID; }
            set { m_USRID = value; }
        }

        ///<summary>所属部门编码</summary> 
        private string m_OBJID_DEP;
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string OBJID_DEP
        {
            get { return m_OBJID_DEP; }
            set { m_OBJID_DEP = value; }
        }
        ///<summary>部门文本</summary> 
        private string m_ORGTX;
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ORGTX
        {
            get { return m_ORGTX; }
            set { m_ORGTX = value; }
        }

        ///<summary>所属GSP部门</summary> 
        private string m_OBJID_COM;
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string OBJID_COM
        {
            get { return m_OBJID_COM; }
            set { m_OBJID_COM = value; }
        }
        ///<summary>GSP小组文本</summary> 
        private string m_STEXT_GSP;

        public string STEXT_GSP
        {
            get { return m_STEXT_GSP; }
            set { m_STEXT_GSP = value; }
        }
        ///<summary>岗位编码</summary> 
        private string m_OBJID_S;
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string OBJID_S
        {
            get { return m_OBJID_S; }
            set { m_OBJID_S = value; }
        }
        ///<summary>岗位文本</summary> 
        private string m_OBJTXT_S;
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string OBJTXT_S
        {
            get { return m_OBJTXT_S; }
            set { m_OBJTXT_S = value; }
        }

        private string m_GBDAT;
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string GBDAT
        {
            get { return m_GBDAT; }
            set { m_GBDAT = value; }
        }

        private string m_BEGDA;
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string BEGDA
        {
            get { return m_BEGDA; }
            set { m_BEGDA = value; }
        }
        private string m_SYSTEM;
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SYSTEM
        {
            get { return m_SYSTEM; }
            set { m_SYSTEM = value; }
        }

        private string m_LOGID;
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LOGID
        {
            get { return m_LOGID; }
            set { m_LOGID = value; }
        }

        private string m_STELL;
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string STELL
        {
            get { return m_STELL; }
            set { m_STELL = value; }
        }

        private string m_STLTX;
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string STLTX
        {
            get { return m_STLTX; }
            set { m_STLTX = value; }
        }

        private string m_STAT2;
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string STAT2
        {
            get { return m_STAT2; }
            set { m_STAT2 = value; }
        }
        

    }
}