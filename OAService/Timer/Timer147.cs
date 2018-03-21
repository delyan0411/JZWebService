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
     //九洲大药房_SAP_HR_PT_130_出差流程_V2.0
    public class Timer147
    {
        public Timer147()
        {
            System.Timers.Timer t = new System.Timers.Timer(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timer147"]) * 1000);//
            t.Elapsed += new System.Timers.ElapsedEventHandler(Start);//到达时间的时候执行事件；
            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件
        }

        public void Start(object source, System.Timers.ElapsedEventArgs e)
        {
            int flowid = 147;
            //遍历某个规则下面的所有符合条件的flow_run
            //List<FlowRun> frlistbyruleid = flowrunall.Where(t => t.flow_id == flowid).ToList();
            List<Flow147> flow147 = MySqlHelper.ExecuteObjectList<Flow147>(string.Format("SELECT flowr.run_id AS run_id,flowr.run_name AS run_name,byname AS user_id,data_95 AS i_begda,data_93 AS i_endda, '1210' AS i_awart FROM flow_data_{0} AS flowdata INNER JOIN flow_run AS flowr ON flowdata.run_id = flowr.run_id INNER JOIN USER AS u ON u.USER_ID = flowdata.begin_user WHERE FLOW_ID = {0} AND DEL_FLAG = 0  AND END_TIME IS NOT NULL AND SYNC_TIME IS NULL AND (TIMES<=2 or RETRY=1)", flowid)).ToList();
            for (int i = 0; i < flow147.Count; i++)
            {
                WebReference.SAP_OA_JK15_HEAD oasaphead15 = new WebReference.SAP_OA_JK15_HEAD();
                //人员编号	Head		I_PERNR	NUMC	8		必填	123
                //出差开始时间	Head		I_BEGDA	DATS	8		必填	20170801
                //出差结束时间	Head		I_ENDDA	DATS	8		必填	20170802
                //出差类型	Head		I_AWART	CHAR	4		必填	"1210 1220"
                oasaphead15.I_PERNR = flow147[i].user_id.ToString();
                oasaphead15.I_BEGDA = flow147[i].i_begda.Replace("-", "").Replace("/", "");
                oasaphead15.I_ENDDA = flow147[i].i_endda.Replace("-", "").Replace("/", "");
                oasaphead15.I_AWART = flow147[i].i_awart.ToString();
                WebReference.SAP_OA_JK15_REQ oasap15 = new WebReference.SAP_OA_JK15_REQ();
                oasap15.HEAD = oasaphead15;
                //发送ws
                string retoastring = "";
                bool ifsuc = true;
                try
                {
                    WebReference.OaWebService oa = new WebReference.OaWebService();
                    Logger.Log(JsonHelper.ObjectToJson(oasap15), flowid.ToString());
                    WebReference.DT_OA_OA15_RespITEM[] retoa = oa.SAP_OA_JK_15(oasap15);
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
                MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("UPDATE FLOW_RUN set TIMES=TIMES+1 where RUN_ID={0} ", flow147[i].run_id), null);
                MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format(" INSERT INTO sync_flowlog (run_id,flow_id,run_name,sendlog,receivelog,sendtime) values ({0},{1},'{2}','{3}','{4}',sysdate())", flow147[i].run_id, flowid, flow147[i].run_name, JsonHelper.ObjectToJson(oasap15), retoastring), null);
                if (ifsuc)
                {
                    int ret = MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("update flow_run set SYNC_TIME=sysdate() where RUN_ID={0} ", flow147[i].run_id), null);
                    //更新数据库表示他已经同步过了并且同步成功了
                    if (ret > 0)
                        Logger.Log(string.Format("RUN_ID={0},已经更新数据库", flow147[i].run_id), flowid.ToString());
                    else
                        Logger.Error(string.Format("RUN_ID={0},更新没有成功", flow147[i].run_id), flowid.ToString());
                }
                else
                {
                    Logger.Log(string.Format("RUN_ID={0},SAP返回错误信息", flow147[i].run_id), flowid.ToString());
                }
            }
        }
    }
}
