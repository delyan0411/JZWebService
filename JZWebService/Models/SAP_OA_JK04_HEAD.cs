using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JZWebService.Models
{
    [Serializable]
    public class SAP_OA_JK04_HEAD
    {
        public string BUKRS { get; set; } //公司代码
        public string ZFKDH { get; set; } //付款单号 按时间生成
        public string ZJYLX { get; set; } //交易类型 1010
        public string BANKA { get; set; } //银行名称
        public string KOINH { get; set; } //帐户持有人姓名
        public string ZBKNO { get; set; } //银行账号(组合)
        public string ZJYJE { get; set; } //总交易金额
        public string WAERS { get; set; } //货币码
        public string ZZHTY { get; set; } //账号类型
        public string BKTXT { get; set; }//抬头文本
        public string ZPRCT { get; set; }//收款开户地
        public string EACBNK { get; set; }//他行户口开户行 
        public string ZCTNO { get; set; }//城市代码
        public string CDTBRD { get; set; }//银行的联行号
        public SAP_OA_JK04_HEAD_ITEM[] ITEMS { get; set; }//行项目
    }
    [Serializable]
    public class SAP_OA_JK04_HEAD_ITEM
    {
        public string ZNUM { get; set; }//序号
        public string KOSTL { get; set; }//成本中心
        public string HKONT { get; set; }//G/L科目号
        public string ZCBJE { get; set; }//成本金额
        public string ZZSKT { get; set; }//税科目
        public string ZSHUE { get; set; }//税额
        public string ZBXJE { get; set; }//报销金额
        public string ZRMTXT { get; set; }//备注文本
    }
}
