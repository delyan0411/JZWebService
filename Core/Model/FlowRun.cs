using System;
using System.Collections.Generic;


namespace Core.Model
{
    public class FlowRun
    {
        /// <summary>
        /// 工作流ID
        /// </summary>
        public int run_id { get; set; }
        /// <summary>
        /// 工作流名称
        /// </summary>
        public string run_name { get; set; }
        /// <summary>
        /// 流程ID
        /// </summary>
        public int flow_id { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string begin_user { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime begin_time { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime end_time { get; set; }
    }
}