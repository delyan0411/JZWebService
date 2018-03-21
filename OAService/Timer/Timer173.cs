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
    /// <summary>
    ///  税率
    /// </summary>
    public class Sl
    {
        /// <summary>
        /// ID
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 税率
        /// </summary>
        public string sl { get; set; }
        /// <summary>
        /// 税率编码
        /// </summary>
        public string slcode { get; set; }
    }
    //多成本中心&单费用科目
    public class Timer173
    {
        public Timer173()
        {
            System.Timers.Timer t = new System.Timers.Timer(Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["timer173"]) * 1000);//
            t.Elapsed += new System.Timers.ElapsedEventHandler(Start);//到达时间的时候执行事件；
            t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件
        }
        /// <summary>
        /// 税率zzskt
        /// </summary>
        /// <param name="sl"></param>
        /// <returns></returns>
        public string getstrbysl(string sl)
        {
            List<Sl> sllist = MySqlHelper.ExecuteObjectList<Sl>("SELECT id,sl,slcode FROM `zzskt_info`").ToList();
            
            string str = "2221001001";
            try
            {
                str = sllist.FirstOrDefault(t => t.sl == sl).slcode;
            }
            catch
            {

            }
            ////3 5 6 11 17
            //if (sl == "0.03")
            //    str = "2221001005";
            //else if (sl == "0.05")
            //    str = " 2221001004";
            //else if (sl == "0.06")
            //    str = " 2221001003";
            //else if (sl == "0.11")
            //    str = " 2221001002";
            //else if (sl == "0.17")
            //    str = " 2221001001";
            //else
            //    str = " 2221001001";
            return str == "" ? "2221001001" : str;
        }
        public void Start(object source, System.Timers.ElapsedEventArgs e)
        {
            int flowid = 173;
            //遍历某个规则下面的所有符合条件的flow_run
            //List<FlowRun> frlistbyruleid = flowrunall.Where(t => t.flow_id == flowid).ToList();
            List<Flow173> flow173 = MySqlHelper.ExecuteObjectList<Flow173>(string.Format("SELECT flowr.run_id AS run_id,flowr.run_name AS run_name,byname AS user_id,data_2430 AS bukrs,'1' AS zfkdh,'1010' AS zjylx,data_259 AS banka,data_255 AS koinh,data_260 AS zbkno,data_263 AS zjyje,'CNY' AS waers, '10' AS zzhty,data_265 AS bktxt FROM flow_data_{0} AS flowdata INNER JOIN flow_run AS flowr ON flowdata.run_id = flowr.run_id INNER JOIN USER AS u ON u.USER_ID = flowdata.begin_user WHERE FLOW_ID = {0} AND DEL_FLAG = 0 AND END_TIME IS NOT NULL AND SYNC_TIME IS NULL AND (TIMES <= 2 OR RETRY = 1) and flowdata.run_id=13983; ", flowid)).ToList();
            for (int i = 0; i < flow173.Count; i++)
            {
                //公司代码 ITEM        BUKRS
                //付款单号 ITEM        ZFKDH 
                //交易类型 ITEM        ZJYLX 
                //银行名称 ITEM        BANKA
                //帐户持有人姓名  ITEM       KOINH
                //银行账号(组合) ITEM        ZBKNO
                //总交易金额 ITEM        ZJYJE            
                //货币码 ITEM        WAERS
                //账号类型 ITEM        ZZHTY
                //抬头文本 ITEM       BKTXT

                //ZNUM 序号
                //KOSTL 成本中心
                //HKONT G/L科目号
                //ZCBJE 成本金额
                //ZZSKT 税科目
                //ZSHUE 税额
                //ZBXJE 报销金额
                //ZRMTXT 备注文本
                #region
                WebReference.SAP_OA_JK04_REQ oasap04 = new WebReference.SAP_OA_JK04_REQ();
                WebReference.SAP_OA_JK04_HEAD oa04head = new WebReference.SAP_OA_JK04_HEAD();
                oa04head.BUKRS = flow173[i].bukrs.Split('|')[0].ToString();
                Random rad = new Random();//实例化随机数产生器rad；
                int value = rad.Next(1000, 9999);
                oa04head.ZFKDH = string.Format("{0:yyyyMMdd}", DateTime.Now) + value.ToString();
                //oa04head.ZFKDH = flow173[i].zfkdh.ToString();
                oa04head.ZJYLX = flow173[i].zjylx.ToString();
                oa04head.BANKA = flow173[i].banka.ToString();
                oa04head.KOINH = flow173[i].koinh.ToString();
                oa04head.ZBKNO = flow173[i].zbkno.ToString();
                oa04head.ZJYJE = flow173[i].zjyje.ToString();
                oa04head.WAERS = flow173[i].waers.ToString();
                oa04head.ZZHTY = flow173[i].zzhty.ToString();
                oa04head.BKTXT = flow173[i].bktxt.ToString();
                oasap04.HEAD = oa04head;
                List<WebReference.SAP_OA_JK04_HEAD_ITEM> oa04headitemlist = new List<WebReference.SAP_OA_JK04_HEAD_ITEM>();
                //6601001008
                #endregion
                List<ITEM> flow173item = MySqlHelper.ExecuteObjectList<ITEM>(string.Format("select item_2 AS kostl, item_4 AS hkont, item_7 as zcbje, item_6 as zzskt, item_8 as zshue, item_10 as zbxje, '备注' as zrmtxt from flow_data_173_list_128 WHERE run_id = {0} and item_2!='' and item_7!='' and item_6!='' and item_8!='' and item_10!=''; ", flow173[i].run_id)).ToList();
                for (int x = 0; x < flow173item.Count; x++)
                {
                    WebReference.SAP_OA_JK04_HEAD_ITEM oa04headitem = new WebReference.SAP_OA_JK04_HEAD_ITEM();
                    oa04headitem.ZNUM = (x + 1).ToString();
                    oa04headitem.KOSTL = flow173item[x].kostl.ToString();
                    oa04headitem.HKONT = flow173item[x].hkont.ToString();//会计科目
                    oa04headitem.ZCBJE = flow173item[x].zcbje.ToString();
                    oa04headitem.ZZSKT = getstrbysl(flow173item[x].zzskt.ToString());//3 5 6 11 17
                    oa04headitem.ZSHUE = flow173item[x].zshue.ToString();
                    oa04headitem.ZBXJE = flow173item[x].zbxje.ToString();
                    oa04headitem.ZRMTXT = flow173item[x].zrmtxt.ToString();
                    oa04headitemlist.Add(oa04headitem);
                }
                oa04head.ITEMS = oa04headitemlist.ToArray();
                string wsretlog = "";
                int errtype = 0;//1接口错误 2ws返回结果错误 0正常
                bool ifsuc = true;
                //发送ws
                try
                {
                    WebReference.OaWebService oa = new WebReference.OaWebService();
                    Logger.Log(JsonHelper.ObjectToJson(oasap04), flowid.ToString());
                    oa.SAP_OA_JK_04(oasap04);
                }
                catch
                {
                    wsretlog = "ws调用不成功";
                    errtype = 1;
                    ifsuc = false;
                }
             
                MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("UPDATE FLOW_RUN set TIMES=TIMES+1 where RUN_ID={0} ", flow173[i].run_id), null);
                MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format(" INSERT INTO sync_flowlog (run_id,flow_id,run_name,sendlog,receivelog,sendtime,errtype) values ({0},{1},'{2}','{3}','{4}',sysdate(),{5})", flow173[i].run_id, flowid, flow173[i].run_name, JsonHelper.ObjectToJson(oasap04), wsretlog, errtype), null);
                if (ifsuc)
                {
                    int ret = MySqlHelper.ExecuteNonQuery(System.Data.CommandType.Text, string.Format("update flow_run set SYNC_TIME=sysdate() where RUN_ID={0} ", flow173[i].run_id), null);
                    //更新数据库表示他已经同步过了并且同步成功了
                    if (ret > 0)
                        Logger.Log(string.Format("RUN_ID={0},已经更新数据库", flow173[i].run_id), flowid.ToString());
                    else
                      Logger.Error(string.Format("RUN_ID={0},更新没有成功", flow173[i].run_id), flowid.ToString());
                }
                else
                {
                    Logger.Log(string.Format("RUN_ID={0},SAP返回错误信息", flow173[i].run_id), flowid.ToString());
                }
            }
        }
    }
}
