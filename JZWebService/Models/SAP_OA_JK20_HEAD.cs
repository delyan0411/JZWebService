using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JZWebService.Models
{
    [Serializable]
    public class SAP_OA_JK20_HEAD
    {
        public string I_PERNR { get; set; } //人员编号    
        public string I_BEGDA{ get; set; } //离职日期
        public string I_MASSG{ get; set; } //离职操作原因代码
    }
}
