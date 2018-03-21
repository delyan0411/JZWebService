using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JZWebService.Models
{
    [Serializable]
    public class SAP_LMS_JK05_HEAD
    {
        public string MATKL { get; set; } //物料组
        public string WGBEZ { get; set; } //物料组描述
        public string KLAHCLASS { get; set; } //物料组层级分类
        public string SWORKSCHL { get; set; } //物料组层级分类描述
    }
}
