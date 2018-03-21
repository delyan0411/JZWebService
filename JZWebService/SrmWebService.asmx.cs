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
    /// SrmWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://jiuzhou.com/srm/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class SrmWebService : System.Web.Services.WebService
    {
        SrmServer srmApp = new SrmServer();
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        
        [WebMethod(Description = "商品数据接收")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/srm/SAP_SRM_JK_01", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public SRM_SAP_JK_RESP SAP_SRM_JK_01(SRM_SAP_JK01_REQ SRM_SAP_JK01_REQ)
        {
            return srmApp.SAP_SRM_JK_01(SRM_SAP_JK01_REQ);
        }
        
        [WebMethod(Description = "供应商数据接收")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/srm/SAP_SRM_JK_02", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public SRM_SAP_JK_RESP SAP_SRM_JK_02(SRM_SAP_JK02_REQ SRM_SAP_JK02_REQ)
        {
            return srmApp.SAP_SRM_JK_02(SRM_SAP_JK02_REQ);
        }
        
        [WebMethod(Description = "客户数据接收")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/srm/SAP_SRM_JK_03", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public SRM_SAP_JK_RESP SAP_SRM_JK_03(SRM_SAP_JK02_REQ SRM_SAP_JK03_REQ)
        {
            return srmApp.SAP_SRM_JK_03(SRM_SAP_JK03_REQ);
        }
        

    }
}
