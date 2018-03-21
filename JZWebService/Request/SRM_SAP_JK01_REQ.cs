using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JZWebService.Models;

namespace JZWebService.Request
{
    public class SRM_SAP_JK01_REQ
    {
        private string sType;

        private SRM_SAP_JK01_HEAD[] m_HEAD;

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

        public SRM_SAP_JK01_HEAD[] HEAD
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