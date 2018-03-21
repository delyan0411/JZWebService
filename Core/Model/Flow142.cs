using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    /// <summary>
    ///  九洲大药房_SAP_HR_PT_110_门店店长请假流程
    /// </summary>
    public class Flow142
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
}
