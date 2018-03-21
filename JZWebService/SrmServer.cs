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


namespace JZWebService
{
    public class SrmServer
    {
        
        //采用如下变量进行控制服务允许执行
        public static string srmFlag = System.Configuration.ConfigurationManager.AppSettings["srmserver"].ToString();
        MsgError loginfo = new MsgError();
        public static string srmSqlConnectionString = System.Configuration.ConfigurationManager.AppSettings["SrmSqlConnString"].ToString();
        SrmSqlDbHelper sqlDB = new SrmSqlDbHelper(srmSqlConnectionString);

        public SRM_SAP_JK_RESP SAP_SRM_JK_01(SRM_SAP_JK01_REQ rq)
        {

            SRM_SAP_JK_RESP resp = new SRM_SAP_JK_RESP();

            SRM_SAP_JK01_HEAD[] heads = rq.HEAD;

            List<DbParameter> paramList = new List<DbParameter>();
            string sqlStr = "";
            int num = 0;
            string xmlStr = "";

            foreach (SRM_SAP_JK01_HEAD h in heads)
            {
                sqlStr = "UPDATE Product SET GoodsName=@GoodsName, ComName=@ComName,Spec=@Spec,Manufactor=@Manufactor,LicenseNo=@LicenseNo, BarCode=@BarCode,Status=@Status  WHERE GoodsCode=@GoodsCode";
                
                paramList.Clear();
                paramList.Add(sqlDB.MakeInParam("GoodsName", h.MAKTX));
                paramList.Add(sqlDB.MakeInParam("ComName", h.ZTYMI));
                paramList.Add(sqlDB.MakeInParam("Spec", h.Spec));
                paramList.Add(sqlDB.MakeInParam("Manufactor", h.ZSCQY));
                paramList.Add(sqlDB.MakeInParam("LicenseNo", h.ZPZWH));
                paramList.Add(sqlDB.MakeInParam("BarCode", h.ZTXMA));
                paramList.Add(sqlDB.MakeInParam("Status", h.Status));
                paramList.Add(sqlDB.MakeInParam("GoodsCode", h.MATNR));
                
                ///执行sql
                num = sqlDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());

                if (num == 0)
                {
                    sqlStr = "";
                    xmlStr += "执行插入操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<SRM_SAP_JK01_HEAD>(h) + "\r\n";
                    sqlStr = "INSERT INTO Product(GoodsName,ComName,Spec,Manufactor,LicenseNo,BarCode,Status) VALUE(@GoodsName,@ComName,@Spec,@Manufactor,@LicenseNo,@BarCode,@Status)";

                    num = sqlDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());
                    if (num == 0)
                    {
                        resp.MSGCODE = "-1";
                        resp.MSGTXT = "[执行失败]：" + xmlStr;
                    }
                    else
                    {
                        resp.MSGCODE = "1";
                        resp.MSGTXT = "[执行成功]：" + xmlStr;
                    }
                }
                else
                {
                    xmlStr += "执行更新操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<SRM_SAP_JK01_HEAD>(h) + "\r\n";
                    resp.MSGCODE = "1";
                    resp.MSGTXT = "[执行成功]：" + xmlStr;
                }
                loginfo.loginfor("接口SAP_SRM_JK_01返回信息：" + resp.MSGTXT);
                ////
            }
            return resp;
        }


        public SRM_SAP_JK_RESP SAP_SRM_JK_02(SRM_SAP_JK02_REQ rq)
        {
            SRM_SAP_JK_RESP resp = new SRM_SAP_JK_RESP();

            SRM_SAP_JK02_HEAD[] heads = rq.HEAD;

            List<DbParameter> paramList = new List<DbParameter>();
            string sqlStr = "";
            int num = 0;
            string xmlStr = "";

            foreach (SRM_SAP_JK02_HEAD h in heads)
            {
                sqlStr = "UPDATE SupplyInfor SET  Sup_Name=@Sup_Name,LinkMan=@LinkMan,address=@address,telcode=@telcode, lxdw=@lxdw,Sup_Type=@Sup_Type  WHERE Sup_Code=@Sup_Code";

                h.Sup_Type = "S";
                paramList.Clear();
                paramList.Add(sqlDB.MakeInParam("Sup_Name", h.NAME1));
                paramList.Add(sqlDB.MakeInParam("LinkMan", h.ZEMM_FZRE));
                paramList.Add(sqlDB.MakeInParam("address", h.STRAS));
                paramList.Add(sqlDB.MakeInParam("telcode", h.TELCODE));
                paramList.Add(sqlDB.MakeInParam("lxdw", h.lxdw));
                paramList.Add(sqlDB.MakeInParam("Sup_Type", h.Sup_Type));
                paramList.Add(sqlDB.MakeInParam("Sup_Code", h.LIFNR));
                 ///执行sql
                num = sqlDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());

                if (num == 0)
                {
                    sqlStr = "";
                    xmlStr += "执行插入操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<SRM_SAP_JK02_HEAD>(h) + "\r\n";

                    sqlStr = "INSERT INTO SupplyInfor(Sup_Code,Sup_Name,LinkMan,address,telcode,lxdw,Sup_Type) VALUE(@Sup_Code,@Sup_Name,@LinkMan,@address,@telcode,@lxdw,@Sup_Type)";
                    
                    num = sqlDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());
                    if (num == 0)
                    {
                        resp.MSGCODE = "-1";
                        resp.MSGTXT = "[执行失败]：" + xmlStr;
                    }
                    else
                    {
                        resp.MSGCODE = "1";
                        resp.MSGTXT = "[执行成功]：" + xmlStr;
                    }
                }
                else
                {
                    xmlStr += "执行更新操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<SRM_SAP_JK02_HEAD>(h) + "\r\n";
                    resp.MSGCODE = "1";
                    resp.MSGTXT = "[执行成功]：" + xmlStr;
                }
                loginfo.loginfor("接口SAP_SRM_JK_02返回信息：" + resp.MSGTXT);
                ////
            }
            return resp;
        }


        public SRM_SAP_JK_RESP SAP_SRM_JK_03(SRM_SAP_JK02_REQ rq) {

            SRM_SAP_JK_RESP resp = new SRM_SAP_JK_RESP();

            SRM_SAP_JK02_HEAD[] heads = rq.HEAD;

            List<DbParameter> paramList = new List<DbParameter>();
            string sqlStr = "";
            int num = 0;
            string xmlStr = "";

            foreach (SRM_SAP_JK02_HEAD h in heads)
            {
                sqlStr = "UPDATE SupplyInfor SET  Sup_Name=@Sup_Name,LinkMan=@LinkMan,address=@address,telcode=@telcode, lxdw=@lxdw,Sup_Type=@Sup_Type  WHERE Sup_Code=@Sup_Code";

                h.Sup_Type = "C";
                paramList.Clear();
                paramList.Add(sqlDB.MakeInParam("Sup_Name", h.NAME1));
                paramList.Add(sqlDB.MakeInParam("LinkMan", h.ZEMM_FZRE));
                paramList.Add(sqlDB.MakeInParam("address", h.STRAS));
                paramList.Add(sqlDB.MakeInParam("telcode", h.TELCODE));
                paramList.Add(sqlDB.MakeInParam("lxdw", h.lxdw));
                paramList.Add(sqlDB.MakeInParam("Sup_Type", h.Sup_Type));
                paramList.Add(sqlDB.MakeInParam("Sup_Code", h.LIFNR));
                ///执行sql
                num = sqlDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());

                if (num == 0)
                {
                    sqlStr = "";
                    xmlStr += "执行插入操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<SRM_SAP_JK02_HEAD>(h) + "\r\n";

                    sqlStr = "INSERT INTO SupplyInfor(Sup_Code,Sup_Name,LinkMan,address,telcode,lxdw,Sup_Type) VALUE(@Sup_Code,@Sup_Name,@LinkMan,@address,@telcode,@lxdw,@Sup_Type)";

                    num = sqlDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());
                    if (num == 0)
                    {
                        resp.MSGCODE = "-1";
                        resp.MSGTXT = "[执行失败]：" + xmlStr;
                    }
                    else
                    {
                        resp.MSGCODE = "1";
                        resp.MSGTXT = "[执行成功]：" + xmlStr;
                    }
                }
                else
                {
                    xmlStr += "执行更新操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<SRM_SAP_JK02_HEAD>(h) + "\r\n";
                    resp.MSGCODE = "1";
                    resp.MSGTXT = "[执行成功]：" + xmlStr;
                }
                loginfo.loginfor("接口SAP_SRM_JK_03返回信息：" + resp.MSGTXT);
                ////
            }
            return resp;

        }


         

    }
}