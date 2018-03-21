using System;
using System.Collections.Generic;


namespace Core.Model
{
    public class RuleInfo
    {
        /// <summary>
        /// 流程ID
        /// </summary>
        public int flowid { get; set; }
        /// <summary>
        /// 调用的webserivce名称
        /// </summary>
        public string wsname { get; set; }
        /// <summary>
        /// 是否结束流程
        /// </summary>
        public int isover { get; set; }
        /// <summary>
        /// 流程数据
        /// </summary>
        public List<RuleInfoData> data { get; set; }
    }
    public class RuleInfoData
    {
        /// <summary>
        /// data字段名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// data值
        /// </summary>
        public string val { get; set; }
        /// <summary>
        /// 是否是列表
        /// </summary>
       public bool is_list { get; set; }
    }
}