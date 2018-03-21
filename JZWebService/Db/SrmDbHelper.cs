using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JZWebService.Db
{
    public class SrmDbHelper
    {
        private static int intCommandTimeout = 300;//单位 秒
        public static string srmConn = System.Configuration.ConfigurationManager.AppSettings["srmconnString"].ToString();
    }
}