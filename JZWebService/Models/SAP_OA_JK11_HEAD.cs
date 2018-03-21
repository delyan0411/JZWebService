using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JZWebService.Models
{
    [Serializable]
    public class SAP_OA_JK11_HEAD
    {
        public string PERNR { get; set; } //
        public string LDATE
        { get; set; } //
        public string LTIME
        { get; set; } //
        public string SATZA
        { get; set; } //
        public string ABWGR
        { get; set; } //
    }
}
