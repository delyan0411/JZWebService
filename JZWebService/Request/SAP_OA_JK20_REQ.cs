using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JZWebService.Models;

namespace JZWebService.Request
{
    public class SAP_OA_JK20_REQ
    {
        private SAP_OA_JK20_HEAD m_HEAD;
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SAP_OA_JK20_HEAD HEAD
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