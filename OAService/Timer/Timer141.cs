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
    //九洲大药房_SAP_HR_PT_100_办公室人员请假流程
    public class Timer141
    {
        public Timer141()
        {
            System.Timers.Timer t = new System.Timers.Timer(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timer141"])* 1000);//
            t.Elapsed += new System.Timers.ElapsedEventHandler(Start);//到达时间的时候执行事件；
            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件
        }

        public void Start(object source, System.Timers.ElapsedEventArgs e)
        {
            int flowid = 141;
            //遍历某个规则下面的所有符合条件的flow_run
            //List<FlowRun> frlistbyruleid = flowrunall.Where(t => t.flow_id == flowid).ToList();
            List<Flow141> flow141 = MySqlHelper.ExecuteObjectList<Flow141>(string.Format("SELECT flowr.run_id AS run_id,flowr.run_name AS run_name,byname AS user_id,data_78 AS DATA,data_90 AS i_awart FROM flow_data_{0} AS flowdata INNER JOIN flow_run AS flowr ON flowdata.run_id = flowr.run_id INNER JOIN USER AS u ON u.USER_ID = flowdata.begin_user WHERE FLOW_ID = {0} AND DEL_FLAG = 0 AND data_78!=\"\" AND END_TIME IS NOT NULL AND SYNC_TIME IS NULL AND (TIMES<=2 or RETRY=1) ", flowid)).ToList();
            for (int i = 0; i < flow141.Count; i++)
            {
                List<WebReference.SAP_OA_JK13_HEAD> oa13saplist = new List<WebReference.SAP_OA_JK13_HEAD>();
                //人员编号 ITEM        I_PERNR NUMC    8           123
                //请假日期 ITEM        I_BEGDA DATS    8           20170101
                //是否全天？‘0‘，是；‘1’，否。	ITEM L_HALF  CHAR    1           是否全天？‘0‘，是；‘1’，否。
                //开始时间 ITEM        L_ BEGUZ    TIMS    8           0800
                //结束时间 ITEM        L_ ENDUZ    TIMS    8           1600
                //请假类型编码 ITEM        I_AWART CHAR    4           2000、2010…
                string tmp = flow141[i].data.ToString();
                string awart = "";
                if (Enum.IsDefined(typeof(I_awartType), flow141[i].i_awart))
                {
                    foreach (string awartType in Enum.GetNames(typeof(I_awartType)))
                    {
                        if (flow141[i].i_awart == awartType)
                        {
                            I_awartType at = (I_awartType)Enum.Parse(typeof(I_awartType), awartType);
                            awart = Convert.ToInt32(at).ToString();
                        }
                    }
                }
                else
                    awart = "2000";                
                if (!string.IsNullOrEmpty(tmp))
                {
                    string[] tmpnewline = tmp.Split(Environment.NewLine.ToCharArray());
                    for (int w = 0; w < tmpnewline.Length; w++)
                    {
                        if (!string.IsNullOrEmpty(tmpnewline[w]))
                        {
                            string[] tmpnewlinedh = tmpnewline[w].Split('`');
                            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                            dtFormat.ShortDatePattern = "yyyy-mm-dd hh24:mi:ss";
                            if (!string.IsNullOrEmpty(tmpnewlinedh[2]) && !string.IsNullOrEmpty(tmpnewlinedh[3]))
                            {
                                DateTime starttime = Convert.ToDateTime(tmpnewlinedh[2], dtFormat);
                                DateTime endtime = Convert.ToDateTime(tmpnewlinedh[3], dtFormat);
                                if (endtime.CompareTo(starttime) > 0)
                                {
                                    //2017-08-14 08:30:00
                                    DateTime startdate = DateTime.Parse(starttime.ToShortDateString());
                                    DateTime enddate = DateTime.Parse(endtime.ToShortDateString());
                                    //开始日期
                                    string startdatestr = tmpnewlinedh[2].Substring(0, 10).Replace("-", "").Replace("/", "");
                                    //结束日期
                                    string enddatestr = tmpnewlinedh[3].Substring(0, 10).Replace("-", "").Replace("/", "");
                                    //开始时间
                                    string starttimestr = tmpnewlinedh[2].Substring(11, 5).ToString().Replace(":", "");
                                    //原始开始时间
                                    DateTime starttimeori = Convert.ToDateTime(tmpnewlinedh[2].Substring(11, 5).ToString());
                                    //结束时间
                                    string endtimestr = tmpnewlinedh[3].Substring(11, 5).ToString().Replace(":", "");
                                    //原始结束时间
                                    DateTime endtimeori = Convert.ToDateTime(tmpnewlinedh[3].Substring(11, 5).ToString());
                                    //大于一天
                                    if (enddate.CompareTo(startdate) > 0)
                                    {
                                        TimeSpan ts = enddate - startdate;
                                        int days = ts.Days;
                                        for (int d = 0; d <= days; d++)
                                        {
                                            WebReference.SAP_OA_JK13_HEAD oa13sap = new WebReference.SAP_OA_JK13_HEAD();
                                            oa13sap.I_PERNR = flow141[i].user_id.ToString();
                                            // if (days > 0)
                                            //{
                                            oa13sap.I_BEGDA = startdate.AddDays(d).ToString("yyyyMMdd");
                                            if (d == 0)
                                            {
                                                if (starttimeori <= (Convert.ToDateTime("08:30")))
                                                {
                                                    oa13sap.L_HALF = "0";
                                                    oa13sap.L_BEGUZ = "";
                                                    oa13sap.L_ENDUZ = "";
                                                }
                                                else
                                                {
                                                    oa13sap.L_HALF = "1";
                                                    oa13sap.L_BEGUZ = starttimestr;
                                                    oa13sap.L_ENDUZ = "1700";
                                                }
                                            }
                                            else if (d == days)
                                            {
                                                if (endtimeori >= (Convert.ToDateTime("17:00")))
                                                {
                                                    oa13sap.L_HALF = "0";
                                                    oa13sap.L_BEGUZ = "";
                                                    oa13sap.L_ENDUZ = "";
                                                }
                                                else
                                                {
                                                    oa13sap.L_HALF = "1";
                                                    oa13sap.L_BEGUZ = "0830";
                                                    oa13sap.L_ENDUZ = endtimestr;
                                                }
                                            }
                                            else
                                            {
                                                oa13sap.L_HALF = "0";
                                                oa13sap.L_BEGUZ = "";
                                                oa13sap.L_ENDUZ = "";
                                            }
                                            oa13sap.I_AWART = awart;
                                            oa13saplist.Add(oa13sap);
                                        }
                                    }
                                    else if (enddate.CompareTo(startdate) == 0)
                                    {
                                        WebReference.SAP_OA_JK13_HEAD oa13sap = new WebReference.SAP_OA_JK13_HEAD();
                                        oa13sap.I_PERNR = flow141[i].user_id.ToString();
                                        oa13sap.I_BEGDA = startdatestr;
                                        if (starttimeori <= (Convert.ToDateTime("08:30")) && endtimeori >= (Convert.ToDateTime("17:00")))
                                        {
                                            oa13sap.L_HALF = "0";
                                            oa13sap.L_BEGUZ = "";
                                            oa13sap.L_ENDUZ = "";
                                        }
                                        else
                                        {
                                            oa13sap.L_HALF = "1";
                                            oa13sap.L_BEGUZ = starttimestr;
                                            oa13sap.L_ENDUZ = endtimestr;
                                        }
                                        oa13sap.I_AWART = awart;
                                        oa13saplist.Add(oa13sap);
                                    }
                                }
                            }
                            #region
                            //    oa13sap.I_BEGDA = tmpnewlinedh[0];
                            //if (tmpnewlinedh[1] == "是")
                            //    oa13sap.L_HALF = "0";
                            //else
                            //    oa13sap.L_HALF = "1";
                            //if (tmpnewlinedh[1] == "是")
                            //{
                            //    oa13sap.L_BEGUZ = "";
                            //    oa13sap.L_ENDUZ = "";
                            //}
                            //else
                            //{
                            //    oa13sap.L_BEGUZ = tmpnewlinedh[2];
                            //    if (!string.IsNullOrEmpty(tmpnewlinedh[2]))
                            //    {
                            //        DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                            //        dtFormat.ShortDatePattern = "yyyy-mm-dd hh24:mi:ss";
                            //        oa13sap.L_BEGUZ = Convert.ToDateTime(tmpnewlinedh[2], dtFormat).ToShortTimeString().ToString().Replace(":", "");
                            //    }
                            //    oa13sap.L_ENDUZ = tmpnewlinedh[3];
                            //    if (!string.IsNullOrEmpty(tmpnewlinedh[3]))
                            //    {
                            //        DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                            //        dtFormat.ShortDatePattern = "yyyy-mm-dd hh24:mi:ss";
                            //        oa13sap.L_ENDUZ = Convert.ToDateTime(tmpnewlinedh[3], dtFormat).ToShortTimeString().ToString().Replace(":", "");
                            //    }
                            //}

                            //oa13saplist.Add(oa13sap);
                            #endregion
                        }
                    }
                    if (oa13saplist.Count > 0)
                    {
                        //发送ws
                        string retoastring = "";
                        bool ifsuc = true;
                        try
                        {
                            //todo处理数据转移到ws中去
                            WebReference.OaWebService oa = new WebReference.OaWebService();
                            Logger.Log(JsonHelper.ObjectToJson(oa13saplist), flowid.ToString());
                            WebReference.DT_OA_OA13_RespITEM[] retoa = oa.SAP_OA_JK_13(oa13saplist.ToArray());
                            Logger.Log(JsonHelper.ObjectToJson(retoa), flowid.ToString());
                            foreach (var item in retoa)
                            {
                                if (item.TYPE != "S")
                                {
                                    ifsuc = false;
                                    break;
                                }
                            }
                            retoastring = JsonHelper.ObjectToJson(retoa);
                            MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("UPDATE FLOW_RUN set TIMES=TIMES+1 where RUN_ID={0} ", flow141[i].run_id), null);
                            Logger.Log(string.Format(" INSERT INTO sync_flowlog (run_id,flow_id,run_name,sendlog,receivelog,sendtime) values ({0},{1},'{2}','{3}','{4}',sysdate())", flow141[i].run_id, flowid, flow141[i].run_name, JsonHelper.ObjectToJson(oa13saplist), retoastring));
                            MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format(" INSERT INTO sync_flowlog (run_id,flow_id,run_name,sendlog,receivelog,sendtime) values ({0},{1},'{2}','{3}','{4}',sysdate())", flow141[i].run_id, flowid, flow141[i].run_name, JsonHelper.ObjectToJson(oa13saplist), retoastring), null);
                            if (ifsuc)
                            {
                                int ret = MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("update flow_run set SYNC_TIME=sysdate() where RUN_ID={0} ", flow141[i].run_id), null);
                                //更新数据库表示他已经同步过了并且同步成功了
                                if (ret > 0)
                                    Logger.Log(string.Format("RUN_ID={0},已经更新数据库", flow141[i].run_id), flowid.ToString());
                                else
                                    Logger.Error(string.Format("RUN_ID={0},更新没有成功", flow141[i].run_id), flowid.ToString());
                            }
                            else
                            {
                                Logger.Log(string.Format("RUN_ID={0},SAP返回错误信息", flow141[i].run_id), flowid.ToString());
                            }
                        }
                        catch
                        {
                            retoastring = "更新失败,接口返回错误";
                            ifsuc = false;
                            MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("UPDATE FLOW_RUN set TIMES=TIMES+1 where RUN_ID={0} ", flow141[i].run_id), null);
                            Logger.Log(string.Format(" INSERT INTO sync_flowlog (run_id,flow_id,run_name,sendlog,receivelog,sendtime) values ({0},{1},'{2}','{3}','{4}',sysdate())", flow141[i].run_id, flowid, flow141[i].run_name, JsonHelper.ObjectToJson(oa13saplist), retoastring));
                            MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format(" INSERT INTO sync_flowlog (run_id,flow_id,run_name,sendlog,receivelog,sendtime) values ({0},{1},'{2}','{3}','{4}',sysdate())", flow141[i].run_id, flowid, flow141[i].run_name, JsonHelper.ObjectToJson(oa13saplist), retoastring), null);
                            if (ifsuc)
                            {
                                int ret = MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("update flow_run set SYNC_TIME=sysdate() where RUN_ID={0} ", flow141[i].run_id), null);
                                //更新数据库表示他已经同步过了并且同步成功了
                                if (ret > 0)
                                    Logger.Log(string.Format("RUN_ID={0},已经更新数据库", flow141[i].run_id), flowid.ToString());
                                else
                                    Logger.Error(string.Format("RUN_ID={0},更新没有成功", flow141[i].run_id), flowid.ToString());
                            }
                            else
                            {
                                Logger.Log(string.Format("RUN_ID={0},SAP返回错误信息", flow141[i].run_id), flowid.ToString());
                            }
                        }                      
                    }
                }
            }
        }
    }
}
