using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using JZWebService.Request;
using JZWebService.Response;

namespace JZWebService
{
    /// <summary>
    /// LmsWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://jiuzhou.com/lms/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]

    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class LmsWebService : System.Web.Services.WebService
    {
        LmsServer lmsApp = new LmsServer();
        //获取SAP商品SAP_LMS_JK_01
        //获取SAP供应商SAP_LMS_JK_02
        //获取SAP商品销售价格，零售价，会员价，最高最低限价SAP_LMS_JK_03
        ///获取采购订单
        
        [WebMethod(Description = "HelloWord")]
        public string HelloWord()
        {
            lmsApp.HelloWord();
            return "";
        }
        
        [WebMethod(Description = "定时入库数据下传SAP")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/lms/LMS_SAP_JK_01", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public LMS_SAP_JK_RESP LMS_SAP_JK_01()
        {
            return lmsApp.LMS_SAP_JK_01();
        }
        
        [WebMethod(Description = "定时同步pos中网店的作业服务接口")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/lms/LMS_POS_JK_04", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public int LMS_POS_JK_04()
        {
            return lmsApp.LMS_POS_JK_04();
        }
        
        //接口如下
        [WebMethod(Description = "商品数据接收")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/lms/SAP_LMS_JK_01", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public SAP_LMS_JK_RESP SAP_LMS_JK_01(SAP_LMS_JK01_REQ SAP_LMS_JK01_REQ)
        {
            return lmsApp.SAP_LMS_JK_01(SAP_LMS_JK01_REQ);
        }
        
        [WebMethod(Description = "供应商数据接收")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/lms/SAP_LMS_JK_02", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public SAP_LMS_JK_RESP SAP_LMS_JK_02(SAP_LMS_JK02_REQ SAP_LMS_JK02_REQ)
        {
            return lmsApp.SAP_LMS_JK_02(SAP_LMS_JK02_REQ);
        }
        [WebMethod(Description = "采购单数据接收")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/lms/SAP_LMS_JK_03", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public SAP_LMS_JK_RESP SAP_LMS_JK_03(SAP_LMS_JK03_REQ SAP_LMS_JK03_REQ)
        {
            return lmsApp.SAP_LMS_JK_03(SAP_LMS_JK03_REQ);
        }
        [WebMethod(Description = "商品价格数据接收")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/lms/SAP_LMS_JK_04", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public SAP_LMS_JK_RESP SAP_LMS_JK_04(SAP_LMS_JK04_REQ SAP_LMS_JK04_REQ)
        {
            return lmsApp.SAP_LMS_JK_04(SAP_LMS_JK04_REQ);
        }
        [WebMethod(Description = "商品类目数据接收")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/lms/SAP_LMS_JK_05", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public SAP_LMS_JK_RESP SAP_LMS_JK_05(SAP_LMS_JK05_REQ SAP_LMS_JK05_REQ)
        {
            return lmsApp.SAP_LMS_JK_05(SAP_LMS_JK05_REQ);
        }
        
        //下传POS接口
        [WebMethod(Description = "销售数据下传POS")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/lms/LMS_POS_JK_01", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public LMS_POS_JK_RESP LMS_POS_JK_01(LMS_POS_JK01_REQ LMS_POS_JK01_REQ)
        {
            return lmsApp.LMS_POS_JK_01(LMS_POS_JK01_REQ);
        }

        [WebMethod(Description = "盘存（调整）数据下传POS")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/lms/LMS_POS_JK_02", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public LMS_POS_JK_RESP LMS_POS_JK_02(LMS_POS_JK02_REQ LMS_POS_JK02_REQ)
        {
            return lmsApp.LMS_POS_JK_02(LMS_POS_JK02_REQ);
        }

        [WebMethod(Description = "报损数据下传POS")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/lms/LMS_POS_JK_03", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public LMS_POS_JK_RESP LMS_POS_JK_03(LMS_POS_JK03_REQ LMS_POS_JK03_REQ)
        {
            return lmsApp.LMS_POS_JK_03(LMS_POS_JK03_REQ);
        }
        
        
        
        [WebMethod(Description = "收货数据下传POS")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/lms/LMS_POS_JK_05", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public LMS_POS_JK_RESP LMS_POS_JK_05(LMS_POS_JK05_REQ LMS_POS_JK05_REQ)
        {
            return lmsApp.LMS_POS_JK_05(LMS_POS_JK05_REQ);
        }
        
        
        [WebMethod(Description = "定时销售数据下传SAP")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/lms/LMS_SAP_JK_02", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public LMS_SAP_JK_RESP LMS_SAP_JK_02()
        {
            return lmsApp.LMS_SAP_JK_02();
        }
        [WebMethod(Description = "定时营业款数据下传SAP")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/lms/LMS_SAP_JK_03", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public LMS_SAP_JK_RESP LMS_SAP_JK_03()
        {
            return lmsApp.LMS_SAP_JK_03();
        }
        [WebMethod(Description = "定时pos中生成网店自采入库单")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/lms/LMS_POS_JK_06", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public int LMS_POS_JK_06()
        {
            return lmsApp.LMS_POS_JK_06();
        }
        [WebMethod(Description = "定时pos中网上药店的配送单自动复核")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/lms/LMS_POS_JK_07", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public int LMS_POS_JK_07()
        {
            return lmsApp.LMS_POS_JK_07();
        }

        [WebMethod(Description = "定时pos中生成网店销售数据")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/lms/LMS_POS_JK_08", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public int LMS_POS_JK_08()
        {
            return lmsApp.LMS_POS_JK_08();
        }
        
    }
}
