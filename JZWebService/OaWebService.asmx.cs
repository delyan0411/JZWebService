
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using JZWebService.Models;
using JZWebService.Request;
using JZWebService.Response;
using System.Net;
using JZWebService.SapOa13Webservice;
using JZWebService.error;

namespace JZWebService
{
    /// <summary>
    /// OaWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://jiuzhou.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class OaWebService : System.Web.Services.WebService
    {
        OaServer oaApp = new OaServer();
        #region 废弃
        //#region HelloWorld
        ///// <summary>
        ///// ZRFC_HR_OA_001 集团组织架构推送
        ///// </summary>
        //[WebMethod]
        //[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com.com/HelloWorld", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        //public string HelloWorld()
        //{
        //    /*
        //    OA_SAP_FIN_JK01_REQ rq = new OA_SAP_FIN_JK01_REQ();
        //    OA_SAP_FIN_JK01_HEAD hd = new OA_SAP_FIN_JK01_HEAD();
        //    hd.KTOPL = "总账";
        //    hd.SAKNR = "002";
        //    hd.KTOKS = "01";
        //    hd.GLACCOUNT_TYPE = "01";
        //    hd.TXT20_ML = "hello";
        //    hd.TXT50_ML = "hell0";

        //    OA_SAP_FIN_JK01_HEAD[] m_HEAD = new OA_SAP_FIN_JK01_HEAD[1];
        //    m_HEAD[0] = hd;
        //    rq.HEAD = m_HEAD;

        //    oaApp.OA_SAP_FIN_JK01(rq);
        //    // oaApp.demo();
        //    */
        //    ZRFC_HR_OA_003_REQ rq = new ZRFC_HR_OA_003_REQ();
        //    ZRFC_HR_OA_003_HEAD hd = new ZRFC_HR_OA_003_HEAD();
        //    hd.NACHN = "李四";
        //    hd.OBJID_DEP = "50000105";
        //    hd.PERNR = "005";


        //    ZRFC_HR_OA_003_HEAD[] m_HEAD = new ZRFC_HR_OA_003_HEAD[1];
        //    m_HEAD[0] = hd;
        //    rq.HEAD = m_HEAD;
        //    oaApp.SaveOrUpdateStaffInfo(rq);

        //    return "Hello World";
            
        //}
        //#endregion
        //#region 会计科目数据接收
        ////[WebMethod(Description = "会计科目数据接收")]
        ////[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com.com/SAP_OA_JK_01", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        ////public OA_SAP_FIN_JK01_RESP SAP_OA_JK_01(OA_SAP_FIN_JK01_REQ OA_SAP_FIN_JK01_REQ)
        ////{
        ////    return oaApp.OA_SAP_FIN_JK01(OA_SAP_FIN_JK01_REQ);
        ////}
        //#endregion
        #region 成本中心数据接收
        //[WebMethod(Description = "成本中心数据接收")]
        //[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com.com/SAP_OA_JK_02", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        //public OA_SAP_FIN_JK02_RESP SAP_OA_JK_02(OA_SAP_FIN_JK02_REQ OA_SAP_FIN_JK02_REQ)
        //{
        //    return oaApp.OA_SAP_FIN_JK02(OA_SAP_FIN_JK02_REQ);
        //}
        #endregion
        #region GSP结构接收
        /// <summary>
        /// ZRFC_HR_OA_002 GPS结构推送
        /// </summary>
        //[WebMethod(Description = " GSP结构接收")]
        //[System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/ZRFC_HR_OA_002", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        //public void ZRFC_HR_OA_002([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://jiuzhou.com/ZRFC_HR_OA_002")]ZRFC_HR_OA_002_REQ ZRFC_HR_OA_002_REQ)
        //{

        //}
        #endregion
        #region 调用年休天数检查
        ///OA回写SAP，检查年休天数是否符合，返回0，符合，-1不符合
        //[WebMethod(Description = "调用年休天数检查")]
        //public OA_SAP_JK_RESP OA_SAP_JK_01(string sStaffNO, int iDay)
        //{
        //    return oaApp.OA_SAP_JK_01(sStaffNO, iDay);
        //}
        #endregion
        #region 报销类单据回写SAP
        /// <summary>
        /// OA回写SAP，将报销结果即时回写SAP
        /// </summary>
        /// <param name="flowType"></param>
        /// <param name="runID"></param>
        /// <returns></returns>
        //[WebMethod(Description = "报销类单据回写SAP")]
        //public OA_SAP_JK_RESP OA_SAP_JK_02(string sFlowType, string sRunID)
        //{

        //    return oaApp.OA_SAP_JK_02(sFlowType, sRunID);

        //}
        #endregion
        #region 定时回写Sap结果
        //[WebMethod(Description = "定时回写Sap结果，作业执行")]
        //public int OA_SAP_JOB_01()
        //{

        //    return oaApp.OA_SAP_JOB_01();

        //}
        #endregion
        #region 打卡数据推送给SAP
        /// <summary>
        /// ZRFC_HR_A01_UPDATE 打卡数据推送给SAP
        /// </summary>
        //[WebMethod(Description = "打卡数据推送给SAP")]
        //public void ZRFC_HR_A01_OUT()
        //{
        //    oaApp.GetCheckInfoList();
        //}
        #endregion        
        #region 查询员工请假数据
        [WebMethod(Description = "查询员工请假数据")]
        [System.Web.Services.Protocols.SoapDocumentMethod("http://jiuzhou.com/SAP_OA_JK_12", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public SapOa12Webservice.DT_OA_OA12_RespITEM[] SAP_OA_JK_12([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://jiuzhou.com/SAP_OA_JK_12")]string uid)
        {
            //拼接xml 
            //oaApp.GetCheckInfoList();
            SapOa12Webservice.SI_OA_OA12_OUTService sapSrv = new SapOa12Webservice.SI_OA_OA12_OUTService();
            //SapOa10WebService.SI_OA_OA10_OUTService sapSrv = new SapOa10WebService.SI_OA_OA10_OUTService();
            SapOa12Webservice.DT_OA_OA12 items = new SapOa12Webservice.DT_OA_OA12();
            //人员编号 ITEM        I_PERNR NUMC    8           123
            //请假日期 ITEM        I_BEGDA DATS    8           20170101
            //是否全天？‘0‘，是；‘1’，否。	ITEM L_HALF  CHAR    1           是否全天？‘0‘，是；‘1’，否。
            //开始时间 ITEM        L_ BEGUZ    TIMS    8           8:00
            //结束时间 ITEM        L_ ENDUZ    TIMS    8           16:00
            //请假类型编码 ITEM        I_AWART CHAR    4           2000、2010…

            SapOa12Webservice.DT_OA_OA12HEAD head = new SapOa12Webservice.DT_OA_OA12HEAD();
            head.I_PERNR = uid;
            items.HEAD = head;
            sapSrv.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["SAPURLUSER"].ToString(), System.Configuration.ConfigurationManager.AppSettings["SAPURLPSW"].ToString());
            SapOa12Webservice.DT_OA_OA12_RespITEM[] ret = sapSrv.SI_OA_OA12_OUT(items);
            return ret;
        }
        #endregion
        #endregion
        #region 组织架构接收
        [WebMethod(Description = "组织架构接收")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/SAP_OA_JK_06", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public void SAP_OA_JK_06([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://jiuzhou.com/SAP_OA_JK_06")]ZRFC_HR_OA_001_REQ ZRFC_HR_OA_001_REQ){
            oaApp.SaveOrUpdateDeptInfo(ZRFC_HR_OA_001_REQ);
        }
        #endregion      
        #region 员工信息接收
        [WebMethod(Description = "员工信息接收")]
        [System.Web.Services.Protocols.SoapDocumentMethod("http://jiuzhou.com/SAP_OA_JK_08", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public void SAP_OA_JK_08([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://jiuzhou.com/SAP_OA_JK_08")]ZRFC_HR_OA_003_REQ ZRFC_HR_OA_003_REQ){
            oaApp.SaveOrUpdateStaffInfo(ZRFC_HR_OA_003_REQ);
        }
        #endregion
        #region 请假数据传入
        [WebMethod(Description = "请假数据传入")]
        [System.Web.Services.Protocols.SoapDocumentMethod("http://jiuzhou.com/SAP_OA_JK_13", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public DT_OA_OA13_RespITEM[] SAP_OA_JK_13([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://jiuzhou.com/SAP_OA_JK_13")]SAP_OA_JK13_REQ SAP_OA_JK13_REQ)
        {
            //拼接xml 
            SapOa13Webservice.SI_OA_OA13_OUTService sapSrv = new SapOa13Webservice.SI_OA_OA13_OUTService();
            SapOa13Webservice.DT_OA_OA13ITEM[] items = new SapOa13Webservice.DT_OA_OA13ITEM[SAP_OA_JK13_REQ.HEAD.Count()];
            //人员编号 ITEM        I_PERNR NUMC    8           123
            //请假日期 ITEM        I_BEGDA DATS    8           20170101
            //是否全天？‘0‘，是；‘1’，否。	ITEM L_HALF  CHAR    1           是否全天？‘0‘，是；‘1’，否。
            //开始时间 ITEM        L_ BEGUZ    TIMS    8           8:00
            //结束时间 ITEM        L_ ENDUZ    TIMS    8           16:00
            //请假类型编码 ITEM        I_AWART CHAR    4           2000、2010…
            for (int i = 0; i < SAP_OA_JK13_REQ.HEAD.Count(); i++)
            {
                SapOa13Webservice.DT_OA_OA13ITEM item = new SapOa13Webservice.DT_OA_OA13ITEM();
                item.I_PERNR = SAP_OA_JK13_REQ.HEAD[i].I_PERNR;//人员编号
                //item.I_PERNR = "2517";
                item.I_BEGDA = SAP_OA_JK13_REQ.HEAD[i].I_BEGDA; //请假日期
                item.L_HALF = SAP_OA_JK13_REQ.HEAD[i].L_HALF; //是否全天
                item.L_BEGUZ = SAP_OA_JK13_REQ.HEAD[i].L_BEGUZ; //开始时间
                item.L_ENDUZ = SAP_OA_JK13_REQ.HEAD[i].L_ENDUZ; //结束时间
                item.I_AWART = SAP_OA_JK13_REQ.HEAD[i].I_AWART; //请假类型编码
                items[i] = item;
            }
            sapSrv.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["SAPURLUSER"].ToString(), System.Configuration.ConfigurationManager.AppSettings["SAPURLPSW"].ToString());
            SapOa13Webservice.DT_OA_OA13_RespITEM[] ret = sapSrv.SI_OA_OA13_OUT(items);
            return ret;
        }
        #endregion
        #region 加班数据传入
        [WebMethod(Description = "加班数据传入")]
        [System.Web.Services.Protocols.SoapDocumentMethod("http://jiuzhou.com/SAP_OA_JK_17", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public SapOa17Webservice.DT_OA_OA17_RespITEM[] SAP_OA_JK_17([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://jiuzhou.com/SAP_OA_JK_17")]SAP_OA_JK17_REQ SAP_OA_JK17_REQ)
        {
            SapOa17Webservice.SI_OA_OA17_OUTService sapSrv = new SapOa17Webservice.SI_OA_OA17_OUTService();
            SapOa17Webservice.DT_OA_OA17 item = new SapOa17Webservice.DT_OA_OA17();
            SapOa17Webservice.DT_OA_OA17HEADER sh = new SapOa17Webservice.DT_OA_OA17HEADER();
            //I_PERNR NUMC    8           123
            //I_BEGDA DATS    8           20170101
            //I_ENDDA DATS    8           20170101
            //I_BEGUZ TIMS    8           17:00
            //I_ENDUZ TIMS    8           18:00
            //I_STDAZ  加班时长
            //I_VERSL CHAR    1           "空格：加班调休 1：加班结薪"
            //item.HEADER.I_PERNR = SAP_OA_JK17_REQ.HEAD.I_PERNR;
            //item.HEADER.I_BEGDA = SAP_OA_JK17_REQ.HEAD.I_BEGDA;
            //item.HEADER.I_ENDDA = SAP_OA_JK17_REQ.HEAD.I_ENDDA;
            sh.I_PERNR = SAP_OA_JK17_REQ.HEAD.I_BEGDA;
            sh.I_BEGDA = SAP_OA_JK17_REQ.HEAD.I_BEGDA;
            sh.I_ENDDA = SAP_OA_JK17_REQ.HEAD.I_ENDDA;
            sh.I_BEGUZ = "";
            sh.I_ENDUZ = "";
            sh.I_STDAZ = SAP_OA_JK17_REQ.HEAD.I_STDAZ;
            sh.I_VERSL = "1";
            item.HEADER = sh;
            sapSrv.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["SAPURLUSER"].ToString(), System.Configuration.ConfigurationManager.AppSettings["SAPURLPSW"].ToString());
            SapOa17Webservice.DT_OA_OA17_RespITEM[] ret = sapSrv.SI_OA_OA17_OUT(item);
            return ret;
        }
        #endregion
        #region 出差数据传入
        [WebMethod(Description = "出差数据传入")]
        [System.Web.Services.Protocols.SoapDocumentMethod("http://jiuzhou.com/SAP_OA_JK_15", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public SapOa15Webservice.DT_OA_OA15_RespITEM[] SAP_OA_JK_15([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://jiuzhou.com/SAP_OA_JK_15")]SAP_OA_JK15_REQ SAP_OA_JK_15_REQ)
        {
            SapOa15Webservice.SI_OA_OA15_OUTService sapSrv = new SapOa15Webservice.SI_OA_OA15_OUTService();
            SapOa15Webservice.DT_OA_OA15 item = new SapOa15Webservice.DT_OA_OA15();
            SapOa15Webservice.DT_OA_OA15HEADER sh = new SapOa15Webservice.DT_OA_OA15HEADER();
            //人员编号 Head        I_PERNR NUMC    8       必填  123
            //出差开始时间 Head        I_BEGDA DATS    8       必填  20170801
            //出差结束时间 Head        I_ENDDA DATS    8       必填  20170802
            //出差类型 Head        I_AWART CHAR    4       必填  "1210"
            // item.HEADER.I_PERNR = SAP_OA_JK_15_REQ.HEAD.I_PERNR;
            //  item.HEADER.I_BEGDA = SAP_OA_JK_15_REQ.HEAD.I_BEGDA;
            // item.HEADER.I_ENDDA = SAP_OA_JK_15_REQ.HEAD.I_ENDDA;
            sh.I_PERNR = SAP_OA_JK_15_REQ.HEAD.I_PERNR;
            sh.I_BEGDA = SAP_OA_JK_15_REQ.HEAD.I_BEGDA;
            sh.I_ENDDA = SAP_OA_JK_15_REQ.HEAD.I_ENDDA;
            sh.I_AWART = "1210";
            item.HEADER = sh;
            sapSrv.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["SAPURLUSER"].ToString(), System.Configuration.ConfigurationManager.AppSettings["SAPURLPSW"].ToString());
            SapOa15Webservice.DT_OA_OA15_RespITEM[] ret = sapSrv.SI_OA_OA15_OUT(item);
            return ret;
        }
        #endregion
        #region 补卡数据转入
        [WebMethod(Description = "补卡数据转入")]
        [System.Web.Services.Protocols.SoapDocumentMethod("http://jiuzhou.com/SAP_OA_JK_11", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public SapOa11Webservice.DT_OA_OA11_RespITEM[] SAP_OA_JK_11([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://jiuzhou.com/SAP_OA_JK_11")]SAP_OA_JK11_REQ SAP_OA_JK_11_REQ)
        {
            SapOa11Webservice.SI_OA_OA11_OUTService sapSrv = new SapOa11Webservice.SI_OA_OA11_OUTService();
            SapOa11Webservice.DT_OA_OA11 item = new SapOa11Webservice.DT_OA_OA11();
            SapOa11Webservice.DT_OA_OA11HEAD sh = new SapOa11Webservice.DT_OA_OA11HEAD();
            //员工编号 Head        PERNR NUMC    8           123
            //日期 Head        LDATE DATS    8           20170101
            //时间 Head        LTIME TIMS    6           13:30
            //时间类型（上班卡或下班卡）	Head SATZA   CHAR    3           "P10 上班卡
            //P20 下班卡"
            //刷卡类型 Head        ABWGR CHAR    4           "ABWGR = ’0003’ 补卡
            //ABWGR = ’0005外出补卡"
            sh.PERNR = SAP_OA_JK_11_REQ.HEAD.PERNR;
            sh.LDATE = SAP_OA_JK_11_REQ.HEAD.LDATE;
            sh.LTIME = SAP_OA_JK_11_REQ.HEAD.LTIME;
            sh.SATZA = SAP_OA_JK_11_REQ.HEAD.SATZA;
            sh.ABWGR = SAP_OA_JK_11_REQ.HEAD.ABWGR;
            item.HEAD = sh;
            sapSrv.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["SAPURLUSER"].ToString(), System.Configuration.ConfigurationManager.AppSettings["SAPURLPSW"].ToString());
            SapOa11Webservice.DT_OA_OA11_RespITEM[] ret = sapSrv.SI_OA_OA11_OUT(item);
            return ret;
        }
        #endregion
        #region 离职数据传入
        [WebMethod(Description = "离职数据传入")]
        [System.Web.Services.Protocols.SoapDocumentMethod("http://jiuzhou.com/SAP_OA_JK_20", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public SapOa20Webservice.DT_OA_OA20_RespITEM[] SAP_OA_JK_20([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://jiuzhou.com/SAP_OA_JK_20")]SAP_OA_JK20_REQ SAP_OA_JK_20_REQ)
        {
            SapOa20Webservice.SI_OA_OA20_OUTService sapSrv = new SapOa20Webservice.SI_OA_OA20_OUTService();
            SapOa20Webservice.DT_OA_OA20 item = new SapOa20Webservice.DT_OA_OA20();
            SapOa20Webservice.DT_OA_OA20HEADER sh = new SapOa20Webservice.DT_OA_OA20HEADER();
            //I_PERNR	NUMC	8		必填	123
            //I_BEGDA DATS    4       必填  20170101
            //I_MASSG CHAR    4       必填  01                                                            
            sh.I_PERNR = SAP_OA_JK_20_REQ.HEAD.I_PERNR;
            sh.I_BEGDA = SAP_OA_JK_20_REQ.HEAD.I_BEGDA;
            sh.I_MASSG = SAP_OA_JK_20_REQ.HEAD.I_MASSG;
            item.HEADER = sh;
            sapSrv.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["SAPURLUSER"].ToString(), System.Configuration.ConfigurationManager.AppSettings["SAPURLPSW"].ToString());
            SapOa20Webservice.DT_OA_OA20_RespITEM[] ret = sapSrv.SI_OA_OA20_OUT(item);
            return ret;
        }
        #endregion
        #region 付款申请上传
        /// <summary>
        /// 付款申请上传
        /// </summary>
        /// <param name="SAP_OA_JK_04_REQ"></param>
        /// <returns></returns>
        [WebMethod(Description = "付款申请上传")]
        [System.Web.Services.Protocols.SoapDocumentMethod("http://jiuzhou.com/SAP_OA_JK_04", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        //
        public void SAP_OA_JK_04([System.Xml.Serialization.XmlElementAttribute(Namespace = "http://jiuzhou.com/SAP_OA_JK_04")]SAP_OA_JK04_REQ SAP_OA_JK04_REQ)
        {
            SapOa04Webservice.SI_OA_OA04_OUTService sapSrv = new SapOa04Webservice.SI_OA_OA04_OUTService();
            SapOa04Webservice.DT_OA_OA04 sapoa04 = new SapOa04Webservice.DT_OA_OA04();
            SapOa04Webservice.DT_OA_OA04HEADER sh = new SapOa04Webservice.DT_OA_OA04HEADER();
            SapOa04Webservice.DT_OA_OA04HEADERITEM[] items = new SapOa04Webservice.DT_OA_OA04HEADERITEM[SAP_OA_JK04_REQ.HEAD.ITEMS.Count()];
            sh.BUKRS = SAP_OA_JK04_REQ.HEAD.BUKRS;
            sh.ZFKDH = SAP_OA_JK04_REQ.HEAD.ZFKDH;
            sh.ZJYLX = SAP_OA_JK04_REQ.HEAD.ZJYLX;
            sh.BANKA = SAP_OA_JK04_REQ.HEAD.BANKA;
            sh.KOINH = SAP_OA_JK04_REQ.HEAD.KOINH;
            sh.ZBKNO = SAP_OA_JK04_REQ.HEAD.ZBKNO;
            sh.ZJYJE = SAP_OA_JK04_REQ.HEAD.ZJYJE;
            sh.WAERS = SAP_OA_JK04_REQ.HEAD.WAERS;
            sh.ZZHTY = SAP_OA_JK04_REQ.HEAD.ZZHTY;
            sh.BKTXT = SAP_OA_JK04_REQ.HEAD.BKTXT;
            for (int i = 0; i < SAP_OA_JK04_REQ.HEAD.ITEMS.Count(); i++)
            {
                SapOa04Webservice.DT_OA_OA04HEADERITEM headeritem = new SapOa04Webservice.DT_OA_OA04HEADERITEM();
                headeritem.ZNUM = SAP_OA_JK04_REQ.HEAD.ITEMS[i].ZNUM;//
                headeritem.KOSTL = SAP_OA_JK04_REQ.HEAD.ITEMS[i].KOSTL; //
                headeritem.HKONT = SAP_OA_JK04_REQ.HEAD.ITEMS[i].HKONT; //
                headeritem.ZCBJE = SAP_OA_JK04_REQ.HEAD.ITEMS[i].ZCBJE; //
                headeritem.ZZSKT = SAP_OA_JK04_REQ.HEAD.ITEMS[i].ZZSKT; //
                headeritem.ZSHUE = SAP_OA_JK04_REQ.HEAD.ITEMS[i].ZSHUE; //
                headeritem.ZBXJE = SAP_OA_JK04_REQ.HEAD.ITEMS[i].ZBXJE; //
                headeritem.ZRMTXT = SAP_OA_JK04_REQ.HEAD.ITEMS[i].ZRMTXT; //
                items[i] = headeritem;
            }
            sh.ITEM = items;
            sapoa04.HEADER = sh;
            sapSrv.Credentials = new NetworkCredential(System.Configuration.ConfigurationManager.AppSettings["SAPURLUSER"].ToString(), System.Configuration.ConfigurationManager.AppSettings["SAPURLPSW"].ToString());
            sapSrv.SI_OA_OA04_OUT(sapoa04);
        }
        #endregion
    }
}
