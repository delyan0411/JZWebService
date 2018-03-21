using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    /// <summary>
    ///  办公室人员辞职
    /// </summary>
    public class Flow168
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
        /// 人员编号
        /// </summary>
        public string user_id { get; set; }
        ///// <summary>
        ///// 请假数据
        ///// </summary>
        //public string i_pernr { get; set; }
        /// <summary>
        /// 离职日期
        /// </summary>
        public string i_begda { get; set; }
        /// <summary>
        /// 离职操作原因
        /// </summary>
        public string i_massg { get; set; }
    }
}
