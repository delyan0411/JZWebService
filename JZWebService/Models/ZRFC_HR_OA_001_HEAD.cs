using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace JZWebService.Models
{

    public class ZRFC_HR_OA_001_HEAD 
    {   

        private string m_BJID;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string BJID
        {
            get { return m_BJID; }
            set { m_BJID = value; }
        }


        private string m_STEXT;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string STEXT
        {
            get { return m_STEXT; }
            set { m_STEXT = value; }
        }

        private string m_PUP;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string PUP
        {
            get { return m_PUP; }
            set { m_PUP = value; }
        }

        private string m_NUM;
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string NUM
        {
            get { return m_NUM; }
            set { m_NUM = value; }
        }

    }
}