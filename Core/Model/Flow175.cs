using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    /// <summary>
    ///  门店人员加班流程（代申请）
    /// </summary>
    public class Flow175
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
        /// 加班数据
        /// </summary>
        public string starttime { get; set; }
        /// <summary>
        /// 加班数据
        /// </summary>
        public string endtime { get; set; }
        /// <summary>
        /// 加班补偿类型
        /// </summary>
        public string i_versl{ get; set; }
    }
}
