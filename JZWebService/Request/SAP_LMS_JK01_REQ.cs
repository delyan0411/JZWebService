using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JZWebService.Models;

namespace JZWebService.Request
{
    public class SAP_LMS_JK01_REQ
    {
        private string sType;

        private SAP_LMS_JK01_HEAD[] m_HEAD;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string SType
        {
            get
            {
                return sType;
            }

            set
            {
                sType = value;
            }
        }
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SAP_LMS_JK01_HEAD[] HEAD
        {
            get
            {
                return m_HEAD;
            }

            set
            {
                m_HEAD = value;
            }
        }
        
    }
}