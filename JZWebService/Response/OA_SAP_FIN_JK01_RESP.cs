using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JZWebService.Response
{
    public class OA_SAP_FIN_JK01_RESP
    {
        private string m_MSGID;

        private string m_MSGTX;

        public string MSGID
        {
            get
            {
                return m_MSGID;
            }

            set
            {
                m_MSGID = value;
            }
        }

        public string MSGTX
        {
            get
            {
                return m_MSGTX;
            }

            set
            {
                m_MSGTX = value;
            }
        }
    }
}