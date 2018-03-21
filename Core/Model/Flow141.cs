using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    /// <summary>
    /// 九洲大药房_SAP_HR_PT_100_办公室人员请假流程
    /// </summary>
    public class Flow141
    {
        /// <summary>
        /// 工作ID
        /// </summary>
        public int run_id { get; set; }
        /// <summary>
        /// 工作名称
        /// </summary>
        public string run_name { get; set; }
        /// <summary>
        /// 请假人用户ID
        /// </summary>
        public string user_id { get; set; }
        /// <summary>
        /// 请假数据
        /// </summary>
        public string data { get; set; }
        /// <summary>
        /// 请假类型
        /// </summary>
        public string i_awart { get; set; }
    }
    public enum I_awartType
    {
        年假 = 2000,
        事假 = 2010,
        病假 = 2020,
        婚假 = 2030,
        丧假 = 2040,
        产假 = 2050,
        工伤假 = 2060,
        流产假 = 2070,
        妇女节福利假 = 2080,
        陪产假 = 2090,
        调休 = 9998
    }
}
