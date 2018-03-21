using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JZWebService.Models
{
    [Serializable]
    public class SAP_LMS_JK02_HEAD
    {
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string LIFNR { get; set; }
        /// <summary>
        /// 供应商描述(名称)
        /// </summary>
        public string NAME1 { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string STRAS { get; set; }
        /// <summary>
        /// 首营企业编号
        /// </summary>
        public string ZSYQYBH { get; set; }
        /// <summary>
        /// 法定代表人 
        /// </summary>
        public string ZFARE { get; set; }
        /// <summary>
        /// 企业联系人
        /// </summary>
        public string ZQYFZR { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string ZQYFZRTEL { get; set; }
        /// <summary>
        /// 统计类型
        /// </summary>
        public string ZTJLX { get; set; }
    }
}