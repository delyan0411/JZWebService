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
        public static string oaSqlConnectionString = System.Configuration.ConfigurationManager.AppSettings["OaSqlConnString"].ToString();
        OaSqlDbHelper sqlDB = new OaSqlDbHelper(oaSqlConnectionString);        
        #region 集团组织架构推送     
        /// <summary>
        /// 集团组织架构推送
        /// </summary>
        /// <param name="ZRFC_HR_OA_001_REQ"></param>
        public void SaveOrUpdateDeptInfo(ZRFC_HR_OA_001_REQ req){
            ZRFC_HR_OA_001_HEAD[] heads = req.HEAD;
            ZRFC_HR_OA_RESP resp = new ZRFC_HR_OA_RESP();
            for (int i = 0; i < heads.Count(); i++)
            {
                Logger.Log("总共部门数:" + heads.Count().ToString());
                ZRFC_HR_OA_001_HEAD entity = heads[i];
                //部门编号
                string sapdept_id = entity.BJID;
                string sappid = entity.PUP;
                string dept_name = entity.STEXT + "(sap)";
                string dept_no = entity.NUM;
                string log = "";
                string xmlStr = XmlSerializeHelper.XmlSerialize<ZRFC_HR_OA_001_HEAD>(entity) + "\r\n";
                log += xmlStr;              
                List<DbParameter> paramList = new List<DbParameter>();
                #region 排除特殊情况
                if (sapdept_id == "10000000")
                    continue;
                string[] stringdeptname = { "挂靠部门", "挂证部门", "虚拟组织" };
                if (Array.IndexOf(stringdeptname, entity.STEXT) > -1)
                {
                    log = log + "排除特殊情况_部门" + "\r\n";
                    //Logger.Log("排除特殊情况_部门");
                    continue;
                }
                // < BJID > 13000000 </ BJID >
                //< STEXT > 杭州九洲大药房连锁有限公司 </ STEXT >
                //< PUP > 10000000 </ PUP >
                //< NUM > 4 </ NUM >

                //< BJID > 50002303 </ BJID >
                //< STEXT > 商采中心 </ STEXT >
                //< PUP > 11000000 </ PUP >
                //< NUM > 23 </ NUM >
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
                #endregion
                //string ret_code;
                //string ret_msg;
                //生成用户账号
                //IN p_username varchar(30),IN p_sapid varchar(30),IN p_sapdeptid int,IN  p_sex varchar(30),IN  p_mobil_no varchar(30),IN  p_birthday varchar(30),IN  p_not_login varchar(30),OUT ret_code INT,OUT ret_msg VARCHAR(30)
                // string tsql = "call proc_sap_addUser(@username,@p_sapid,@p_sapdeptid,@p_sex,@p_mobil_no,@p_birthday,@p_not_login,@ret_code,@ret_msg)";
                string tsql = "call proc_sap_addDept(@p_sapdeptid,@p_sapparentdeptid,@p_deptname)";
                paramList.Clear();
                paramList.Add(oaDB.MakeInParam("p_sapdeptid", sapdept_id));//
                paramList.Add(oaDB.MakeInParam("p_sapparentdeptid", sappid));//           
                paramList.Add(oaDB.MakeInParam("p_deptname", dept_name));//
                //paramList.Add(oaDB.MakeParam("ret_msg", "", ParameterDirection.Output));
                //paramList.Add(oaDB.MakeParam("ret_code", 0, ParameterDirection.Output));                            
                DataSet pResult = oaDB.ExecuteDataset(CommandType.Text, tsql, paramList.ToArray());
                //ret_code = paramList[7].Value.ToString();
                //ret_msg = paramList[8].Value.ToString();
                if (pResult != null && pResult.Tables.Count > 0)
                {
                    DataRowCollection rows = pResult.Tables[0].Rows;
                    if (rows.Count > 0)
                    {
                        DataRow row = rows[0];
                        if (Convert.ToInt16(row[1]) > 0)
                        {
                            // select ret_msg,ret_code,v_uid,p_username,p_sapid,p_sapdeptid,p_sex,p_mobil_no,p_birthday,v_wx_dept_id;
                            CreateDept dept = new CreateDept();
                            //dept.id = Convert.ToInt16(row[4]);
                            dept.name = dept_name;
                            dept.parentid = Convert.ToInt16(row[5]);
                            //更新数据
                            log = log + string.Format("{0}_{1}", dept.name, dept.parentid) + "\r\n";
                            //Logger.Log(string.Format("{0}_{1}", dept.name, dept.parentid));
                            if (Convert.ToInt16(row[1]) == 1)
                            {
                                //添加日志
                                dept.id = Convert.ToInt16(row[4]);
                                AccessTokenManage.UpdateDeptApi(dept, ref log);
                            }
                            //添加数据
                            if (Convert.ToInt16(row[1]) == 2)
                            {
                                int id = AccessTokenManage.CreateDeptApi(dept,ref log);
                                if (id > 0)
                                { 
                                    string updatewxdeptid = "update department set WEIXIN_DEPT_ID=@weixindeptid where  dept_id=@dept_id";
                                    paramList.Clear();
                                    paramList.Add(oaDB.MakeInParam("weixindeptid", id));
                                    paramList.Add(oaDB.MakeInParam("dept_id", Convert.ToInt16(row[2])));
                                    int issuc=oaDB.ExecuteNonQuery(CommandType.Text, updatewxdeptid, paramList.ToArray());
                                    if (issuc == 1)
                                    {
                                        log = log + string.Format("更新成功WEIXIN_DEPT_ID成功,WEIXIN_DEPT_ID{0}", id) + "\r\n";
                                    }
                                    else
                                    {
                                        log = log + "更新WEIXIN_DEPT_ID失败" + "\r\n";
                                    }
                                }
                            }
                        }
                        else
                        {
                            log =log + (string.Format("存储过程返回错误,错误值{0},错误描述{1}", row[0].ToString(), row[1]) + "\r\n");
                            //Logger.Log(string.Format("存储过程返回错误,错误值{0},错误描述{1}", row[0].ToString(), row[1]));
                        }
                        //resp.MSGTXT += xmlStr + row[1].ToString() + "\r\n";
                    }
                }
                else
                {
                    log = log + "存储过程范围值为空" + "\r\n";
                    //Logger.Log("存储过程范围值为空");
                    //resp.MSGTXT += xmlStr + "操作失败没有table" + "\r\n";
                }
                Logger.Log(log);
            }           
        }
        #endregion
        #region 员工信息推送
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
                Logger.Log("总共员工数:"+heads.Count().ToString());
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
                string ORGTX = entity.ORGTX;//所属部门名称
                //string OBJID_COM = entity.OBJID_COM;//所属GSP部门编码
                string OBJID_S = entity.OBJID_S;//岗位编码
                string GBDAT = entity.GBDAT;//生日
                string BEGDA = entity.BEGDA;//入职日期
                string SYSTEM = entity.SYSTEM;//人员账号类别
                //string LOGID = entity.LOGID;//人员账号
                string STELL = entity.STELL;//职务编码
                string STLTX = entity.STLTX;//职务文本
                string STAT2 = entity.STAT2;//在职状态1:离职  2：退休  3：在职
                string log = "";
                #region 排除特殊情况
                List<DbParameter> paramList = new List<DbParameter>();
                string xmlStr = XmlSerializeHelper.XmlSerialize<ZRFC_HR_OA_003_HEAD>(entity) + "\r\n";
                //Logger.Log(xmlStr);
                log = xmlStr;
                string[] idarray = { "4317", "4320", "4322", "4321", "4319", "4323", "4325", "4309", "4310", "4312", "4318", "4311", "4327", "4313", "4314" };
                if (Array.IndexOf(idarray, PERNR.TrimStart('0')) > -1)
                {
                    log = log + "排除特殊情况_人员" + "\r\n";
                    continue;
                }

                string[] stringdeptname = { "挂靠部门", "挂证部门", "虚拟组织" };
                if (Array.IndexOf(stringdeptname, ORGTX) > -1)
                {
                    log = log + "排除特殊情况_部门" + "\r\n";
                    continue;
                }
                #endregion
                #region
                //List<DbParameter> paramList = new List<DbParameter>();
                //string xmlStr = XmlSerializeHelper.XmlSerialize<ZRFC_HR_OA_003_HEAD>(entity) + "\r\n";
                //判断该账号是否存在

                //if (string.IsNullOrEmpty(LOGID))
                //{
                //    LOGID = PERNR;
                //}
                // 获取deptid
                #endregion
                string username = NACHN + "(sap)";
                string p_sapid = PERNR.TrimStart('0');
                string p_sapdeptid = OBJID_DEP;
                string p_sex = GESCH == "1" ? "0" : "1";
                string p_mobil_no = string.IsNullOrEmpty(USRID) ? "" : USRID;
                string p_birthday = string.IsNullOrEmpty(GBDAT) ? "" : GBDAT;
                string p_not_login = string.IsNullOrEmpty(STAT2) ? "0" : (STAT2 == "3" ? "0" : "1");
                //string ret_code;
                //string ret_msg;
                //生成用户账号
                //IN p_username varchar(30),IN p_sapid varchar(30),IN p_sapdeptid int,IN  p_sex varchar(30),IN  p_mobil_no varchar(30),IN  p_birthday varchar(30),IN  p_not_login varchar(30),OUT ret_code INT,OUT ret_msg VARCHAR(30)
                // string tsql = "call proc_sap_addUser(@username,@p_sapid,@p_sapdeptid,@p_sex,@p_mobil_no,@p_birthday,@p_not_login,@ret_code,@ret_msg)";
                string tsql = "call proc_sap_addUser(@username,@p_sapid,@p_sapdeptid,@p_sex,@p_mobil_no,@p_birthday,@p_not_login)";
                paramList.Clear();
                paramList.Add(oaDB.MakeInParam("username", username));//姓名
                paramList.Add(oaDB.MakeInParam("p_sapid", p_sapid));//sapid todo去除前导0            
                paramList.Add(oaDB.MakeInParam("p_sapdeptid", p_sapdeptid));//所属部门编码
                paramList.Add(oaDB.MakeInParam("p_sex", p_sex));//性别
                paramList.Add(oaDB.MakeInParam("p_mobil_no", p_mobil_no));//手机              
                paramList.Add(oaDB.MakeInParam("p_birthday", p_birthday));//生日//在职状态1:离职  2：退休  3：在职
                paramList.Add(oaDB.MakeInParam("p_not_login", p_not_login));//
                                                                            //paramList.Add(oaDB.MakeParam("ret_msg", "", ParameterDirection.Output));
                                                                            //paramList.Add(oaDB.MakeParam("ret_code", 0, ParameterDirection.Output));                           
                log = log + (string.Format("{0}_{1}_{2}_{3}_{4}_{5}_{6}", username, p_sapid, p_sapdeptid, p_sex, p_mobil_no, p_birthday, p_not_login) + "\r\n");
                DataSet pResult = oaDB.ExecuteDataset(CommandType.Text, tsql, paramList.ToArray());
                //ret_code = paramList[7].Value.ToString();
                //ret_msg = paramList[8].Value.ToString();
                if (pResult != null && pResult.Tables.Count > 0)
                {
                    DataRowCollection rows = pResult.Tables[0].Rows;
                    if (rows.Count > 0 )
                    {
                        DataRow row = rows[0];
                        if (Convert.ToInt16(row[1]) > 0)
                        {
                            // select ret_msg,ret_code,v_uid,p_username,p_sapid,p_sapdeptid,p_sex,p_mobil_no,p_birthday,v_wx_dept_id;
                            CreateUser user = new CreateUser();
                            user.userid = row[2].ToString();
                            user.name = row[3].ToString();
                            user.mobile = row[7].ToString();
                            user.gender = GESCH == "1" ? "1" : "2";
                            user.email = "";
                            user.department = new List<int> { Convert.ToInt32(row[9]) };
                            user.to_invite = false;
                            //更新数据
                            //log = log + (string.Format("{0}_{1}_{2}_{3}_{4}", user.userid, user.name, user.mobile, user.gender, Convert.ToInt32(row[9])));
                            if (Convert.ToInt16(row[1]) == 1)
                            {
                                //添加日志
                                if (user.mobile != "")
                                    AccessTokenManage.UpdateUserApi(user, ref log);
                                else
                                    log = log + "手机号为空不更新" + "\r\n";
                                //AccessTokenManage.CreateUserApi(user);
                            }
                            //添加数据
                            if (Convert.ToInt16(row[1]) == 2)
                            {
                                AccessTokenManage.CreateUserApi(user, ref log);
                            }
                        }
                        else
                        {
                            log = log + string.Format("存储过程值{0},描述{1}", row[0].ToString(), row[1]) + "\r\n";
                            //Logger.Log(string.Format("存储过程值{0},描述{1}", row[0].ToString(), row[1]));
                        }
                        //resp.MSGTXT += xmlStr + row[1].ToString() + "\r\n";
                    }
                }
                else
                {
                    log = log + "存储过程范围值为空" + "\r\n";
                    //Logger.Log("存储过程范围值为空");
                    //resp.MSGTXT += xmlStr + "操作失败没有table" + "\r\n";
                }
                Logger.Log(log);
                //loginfo.loginfor(resp.MSGTXT);
            }
            // }
        }
        #endregion
        #region 考勤日志
        public string GetCheckInfoList()
        {
            string start_time = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 00:00:00";
            string end_time = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd") + " 23:59:59";
            List<DbParameter> paramList = new List<DbParameter>();
            string sql = @"SELECT TOP 100 PERCENT  userinfo.badgenumber AS PERNR,CASE attrecabnormite.NewType WHEN 'O' THEN 'P20' WHEN 'I' THEN 'P10' ELSE '' END AS SATZA,
                            CONVERT(varchar(100), attrecabnormite.checktime, 112) AS LDATE, 
                            REPLACE(CONVERT(varchar(100), attrecabnormite.checktime, 108),':','') AS LTIME
                            FROM attrecabnormite ,userinfo 
                            WHERE attrecabnormite.userid = userinfo.userid  
                            AND  attrecabnormite.AttDate>=@start_time 
                            AND  attrecabnormite.AttDate<=@end_time 
                            ORDER BY userinfo.badgenumber,userinfo.defaultdeptid ";
            paramList.Add(sqlDB.MakeInParam("start_time", start_time));
            paramList.Add(sqlDB.MakeInParam("end_time", end_time));
            DataSet dateset = sqlDB.ExecuteDataset(CommandType.Text, sql, paramList.ToArray());
            if (dateset != null && dateset.Tables != null
                   && (dateset.Tables == null || (dateset.Tables.Count != 0 && dateset.Tables[0] != null
                           && dateset.Tables[0].Rows.Count != 0)))
            {
                int size = dateset.Tables[0].Rows.Count;
                SapOa10WebService.SI_OA_OA10_OUTService sapSrv = new SapOa10WebService.SI_OA_OA10_OUTService();

                SapOa10WebService.DT_OA_OA10ITEM[] items = new SapOa10WebService.DT_OA_OA10ITEM[size];

                for (int i = 0; i < dateset.Tables[0].Rows.Count; i++)
                {
                    DataRow row = dateset.Tables[0].Rows[i];
                    SapOa10WebService.DT_OA_OA10ITEM item = new SapOa10WebService.DT_OA_OA10ITEM();
                    item.PERNR = row["PERNR"].ToString();
                    item.SATZA = row["SATZA"].ToString();
                    item.LTIME = row["LTIME"].ToString();
                    item.LDATE = row["LDATE"].ToString();
                    items[i] = item;
                }
                sapSrv.Credentials = new NetworkCredential(sapUser, sapPsw);
                try
                {
                    sapSrv.SI_OA_OA10_OUT(items);
                }
                catch (Exception ex)
                {
                    //loginfo.logerror("接口ZRFC_HR_A01_OUT返回信息：" + ex.Message.ToString());
                }
                //loginfo.loginfor("接口ZRFC_HR_A01_OUT返回信息：执行成功");
            }
            return "";
        }
        #endregion
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
    }
}