using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JZWebService.Models;

namespace JZWebService.Request
{
    public class LMS_POS_JK02_REQ
    {
        private string sType;  //可以默认为LMS

        private LMS_POS_JK02_HEAD[] m_HEAD;

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

        public LMS_POS_JK02_HEAD[] HEAD
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