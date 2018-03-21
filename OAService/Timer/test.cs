using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Model;
using Core.Data;
using Core.Common;
using System.Globalization;

namespace OAService.Timer
{
    public class test
    {
        public test()
        {
            string uid = "2517";
            WebReference.OaWebService oa = new WebReference.OaWebService();
            WebReference.DT_OA_OA12_RespITEM[] retoa = oa.SAP_OA_JK_12(uid);
            Logger.Log(JsonHelper.ObjectToJson(retoa));
        }
    }
}
