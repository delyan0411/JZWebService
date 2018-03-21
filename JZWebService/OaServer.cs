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
using System.Net;



namespace JZWebService
{
    public class OaServer
    {
        OaDbHelper oaDB = new OaDbHelper();        
        //采用如下变量进行控制服务允许执行
        public static string oaFlag = System.Configuration.ConfigurationManager.AppSettings["oaserver"].ToString();
        public static string sapUser = System.Configuration.ConfigurationManager.AppSettings["SAPURLUSER"].ToString();
        public static string sapPsw = System.Configuration.ConfigurationManager.AppSettings["SAPURLPSW"].ToString();
        MsgError loginfo = new MsgError();
        //public static string oaSqlConnectionString = System.Configuration.ConfigurationManager.AppSettings["OaSqlConnString"].ToString();
        //OaSqlDbHelper sqlDB = new OaSqlDbHelper(oaSqlConnectionString);
        ///编写对应的方法 
        //public int demo()
        //{
        //    /*
        //    if (oaFlag.Equals("1")) {
        //        System.Console.Write("Hello world");
        //    }
        //    */
        //   // oaDB.GetMysqlRead("select * from tmp001");
           
        // //   IList<OA_SAP_FIN_JK01_HEAD> list=oaDB.ExecuteObjectList<OA_SAP_FIN_JK01_HEAD>("select KTOPL,SAKNR,KTOKS,GLACCOUNT_TYPE,TXT20_ML,TXT50_ML from fn_sap_kjkm");
            
        //    return 1;
        //}
        //public OA_SAP_FIN_JK01_RESP OA_SAP_FIN_JK01(OA_SAP_FIN_JK01_REQ  req)
        //{
            
        //    OA_SAP_FIN_JK01_RESP resp = new OA_SAP_FIN_JK01_RESP();
        //    OA_SAP_FIN_JK01_HEAD[] heads = req.HEAD;
            
        //    List<DbParameter> paramList = new List<DbParameter>();
        //    string sqlStr = "";
        //    int num = 0;
        //    string xmlStr = "";

        //    foreach (OA_SAP_FIN_JK01_HEAD h in heads) {
        //        sqlStr = "UPDATE td_oa_outer.fn_sap_kjkm SET KTOPL=@KTOPL, KTOKS=@KTOKS,SAKNR=@SAKNR,GLACCOUNT_TYPE=@GLACCOUNT_TYPE,TXT20_ML=@TXT20_ML, TXT50_ML=@TXT50_ML  WHERE SAKNR=@SAKNR";

        //        paramList.Clear();
        //        paramList.Add(oaDB.MakeInParam("KTOPL", h.KTOPL));
        //        paramList.Add(oaDB.MakeInParam("KTOKS", h.KTOKS));
        //        paramList.Add(oaDB.MakeInParam("SAKNR", h.SAKNR));
        //        paramList.Add(oaDB.MakeInParam("GLACCOUNT_TYPE", h.GLACCOUNT_TYPE));
        //        paramList.Add(oaDB.MakeInParam("TXT20_ML", h.TXT20_ML));
        //        paramList.Add(oaDB.MakeInParam("TXT50_ML", h.TXT50_ML));

        //         ///执行sql
        //        num = oaDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());

        //        if (num == 0)
        //        {
        //            sqlStr = "";
        //            xmlStr += "执行插入操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<OA_SAP_FIN_JK01_HEAD>(h) +"\r\n";

        //            sqlStr = "INSERT INTO td_oa_outer.fn_sap_kjkm(KTOPL,SAKNR,KTOKS,GLACCOUNT_TYPE,TXT20_ML,TXT50_ML) VALUE(@KTOPL,@SAKNR,@KTOKS,@GLACCOUNT_TYPE,@TXT20_ML,@TXT50_ML)";

        //            num = oaDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());
        //            if (num == 0)
        //            {
        //                resp.MSGID = "-1";
        //                resp.MSGTX = "[执行失败]：" + xmlStr;
        //            }else {
        //                resp.MSGID = "1";
        //                resp.MSGTX = "[执行成功]：" + xmlStr;
        //            }
        //        }else {
        //            xmlStr += "执行更新操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<OA_SAP_FIN_JK01_HEAD>(h) + "\r\n";
        //            resp.MSGID = "1";
        //            resp.MSGTX = "[执行成功]：" + xmlStr;
        //        }
        //        loginfo.loginfor("接口SAP_OA_JK_01返回信息：" + resp.MSGTX );
        //        ////
        //    }
        //    return resp;
        //}
        //public OA_SAP_FIN_JK02_RESP OA_SAP_FIN_JK02(OA_SAP_FIN_JK02_REQ req)
        //{

        //    OA_SAP_FIN_JK02_RESP resp = new OA_SAP_FIN_JK02_RESP();
        //    OA_SAP_FIN_JK02_HEAD[] heads = req.HEAD;

        //    List<DbParameter> paramList = new List<DbParameter>();
        //    string sqlStr = "";
        //    int num = 0;
        //    string xmlStr = "";

        //    foreach (OA_SAP_FIN_JK02_HEAD h in heads)
        //    {

        //        sqlStr = "UPDATE td_oa_outer.fn_sap_cbzx SET KOKRS=@KOKRS,KOSTL=@KOSTL,DATAB=@DATAB,DATBI=@DATBI,KTEXT=@KTEXT, LTEXT=@LTEXT,VERAK=@VERAK,KOSAR=@KOSAR,KHINR=@KHINR, BUKRS=@BUKRS,FUNC_AREA=@FUNC_AREA,WAERS=@WAERS,PRCTR=@PRCTR,BKZKP=@BKZKP,PKZKP=@PKZKP,BKZKS=@BKZKS,PKZKS=@PKZKS, BKZER=@BKZER,PKZER=@PKZER,BKZOB=@BKZOB   WHERE KOSTL=@KOSTL";

        //        paramList.Clear();
        //        paramList.Add(oaDB.MakeInParam("KOKRS", h.KOKRS));
        //        paramList.Add(oaDB.MakeInParam("KOSTL", h.KOSTL));
        //        paramList.Add(oaDB.MakeInParam("DATAB", h.DATAB));
        //        paramList.Add(oaDB.MakeInParam("DATBI", h.DATBI));
        //        paramList.Add(oaDB.MakeInParam("KTEXT", h.KTEXT));
        //        paramList.Add(oaDB.MakeInParam("LTEXT", h.LTEXT));

        //        paramList.Add(oaDB.MakeInParam("VERAK", h.VERAK));
        //        paramList.Add(oaDB.MakeInParam("KOSAR", h.KOSAR));
        //        paramList.Add(oaDB.MakeInParam("KHINR", h.KHINR));
        //        paramList.Add(oaDB.MakeInParam("BUKRS", h.BUKRS));

        //        paramList.Add(oaDB.MakeInParam("FUNC_AREA", h.FUNC_AREA));
        //        paramList.Add(oaDB.MakeInParam("WAERS", h.WAERS));
        //        paramList.Add(oaDB.MakeInParam("PRCTR", h.PRCTR));

        //        paramList.Add(oaDB.MakeInParam("BKZKP", h.BKZKP));
        //        paramList.Add(oaDB.MakeInParam("PKZKP", h.PKZKP));
        //        paramList.Add(oaDB.MakeInParam("BKZKS", h.BKZKS));

        //        paramList.Add(oaDB.MakeInParam("PKZKS", h.PKZKS));
        //        paramList.Add(oaDB.MakeInParam("BKZER", h.BKZER));
        //        paramList.Add(oaDB.MakeInParam("PKZER", h.PKZER));
        //        paramList.Add(oaDB.MakeInParam("BKZOB", h.BKZOB));
                
        //        ///执行sql
        //        num = oaDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());

        //        if (num == 0)
        //        {
        //            sqlStr = "";
        //            xmlStr += "执行插入操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<OA_SAP_FIN_JK02_HEAD>(h) + "\r\n";

        //            sqlStr = "INSERT INTO td_oa_outer.fn_sap_cbzx(KOKRS,KOSTL,DATAB,DATBI,KTEXT,LTEXT,VERAK,KOSAR,KHINR,BUKRS,FUNC_AREA,WAERS,PRCTR,BKZKP,PKZKP,BKZKS,PKZKS,BKZER,PKZER,BKZOB) VALUE(@KOKRS,@KOSTL,@DATAB,@DATBI,@KTEXT,@LTEXT,@VERAK,@KOSAR,@KHINR,@BUKRS,@FUNC_AREA,@WAERS,@PRCTR,@BKZKP,@PKZKP,@BKZKS,@PKZKS,@BKZER,@PKZER,@BKZOB)";
        //            num = oaDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());
        //            if (num == 0)
        //            {
        //                resp.MSGID = "-1";
        //                resp.MSGTX = "[执行失败]：" + xmlStr;
        //            }
        //            else
        //            {
        //                resp.MSGID = "1";
        //                resp.MSGTX = "[执行成功]：" + xmlStr;
        //            }
        //        }
        //        else
        //        {
        //            xmlStr += "执行更新操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<OA_SAP_FIN_JK02_HEAD>(h) + "\r\n";
        //            resp.MSGID = "1";
        //            resp.MSGTX = "[执行成功]：" + xmlStr;
        //        }
        //        loginfo.loginfor("接口SAP_OA_JK_02返回信息：" + resp.MSGTX);
        //        ////
        //    }
        //    return resp;

        //}
        /// <summary>
        /// 集团组织架构推送
        /// </summary>
        /// <param name="ZRFC_HR_OA_001_REQ"></param>
        public void SaveOrUpdateDeptInfo(ZRFC_HR_OA_001_REQ req){
            ZRFC_HR_OA_001_HEAD[] heads = req.HEAD;
            ZRFC_HR_OA_RESP resp = new ZRFC_HR_OA_RESP();    
            int num = 0;
            for (int i = 0; i < heads.Count(); i++) {
                string xmlStr = "";
                ZRFC_HR_OA_001_HEAD entity = heads[i];
                //部门编号
                string bjid = entity.BJID;
                string pup  = entity.PUP;
                string name = entity.STEXT+"(sap)";
                string dept_no  = entity.NUM;
                if (bjid == "10000000")
                    continue;
                // < BJID > 13000000 </ BJID >
                //< STEXT > 杭州九洲大药房连锁有限公司 </ STEXT >
                //< PUP > 10000000 </ PUP >
                //< NUM > 4 </ NUM >
                #region 测试
                //List <DbParameter> paramList = new List<DbParameter>();
                //string sql = "SELECT DEPT_ID FROM department WHERE dept_name=@name";
                //paramList.Add(oaDB.MakeInParam("name", name+ "(新)"));
                //DataSet dataSet = oaDB.ExecuteDataset(CommandType.Text, sql, paramList.ToArray());
                //if (dataSet != null && dataSet.Tables.Count > 0)
                //{
                //    DataRowCollection rows = dataSet.Tables[0].Rows;
                //    if (rows.Count > 0)
                //    {
                //        DataRow row = rows[0];
                //        int DEPT_ID = Int32.Parse(row[0].ToString());
                //        //string SAP_DEPT_ID =row[1].ToString();
                //        string operate_sql = "UPDATE department SET SAP_DEPT_ID=@SAP_DEPT_ID WHERE DEPT_ID=@dept_id";
                //        paramList.Clear();
                //        paramList.Add(oaDB.MakeInParam("DEPT_ID", DEPT_ID));
                //        paramList.Add(oaDB.MakeInParam("SAP_DEPT_ID", bjid));
                //        num = oaDB.ExecuteNonQuery(CommandType.Text, operate_sql, paramList.ToArray());
                //    }
                //}
                #endregion
                //xmlStr = XmlSerializeHelper.XmlSerialize<ZRFC_HR_OA_001_HEAD>(entity) + "\r\n";
                List<DbParameter> paramList = new List<DbParameter>();
                //判断该部门编号是否存在，若存在则更新 若不存在则插入
                string sql = "SELECT DEPT_ID,SAP_DEPT_ID FROM department WHERE SAP_DEPT_ID=@dept_no";
                paramList.Add(oaDB.MakeInParam("dept_no", bjid));
                DataSet dataSet = oaDB.ExecuteDataset(CommandType.Text, sql, paramList.ToArray());
                int parent_dept_id = 0;

                //获取父节点编号
                paramList.Clear();
                paramList.Add(oaDB.MakeInParam("dept_no", pup));
                DataSet pSet = oaDB.ExecuteDataset(CommandType.Text, sql, paramList.ToArray());
                if (pSet != null && pSet.Tables.Count > 0)
                {
                    DataRowCollection rows = pSet.Tables[0].Rows;
                    if (rows.Count > 0)
                    {
                        DataRow row = rows[0];
                        parent_dept_id = Int32.Parse(row[0].ToString());
                    }
                }
                string operate_sql = "";
                if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    operate_sql = "UPDATE department SET DEPT_PARENT=@dept_parent, DEPT_NAME=@dept_name WHERE SAP_DEPT_ID=@dept_id";
                    paramList.Clear();
                    paramList.Add(oaDB.MakeInParam("dept_parent", parent_dept_id));
                    paramList.Add(oaDB.MakeInParam("dept_name", name));
                    //paramList.Add(oaDB.MakeInParam("dept_no", dept_no));
                    paramList.Add(oaDB.MakeInParam("dept_id", bjid));

                    xmlStr += "更新" + "\r\n" + XmlSerializeHelper.XmlSerialize<ZRFC_HR_OA_001_HEAD>(entity) + "\r\n";

                }
                else
                {
                    operate_sql = "INSERT INTO department (DEPT_PARENT, DEPT_NAME, SAP_DEPT_ID) VALUES (@dept_parent, @dept_name, @dept_id)";
                    paramList.Clear();
                    paramList.Add(oaDB.MakeInParam("dept_parent", parent_dept_id));
                    paramList.Add(oaDB.MakeInParam("dept_name", name));
                    //paramList.Add(oaDB.MakeInParam("dept_no", dept_no));
                    paramList.Add(oaDB.MakeInParam("dept_id", bjid));
                    xmlStr += "插入" + "\r\n" + XmlSerializeHelper.XmlSerialize<ZRFC_HR_OA_001_HEAD>(entity) + "\r\n";
                }

                num = oaDB.ExecuteNonQuery(CommandType.Text, operate_sql, paramList.ToArray());
                if (num > 0)
                {
                    resp.MSGCODE = "1";
                    resp.MSGTXT = "[执行成功]：" + xmlStr;
                }
                else
                {
                    resp.MSGCODE = "-1";
                    resp.MSGTXT = "[执行失败]：" + xmlStr;
                }
                loginfo.loginfor(resp.MSGTXT);
            }           
        }
        /// <summary>
        /// 员工信息推送
        /// </summary>
        /// <param name="req"></param>
        public void SaveOrUpdateStaffInfo(ZRFC_HR_OA_003_REQ req)
        {
            ZRFC_HR_OA_003_HEAD[] heads = req.HEAD;
            ZRFC_HR_OA_RESP resp = new ZRFC_HR_OA_RESP();
            // string xmlStr = "";
            for (int i = 0; i < heads.Count(); i++)
            {
                resp.MSGTXT = "";
                ZRFC_HR_OA_003_HEAD entity = heads[i];
                //部门编号
                string NACHN = entity.NACHN; //姓名
                string PERNR = entity.PERNR; //员工编号
                string PERSG = entity.PERSG; //员工组
                string PERSK = entity.PERSK; //员工子组
                string GESCH = entity.GESCH; //性别
                //string ULONG = entity.USRID_LONG; //电子邮件
                string USRID = entity.USRID; //手机
                string OBJID_DEP = entity.OBJID_DEP;//所属部门编码
                //string OBJID_COM = entity.OBJID_COM;//所属GSP部门编码
                string OBJID_S = entity.OBJID_S;//岗位编码
                string GBDAT = entity.GBDAT;//生日
                string BEGDA = entity.BEGDA;//入职日期
                string SYSTEM = entity.SYSTEM;//人员账号类别
                //string LOGID = entity.LOGID;//人员账号
                string STELL = entity.STELL;//职务编码
                string STLTX = entity.STLTX;//职务文本
                string STAT2 = entity.STAT2;//在职状态1:离职  2：退休  3：在职
                                            //GESCH1男2女
                                            //sex0男1女
                                            //if (PERNR.TrimStart('0') == "2191")
                                            //{
                #region
                List<DbParameter> paramList = new List<DbParameter>();
                string xmlStr = XmlSerializeHelper.XmlSerialize<ZRFC_HR_OA_003_HEAD>(entity) + "\r\n";
                //判断该账号是否存在

                //if (string.IsNullOrEmpty(LOGID))
                //{
                //    LOGID = PERNR;
                //}
                // 获取deptid
                string dept_Sql = "SELECT DEPT_ID,SAP_DEPT_ID FROM department WHERE SAP_DEPT_ID=@dept_no";
                int dept_id = 0;

                //获取父节点编号
                paramList.Clear();
                paramList.Add(oaDB.MakeInParam("dept_no", OBJID_DEP));
                DataSet pSet = oaDB.ExecuteDataset(CommandType.Text, dept_Sql, paramList.ToArray());
                if (pSet != null && pSet.Tables.Count > 0)
                {
                    DataRowCollection rows = pSet.Tables[0].Rows;
                    if (rows.Count > 0)
                    {
                        DataRow row = rows[0];
                        dept_id = Int32.Parse(row[0].ToString());
                    }
                }
                ///生成用户账号

                string tsql = "call proc_sap_addUser_copy(@username,@pernr,@deptid,@gesch,@usrid,@gbdat,@begda,@stat2) ";
                paramList.Clear();
                #endregion
                paramList.Add(oaDB.MakeInParam("username", NACHN + "(sap)"));//姓名
                paramList.Add(oaDB.MakeInParam("pernr", PERNR.TrimStart('0')));//sapid todo去除前导0
                                                                               // paramList.Add(oaDB.MakeInParam("loginid", LOGID));                
                paramList.Add(oaDB.MakeInParam("deptid", dept_id));//所属部门编码
                paramList.Add(oaDB.MakeInParam("gesch", GESCH == "2" ? "1" : "0"));//性别
                                                                                   //paramList.Add(oaDB.MakeInParam("USRID_LONG", ULONG));//电子邮件
                paramList.Add(oaDB.MakeInParam("usrid", string.IsNullOrEmpty(USRID) ? "" : USRID));//手机               
                paramList.Add(oaDB.MakeInParam("gbdat", string.IsNullOrEmpty(GBDAT) ? "" : GBDAT));//生日
                paramList.Add(oaDB.MakeInParam("begda", string.IsNullOrEmpty(BEGDA) ? "" : BEGDA));//入职日期
                paramList.Add(oaDB.MakeInParam("stat2", string.IsNullOrEmpty(STAT2) ? "0" : (STAT2 == "3" ? "0" : "1")));//在职状态1:离职  2：退休  3：在职
                DataSet pResult = oaDB.ExecuteDataset(CommandType.Text, tsql, paramList.ToArray());
                if (pResult != null && pResult.Tables.Count > 0)
                {
                    DataRowCollection rows = pResult.Tables[0].Rows;
                    if (rows.Count > 0)
                    {
                        DataRow row = rows[0];
                        resp.MSGTXT += xmlStr + row[1].ToString() + "\r\n";
                    }
                    else
                    {
                        resp.MSGTXT += xmlStr + "操作失败没有row" + "\r\n";
                    }
                }
                else
                {
                    resp.MSGTXT += xmlStr + "操作失败没有table" + "\r\n";
                }
                loginfo.loginfor(resp.MSGTXT);
            }
            // }
        }
       // public string GetCheckInfoList() {
       //     string start_time = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd")+" 00:00:00";
       //     string end_time = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59";
       //     List<DbParameter> paramList = new List<DbParameter>();
       //     string sql = @"SELECT TOP 100 PERCENT  userinfo.badgenumber AS PERNR,CASE attrecabnormite.NewType WHEN 'O' THEN 'P20' WHEN 'I' THEN 'P10' ELSE '' END AS SATZA,
       //                     CONVERT(varchar(100), attrecabnormite.checktime, 112) AS LDATE, 
       //                     REPLACE(CONVERT(varchar(100), attrecabnormite.checktime, 108),':','') AS LTIME
       //                     FROM attrecabnormite ,userinfo 
       //                     WHERE attrecabnormite.userid = userinfo.userid  
       //                     AND  attrecabnormite.AttDate>=@start_time 
       //                     AND  attrecabnormite.AttDate<=@end_time 
       //                     ORDER BY userinfo.badgenumber,userinfo.defaultdeptid ";
       //     paramList.Add(sqlDB.MakeInParam("start_time", start_time));
       //     paramList.Add(sqlDB.MakeInParam("end_time", end_time));
       //     DataSet dateset = sqlDB.ExecuteDataset(CommandType.Text, sql, paramList.ToArray());
       //     if (dateset != null && dateset.Tables != null
       //            && (dateset.Tables == null || (dateset.Tables.Count != 0 && dateset.Tables[0] != null
       //                    && dateset.Tables[0].Rows.Count != 0)))
       //     {
       //         int size = dateset.Tables[0].Rows.Count;
       //         SapOa10WebService.SI_OA_OA10_OUTService sapSrv = new SapOa10WebService.SI_OA_OA10_OUTService();

       //         SapOa10WebService.DT_OA_OA10ITEM[] items = new SapOa10WebService.DT_OA_OA10ITEM[size];

       //         for (int i = 0; i < dateset.Tables[0].Rows.Count; i++)
       //         {
       //             DataRow row = dateset.Tables[0].Rows[i];
       //             SapOa10WebService.DT_OA_OA10ITEM item = new SapOa10WebService.DT_OA_OA10ITEM();
       //             item.PERNR = row["PERNR"].ToString();
       //             item.SATZA = row["SATZA"].ToString();
       //             item.LTIME = row["LTIME"].ToString();
       //             item.LDATE = row["LDATE"].ToString();
       //             items[i] = item;
       //         }
       //         sapSrv.Credentials = new NetworkCredential(sapUser, sapPsw);
       //         try
       //         {
       //             sapSrv.SI_OA_OA10_OUT(items);
       //         }
       //         catch (Exception ex)
       //         {
       //             loginfo.logerror("接口ZRFC_HR_A01_OUT返回信息：" + ex.Message.ToString());
       //         }
       //         loginfo.loginfor("接口ZRFC_HR_A01_OUT返回信息：执行成功" );
       //     }
       //     return "";
       // }
       // //考勤检查，保存前置检查类应用，必须独立写
       // public OA_SAP_JK_RESP OA_SAP_JK_01(string sStaffNO, int iDay)
       // {
       //     OA_SAP_JK_RESP resp = new OA_SAP_JK_RESP();
       //     if (iDay > 10)
       //     {
       //         loginfo.logerror("接口OA_SAP_JK_01返回信息：" + sStaffNO +"|"+iDay.ToString());
       //         resp.MSGCODE ="-1";
       //         resp.MSGTXT = "处理失败";
       //         return resp;
       //     }
       //     else
       //     {
       //         loginfo.loginfor("接口OA_SAP_JK_01返回信息：" + sStaffNO + "|" + iDay.ToString());

       //         resp.MSGCODE ="0";
       //         resp.MSGTXT = "处理成功";
       //         return resp;
       //     }
       // }
       // //保存后置检查处理，实现即时推送，一般应用流程结束，结果推送sap
       // public OA_SAP_JK_RESP OA_SAP_JK_02(string sFlowType, string sRunID)
       // {
       //     OA_SAP_JK_RESP resp = new OA_SAP_JK_RESP();
       //     loginfo.loginfor("接口OA_SAP_JK_02返回信息：" + sFlowType + "|" + sRunID);
       //     resp.MSGCODE = "0";
       //     resp.MSGTXT = "处理成功";
       //     return resp;
       // }
       // //定时推送SAP
       //public int OA_SAP_JOB_01()
       // {
       //     loginfo.loginfor("执行接口OA_SAP_JK_JOB01");
       //     return 0;
       // }
    }
}