using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    /// <summary>
    ///  补考勤流程（代请 )
    /// </summary>
    public class Flow178
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
        /// 员工编号
        /// </summary>
        public string user_id { get; set; }
        /// <summary>
        /// 日期时间
        /// </summary>
        public string time{ get; set; }
        /// <summary>
        /// 时间类型
        /// </summary>
        public string satza{ get; set; }
        /// <summary>
        /// 刷卡类型
        /// </summary>
        public string abwgr{ get; set; }
    }
}
