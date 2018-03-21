using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JZWebService.Db;
using JZWebService.Models;
using System.Configuration;
using JZWebService.Request;
using JZWebService.Response;
using System.Data.Common;
using System.Data;
using JZWebService.Util;
using JZWebService.error;
using System.Data.SqlClient;

namespace JZWebService
{
    public class PosService
    {
        PosDbHelper posDB = new PosDbHelper();

        //采用如下变量进行控制服务允许执行
        public static string jzposFlag = System.Configuration.ConfigurationManager.AppSettings["jzposserver"].ToString();
        public static string laposFlag = System.Configuration.ConfigurationManager.AppSettings["laposserver"].ToString();
        public static string qnposFlag = System.Configuration.ConfigurationManager.AppSettings["qnposserver"].ToString();
        MsgError loginfo = new MsgError();
        public static string posSqlConnectionString = System.Configuration.ConfigurationManager.AppSettings["jzposconnString"].ToString();
        LmsSqlDbHelper sqlDB = new LmsSqlDbHelper(posSqlConnectionString);


        ///编写对应的方法 
        public int demo()
        {
            if (jzposFlag.Equals("1"))
            {
                System.Console.Write("Hello world");
            }
            if (laposFlag.Equals("1"))
            {
                System.Console.Write("Hello world");
            }

            if (qnposFlag.Equals("1"))
            {
                System.Console.Write("Hello world");
            }

            return 1;
        }

        //定时同步数据，直接调用过程处理
        public int POS_SAP_JK_01()
        {
            POS_SAP_JK_RESP resp = new POS_SAP_JK_RESP();
            loginfo.loginfor("定时同步网店自采的采购入库数据至SAP：" + resp.MSGTXT);
            return 0;

        }

        //////
    }
}

