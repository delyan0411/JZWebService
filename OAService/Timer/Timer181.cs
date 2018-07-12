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
    //门店店员出差流程（代请）
    public class Timer181
    {
        public Timer181()
        {
            System.Timers.Timer t = new System.Timers.Timer(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timer181"]) * 1000);//
            t.Elapsed += new System.Timers.ElapsedEventHandler(Start);//到达时间的时候执行事件；
            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件
        }

        public void Start(object source, System.Timers.ElapsedEventArgs e)
        {
            int flowid = 181;
            //遍历某个规则下面的所有符合条件的flow_run
            //List<FlowRun> frlistbyruleid = flowrunall.Where(t => t.flow_id == flowid).ToList();
            List<Flow181> flow181 = MySqlHelper.ExecuteObjectList<Flow181>(string.Format("SELECT flowr.run_id AS run_id,flowr.run_name AS run_name,data_89 AS user_id,data_127 AS i_begda,data_128 AS i_endda, '1210' AS i_awart FROM flow_data_{0} AS flowdata INNER JOIN flow_run AS flowr ON flowdata.run_id = flowr.run_id INNER JOIN USER AS u ON u.USER_ID = flowdata.begin_user WHERE FLOW_ID = {0} AND DEL_FLAG = 0  AND END_TIME IS NOT NULL AND SYNC_TIME IS NULL AND (TIMES<=2 or RETRY=1)", flowid)).ToList();
            for (int i = 0; i < flow181.Count; i++)
            {
                string wsretlog = "";
                int errtype = 0;//1接口错误 2ws返回结果错误 0正常
                bool ifsuc = true;
                if (string.IsNullOrEmpty(flow181[i].i_begda)|| string.IsNullOrEmpty(flow181[i].i_endda))
                {
                    wsretlog = "日期为空";
                    errtype = 3;
                    MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("UPDATE FLOW_RUN set TIMES=TIMES+1,RETRY=0 where RUN_ID={0} ", flow181[i].run_id), null);
                    MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("INSERT INTO sync_flowlog (run_id,flow_id,run_name,sendlog,receivelog,sendtime,errtype) values ({0},{1},'{2}','{3}','{4}',sysdate(),{5}) ", flow181[i].run_id, flowid, flow181[i].run_name, "", wsretlog, errtype), null);
                    continue;
                }
                WebReference.SAP_OA_JK15_HEAD oasaphead15 = new WebReference.SAP_OA_JK15_HEAD();
                //人员编号	Head		I_PERNR	NUMC	8		必填	123
                //出差开始时间	Head		I_BEGDA	DATS	8		必填	20170801
                //出差结束时间	Head		I_ENDDA	DATS	8		必填	20170802
                //出差类型	Head		I_AWART	CHAR	4		必填	"1210 1220"
                oasaphead15.I_PERNR = flow181[i].user_id.ToString();
                oasaphead15.I_BEGDA = flow181[i].i_begda.Replace("-", "").Replace("/", "");
                oasaphead15.I_ENDDA = flow181[i].i_endda.Replace("-", "").Replace("/", "");
                oasaphead15.I_AWART = flow181[i].i_awart.ToString();
                WebReference.SAP_OA_JK15_REQ oasap15 = new WebReference.SAP_OA_JK15_REQ();
                oasap15.HEAD = oasaphead15;
                //发送ws
              
                try
                {
                    WebReference.OaWebService oa = new WebReference.OaWebService();
                    Logger.Log(flow181[i].run_id + "(" + flow181[i].run_name + "-" + flow181[i].user_id + "):" + JsonHelper.ObjectToJson(oasap15), flowid.ToString());
                    WebReference.DT_OA_OA15_RespITEM[] retoa = oa.SAP_OA_JK_15(oasap15);
                    wsretlog = JsonHelper.ObjectToJson(retoa);
                    Logger.Log(flow181[i].run_id + "(" + flow181[i].run_name + "-" + flow181[i].user_id + "):" + JsonHelper.ObjectToJson(retoa), flowid.ToString());
                    foreach (var item in retoa)
                    {
                        if (item.TYPE != "S")
                        {
                            errtype = 2;
                            ifsuc = false;
                            break;
                        }
                    }
                }
                catch
                {
                    wsretlog = "ws调用不成功";
                    errtype = 1;
                    ifsuc = false;
                }
                MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("UPDATE FLOW_RUN set TIMES=TIMES+1,RETRY=0 where RUN_ID={0} ", flow181[i].run_id), null);
                MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("INSERT INTO sync_flowlog (run_id,flow_id,run_name,sendlog,receivelog,sendtime,errtype) values ({0},{1},'{2}','{3}','{4}',sysdate(),{5})", flow181[i].run_id, flowid, flow181[i].run_name, JsonHelper.ObjectToJson(oasap15), wsretlog, errtype), null);
                if (ifsuc)
                {
                    //这里改版 oaphp加页面 加表 3次 3次以上 点重新发送 retry重置为1
                    int ret = MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("update flow_run set SYNC_TIME=sysdate(),RETRY=0  where RUN_ID={0} ", flow181[i].run_id), null);
                    //更新数据库表示他已经同步过了并且同步成功了
                    if (ret > 0)
                        Logger.Log(string.Format("RUN_ID={0},已经更新数据库", flow181[i].run_id), flowid.ToString());
                    else
                        Logger.Error(string.Format("RUN_ID={0},更新没有成功", flow181[i].run_id), flowid.ToString());
                }
                else
                {
                    Logger.Log(string.Format("RUN_ID={0},SAP返回错误信息", flow181[i].run_id), flowid.ToString());
                }
            }
        }
    }
}
