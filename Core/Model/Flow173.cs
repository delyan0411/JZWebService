using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    /// <summary>
    ///  付款申请上传
    /// </summary>
    public class Flow173
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
        /// 申请人ID
        /// </summary>
        public string user_id { get; set; }
        /// <summary>
        /// 公司代码 data_257 1000
        /// </summary>
        public string bukrs { get; set; }
        /// <summary>
        /// 付款单号 1010??
        /// </summary>
        public string zfkdh { get; set; }
        /// <summary>
        /// 交易类型 ??
        /// </summary>
        public string zjylx { get; set; }
        /// <summary>
        /// 银行名称  招商银行杭州转塘支行 data_259
        /// </summary>
        public string banka { get; set; }
        /// <summary>
        /// 帐户持有人姓名  黄宇兰 data_255
        /// </summary>
        public string koinh { get; set; }
        /// <summary>
        /// 银行账号(组合) 6214835897506770 data_260
        /// </summary>
        public string zbkno { get; set; }
        /// <summary>
        /// 总交易金额  100 
        /// </summary>
        public string zjyje { get; set; }
        /// <summary>
        /// 货币码 CNY
        /// </summary>
        public string waers { get; set; }
        /// <summary>
        /// 账号类型  10/20
        /// </summary>
        public string zzhty { get; set; }
        /// <summary>
        /// 抬头文本
        /// </summary>
        public string bktxt { get; set; }        
        /// <summary>
        /// 行项目 
        /// </summary>
        public List<ITEM> items { get; set; }
    }
    /// <summary>
    /// 行项目
    /// </summary>
    public class ITEM
    {
        /// <summary>
        /// 序号 1
        /// </summary>
        public string znum { get; set; }
        /// <summary>
        /// 成本中心 1000000001 item_2
        /// </summary>
        public string kostl { get; set; }
        /// <summary>
        /// G/L科目号 6601001001
        /// </summary>
        public string hkont { get; set; }
        /// <summary>
        ///成本金额 95.24 item_5
        /// </summary>
        public string zcbje { get; set; }
        /// <summary>
        /// 税科目 费用-工资
        /// </summary>
        public string zzskt { get; set; }
        /// <summary>
        /// 税额 4.76 item_6
        /// </summary>
        public string zshue { get; set; }
        /// <summary>
        /// 报销金额 100 item_8
        /// </summary>
        public string zbxje { get; set; }
        /// <summary>
        /// 备注文本 无
        /// </summary>
        public string zrmtxt { get; set; }
    }
}
