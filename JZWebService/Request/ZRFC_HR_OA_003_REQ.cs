using JZWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JZWebService.Request
{

    public class ZRFC_HR_OA_003_REQ{

        private ZRFC_HR_OA_003_HEAD[] m_HEAD;

        [System.Xml.Serialization.XmlElementAttribute("HEAD", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ZRFC_HR_OA_003_HEAD[] HEAD
        {
            get { return m_HEAD; }
            set { m_HEAD = value; }
        }

    }
}