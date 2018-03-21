using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;

namespace JZWebService
{
    public class SapWebServiceHelper
    {
        public int demo() {
            /*
            SapWebService.SI_POS_DEMO2_OUTService sapSrv = new SapWebService.SI_POS_DEMO2_OUTService();


            SapWebService.DT_S4_DEMO1_OUT args = new SapWebService.DT_S4_DEMO1_OUT();
            SapWebService.DT_S4_DEMO1_OUTHEADER header = new SapWebService.DT_S4_DEMO1_OUTHEADER();

            SapWebService.DT_S4_DEMO1_OUTHEADERITEM[] items = new SapWebService.DT_S4_DEMO1_OUTHEADERITEM[10];
            SapWebService.DT_S4_DEMO1_OUTHEADERITEM item = new SapWebService.DT_S4_DEMO1_OUTHEADERITEM();

            ///赋值
            /////表头
            header.EBELN = "11111";
            header.BUKRS = "11111";
            header.BSTYP = "11111";

            /////明细
            item.EBELP = "1";
            item.MATNR = "2";
            item.MENGE = "3";
            item.MENGEA = "4";
            items[0] = item;
            header.ITEM = items;

            args.HEADER = header;

            ////sap用户名称，密码
            sapSrv.Credentials = new NetworkCredential("POSCONN", "POS123");


            SapWebService.DT_S4_DEMO1_OUT_RESP resp = sapSrv.SI_POS_DEMO2_OUT(args); ///调用执行
            string rts = resp.MSGTX; ///返回内容
            */
            return 0;
        } 
    }
}