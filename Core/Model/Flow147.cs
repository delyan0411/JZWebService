using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    /// <summary>
    ///  九洲大药房_SAP_HR_PT_130_出差流程_V2.0
    /// </summary>
    public class Flow147
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
        /// 加班人用户ID
        /// </summary>
        public string user_id { get; set; }
        /// <summary>
        /// 出差结束时间
        /// </summary>
        public string i_begda { get; set; }
        /// <summary>
        /// 出差开始时间
        /// </summary>
        public string i_endda { get; set; }
        /// <summary>
        /// 出差类型
        /// </summary>
        public string i_awart { get; set; }
    }
}
