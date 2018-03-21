using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace JZWebService.error
{
    public class MsgError
    {
        public static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("JZWebService.Logging");



        public void loginfor(string msg) {
            loginfo.Info(msg);
        }

        public void logerror(string errormsg)
        {
            loginfo.Error(errormsg);
        }

    }



}