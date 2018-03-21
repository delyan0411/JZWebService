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
    //办公室人员加班审批流程（在集团中心核算成本）
    public class Timer179
    {
        public Timer179()
        {
            System.Timers.Timer t = new System.Timers.Timer(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timer179"]) * 1000);//
            t.Elapsed += new System.Timers.ElapsedEventHandler(Start);//到达时间的时候执行事件；
            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件
        }

        public void Start(object source, System.Timers.ElapsedEventArgs e)
        {
            int flowid = 179;
            //遍历某个规则下面的所有符合条件的flow_run
            //List<FlowRun> frlistbyruleid = flowrunall.Where(t => t.flow_id == flowid).ToList();
            List<Flow179> flow179 = MySqlHelper.ExecuteObjectList<Flow179>(string.Format("SELECT flowr.run_id AS run_id,flowr.run_name AS run_name,byname AS user_id,data_95 AS starttime,data_96 AS endtime, ' ' AS i_versl FROM flow_data_{0} AS flowdata INNER JOIN flow_run AS flowr ON flowdata.run_id = flowr.run_id INNER JOIN USER AS u ON u.USER_ID = flowdata.begin_user WHERE FLOW_ID = {0} AND DEL_FLAG = 0 AND END_TIME IS NOT NULL AND SYNC_TIME IS NULL AND (TIMES<=2 or RETRY=1)", flowid)).ToList();
            for (int i = 0; i < flow179.Count; i++)
            {
                WebReference.SAP_OA_JK17_HEAD oasaphead17 = new WebReference.SAP_OA_JK17_HEAD();
                //人员编号	Head		I_PERNR	NUMC	8			123
                //开始日期 Head        I_BEGDA DATS    8           20170101
                //结束日期 Head        I_ENDDA DATS    8           20170101
                //开始时间 Head        I_BEGUZ TIMS    8           1700
                //结束时间 Head        I_ENDUZ TIMS    8           1800
                //加班补偿类型 head        I_VERSL CHAR    1           "空格：加班调休 1：加班结薪"     
                string st = flow179[i].starttime;
                string et = flow179[i].endtime;
                if (!string.IsNullOrEmpty(st) && !string.IsNullOrEmpty(et))
                {
                    DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                    dtFormat.ShortDatePattern = "yyyy-mm-dd hh24:mi:ss";
                    DateTime starttime = Convert.ToDateTime(st, dtFormat);
                    DateTime endtime = Convert.ToDateTime(et, dtFormat);
                    if (endtime.CompareTo(starttime) > 0)
                    {
                        //开始日期
                        string startdatestr = st.Substring(0, 10).Replace("-", "").Replace("/", "");
                        //结束日期
                        string enddatestr = st.Substring(0, 10).Replace("-", "").Replace("/", "");
                        //开始时间
                        string starttimestr = et.Substring(11, 5).ToString().Replace(":", "");
                        //原始开始时间
                        //DateTime starttimeori = Convert.ToDateTime(tmpnewlinedh[2].Substring(11, 5).ToString());
                        //结束时间
                        string endtimestr = et.Substring(11, 5).ToString().Replace(":", "");
                        double stdaz = Math.Round((endtime - starttime).TotalHours, 1);
                        //原始结束时间
                        //DateTime endtimeori = Convert.ToDateTime(tmpnewlinedh[3].Substring(11, 5).ToString());
                        //oasaphead17.HEAD head = new oasap17.HEAD();
                        oasaphead17.I_PERNR = flow179[i].user_id.ToString();
                        oasaphead17.I_BEGDA = startdatestr;
                        oasaphead17.I_ENDDA = enddatestr;
                        oasaphead17.I_BEGUZ = "";
                        oasaphead17.I_ENDUZ = "";
                        oasaphead17.I_STDAZ = stdaz.ToString();
                        oasaphead17.I_VERSL = "1";
                        WebReference.SAP_OA_JK17_REQ oasap17 = new WebReference.SAP_OA_JK17_REQ();
                        oasap17.HEAD = oasaphead17;
                        //发送ws
                        string retoastring = "";
                        bool ifsuc = true;
                        try
                        {
                            WebReference.OaWebService oa = new WebReference.OaWebService();
                            Logger.Log(JsonHelper.ObjectToJson(oasap17), flowid.ToString());
                            WebReference.DT_OA_OA17_RespITEM[] retoa = oa.SAP_OA_JK_17(oasap17);
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
                        }
                        catch
                        {
                            retoastring = "";
                            ifsuc = false;
                        }
                        MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("UPDATE FLOW_RUN set TIMES=TIMES+1 where RUN_ID={0} ", flow179[i].run_id), null);
                        MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("  INSERT INTO sync_flowlog (run_id,flow_id,run_name,sendlog,receivelog,sendtime) values ({0},{1},'{2}','{3}','{4}',sysdate()) ", flow179[i].run_id, flowid, flow179[i].run_name, JsonHelper.ObjectToJson(oasap17), retoastring), null);
                        if (ifsuc)
                        {
                            int ret = MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("update flow_run set SYNC_TIME=sysdate() where RUN_ID={0} ", flow179[i].run_id), null);
                            //更新数据库表示他已经同步过了并且同步成功了
                            if (ret > 0)
                                Logger.Log(string.Format("RUN_ID={0},已经更新数据库", flow179[i].run_id), flowid.ToString());
                            else
                                Logger.Error(string.Format("RUN_ID={0},更新没有成功", flow179[i].run_id), flowid.ToString());
                        }
                        else
                        {
                            Logger.Log(string.Format("RUN_ID={0},SAP返回错误信息", flow179[i].run_id), flowid.ToString());
                        }
                    }
                }
            }
        }
    }
}
