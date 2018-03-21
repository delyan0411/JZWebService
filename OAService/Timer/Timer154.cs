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
    //九洲大药房_SAP_HR_PT_160_补考勤流程_V2.0
    public class Timer154
    {
        public Timer154()
        {
            System.Timers.Timer t = new System.Timers.Timer(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timer154"]) * 1000);//
            t.Elapsed += new System.Timers.ElapsedEventHandler(Start);//到达时间的时候执行事件；
            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件
        }

        public void Start(object source, System.Timers.ElapsedEventArgs e)
        {
            int flowid = 154;
            //遍历某个规则下面的所有符合条件的flow_run
            //List<FlowRun> frlistbyruleid = flowrunall.Where(t => t.flow_id == flowid).ToList();
            List<Flow154> flow154 = MySqlHelper.ExecuteObjectList<Flow154>(string.Format("SELECT flowr.run_id AS run_id,flowr.run_name AS run_name,byname AS user_id,data_2 as time,data_79 AS SATZA,data_1 AS ABWGR FROM flow_data_{0} AS flowdata INNER JOIN flow_run AS flowr ON flowdata.run_id = flowr.run_id INNER JOIN USER AS u ON u.USER_ID = flowdata.begin_user WHERE FLOW_ID = {0} AND DEL_FLAG = 0  AND END_TIME IS NOT NULL AND SYNC_TIME IS NULL AND (TIMES<=2 or RETRY=1)", flowid)).ToList();
            for (int i = 0; i < flow154.Count; i++)
            {
                WebReference.SAP_OA_JK11_HEAD oasaphead11 = new WebReference.SAP_OA_JK11_HEAD();
                //员工编号 Head        PERNR NUMC    8           123
                //日期 Head        LDATE DATS    8           20170101
                //时间 Head        LTIME TIMS    6           13:30
                //时间类型（上班卡或下班卡）	Head SATZA   CHAR    3           "P10 上班卡
                //P20 下班卡"
                //刷卡类型 Head        ABWGR CHAR    4           "ABWGR = ’0003’ 补卡
                //ABWGR = ’0005外出补卡"
                oasaphead11.PERNR = flow154[i].user_id.ToString();
                oasaphead11.SATZA = flow154[i].satza == "上班卡" ? "P10" : "P20";
                oasaphead11.ABWGR = flow154[i].abwgr == "补卡" ? "0003" : "0005";
                oasaphead11.LDATE = flow154[i].time.Substring(0, 10).Replace("-", "").Replace("/", "");
                //oasaphead11.LTIME = flow154[i].time.Substring(11, 5).ToString().Replace(":", "");
                oasaphead11.LTIME = flow154[i].time.Substring(11).ToString();
                WebReference.SAP_OA_JK11_REQ oasap11 = new WebReference.SAP_OA_JK11_REQ();
                oasap11.HEAD = oasaphead11;
                //发送ws
                string retoastring = "";
                bool ifsuc = true;
                try
                {
                    WebReference.OaWebService oa = new WebReference.OaWebService();
                    Logger.Log(JsonHelper.ObjectToJson(oasap11), flowid.ToString());
                    WebReference.DT_OA_OA11_RespITEM[] retoa = oa.SAP_OA_JK_11(oasap11);
                    Logger.Log(JsonHelper.ObjectToJson(retoa), flowid.ToString());
                    foreach (var item in retoa)
                    {
                        if (item.TYPE != "S")
                        {
                            ifsuc = false;                            
                        }
                    }
                    retoastring = JsonHelper.ObjectToJson(retoa);
                }
                catch
                {
                    retoastring = "";
                    ifsuc = false;
                }
                MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("UPDATE FLOW_RUN set TIMES=TIMES+1 where RUN_ID={0} ", flow154[i].run_id), null);
                Logger.Log(string.Format(" INSERT INTO sync_flowlog (run_id,flow_id,run_name,sendlog,receivelog,sendtime) values ({0},{1},'{2}','{3}','{4}',sysdate()) ", flow154[i].run_id, flowid, flow154[i].run_name, JsonHelper.ObjectToJson(oasap11), retoastring));
                MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format(" INSERT INTO sync_flowlog (run_id,flow_id,run_name,sendlog,receivelog,sendtime) values ({0},{1},'{2}','{3}','{4}',sysdate()) ", flow154[i].run_id, flowid, flow154[i].run_name, JsonHelper.ObjectToJson(oasap11), retoastring), null);
                if (ifsuc)
                {
                    int ret = MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("update flow_run set SYNC_TIME=sysdate() where RUN_ID={0} ", flow154[i].run_id), null);
                    //更新数据库表示他已经同步过了并且同步成功了
                    if (ret > 0)
                        Logger.Log(string.Format("RUN_ID={0},已经更新数据库", flow154[i].run_id), flowid.ToString());
                    else
                        Logger.Error(string.Format("RUN_ID={0},更新没有成功", flow154[i].run_id), flowid.ToString());
                }
                else
                {
                    Logger.Log(string.Format("RUN_ID={0},SAP返回错误信息", flow154[i].run_id), flowid.ToString());
                }
            }
        }
    }
}
