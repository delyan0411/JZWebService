using Core;
using Core.Common;
using Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace OAService
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //写成服务 定时调用 多线程调用 
            //读取json配置
            //RuleInfo rules = Settings.GetRuleList;
            //if (rules == null)
            //{
            //    Logger.Error("规则文件不存在或者为空");
            //}
            //else
            //{
            //Thread t1 = new Thread(new ThreadStart(this.Thread141));
            //t1.IsBackground = true;
            //t1.Start();
            //new Timer.Timer141();
            //}
            //Thread t1 = new Thread(new ThreadStart(this.Thread141));
            //t1.IsBackground = true;
            //t1.Start();
            //Thread t2 = new Thread(new ThreadStart(this.Thread142));
            //t2.IsBackground = true;
            //t2.Start();
            //Thread t3 = new Thread(new ThreadStart(this.Thread145));
            //t3.IsBackground = true;
            //t3.Start();
            //new Timer.Timer142();
            //new Timer.test();
            new Timer.Timer146();
            //new Timer.Timer154();
            //new Timer.Timer147();
            //new Timer.Timer173();
        }
        private void Thread141()
        {
            new Timer.Timer141();
        }
        private void Thread142()
        {
            new Timer.Timer142();
        }
        private void Thread145()
        {
            new Timer.Timer145();
        }
        private void Thread146()
        {
            new Timer.Timer146();
        }
        private void Thread147()
        {
            new Timer.Timer147();
        }
        private void Thread149()
        {
            new Timer.Timer149();
        }
        private void Thread154()
        {
            new Timer.Timer154();
        }
        private void Thread168()
        {
            new Timer.Timer168();
        }
        private void Thread169()
        {
            new Timer.Timer169();
        }
        private void Thread173()
        {
            new Timer.Timer173();
        }
        private void Thread180()
        {
            new Timer.Timer180();
        }
        private void Thread179()
        {
            new Timer.Timer179();
        }
        private void Thread175()
        {
            new Timer.Timer175();
        }
        private void Thread177()
        {
            new Timer.Timer177();
        }
        private void Thread181()
        {
            new Timer.Timer181();
        }
        private void Thread178()
        {
            new Timer.Timer178();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            // DateTime dt = DateTime.Now;
            // string b= string.Format("{0:yyyyMMddHHmmssffff}", dt);
            //int a = (5088 / 800) + 1;
            // int a = (5588 / 800) + 1;
            //double b = 5588 / 800;
            //MessageBox.Show("10000".IndexOf("1").ToString());
            string sMobile = textBox2.Text;
            string content = textBox1.Text;
            //SendBatchSMS(sMobile, content);
            SendMongateMULTIX(sMobile, content);
        }

        public long SendBatchSMS(string sMobile, string content)
        {
            //-1  参数为空。信息、电话号码等有空指针，登陆失败
            //- 12 有异常电话号码
            //- 14 实际号码个数超过100
            //- 999    web服务器内部错误
            //-2 短信内容大于350个字
            //-3 http发送失败
            //1 发送成功
            //string url = "http://120.204.199.44:8026/MWGate/wmgw.asmx/MongateSendSubmit";
            string url = "http://61.145.229.29:8892/MWGate/wmgw.asmx/MongateSendSubmit";
            string requestStr = "";
            string[] sMobileArray = sMobile.Split(',');
            if (sMobileArray.Length > 100)
            {
                return -14;
            }
            if (content.Length > 350)
            {
                return -2;
            }
            Random rad = new Random();//实例化随机数产生器rad；
            int value = rad.Next(1000000000, 1999999999);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //dic.Add("userId", "JE0036");
            //dic.Add("password", "368870");
            dic.Add("userId", "JH2885");
            dic.Add("password", "658452");
            dic.Add("pszMobis", sMobile);
            dic.Add("pszMsg", content);
            dic.Add("iMobiCount", sMobileArray.Length.ToString());
            dic.Add("pszSubPort", "*");
            dic.Add("MsgId", value.ToString());
            List<string> requestStrList = new List<string>();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                requestStrList.Add(kvp.Key + "=" + kvp.Value);
            }
            requestStr = string.Join<string>("&", requestStrList);
            try
            {
                string res = HttpUtils.HttpPost(url, requestStr);
                XmlDocument document = new XmlDocument();
                document.LoadXml(res);
                XmlNode bianhao = document.GetElementsByTagName("string")[0];
                long debugValue =Convert.ToInt64(bianhao.InnerText);
                return debugValue;
            }
            catch
            {
                return -3;
            }
        }
        //MongateMULTIXSend
        public long SendMongateMULTIX(string sMobile, string sContent)
        {
            //-1  参数为空。信息、电话号码等有空指针，登陆失败
            //- 12 有异常电话号码
            //- 14 实际号码个数超过100
            //- 999    web服务器内部错误
            //-2 短信内容大于350个字
            //-3 http发送失败
            //1 发送成功
            //string url = "http://120.204.199.44:8026/MWGate/wmgw.asmx/MongateSendSubmit";
            //string url = "http://61.145.229.29:8892/MWGate/wmgw.asmx/MongateSendSubmit";
            string url = "http://61.145.229.29:8892/MWGate/wmgw.asmx/MongateMULTIXSend";
            string requestStr = "";
            string[] sMobileArray = sMobile.Split(',');
            if (sMobileArray.Length > 100)
            {
                return -14;
            }
            //if (content.Length > 350)
            //{
            //    return -2;
            //}
            Random rad = new Random();//实例化随机数产生器rad；
            int value = rad.Next(1000000000, 1999999999);
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //dic.Add("userId", "JE0036");
            //dic.Add("password", "368870");
            string[] sContentArray = sContent.Split('|');
            int k = 0;
            List<string> smsls = new List<string>();
            foreach (var item in sContentArray)
            {
                //自定义消息编号 0
                string zzyxxbh = "0";
                //通道号 *
                string tth = "*";
                //手机号
                string sjh = sMobileArray[k];
                //短信内容 base64
                byte[] barray;
                barray = Encoding.Default.GetBytes(item);
                string ddnr = Convert.ToBase64String(barray);
                string signle = string.Format("{0}|{1}|{2}|{3}", zzyxxbh, tth, sjh, ddnr);
                smsls.Add(signle);
                k++;
            }
            string multixmt = string.Join(",", smsls.ToArray());
            dic.Add("userId", "JH2885");
            dic.Add("password", "658452");
            dic.Add("multixmt", multixmt);
            List<string> requestStrList = new List<string>();
            foreach (KeyValuePair<string, string> kvp in dic)
            {
                requestStrList.Add(kvp.Key + "=" + kvp.Value);
            }
            requestStr = string.Join<string>("&", requestStrList);
            try
            {
                string res = HttpUtils.HttpPost(url, requestStr);
                XmlDocument document = new XmlDocument();
                document.LoadXml(res);
                XmlNode bianhao = document.GetElementsByTagName("string")[0];
                long debugValue = Convert.ToInt64(bianhao.InnerText);
                return debugValue;
            }
            catch
            {
                return -3;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string itemproidstring = "50178,60935";
            string itemtypeidstring = "7,8,16,220,116,1238,18,845,848";
            //List<InsProItem> oldproitems = new List<InsProItem>();
            //List<InsTypeItem> oldtypeitems = new List<InsTypeItem>();
            //var res = QueryInsProduct.Do(id);
            //if (res != null && res.Body != null)
            //{
            //if (res.Body.insproduct_list != null)
            List<string> oldproitems = new List<string>();
            oldproitems.Add("113183");

            //if (res.Body.instype_list != null)
            List<string> oldtypeitems = new List<string>();
            oldtypeitems.Add("7");
            oldtypeitems.Add("8");
            // }
            List <int> oldproids = new List<int>();

            string[] itemproids = itemproidstring.Split(',');

            List<int> itemproids2 = new List<int>();

            string delids = "";
            string addids = "";
            foreach (var product_id in oldproitems)
            {
                oldproids.Add(Convert.ToInt32(product_id));
            }

            int _count = 0;
            //新增的proID
            if (!itemproidstring.Equals(""))
            {
                for (int i = 0; i < itemproids.Length; i++)
                {
                    itemproids2.Add(Utils.StrToInt(itemproids[i], 0));
                    if (!oldproids.Contains(Utils.StrToInt(itemproids[i], 0)))
                    {
                        if (_count > 0) addids += ",";
                        addids += Utils.StrToInt(itemproids[i], 0);
                        _count++;
                    }
                }
            }
            _count = 0;
            ////删除的proID "add_proids":"50178,60935,,,,,,,,"
            foreach (var product_id in oldproitems)
            {
                if (!itemproids2.Contains(Convert.ToInt32(product_id)))
                {
                    if (_count == 0)
                    {
                        delids = product_id.ToString();
                    }
                    else
                    {
                        delids = delids + "," + product_id;
                    }
                    _count++;
                }
            }
            List<int> oldtypeids = new List<int>();
            string[] itemtypeids = itemtypeidstring.Split(',');
            List<int> itemtypeids2 = new List<int>();
            string deltypeids = "";
            string addtypeids = "";
            foreach (var type_id in oldtypeitems)
            {
                oldtypeids.Add(Convert.ToInt32(type_id));
            }
            _count = 0;
            //新增的typeID            
            //"addtypeids":"7816220116123818845848"
            if (!itemtypeidstring.Equals(""))
            {
                for (int i = 0; i < itemtypeids.Length; i++)
                {
                    itemtypeids2.Add(Utils.StrToInt(itemtypeids[i], 0));
                    if (!oldtypeids.Contains(Utils.StrToInt(itemtypeids[i], 0)))
                    {
                        if (_count > 0) addtypeids += ",";
                        addtypeids += Utils.StrToInt(itemtypeids[i], 0);
                        _count++;
                    }
                }
            }
            _count = 0;
            //删除的typeID
            foreach (var type_id in oldtypeitems)
            {
                if (!itemtypeids2.Contains(Convert.ToInt32(type_id)))
                {
                    if (_count == 0)
                    {
                        deltypeids = type_id.ToString();
                    }
                    else
                    {
                        deltypeids = deltypeids + "," + type_id;
                    }
                    _count++;
                }
            }
            MessageBox.Show(addids).ToString();
            MessageBox.Show(delids).ToString();
            MessageBox.Show(addtypeids).ToString();
            MessageBox.Show(deltypeids).ToString();
        }
    }
}
