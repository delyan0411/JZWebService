using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JZWebService.Db;
using JZWebService.Models;
using System.Configuration;
using JZWebService.Request;
using JZWebService.Response;
using System.Data.Common;
using System.Data;
using JZWebService.Util;
using JZWebService.error;
using System.Data.SqlClient;
using System.Net;
using System.Threading;

namespace JZWebService
{
    public class LmsServer
    {
        //采用如下变量进行控制服务允许执行
        public static string lmsFlag = System.Configuration.ConfigurationManager.AppSettings["lmsserver"].ToString();
        MsgError loginfo = new MsgError();
        public static string lmsSqlConnectionString = System.Configuration.ConfigurationManager.AppSettings["LmsSqlConnString"].ToString();
        public static string lmsConnectionString = System.Configuration.ConfigurationManager.AppSettings["lmsconnString"].ToString();

        public static string sapUser = System.Configuration.ConfigurationManager.AppSettings["SAPURLLMSUSER"].ToString();
        public static string sapPsw = System.Configuration.ConfigurationManager.AppSettings["SAPURLLMSPSW"].ToString();


        LmsSqlDbHelper sqlDB = new LmsSqlDbHelper(lmsSqlConnectionString);
        LmsDbHelper lmsDB = new LmsDbHelper(lmsConnectionString);

        public string HelloWord()
        {
            LMS_POS_JK01_REQ rq = new LMS_POS_JK01_REQ();
            LMS_POS_JK01_HEAD hd = new LMS_POS_JK01_HEAD();
            hd.Fh_lsh = "121212121";
            hd.Order_No = "888888";
            hd.Quantity = 1212;
            hd.GoodsCode = "201050520";
            hd.Order_Date = Convert.ToDateTime("2017-10-01");

            LMS_POS_JK01_HEAD[] m_HEAD = new LMS_POS_JK01_HEAD[1];
            m_HEAD[0] = hd;
            rq.HEAD = m_HEAD;
            this.LMS_POS_JK_01(rq);
            return "helloword";
        }
        /// <summary>
        /// 商品数据接收
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        public SAP_LMS_JK_RESP SAP_LMS_JK_01(SAP_LMS_JK01_REQ rq)
        {
            SAP_LMS_JK_RESP resp = new SAP_LMS_JK_RESP();
            #region 处理逻辑

            //处理逻辑
            SAP_LMS_JK01_HEAD[] heads = rq.HEAD;
            List<DbParameter> paramList = new List<DbParameter>();
            string sqlStr = "";
            int num = 0;
            string xmlStr = "";
            string tempId = Guid.NewGuid().ToString();
            foreach (SAP_LMS_JK01_HEAD h in heads)
            {
                //商品分类ID
                int typeID = 0;
                if (!int.TryParse(h.MATKL, out typeID))
                {
                    //物料组字段类型有误
                }
                decimal SL = 0;
                if (!decimal.TryParse(h.ZJXSL, out SL))
                {
                }
                string productName = string.Empty;
                if (string.IsNullOrEmpty(h.ZTYMI))
                {
                    if (!string.IsNullOrEmpty(h.MAKTM) && h.MAKTM.IndexOf('|') > 0)
                        productName = h.MAKTM.Substring(0, h.MAKTM.IndexOf('|'));
                    else
                        productName = h.MAKTM;
                }
                else
                {
                    productName = h.ZTYMI;
                }
                //物料分类信息
                //DataSet ds = lmsDB.ExecuteDataset(string.Format("SELECT TOP 1 n_id,n_name from  ProductCore_Server.dbo.Product_Type_Main where n_id={0} ", typeID));
                //string p_rootName = "";
                //if (ds.Tables.Count > 0)
                //{
                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        DataTable dt = ds.Tables[0];
                //        p_rootName = dt.Rows[0]["n_name"].ToString();
                //    }
                //}

                xmlStr += "执行商品同步操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<SAP_LMS_JK01_HEAD>(h) + "\r\n";
                //string pName = h.MAKTM;
                //if (!string.IsNullOrEmpty(h.MAKTM) && h.MAKTM.IndexOf('|') > 0)
                //    pName = h.MAKTM.Substring(0, h.MAKTM.IndexOf('|'));
                //将信息插入临时表
                sqlStr += string.Format(" insert into ProductCore_Server.dbo.SapToProductTemp (Weight,Commentcount,Integral,Isonsale,Isvisible,Product_Num,Market_Price,Member_Price,P_Spec,P_License,P_Manufacturer,P_Name,Product_Num_Zb,P_Common_Name,P_Ccfl,Brand,Shopid,Product_Num_Main,Price_New,Price_Avg,P_Type_Main,P_Is_Prescription_Drug,Iszp,Add_User,tempid,[status],IsFreeFare,FreeFareStartTime,FreeFareEndTime,show_name,monthSales,monthClicks,mobile_price,weekSales,VirtualMoney,sales7,sales30,sales90,sales15,suggestRetailPrice,minRetailPrice,explosionPrice,activePrice,IsVIP,tax_rate,add_date) VALUES (0,0,0,0,1,'{0}',0,0,'{1}','{2}','{3}','{4}','{0}','{5}',0,'{6}',1,'{0}',0,0,{9},'{7}',0,'SAP','{8}',0,1,GETDATE(),GETDATE(),'1',1,1,1,1,1,0,0,0,0,1,1,1,1,1,{10},GETDATE());\r\n", h.MATNR, h.ZGUGE, h.ZPZWH, h.NAME1, productName, h.ZTYMI, h.ZPPAI, h.ZSFYP, tempId, typeID, SL);
            }
            try
            {
                if (sqlStr != "")
                {
                    num = lmsDB.ExecuteNonQuery(sqlStr);
                    if (num > 0)
                    {
                        //执行插入商品信息处理
                        if (lmsDB.ExecuteNonQuery(string.Format("exec ProductCore_Server.dbo.sp_SapProductToLMS '{0}'", tempId)) > 0)
                        {
                            resp.MSGCODE = "1";
                            resp.MSGTXT = "[执行成功]：" + xmlStr;
                        }
                        else
                        {
                            resp.MSGCODE = "-1";
                            resp.MSGTXT = "[执行失败]：";
                        }
                    }
                }
                else
                {
                    resp.MSGCODE = "-1";
                    resp.MSGTXT = "[执行失败]：推送值有误";
                }
            }
            catch (Exception ex)
            {
                resp.MSGCODE = "-1";
                resp.MSGTXT = ex.Message;
                loginfo.logerror("接口SAP_LMS_JK_01错误信息：" + resp.MSGTXT);
            }

            loginfo.loginfor("接口SAP_LMS_JK_01返回信息：" + resp.MSGTXT);

            #endregion
            return resp;
        }
        /// <summary>
        /// 供应商数据接收
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        public SAP_LMS_JK_RESP SAP_LMS_JK_02(SAP_LMS_JK02_REQ rq)
        {
            SAP_LMS_JK_RESP resp = new SAP_LMS_JK_RESP();
            #region 处理逻辑
            //处理逻辑
            SAP_LMS_JK02_HEAD[] heads = rq.HEAD;

            List<DbParameter> paramList = new List<DbParameter>();
            string sqlStr = "";
            int num = 0;
            string xmlStr = "";

            foreach (SAP_LMS_JK02_HEAD h in heads)
            {
                sqlStr = "UPDATE ErpCore_Server.dbo.erp_SupZB SET f_id=@f_id, sup_name=@sup_name,address=@address,owner=@owner,conter=@conter, conttel=@conttel  WHERE f_id=@f_id";

                paramList.Clear();
                paramList.Add(lmsDB.MakeInParam("f_id", int.Parse(h.LIFNR)));
                paramList.Add(lmsDB.MakeInParam("sup_name", h.NAME1));
                paramList.Add(lmsDB.MakeInParam("address", h.STRAS));
                paramList.Add(lmsDB.MakeInParam("owner", h.ZFARE));
                paramList.Add(lmsDB.MakeInParam("conter", h.ZQYFZR));
                paramList.Add(lmsDB.MakeInParam("conttel", h.ZQYFZRTEL));
                //paramList.Add(lmsDB.MakeInParam("entId", h.ZSYQYBH));

                try
                {
                    ///执行sql
                    num = lmsDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());

                    if (num == 0)
                    {
                        sqlStr = "";
                        xmlStr += "执行插入操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<SAP_LMS_JK02_HEAD>(h) + "\r\n";
                        sqlStr = "INSERT INTO ErpCore_Server.dbo.erp_SupZB(f_id,sup_name,address,owner,conter,conttel,f_source,isEnable,sup_type) VALUES(@f_id,@sup_name,@address,@owner,@conter,@conttel,2,1,0)";

                        num = lmsDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());
                        if (num == 0)
                        {
                            resp.MSGCODE = "-1";
                            resp.MSGTXT = "[执行失败]：" + xmlStr;
                        }
                        else
                        {
                            resp.MSGCODE = "1";
                            resp.MSGTXT = "[执行成功]：" + xmlStr;
                        }
                    }
                    else
                    {
                        xmlStr += "执行更新操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<SAP_LMS_JK02_HEAD>(h) + "\r\n";
                        resp.MSGCODE = "1";
                        resp.MSGTXT = "[执行成功]：" + xmlStr;
                    }

                }
                catch (Exception ex)
                {
                    resp.MSGCODE = "-1";
                    resp.MSGTXT = ex.Message;
                    loginfo.logerror("接口SAP_LMS_JK_02错误信息：" + resp.MSGTXT);
                }

                loginfo.loginfor("接口SAP_LMS_JK_02返回信息：" + resp.MSGTXT);
                ////
            }
            #endregion
            return resp;
        }
        /// <summary>
        /// 采购单数据接收
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        public SAP_LMS_JK_RESP SAP_LMS_JK_03(SAP_LMS_JK03_REQ rq)
        {
            SAP_LMS_JK_RESP resp = new SAP_LMS_JK_RESP();
            #region 处理逻辑
            //处理逻辑
            SAP_LMS_JK03_HEAD[] heads = rq.HEAD;

            List<DbParameter> paramList = new List<DbParameter>();
            string sqlStr = "";
            int num = 0;
            string xmlStr = "";
            string Arrivedate = "";
            decimal allMoney = 0;
            string paybill = "";
            foreach (SAP_LMS_JK03_HEAD h in heads)
            {
                bool isIn = true;
                foreach (Products pp in h.ITEM)
                {
                    Arrivedate = !string.IsNullOrEmpty(pp.EINDT) ? pp.EINDT.Substring(0, 4) + "/" + pp.EINDT.Substring(4, 2) + "/" + pp.EINDT.Substring(6) : DateTime.Now.ToString("yyyy/MM/dd");
                    allMoney = allMoney + (pp.MENGE * pp.NETPR);

                }
                string supName = "";
                long supId = 0;
                DataSet ds = lmsDB.ExecuteDataset(string.Format("select top 1  sup_id ,sup_name,f_id from ErpCore_Server.dbo.erp_SupZB where f_id={0} ", int.Parse(h.LIFNR)));
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        supName = dt.Rows[0]["sup_name"].ToString();
                        supId = long.Parse(dt.Rows[0]["sup_id"].ToString());
                    }
                }

                switch (h.BSART)
                {

                    case "Z001":
                        paybill = "经销商品采购";
                        break;
                    case "Z002":
                        paybill = "经销商品采购退货";
                        isIn = false;
                        break;
                    case "Z003":
                        paybill = "实销月结采购";
                        break;
                    case "Z004":
                        paybill = "实销月结采购退货";
                        isIn = false;
                        break;
                    case "Z005":
                        paybill = "非商品类采购";
                        break;
                    case "Z006":
                        paybill = "非商品类采购退货";
                        isIn = false;
                        break;
                }
                //正向入库（退货待考虑）
                if (isIn)
                {
                    try
                    {
                        //已审核的采购单不存在更新情况
                        sqlStr += string.Format(" insert into  ErpCore_Server.dbo.erp_purchaseBill (billNo,supplier,supName,status,Arrivedate,isVisible,isReceive,addDate,supId,billMoney,payBill) values('{0}','九洲采购','{1}',2,'{2}',1,0,'{3}',{4},{5},'{6}'); \r\n", h.EBELN, supName, Arrivedate, DateTime.Now.ToString(), supId, allMoney, paybill);
                        foreach (Products p in h.ITEM)
                        {
                            #region 取出网店维护商品信息

                            string productNum = p.MATNR;
                            decimal rate = 17;
                            decimal ratePrice = p.NETPR;
                            DataSet pds = lmsDB.ExecuteDataset(string.Format("select top 1  product_num ,tax_rate from ProductCore_Server.dbo.Product where product_num_zb='{0}' and isVisible=1 ", p.MATNR));
                            if (pds.Tables.Count > 0)
                            {
                                if (pds.Tables[0].Rows.Count > 0)
                                {
                                    DataTable dt = pds.Tables[0];
                                    productNum = dt.Rows[0]["product_num"].ToString();
                                    rate = decimal.Parse(dt.Rows[0]["tax_rate"].ToString());
                                    ratePrice = Math.Round(p.NETPR * (1 + (rate / 100)), 2, MidpointRounding.AwayFromZero);
                                }
                            }

                            #endregion
                            sqlStr += string.Format(" insert into ErpCore_Server.dbo.erp_purchaseBillDetail(billNumber,product_number,quantity,price,batch_number,isVisible,Stocked,itemIndex,product_num_sap)values('{0}','{1}',{2},{3},'{4}',1,0,{5},'{6}'); \r\n", h.EBELN, productNum, p.MENGE, ratePrice, p.CHARG, int.Parse(p.EBELP), p.MATNR);
                        }

                        ///执行sql
                        num = lmsDB.ExecuteNonQuery(sqlStr);
                        if (num == 0)
                        {
                            xmlStr += "执行插入操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<SAP_LMS_JK03_HEAD>(h) + "\r\n";
                            resp.MSGCODE = "-1";
                            resp.MSGTXT = "[执行失败]：" + xmlStr;

                        }
                        else
                        {
                            xmlStr += "执行插入操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<SAP_LMS_JK03_HEAD>(h) + "\r\n";
                            resp.MSGCODE = "1";
                            resp.MSGTXT = "[执行成功]：" + xmlStr;
                        }
                    }
                    catch (Exception ex)
                    {
                        resp.MSGCODE = "-1";
                        resp.MSGTXT = resp.MSGTXT + "/r/n错误开始记录位置：" + ex.Message;
                        loginfo.logerror("接口SAP_LMS_JK_03错误信息：" + resp.MSGTXT);
                    }
                }
                //采购退货
                else
                {
                    try
                    {
                        string sql11 = string.Format(" insert into  ErpCore_Server.dbo.erp_purchaseReturnBill (billNo,supplier,supName,status,addDate,supId,storage_number) values('{0}','九洲采购','{1}',1,'{2}',{3},'{4}') \r\n", h.EBELN, supName, DateTime.Now.ToString(), supId, "");
                        if (lmsDB.ExecuteNonQuery(sql11) > 0)
                        {
                            foreach (Products p in h.ITEM)
                            {
                                string productNum = p.MATNR;
                                decimal rate = 17;
                                decimal ratePrice = p.NETPR;
                                DataSet pds = lmsDB.ExecuteDataset(string.Format("select top 1  product_num ,tax_rate from ProductCore_Server.dbo.Product where product_num_zb='{0}' and isVisible=1", p.MATNR));
                                if (pds.Tables.Count > 0)
                                {
                                    if (pds.Tables[0].Rows.Count > 0)
                                    {
                                        DataTable dt = pds.Tables[0];
                                        productNum = dt.Rows[0]["product_num"].ToString();
                                        rate = decimal.Parse(dt.Rows[0]["tax_rate"].ToString());
                                        ratePrice = Math.Round(p.NETPR * (1 + (rate / 100)), 2, MidpointRounding.AwayFromZero);
                                    }
                                }
                                //int batchId = 0;
                                //DataSet bds = lmsDB.ExecuteDataset(string.Format("select top 1  id  from ErpCore_Server.dbo.erp_batchstorage where product_number='{0}' and quantity={1} ", productNum, p.MENGE));
                                //if (bds.Tables.Count > 0)
                                //{
                                //    if (bds.Tables[0].Rows.Count > 0)
                                //    {
                                //        DataTable dt = bds.Tables[0];
                                //        batchId = int.Parse(dt.Rows[0]["id"].ToString());
                                //        productNum = dt.Rows[0]["product_number"].ToString();
                                //    }
                                //}
                                sqlStr += string.Format(" insert into ErpCore_Server.dbo.erp_purchaseReturnBillDetail(billId,batchId,product_number,price,quantity,actualQuantity,isVisible,product_num_sap,itemIndex) select id,{1},'{2}',{3},{4},1,1,'{5}',{6} from ErpCore_Server.dbo.erp_purchaseReturnBill  where billNo='{0}' \r\n", h.EBELN, 0, productNum, ratePrice, p.MENGE, p.MATNR, int.Parse(p.EBELP));
                            }

                            ///执行sql
                            num = lmsDB.ExecuteNonQuery(sqlStr);
                            if (num == 0)
                            {
                                xmlStr += "执行插入操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<SAP_LMS_JK03_HEAD>(h) + "\r\n";
                                resp.MSGCODE = "-1";
                                resp.MSGTXT = "[执行失败]：" + xmlStr;

                            }
                            else
                            {
                                xmlStr += "执行插入操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<SAP_LMS_JK03_HEAD>(h) + "\r\n";
                                resp.MSGCODE = "1";
                                resp.MSGTXT = "[执行成功]：" + xmlStr;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        resp.MSGCODE = "-1";
                        resp.MSGTXT = resp.MSGTXT + "/r/n错误开始记录位置：" + ex.Message;
                        loginfo.logerror("接口SAP_LMS_JK_03错误信息：" + resp.MSGTXT);
                    }
                }
                loginfo.loginfor("接口SAP_LMS_JK_03返回信息：" + resp.MSGTXT);
            }
            #endregion
            return resp;
        }
        /// <summary>
        /// 商品价格数据接收
        /// </summary>
        /// <param name="rq"></param>
        /// <returns></returns>
        public SAP_LMS_JK_RESP SAP_LMS_JK_04(SAP_LMS_JK04_REQ rq)
        {
            SAP_LMS_JK_RESP resp = new SAP_LMS_JK_RESP();
            #region 处理逻辑
            //处理逻辑
            SAP_LMS_JK04_HEAD[] heads = rq.HEAD;

            List<DbParameter> paramList = new List<DbParameter>();
            string sqlStr = "";
            int num = 0;
            string xmlStr = "";
            foreach (SAP_LMS_JK04_HEAD h in heads)
            {
                try
                {
                    //零售价
                    if (h.KSCHL == "ZP01") //2700代表线上门店
                    {
                        foreach (Prices p in h.ITEM)
                        {
                            if (p.KUNNR == "2700" || p.KONDA == "10" || p.KONDA == "20" || p.KONDA == "50")//2700代表线上门店,10，20代表所有门店，及九洲门店
                            {
                                sqlStr = string.Format("UPDATE ProductCore_Server.dbo.Product SET suggestretailprice=@suggestretailprice  WHERE product_num=@product_num");
                                paramList.Clear();
                                paramList.Add(lmsDB.MakeInParam("suggestretailprice", p.KBETR));
                                paramList.Add(lmsDB.MakeInParam("product_num", p.MATNR));
                                ///执行sql
                                num = lmsDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());

                                if (num == 0)
                                {
                                    xmlStr += "执行插入操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<Prices>(p) + "\r\n";
                                    resp.MSGCODE = "-1";
                                    resp.MSGTXT = "[执行失败]：" + xmlStr;
                                }
                                else
                                {
                                    xmlStr += "执行更新操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<Prices>(p) + "\r\n";
                                    resp.MSGCODE = "1";
                                    resp.MSGTXT = "[执行成功]：" + xmlStr;
                                }
                            }
                            ///
                            loginfo.loginfor("接口SAP_LMS_JK_04返回信息：" + resp.MSGTXT);
                        }
                    }
                    //会员价
                    else if (h.KSCHL == "ZP02")
                    {
                        foreach (Prices p in h.ITEM)
                        {
                            if (p.KUNNR == "2700" || p.KONDA == "10" || p.KONDA == "20" || p.KONDA == "50")//2700代表线上门店,10，20代表所有门店，及九洲门店
                            {
                                sqlStr = string.Format("UPDATE ProductCore_Server.dbo.Product SET member_price=@member_price  WHERE product_num=@product_num");
                                paramList.Clear();
                                paramList.Add(lmsDB.MakeInParam("member_price", p.KBETR));
                                paramList.Add(lmsDB.MakeInParam("product_num", p.MATNR));
                                ///执行sql
                                num = lmsDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());

                                if (num == 0)
                                {
                                    xmlStr += "执行插入操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<Prices>(p) + "\r\n";
                                    resp.MSGCODE = "-1";
                                    resp.MSGTXT = "[执行失败]：" + xmlStr;
                                }
                                else
                                {
                                    xmlStr += "执行更新操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<Prices>(p) + "\r\n";
                                    resp.MSGCODE = "1";
                                    resp.MSGTXT = "[执行成功]：" + xmlStr;
                                }
                            }
                            loginfo.loginfor("接口SAP_LMS_JK_04返回信息：" + resp.MSGTXT);
                        }
                    }
                    //促销价暂无对应项
                    else if (h.KSCHL == "ZP03")
                    {

                    }
                }
                catch (Exception ex)
                {
                    resp.MSGCODE = "-1";
                    resp.MSGTXT = ex.Message;
                    loginfo.logerror("接口SAP_LMS_JK_04错误信息：" + resp.MSGTXT);
                }

            }
            #endregion
            return resp;
        }
        //商品分类信息获取
        public SAP_LMS_JK_RESP SAP_LMS_JK_05(SAP_LMS_JK05_REQ rq)
        {

            SAP_LMS_JK_RESP resp = new SAP_LMS_JK_RESP();
            SAP_LMS_JK05_HEAD[] heads = rq.HEAD;
            List<DbParameter> paramList = new List<DbParameter>();
            string sqlStr = "";
            int num = 0;
            string xmlStr = "";
            foreach (SAP_LMS_JK05_HEAD h in heads)
            {
                try
                {
                    num = num + 1;
                    int parentid = 0;
                    if (!string.IsNullOrEmpty(h.KLAHCLASS))
                        if (!int.TryParse(h.KLAHCLASS, out parentid))
                        {
                            xmlStr += "命令行：" + "\r\n" + XmlSerializeHelper.XmlSerialize<SAP_LMS_JK05_HEAD>(h) + "\r\n";
                            xmlStr += "KLAHCLASS参数格式有误";
                            resp.MSGCODE = "-1";
                            resp.MSGTXT = "[执行失败]：" + xmlStr;
                            continue;
                        }
                    if (num < 100)
                    {
                        sqlStr = "UPDATE ProductCore_Server.dbo.Product_Type_Main set n_name=@n_name,parentid=@parentid WHERE n_id=@n_id";
                        paramList.Clear();
                        paramList.Add(lmsDB.MakeInParam("n_name", h.WGBEZ));
                        paramList.Add(lmsDB.MakeInParam("parentid", parentid));
                        paramList.Add(lmsDB.MakeInParam("n_id", int.Parse(h.MATKL)));

                        int count = lmsDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());
                        if (count == 0)
                        {
                            sqlStr = "";
                            xmlStr += "执行插入操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<SAP_LMS_JK05_HEAD>(h) + "\r\n";
                            sqlStr = "INSERT INTO ProductCore_Server.dbo.Product_Type_Main(n_id,n_name,parentid) VALUES(@n_id,@n_name,@parentid)";
                            paramList.Clear();
                            paramList.Add(lmsDB.MakeInParam("n_name", h.WGBEZ));
                            paramList.Add(lmsDB.MakeInParam("parentid", parentid));
                            paramList.Add(lmsDB.MakeInParam("n_id", int.Parse(h.MATKL)));

                            count = lmsDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());
                            if (count == 0)
                            {
                                resp.MSGCODE = "-1";
                                resp.MSGTXT = "[执行失败]：" + xmlStr;
                            }
                            else
                            {
                                resp.MSGCODE = "1";
                                resp.MSGTXT = "[执行成功]：" + xmlStr;
                            }
                        }
                        else
                        {
                            xmlStr += "执行更新操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<SAP_LMS_JK05_HEAD>(h) + "\r\n";
                            resp.MSGCODE = "1";
                            resp.MSGTXT = "[执行成功]：" + xmlStr;
                        }
                    }
                }
                catch (Exception ex)
                {
                    resp.MSGCODE = "-1";
                    resp.MSGTXT = ex.Message;
                    loginfo.logerror("接口LMS_POS_JK_01错误信息：" + resp.MSGTXT);
                }


            }
            loginfo.loginfor("接口SAP_LMS_JK_05返回信息：" + resp.MSGTXT);
            ////

            return resp;
        }


        //销售数据下传
        public LMS_POS_JK_RESP LMS_POS_JK_01(LMS_POS_JK01_REQ rq)
        {

            LMS_POS_JK_RESP resp = new LMS_POS_JK_RESP();
            LMS_POS_JK01_HEAD[] heads = rq.HEAD;
            List<DbParameter> paramList = new List<DbParameter>();
            string xmlStr = "";
            foreach (LMS_POS_JK01_HEAD h in heads)
            {
                string sqlStr = "";
                int num = 0;
                paramList.Clear();
                paramList.Add(sqlDB.MakeInParam("Fh_Lsh", h.Fh_lsh));
                paramList.Add(sqlDB.MakeInParam("Order_No", h.Order_No));
                paramList.Add(sqlDB.MakeInParam("PlatType", h.Plat_Type));
                paramList.Add(sqlDB.MakeInParam("Order_Date", h.Order_Date));
                paramList.Add(sqlDB.MakeInParam("Store_No", h.Store_No));
                paramList.Add(sqlDB.MakeInParam("Order_Money", h.Order_Money));
                paramList.Add(sqlDB.MakeInParam("Trans_Money", h.Trans_Money));
                paramList.Add(sqlDB.MakeInParam("Discount_Money", h.Discount_Money));
                paramList.Add(sqlDB.MakeInParam("Pay_Type", h.Pay_Type));
                paramList.Add(sqlDB.MakeInParam("Order_Remark", h.Order_Remark));

                paramList.Add(sqlDB.MakeInParam("Order_Status", h.Order_Status));
                paramList.Add(sqlDB.MakeInParam("GoodsCode", h.GoodsCode));
                paramList.Add(sqlDB.MakeInParam("Quantity", h.Quantity));
                paramList.Add(sqlDB.MakeInParam("Price", h.SalePrice));
                paramList.Add(sqlDB.MakeInParam("DeliverType", h.DeliverType));
                paramList.Add(sqlDB.MakeInParam("ReceiveMan", h.ReceiveMan));

                paramList.Add(sqlDB.MakeInParam("ReceiveTel", h.ReceiveTel));
                paramList.Add(sqlDB.MakeInParam("ReceiveAddress", h.ReceiveAddress));
                paramList.Add(sqlDB.MakeInParam("dataDate", DateTime.Now.ToString()));
                paramList.Add(sqlDB.MakeInParam("Express_Type", h.Express_Type));
                paramList.Add(sqlDB.MakeInParam("Express_No", h.Express_No));
                paramList.Add(sqlDB.MakeInParam("Order_Flag", h.Order_Flag));

                paramList.Add(sqlDB.MakeInParam("Batch_NO", h.Batch_NO));
                paramList.Add(sqlDB.MakeInParam("Batch_ID", h.Batch_ID));


                if (num == 0)
                {
                    xmlStr += "执行插入操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<LMS_POS_JK01_HEAD>(h) + "\r\n";
                    sqlStr = "INSERT INTO so_OrderList(Fh_Lsh,Order_No,PlatType,Order_Date,Store_No,Order_Money,Trans_Money,Discount_Money,Pay_Type,Order_Remark,Order_Status,GoodsCode,Quantity,Price,DeliverType,ReceiveMan,ReceiveTel,ReceiveAddress,dataDate,Express_Type,Express_No,Batch_NO,Batch_ID,Order_Flag) VALUES(@Fh_Lsh,@Order_No,@PlatType,@Order_Date,@Store_No,@Order_Money,@Trans_Money,@Discount_Money,@Pay_Type,@Order_Remark,@Order_Status,@GoodsCode,@Quantity,@Price,@DeliverType,@ReceiveMan,@ReceiveTel,@ReceiveAddress,@dataDate,@Express_Type,@Express_No,@Batch_NO,@Batch_ID,@Order_Flag)";

                    try
                    {
                        num = sqlDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());
                        if (num == 0)
                        {
                            resp.MSGCODE = "-1";
                            resp.MSGTXT = "[执行失败]：" + xmlStr;
                        }
                        else
                        {
                            resp.MSGCODE = "1";
                            resp.MSGTXT = "[执行成功]：" + xmlStr;
                        }
                    }
                    catch (Exception ex)
                    {
                        resp.MSGCODE = "-1";
                        resp.MSGTXT = ex.Message;
                        loginfo.logerror("接口LMS_POS_JK_01错误信息：" + resp.MSGTXT);
                    }

                }
                loginfo.loginfor("接口LMS_POS_JK_01返回信息：" + resp.MSGTXT);
                ////
            }
            ////
            return resp;
        }

        //盘存（调整）数据下传
        public LMS_POS_JK_RESP LMS_POS_JK_02(LMS_POS_JK02_REQ rq)
        {

            LMS_POS_JK_RESP resp = new LMS_POS_JK_RESP();
            LMS_POS_JK02_HEAD[] heads = rq.HEAD;

            List<DbParameter> paramList = new List<DbParameter>();
            string xmlStr = "";

            foreach (LMS_POS_JK02_HEAD h in heads)
            {
                string sqlStr = "";
                int num = 0;
                paramList.Clear();
                paramList.Add(sqlDB.MakeInParam("Adjust_Lsh", h.Adjust_Lsh));
                paramList.Add(sqlDB.MakeInParam("Adjust_No", h.Adjust_No));
                paramList.Add(sqlDB.MakeInParam("Adjust_Flag", h.Adjust_Flag));
                paramList.Add(sqlDB.MakeInParam("Stock_Locale", h.Stock_Locale));

                paramList.Add(sqlDB.MakeInParam("GoodsCode", h.GoodsCode));
                paramList.Add(sqlDB.MakeInParam("Adjust_Date", h.Adjust_Date));
                paramList.Add(sqlDB.MakeInParam("Batch_NO", h.Batch_NO));
                paramList.Add(sqlDB.MakeInParam("Batch_ID", h.Batch_ID));

                paramList.Add(sqlDB.MakeInParam("Quantity", h.Quantity));
                paramList.Add(sqlDB.MakeInParam("Amount", h.Amount));
                paramList.Add(sqlDB.MakeInParam("Adjust_Cause", h.Adjust_Cause));
                paramList.Add(sqlDB.MakeInParam("DataDate", DateTime.Now.ToString()));

                if (num == 0)
                {
                    xmlStr += "执行插入操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<LMS_POS_JK02_HEAD>(h) + "\r\n";
                    sqlStr = "INSERT INTO so_AdjustBatch(Adjust_Lsh,Adjust_No,Adjust_Flag,Stock_Locale,GoodsCode,Adjust_Date,Batch_NO,Batch_ID,Quantity,Amount,Adjust_Cause,DataDate) VALUES(@Adjust_Lsh,@Adjust_No,@Adjust_Flag, @Stock_Locale,@GoodsCode, @Adjust_Date, @Batch_NO, @Batch_ID,@Quantity, @Amount, @Adjust_Cause,@DataDate)";

                    try
                    {
                        num = sqlDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());
                        if (num == 0)
                        {
                            resp.MSGCODE = "-1";
                            resp.MSGTXT = "[执行失败]：" + xmlStr;
                        }
                        else
                        {
                            resp.MSGCODE = "1";
                            resp.MSGTXT = "[执行成功]：" + xmlStr;
                        }
                    }
                    catch (Exception ex)
                    {
                        resp.MSGCODE = "-1";
                        resp.MSGTXT = ex.Message;
                        loginfo.logerror("接口LMS_POS_JK_02错误信息：" + resp.MSGTXT);
                    }

                }
                loginfo.loginfor("接口LMS_POS_JK_02返回信息：" + resp.MSGTXT);
                ////
            }

            //
            return resp;
        }

        //报损数据下传
        public LMS_POS_JK_RESP LMS_POS_JK_03(LMS_POS_JK03_REQ rq)
        {

            LMS_POS_JK_RESP resp = new LMS_POS_JK_RESP();
            LMS_POS_JK03_HEAD[] heads = rq.HEAD;
            List<DbParameter> paramList = new List<DbParameter>();

            string xmlStr = "";

            foreach (LMS_POS_JK03_HEAD h in heads)
            {
                string sqlStr = "";
                int num = 0;
                paramList.Clear();
                paramList.Add(sqlDB.MakeInParam("Loss_Lsh", h.Loss_Lsh));
                paramList.Add(sqlDB.MakeInParam("Loss_No", h.Loss_No));
                paramList.Add(sqlDB.MakeInParam("Loss_Flag", h.Loss_Flag));
                paramList.Add(sqlDB.MakeInParam("Stock_Locale", h.Stock_Locale));

                paramList.Add(sqlDB.MakeInParam("GoodsCode", h.GoodsCode));
                paramList.Add(sqlDB.MakeInParam("Loss_Date", h.Loss_Date));
                paramList.Add(sqlDB.MakeInParam("Batch_NO", h.Batch_NO));
                paramList.Add(sqlDB.MakeInParam("Batch_ID", h.Batch_ID));

                paramList.Add(sqlDB.MakeInParam("Quantity", h.Quantity));
                paramList.Add(sqlDB.MakeInParam("Amount", h.Amount));
                paramList.Add(sqlDB.MakeInParam("Loss_Cause", h.Loss_Cause));
                paramList.Add(sqlDB.MakeInParam("DataDate", DateTime.Now.ToString()));

                if (num == 0)
                {
                    xmlStr += "执行插入操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<LMS_POS_JK03_HEAD>(h) + "\r\n";
                    sqlStr = "INSERT INTO so_LossBatch(Loss_Lsh,Loss_No,Loss_Flag,Stock_Locale,GoodsCode,Loss_Date,Batch_NO,Batch_ID,Quantity,Amount,Loss_Cause,DataDate) VALUES(@Loss_Lsh,@Loss_No,@Loss_Flag, @Stock_Locale,@GoodsCode, @Loss_Date, @Batch_NO, @Batch_ID,@Quantity, @Amount, @Loss_Cause,@DataDate)";

                    try
                    {
                        num = sqlDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());
                        if (num == 0)
                        {
                            resp.MSGCODE = "-1";
                            resp.MSGTXT = "[执行失败]：" + xmlStr;
                        }
                        else
                        {
                            resp.MSGCODE = "1";
                            resp.MSGTXT = "[执行成功]：" + xmlStr;
                        }
                    }
                    catch (Exception ex)
                    {
                        resp.MSGCODE = "-1";
                        resp.MSGTXT = ex.Message;
                        loginfo.logerror("接口LMS_POS_JK_03错误信息：" + resp.MSGTXT);
                    }

                }
                loginfo.loginfor("接口LMS_POS_JK_03返回信息：" + resp.MSGTXT);
                ////
            }

            return resp;
        }

        //定时同步数据，直接调用过程处理
        public int LMS_POS_JK_04()
        {
            LMS_POS_JK_RESP resp = new LMS_POS_JK_RESP();
            List<DbParameter> paramList = new List<DbParameter>();
            paramList.Clear();
            paramList.Add(sqlDB.MakeInParam("sType", "LMS"));
            try
            {
                ///执行存储过程
                string tsql = "exec proc_jobexecone  @sType";
                DataSet pResult = sqlDB.ExecuteDataset(CommandType.Text, tsql, paramList.ToArray());
                if (pResult != null && pResult.Tables.Count > 0)
                {
                    DataRowCollection rows = pResult.Tables[0].Rows;
                    if (rows.Count > 0)
                    {
                        DataRow row = rows[0];
                        resp.MSGTXT = row[1].ToString() + "\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                resp.MSGCODE = "-1";
                resp.MSGTXT = ex.Message;
                loginfo.logerror("接口LMS_POS_JK_04错误信息：" + resp.MSGTXT);
            }
            loginfo.loginfor("接口LMS_POS_JK_04返回信息：" + resp.MSGTXT);


            return 0;

        }

        //LMS自采收货入库下传pos
        public LMS_POS_JK_RESP LMS_POS_JK_05(LMS_POS_JK05_REQ rq)
        {
            LMS_POS_JK_RESP resp = new LMS_POS_JK_RESP();
            LMS_POS_JK05_HEAD[] heads = rq.HEAD;

            List<DbParameter> paramList = new List<DbParameter>();

            string xmlStr = "";

            foreach (LMS_POS_JK05_HEAD h in heads)
            {
                string sqlStr = "";
                int num = 0;
                paramList.Clear();
                paramList.Add(sqlDB.MakeInParam("Rk_lsh", h.Rk_lsh));
                paramList.Add(sqlDB.MakeInParam("Rk_No", h.Rk_no));
                paramList.Add(sqlDB.MakeInParam("gys_No", h.gys_no));
                paramList.Add(sqlDB.MakeInParam("rkrq", h.rkrq));
                paramList.Add(sqlDB.MakeInParam("remark", h.remark));
                paramList.Add(sqlDB.MakeInParam("StoreCode", h.StoreCode));
                paramList.Add(sqlDB.MakeInParam("GoodsCode", h.GoodsCode));
                paramList.Add(sqlDB.MakeInParam("Batch_NO", h.BatchNo));
                paramList.Add(sqlDB.MakeInParam("Batch_ID", h.BatchID));
                paramList.Add(sqlDB.MakeInParam("Quantity", h.Quantity));
                paramList.Add(sqlDB.MakeInParam("RkPrice", h.RkPrice));
                paramList.Add(sqlDB.MakeInParam("RkAmount", h.RkAmount));
                paramList.Add(sqlDB.MakeInParam("scrq", h.scrq));
                paramList.Add(sqlDB.MakeInParam("rknote", h.rknote));
                paramList.Add(sqlDB.MakeInParam("DataDate", DateTime.Now.ToString()));
                paramList.Add(sqlDB.MakeInParam("order_no", h.order_no));
                paramList.Add(sqlDB.MakeInParam("order_num", h.order_num));
                paramList.Add(sqlDB.MakeInParam("rkflag", h.rkflag));
                paramList.Add(sqlDB.MakeInParam("validDate", h.validDate));

                if (num == 0)
                {
                    xmlStr += "执行插入操作：" + "\r\n" + XmlSerializeHelper.XmlSerialize<LMS_POS_JK05_HEAD>(h) + "\r\n";
                    sqlStr = "INSERT INTO so_InStorage(Rk_Lsh,Rk_No,gys_no,rkrq,remark,StoreCode,GoodsCode,Batch_NO,Batch_ID,Quantity,RkPrice,RkAmount,scrq,rknote,DataDate,order_no,order_num,rkflag,validDate) VALUES(@Rk_lsh,@Rk_No,@gys_no, @rkrq,@remark,@StoreCode,@GoodsCode, @Batch_NO, @Batch_ID,@Quantity,@RkPrice,@RkAmount,@scrq,@rknote,@DataDate,@order_no,@order_num,@rkflag,@validDate)";
                    try
                    {
                        num = sqlDB.ExecuteNonQuery(CommandType.Text, sqlStr, paramList.ToArray());
                        if (num == 0)
                        {
                            resp.MSGCODE = "-1";
                            resp.MSGTXT = "[执行失败]：" + xmlStr;
                        }
                        else
                        {
                            resp.MSGCODE = "1";
                            resp.MSGTXT = "[执行成功]：" + xmlStr;
                        }
                    }
                    catch (Exception ex)
                    {
                        resp.MSGCODE = "-1";
                        resp.MSGTXT = ex.Message;
                        loginfo.logerror("接口LMS_POS_JK_05错误信息：" + resp.MSGTXT);
                    }

                }
                loginfo.loginfor("接口LMS_POS_JK_05返回信息：" + resp.MSGTXT);
                ////
            }
            return resp;
        }
        //LMS自采收货入库上传sap
        public LMS_SAP_JK_RESP LMS_SAP_JK_01()
        {
            LMS_SAP_JK_RESP resp = new LMS_SAP_JK_RESP();
            List<DbParameter> paramList = new List<DbParameter>();

            resp.MSGCODE = "1";
            resp.MSGTXT = "成功";

            string order_Sql = "SELECT top 1 Order_no FROM so_InStorage WHERE isnull(syncFlag,0) =0 and order_no >''";
            string order_no = "";
            paramList.Clear();
            DataSet pSet = sqlDB.ExecuteDataset(CommandType.Text, order_Sql, paramList.ToArray());
            if (pSet != null && pSet.Tables.Count > 0)
            {
                DataRowCollection rows = pSet.Tables[0].Rows;
                if (rows.Count > 0)
                {
                    DataRow row = rows[0];
                    order_no = row[0].ToString();
                }
                //获取本订单入库明细
                string rk_sql = "SELECT Rk_No,gys_no,CONVERT(varchar(10),rkrq,112) as rkrq,remark,StoreCode,GoodsCode,Batch_NO,Batch_ID,convert(varchar(13),Quantity) as Quantity,RkPrice,RkAmount,scrq,rknote,validDate,order_no,order_num, Order_no FROM so_InStorage WHERE isnull(syncFlag,0) =0 and order_no =@Order_no";
                paramList.Clear();
                paramList.Add(sqlDB.MakeInParam("Order_no", order_no));
                DataSet pSetItem = sqlDB.ExecuteDataset(CommandType.Text, rk_sql, paramList.ToArray());
                if (pSetItem != null && pSetItem.Tables.Count > 0)
                {
                    DataRowCollection rkrows = pSetItem.Tables[0].Rows;
                    int size = rkrows.Count;
                    if (size > 0)
                    {
                        ///调用sap服务
                        SapLms07WebService.SI_LMS_LMS07_OUTService sapSrv = new SapLms07WebService.SI_LMS_LMS07_OUTService();
                        SapLms07WebService.ZMBGMCR01 arg = new SapLms07WebService.ZMBGMCR01();

                        SapLms07WebService.ZMBGMCR01HEAD head = new SapLms07WebService.ZMBGMCR01HEAD();
                        SapLms07WebService.ZMBGMCR01HEADITEM[] items = new SapLms07WebService.ZMBGMCR01HEADITEM[size];

                        head.BKTXT = "凭证抬头文本1";
                        head.BLDAT = rkrows[0]["rkrq"].ToString();
                        head.BUDAT = rkrows[0]["rkrq"].ToString();
                        head.FRBNR = rkrows[0]["Rk_No"].ToString();
                        head.ZYWLX = "01";

                        for (int i = 0; i < size; i++)
                        {
                            DataRow row = rkrows[i];
                            SapLms07WebService.ZMBGMCR01HEADITEM item = new SapLms07WebService.ZMBGMCR01HEADITEM();
                            item.EBELN = row["order_no"].ToString();
                            item.EBELP = row["order_num"].ToString();
                            item.MATNR = row["GoodsCode"].ToString();
                            item.LGORT = "1000"; //门店合格品库
                            item.WERKS = "2700"; //网店
                            item.CHARG = row["Batch_ID"].ToString();
                            item.LICHA = row["Batch_NO"].ToString();
                            item.ERFMG = row["Quantity"].ToString();

                            items[i] = item;

                        }
                        head.ITEM = items;
                        sapSrv.Credentials = new NetworkCredential(sapUser, sapPsw);
                        try
                        {
                            arg.HEAD = head;
                            sapSrv.SI_LMS_LMS07_OUT(arg);

                        }
                        catch (Exception ex)
                        {
                            resp.MSGCODE = "-100";
                            resp.MSGTXT = "调用sap服务失败";
                            ///
                            loginfo.loginfor("接口LMS_SAP_JK_01返回信息：" + resp.MSGTXT);
                            return resp;
                        }

                    }
                    ///更新上传标志
                    ///
                    if (resp.MSGCODE.Equals("1"))
                    {
                        string sDate = DateTime.Now.ToString();
                        string update_Sql = "update so_InStorage set syncFlag =1,SyncTime =@syntime WHERE isnull(syncFlag,0) =0 and order_no =@Order_no";
                        paramList.Clear();
                        paramList.Add(sqlDB.MakeInParam("syntime", sDate));
                        paramList.Add(sqlDB.MakeInParam("Order_no", order_no));
                        int num = sqlDB.ExecuteNonQuery(CommandType.Text, update_Sql, paramList.ToArray());
                    }

                }

            }
            loginfo.loginfor("接口LMS_SAP_JK_01返回信息：上传入库数据成功");
            return resp;
        }

        //销售数据
        #region 老方法备份
        //public LMS_SAP_JK_RESP LMS_SAP_JK_02()
        //{
        //    LMS_SAP_JK_RESP resp = new LMS_SAP_JK_RESP();
        //    List<DbParameter> paramList = new List<DbParameter>();
        //    int maxLsh = 0;

        //    resp.MSGCODE = "1";
        //    resp.MSGTXT = "成功";

        //    string tSql = "exec proc_getDayOrders ";
        //    DataSet pSet = sqlDB.ExecuteDataset(CommandType.Text, tSql);
        //    if (pSet != null && pSet.Tables.Count > 0)
        //    {
        //        DataRowCollection rows = pSet.Tables[0].Rows;
        //        SapLms08Webservice.SI_LMS_LMS08_OUTService server = new SapLms08Webservice.SI_LMS_LMS08_OUTService();
        //        SapLms08Webservice.DT_LMS_LMS08 arg = new SapLms08Webservice.DT_LMS_LMS08();
        //        SapLms08Webservice.DT_LMS_LMS08HEAD head = new SapLms08Webservice.DT_LMS_LMS08HEAD();
        //        SapLms08Webservice.DT_LMS_LMS08HEADITEM[] items = new SapLms08Webservice.DT_LMS_LMS08HEADITEM[rows.Count];
        //        if (rows.Count > 0)
        //        {
        //            head.KUNNR = rows[0]["store_no"].ToString();
        //            head.BELEGDATUM = rows[0]["xsrq"].ToString().Replace("-", "");
        //            head.BELEGWAERS = "CNY";
        //            for (int i = 0; i < rows.Count; i++)
        //            {
        //                DataRow row = rows[i];
        //                if (maxLsh < int.Parse(row["lsh"].ToString()))
        //                {
        //                    maxLsh = int.Parse(row["lsh"].ToString());
        //                }
        //                ///调用sap服务接口传输
        //                SapLms08Webservice.DT_LMS_LMS08HEADITEM item = new SapLms08Webservice.DT_LMS_LMS08HEADITEM();
        //                item.PRCTR = row["plattype"].ToString();
        //                item.ARTNR_LONG = row["GoodsCode"].ToString();
        //                item.UMSMENGE = row["quantity"].ToString();
        //                if (row["Order_Flag"].ToString() == "1")
        //                    item.VORZMENGE = "-";
        //                else
        //                    item.VORZMENGE = "+";
        //                item.UMSWERT = row["amount"].ToString();
        //                item.FLDVAL = row["batch_id"] != DBNull.Value ? row["batch_id"].ToString() : "";
        //                items[i] = item;

        //            }
        //            head.ITEM = items;
        //            server.Credentials = new NetworkCredential(sapUser, sapPsw);
        //            try
        //            {
        //                arg.HEAD = head;
        //                server.SI_LMS_LMS08_OUT(arg);
        //            }
        //            catch (Exception ex)
        //            {
        //                resp.MSGCODE = "-100";
        //                resp.MSGTXT = "调用sap服务失败";
        //                loginfo.loginfor("接口LMS_SAP_JK_02返回信息：" + resp.MSGTXT);
        //                return resp;
        //            }

        //        }
        //        ///更新上传标志
        //        if (resp.MSGCODE.Equals("1"))
        //        {
        //            string sDate = DateTime.Now.ToString();
        //            string sql = "select top 1 lastDate from syncDataLog where sType =8";
        //            DataSet ds = sqlDB.ExecuteDataset(sql);
        //            DateTime lastDate;
        //            string update_Sql = "";
        //            if (ds.Tables.Count > 0)
        //            {
        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    DataTable dt = ds.Tables[0];
        //                    lastDate = DateTime.Parse(dt.Rows[0]["lastDate"].ToString());
        //                    update_Sql = string.Format(" update so_OrderList set SyncFlag=1 where SyncFlag =0 and order_date >'{0}' and order_date <'{1}' \r\n", lastDate, lastDate.AddDays(1).ToString("yyyy-MM-dd"));
        //                }
        //            }
        //            update_Sql += " update syncDataLog set lastDate =transDate WHERE sType =8";
        //            int num = sqlDB.ExecuteNonQuery(CommandType.Text, update_Sql, paramList.ToArray());
        //        }
        //    }
        //    loginfo.loginfor("接口LMS_SAP_JK_02返回信息：上传销售数据成功");
        //    return resp;
        //}
        #endregion

        //销售数据
        public LMS_SAP_JK_RESP LMS_SAP_JK_02()
        {
            LMS_SAP_JK_RESP resp = new LMS_SAP_JK_RESP();
            List<DbParameter> paramList = new List<DbParameter>();
            //int maxLsh = 0;


            resp.MSGCODE = "1";
            resp.MSGTXT = "成功";

            string tSql = "exec proc_getDayOrders ";
            DataSet pSet = sqlDB.ExecuteDataset(CommandType.Text, tSql);
            if (pSet != null && pSet.Tables.Count > 0)
            {
                DataRowCollection rows = pSet.Tables[0].Rows;
                SapLms08Webservice.SI_LMS_LMS08_OUTService server = new SapLms08Webservice.SI_LMS_LMS08_OUTService();
                SapLms08Webservice.DT_LMS_LMS08 arg = new SapLms08Webservice.DT_LMS_LMS08();
                SapLms08Webservice.DT_LMS_LMS08HEAD head = new SapLms08Webservice.DT_LMS_LMS08HEAD();
                SapLms08Webservice.DT_LMS_LMS08HEADITEM[] items = new SapLms08Webservice.DT_LMS_LMS08HEADITEM[rows.Count];
                if (rows.Count > 0)
                {
                    head.KUNNR = rows[0]["store_no"].ToString();
                    head.BELEGDATUM = rows[0]["xsrq"].ToString().Replace("-", "");
                    head.BELEGWAERS = "CNY";
                    int beishu = rows.Count / 400;//取总行数除以400的整数
                    int yushu = rows.Count % 400;//取总行数除以400的整数
                    if (beishu == 0)
                    {
                        head = new SapLms08Webservice.DT_LMS_LMS08HEAD();
                        items = new SapLms08Webservice.DT_LMS_LMS08HEADITEM[rows.Count];
                        for (int i = 0; i < rows.Count; i++)
                        {
                            DataRow row = rows[i];
                            //if (maxLsh < int.Parse(row["lsh"].ToString()))
                            //{
                            //    maxLsh = int.Parse(row["lsh"].ToString());
                            //}
                            ///调用sap服务接口传输
                            SapLms08Webservice.DT_LMS_LMS08HEADITEM item = new SapLms08Webservice.DT_LMS_LMS08HEADITEM();
                            item.PRCTR = row["plattype"].ToString();
                            item.ARTNR_LONG = row["GoodsCode"].ToString();
                            item.UMSMENGE = row["quantity"].ToString();
                            if (row["Order_Flag"].ToString() == "1")
                                item.VORZMENGE = "-";
                            else
                                item.VORZMENGE = "+";
                            item.UMSWERT = row["amount"].ToString();
                            item.FLDVAL = row["batch_id"] != DBNull.Value ? row["batch_id"].ToString() : "";
                            items[i] = item;
                        }
                        head.ITEM = items;
                        server.Credentials = new NetworkCredential(sapUser, sapPsw);
                        try
                        {
                            arg.HEAD = head;
                            server.SI_LMS_LMS08_OUT(arg);
                        }
                        catch (Exception ex)
                        {
                            resp.MSGCODE = "-100";
                            resp.MSGTXT = "调用sap服务失败";
                            loginfo.loginfor("接口LMS_SAP_JK_02返回信息：" + resp.MSGTXT);
                            return resp;
                        }
                    }
                    else
                    {
                        int index = 0;//明细索引
                        int beushuindex = 1;//倍数索引
                        for (int i = 0; i < rows.Count; i++)
                        {
                            //先将余数部分传输
                            if (i < yushu)
                            {
                                if (i == 0)
                                {
                                    head = new SapLms08Webservice.DT_LMS_LMS08HEAD();
                                    items = new SapLms08Webservice.DT_LMS_LMS08HEADITEM[yushu];
                                }
                                DataRow row = rows[i];
                                //if (maxLsh < int.Parse(row["lsh"].ToString()))
                                //{
                                //    maxLsh = int.Parse(row["lsh"].ToString());
                                //}
                                ///调用sap服务接口传输
                                SapLms08Webservice.DT_LMS_LMS08HEADITEM item = new SapLms08Webservice.DT_LMS_LMS08HEADITEM();
                                item.PRCTR = row["plattype"].ToString();
                                item.ARTNR_LONG = row["GoodsCode"].ToString();
                                item.UMSMENGE = row["quantity"].ToString();
                                if (row["Order_Flag"].ToString() == "1")
                                    item.VORZMENGE = "-";
                                else
                                    item.VORZMENGE = "+";
                                item.UMSWERT = row["amount"].ToString();
                                item.FLDVAL = row["batch_id"] != DBNull.Value ? row["batch_id"].ToString() : "";
                                items[i] = item;
                                index++;
                                //余数部分打包传输
                                if (i == (yushu - 1))
                                {
                                    index = 0;
                                    head.ITEM = items;
                                    server.Credentials = new NetworkCredential(sapUser, sapPsw);
                                    try
                                    {
                                        arg.HEAD = head;
                                        server.SI_LMS_LMS08_OUT(arg);
                                        Thread.Sleep(1000);
                                    }
                                    catch (Exception ex)
                                    {
                                        resp.MSGCODE = "-100";
                                        resp.MSGTXT = "调用sap服务失败";
                                        loginfo.loginfor("接口LMS_SAP_JK_02返回信息：" + resp.MSGTXT);
                                        return resp;
                                    }
                                }

                            }
                            //整400部分
                            else
                            {
                                //循环到400的整数倍项
                                if ((400 - yushu + i) / 400 == beushuindex)
                                {
                                    if ((400 - yushu + i) % 400 == 0)
                                    {
                                        head = new SapLms08Webservice.DT_LMS_LMS08HEAD();
                                        items = new SapLms08Webservice.DT_LMS_LMS08HEADITEM[400];
                                    }
                                    DataRow row = rows[i];
                                    //if (maxLsh < int.Parse(row["lsh"].ToString()))
                                    //{
                                    //    maxLsh = int.Parse(row["lsh"].ToString());
                                    //}
                                    ///调用sap服务接口传输
                                    SapLms08Webservice.DT_LMS_LMS08HEADITEM item = new SapLms08Webservice.DT_LMS_LMS08HEADITEM();
                                    item.PRCTR = row["plattype"].ToString();
                                    item.ARTNR_LONG = row["GoodsCode"].ToString();
                                    item.UMSMENGE = row["quantity"].ToString();
                                    if (row["Order_Flag"].ToString() == "1")
                                        item.VORZMENGE = "-";
                                    else
                                        item.VORZMENGE = "+";
                                    item.UMSWERT = row["amount"].ToString();
                                    item.FLDVAL = row["batch_id"] != DBNull.Value ? row["batch_id"].ToString() : "";
                                    items[index] = item;
                                    index++;
                                    if (index == 400)
                                    {
                                        index = 0;
                                        beushuindex++;
                                        head.ITEM = items;
                                        server.Credentials = new NetworkCredential(sapUser, sapPsw);
                                        try
                                        {
                                            arg.HEAD = head;
                                            server.SI_LMS_LMS08_OUT(arg);
                                            Thread.Sleep(1000);
                                        }
                                        catch (Exception ex)
                                        {
                                            resp.MSGCODE = "-100";
                                            resp.MSGTXT = "调用sap服务失败";
                                            loginfo.loginfor("接口LMS_SAP_JK_02返回信息：" + resp.MSGTXT);
                                            return resp;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ///更新上传标志
                if (resp.MSGCODE.Equals("1"))
                {
                    string sDate = DateTime.Now.ToString();
                    string sql = "select top 1 lastDate from syncDataLog where sType =8";
                    DataSet ds = sqlDB.ExecuteDataset(sql);
                    DateTime lastDate;
                    string update_Sql = "";
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataTable dt = ds.Tables[0];
                            lastDate = DateTime.Parse(dt.Rows[0]["lastDate"].ToString());
                            update_Sql = string.Format(" update so_OrderList set SyncFlag=1 where SyncFlag =0 and order_date >'{0}' and order_date <'{1}' \r\n", lastDate, lastDate.AddDays(1).ToString("yyyy-MM-dd"));
                        }
                    }
                    update_Sql += " update syncDataLog set lastDate =transDate WHERE sType =8";
                    int num = sqlDB.ExecuteNonQuery(CommandType.Text, update_Sql, paramList.ToArray());
                }
            }
            loginfo.loginfor("接口LMS_SAP_JK_02返回信息：上传销售数据成功");
            return resp;
        }
        //营业收款上传
        public LMS_SAP_JK_RESP LMS_SAP_JK_03()
        {
            LMS_SAP_JK_RESP resp = new LMS_SAP_JK_RESP();
            List<DbParameter> paramList = new List<DbParameter>();

            resp.MSGCODE = "1";
            resp.MSGTXT = "成功";

            string tSql = "exec proc_getDaySales ";
            DataSet pSet = sqlDB.ExecuteDataset(CommandType.Text, tSql);
            if (pSet != null && pSet.Tables.Count > 0)
            {
                DataRowCollection rows = pSet.Tables[0].Rows;
                SapLms09Webservice.SI_LMS_LMS09_OUTService server = new SapLms09Webservice.SI_LMS_LMS09_OUTService();
                SapLms09Webservice.DT_LMS_LMS09 arg = new SapLms09Webservice.DT_LMS_LMS09();
                SapLms09Webservice.DT_LMS_LMS09HEAD head = new SapLms09Webservice.DT_LMS_LMS09HEAD();
                SapLms09Webservice.DT_LMS_LMS09HEADITEM[] items = new SapLms09Webservice.DT_LMS_LMS09HEADITEM[rows.Count];

                if (rows.Count > 0)
                {
                    head.BUDAT = rows[0]["xsrq"].ToString().Replace("-", "");
                    head.LOCNR = rows[0]["store_no"].ToString();
                    for (int i = 0; i < rows.Count; i++)
                    {
                        DataRow row = rows[i];
                        ///调用sap服务接口传输
                        SapLms09Webservice.DT_LMS_LMS09HEADITEM item = new SapLms09Webservice.DT_LMS_LMS09HEADITEM();
                        item.ZAHLART = row["plattype"].ToString();
                        if (row["Order_Flag"].ToString() == "1")
                            item.VORZEICHEN = "-";
                        else
                            item.VORZEICHEN = "+";
                        item.SUMME = row["amount"].ToString();
                        item.WAEHRUNG = "CNY";
                        items[i] = item;

                    }
                    head.ITEM = items;
                    server.Credentials = new NetworkCredential(sapUser, sapPsw);
                    try
                    {
                        arg.HEAD = head;
                        server.SI_LMS_LMS09_OUT(arg);
                    }
                    catch (Exception ex)
                    {
                        resp.MSGCODE = "-100";
                        resp.MSGTXT = "调用sap服务失败";
                        loginfo.loginfor("接口LMS_SAP_JK_03返回信息：" + resp.MSGTXT);
                        return resp;
                    }
                }
                ///更新上传标志
                if (resp.MSGCODE.Equals("1"))
                {
                    string sDate = DateTime.Now.ToString();
                    string update_Sql = "update syncDataLog set lastDate =transDate WHERE sType =9";
                    int num = sqlDB.ExecuteNonQuery(CommandType.Text, update_Sql, paramList.ToArray());
                }
            }
            loginfo.loginfor("接口LMS_SAP_JK_03返回信息：上传营业收款数据成功");
            return resp;
        }

        //定时生成pos中网店的入库单数据
        public int LMS_POS_JK_06()
        {
            LMS_POS_JK_RESP resp = new LMS_POS_JK_RESP();
            List<DbParameter> paramList = new List<DbParameter>();
            paramList.Clear();
            paramList.Add(sqlDB.MakeInParam("sType", "LMS"));
            try
            {
                ///执行存储过程
                string tsql = "exec proc_jobexecone001  @sType";
                DataSet pResult = sqlDB.ExecuteDataset(CommandType.Text, tsql, paramList.ToArray());
                if (pResult != null && pResult.Tables.Count > 0)
                {
                    DataRowCollection rows = pResult.Tables[0].Rows;
                    if (rows.Count > 0)
                    {
                        DataRow row = rows[0];
                        resp.MSGTXT = row[1].ToString() + "\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                resp.MSGCODE = "-1";
                resp.MSGTXT = ex.Message;
                loginfo.logerror("接口LMS_POS_JK_06错误信息：" + resp.MSGTXT);
            }
            loginfo.loginfor("接口LMS_POS_JK_06返回信息：" + resp.MSGTXT);


            return 0;

        }

        //定时pos中网店的配送单自动复核
        public int LMS_POS_JK_07()
        {
            LMS_POS_JK_RESP resp = new LMS_POS_JK_RESP();
            List<DbParameter> paramList = new List<DbParameter>();
            paramList.Clear();
            paramList.Add(sqlDB.MakeInParam("sType", "LMS"));
            try
            {
                ///执行存储过程
                string tsql = "exec proc_jobexecone004  @sType";
                DataSet pResult = sqlDB.ExecuteDataset(CommandType.Text, tsql, paramList.ToArray());
                if (pResult != null && pResult.Tables.Count > 0)
                {
                    DataRowCollection rows = pResult.Tables[0].Rows;
                    if (rows.Count > 0)
                    {
                        DataRow row = rows[0];
                        resp.MSGTXT = row[1].ToString() + "\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                resp.MSGCODE = "-1";
                resp.MSGTXT = ex.Message;
                loginfo.logerror("接口LMS_POS_JK_07错误信息：" + resp.MSGTXT);
            }
            loginfo.loginfor("接口LMS_POS_JK_07返回信息：" + resp.MSGTXT);


            return 0;

        }


        //定时pos中生成网店销售数据
        public int LMS_POS_JK_08()
        {
            LMS_POS_JK_RESP resp = new LMS_POS_JK_RESP();
            List<DbParameter> paramList = new List<DbParameter>();
            paramList.Clear();
            paramList.Add(sqlDB.MakeInParam("sType", "LMS"));
            try
            {
                ///执行存储过程
                string tsql = "exec proc_jobexecone002  @sType";
                DataSet pResult = sqlDB.ExecuteDataset(CommandType.Text, tsql, paramList.ToArray());
                if (pResult != null && pResult.Tables.Count > 0)
                {
                    DataRowCollection rows = pResult.Tables[0].Rows;
                    if (rows.Count > 0)
                    {
                        DataRow row = rows[0];
                        resp.MSGTXT = row[1].ToString() + "\r\n";
                    }
                }
            }
            catch (Exception ex)
            {
                resp.MSGCODE = "-1";
                resp.MSGTXT = ex.Message;
                loginfo.logerror("接口LMS_POS_JK_08错误信息：" + resp.MSGTXT);
            }
            loginfo.loginfor("接口LMS_POS_JK_08返回信息：" + resp.MSGTXT);


            return 0;

        }

    }

}