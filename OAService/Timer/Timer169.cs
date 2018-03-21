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
    //门店人员离职流程
    public class Timer169
    {
        public Timer169()
        {
            System.Timers.Timer t = new System.Timers.Timer(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timer169"])* 1000);//
            t.Elapsed += new System.Timers.ElapsedEventHandler(Start);//到达时间的时候执行事件；
            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件
        }

        public void Start(object source, System.Timers.ElapsedEventArgs e)
        {
            int flowid = 169;
            //遍历某个规则下面的所有符合条件的flow_run
            List<Flow169> flow169 = MySqlHelper.ExecuteObjectList<Flow169>(string.Format("SELECT flowr.run_id AS run_id,flowr.run_name AS run_name,data_2413  as user_id,data_2419 as i_begda,data_2420 as i_massg FROM flow_data_{0} AS flowdata INNER JOIN flow_run AS flowr ON flowdata.run_id = flowr.run_id INNER JOIN USER AS u ON u.USER_ID = flowdata.begin_user WHERE FLOW_ID = {0} AND DEL_FLAG = 0  AND END_TIME IS NOT NULL AND SYNC_TIME IS NULL AND (TIMES<=2 or RETRY=1)", flowid)).ToList();
            for (int i = 0; i < flow169.Count; i++)
            {
                WebReference.SAP_OA_JK20_HEAD oasaphead20 = new WebReference.SAP_OA_JK20_HEAD();
                // I_PERNR	NUMC	8		必填	123
                //I_BEGDA	DATS	4		必填	20170101
                //I_MASSG	CHAR	4		必填	01
                oasaphead20.I_PERNR = flow169[i].user_id.ToString();
                oasaphead20.I_BEGDA = flow169[i].i_begda.Replace("-", "").Replace("/", "");
                //oasaphead20.I_MASSG = flow169[i].i_massg;
                oasaphead20.I_MASSG = "01";
                WebReference.SAP_OA_JK20_REQ oasap20 = new WebReference.SAP_OA_JK20_REQ();
                oasap20.HEAD = oasaphead20;
                //发送ws
                string retoastring = "";
                bool ifsuc = true;
                try
                {
                    WebReference.OaWebService oa = new WebReference.OaWebService();
                    Logger.Log(JsonHelper.ObjectToJson(oasap20), flowid.ToString());
                    WebReference.DT_OA_OA20_RespITEM[] retoa = oa.SAP_OA_JK_20(oasap20);
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
                MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("UPDATE FLOW_RUN set TIMES=TIMES+1 where RUN_ID={0} ", flow169[i].run_id), null);
                MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format(" INSERT INTO sync_flowlog (run_id,flow_id,run_name,sendlog,receivelog,sendtime) values ({0},{1},'{2}','{3}','{4}',sysdate())", flow169[i].run_id, flowid, flow169[i].run_name, JsonHelper.ObjectToJson(oasap20), retoastring), null);
                if (ifsuc)
                {
                    int ret = MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("update flow_run set SYNC_TIME=sysdate() where RUN_ID={0} ", flow169[i].run_id), null);
                    //更新数据库表示他已经同步过了并且同步成功了
                    if (ret > 0)
                        Logger.Log(string.Format("RUN_ID={0},已经更新数据库", flow169[i].run_id), flowid.ToString());
                    else
                        Logger.Error(string.Format("RUN_ID={0},更新没有成功", flow169[i].run_id), flowid.ToString());
                }
                else
                {
                    Logger.Log(string.Format("RUN_ID={0},SAP返回错误信息", flow169[i].run_id), flowid.ToString());
                }
            }
        }
    }
}
