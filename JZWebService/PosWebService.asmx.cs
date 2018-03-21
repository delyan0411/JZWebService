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
    /// PosWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://jiuzhou.com/pos/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class PosWebService : System.Web.Services.WebService
    {
        PosService   posApp = new PosService();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod(Description = "定时同步上传网店自采入库数据至sap")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://jiuzhou.com/pos/POS_SAP_JK_01", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public int POS_SAP_JK_01()
        {
            return posApp.POS_SAP_JK_01();
        }


    }
}
