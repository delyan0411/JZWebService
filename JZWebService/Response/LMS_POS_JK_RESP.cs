using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JZWebService.Response
{
    public class LMS_POS_JK_RESP
    {
        private string m_MSGCODE;
        
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MSGCODE
        {
            get { return m_MSGCODE; }
            set { m_MSGCODE = value; }
        }


        private string m_MSGTXT;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string MSGTXT
        {
            get { return m_MSGTXT; }
            set { m_MSGTXT = value; }
        }


    }
}