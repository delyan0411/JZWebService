using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using System.Data.Common;

namespace JZWebService.Util
{
    public class AccessTokenManage
    {
        static System.Web.Caching.Cache objCache = HttpRuntime.Cache;


        /// <summary>  
        /// DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time"> DateTime时间格式</param>  
        /// <returns>Unix时间戳格式</returns>  
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void CreateUserApi(CreateUser user, ref string log)
        {
            string accessToken = string.Empty;
            if (objCache["AccessToken"] == null)
            {
                GetAccessToken();
            }
            accessToken = objCache["AccessToken"] as string;
            if (!string.IsNullOrEmpty(accessToken))
            {                
                accessToken = objCache["AccessToken"] as string;
                string url = "https://qyapi.weixin.qq.com/cgi-bin/user/create?access_token=" + accessToken + "";
                string jsonparam = JsonConvert.SerializeObject(user);
                log = log + jsonparam + "\r\n";
                string resStr = HttpPost(url, jsonparam);
                log = log + resStr + "\r\n";
                Ret result = JsonConvert.DeserializeObject<Ret>(resStr);
                if (result.errcode == 0)
                {
                    log = log + string.Format("CreateUserApi:sucuess");
                }
                else
                {
                    log = log + string.Format("CreateUserApi:fail_{0}_{1}", result.errcode, result.errmsg);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static void UpdateUserApi(CreateUser user, ref string log)
        {
            string accessToken = string.Empty;
            if (objCache["AccessToken"] == null)
            {
                GetAccessToken();
            }
            accessToken = objCache["AccessToken"] as string;
            if (!string.IsNullOrEmpty(accessToken))
            {
                accessToken = objCache["AccessToken"] as string;
                string url = "https://qyapi.weixin.qq.com/cgi-bin/user/update?access_token=" + accessToken + "";
                string jsonparam = JsonConvert.SerializeObject(user);
                log = log + jsonparam + "\r\n";
                string resStr = HttpPost(url, jsonparam);
                log = log + resStr + "\r\n";
                Ret result = JsonConvert.DeserializeObject<Ret>(resStr);
                if (result.errcode == 0)
                {
                    log = log + (string.Format("UpdateUserApi:sucuess"));
                }
                else
                {
                    log = log + (string.Format("UpdateUserApi:fail_{0}_{1}", result.errcode, result.errmsg));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static int CreateDeptApi(CreateDept dept,ref string log)
        {
            string accessToken = string.Empty;
            if (objCache["AccessToken"] == null)
            {
                GetAccessToken();
            }
            accessToken = objCache["AccessToken"] as string;
            if (!string.IsNullOrEmpty(accessToken))
            {
                accessToken = objCache["AccessToken"] as string;
                string url = "https://qyapi.weixin.qq.com/cgi-bin/department/create?access_token=" + accessToken + "";
                string jsonparam = JsonConvert.SerializeObject(dept);
                log = log + jsonparam + "\r\n";
                string resStr = HttpPost(url, jsonparam);
                log = log + resStr + "\r\n";
                UpdateDeptRet result = JsonConvert.DeserializeObject<UpdateDeptRet>(resStr);
                if (result.errcode == 0)
                {
                    log = log + string.Format("CreateDeptApi:sucuess");
                    return result.id;
                }
                else
                {
                    log = log + string.Format("CreateDeptApi:fail_{0}_{1}", result.errcode, result.errmsg);
                    return 0;
                }         
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void UpdateDeptApi(CreateDept dept,ref string log)
        {
            string accessToken = string.Empty;
            if (objCache["AccessToken"] == null)
            {
                GetAccessToken();
            }
            accessToken = objCache["AccessToken"] as string;
            if (!string.IsNullOrEmpty(accessToken))
            {
                accessToken = objCache["AccessToken"] as string;
                string url = "https://qyapi.weixin.qq.com/cgi-bin/department/update?access_token=" + accessToken + "";
                string jsonparam = JsonConvert.SerializeObject(dept);
                log = log + jsonparam + "\r\n";
                string resStr = HttpPost(url, jsonparam);
                log = log + resStr + "\r\n";
                Ret result = JsonConvert.DeserializeObject<Ret>(resStr);
                if (result.errcode == 0)
                {
                    log = log + string.Format("UpdateDeptApi:sucuess");
                }
                else
                {
                    log = log + string.Format("UpdateDeptApi:fail_{0}_{1}", result.errcode, result.errmsg);
                }
            }
        }

        /// <summary>
        /// 获取access_token
        /// 有效期7200秒，开发者必须在自己的服务全局缓存access_token
        /// </summary>
        /// <returns></returns>
        private static void GetAccessToken()
        {
            string corpid = "wx9aa71cfd6e9ee061";
            string corpsecret = "I7LpyLObm8gaPvZh2ANsf3A1tZBrs6vECkM27ASQXuec_g5D_AeyRB-MHQkxZQ_B";
            string url =
                "https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid=" + corpid + "&corpsecret=" + corpsecret;
            string resStr = HttpGet(url);
            Ret ret = JsonConvert.DeserializeObject<Ret>(resStr);
            if (ret.errcode == 0)//string.IsNullOrEmpty(model.errcode)
            {
                //请求成功了
                AccessTokenModel model = JsonConvert.DeserializeObject<AccessTokenModel>(resStr);
                DateTime dt = DateTime.Now.AddSeconds(Convert.ToInt32(model.expires_in));
                objCache.Insert("AccessToken", model.access_token, null, dt, System.Web.Caching.Cache.NoSlidingExpiration);
                Logger.Log(string.Format("GetAccessToken:{0}", model.access_token));
            }
            else
            {
                Logger.Log(string.Format("GetAccessToken:fail_{0}_{1}", ret.errcode, ret.errmsg));
            }
        }

        /// <summary>
        /// HttpGet请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            Logger.Log(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            Logger.Log(retString);
            return retString;
        }

        /// <summary>
        /// HttpPost请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="jsonParas"></param>
        /// <returns></returns>
        public static string HttpPost(string Url, string jsonParas)
        {
            //Logger.Log(jsonParas);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            //Post请求方式  
            request.Method = "POST";
            //内容类型
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] payload;
            //将Json字符串转化为字节  
            payload = System.Text.Encoding.UTF8.GetBytes(jsonParas);
            //设置请求的ContentLength   
            request.ContentLength = payload.Length;
            //发送请求，获得请求流 
            Stream writer;
            try
            {
                writer = request.GetRequestStream();//获取用于写入请求数据的Stream对象
            }
            catch (Exception)
            {
                writer = null;
                Console.Write("连接服务器失败!");
            }
            //将请求参数写入流
            writer.Write(payload, 0, payload.Length);
            writer.Close();//关闭请求流            
            HttpWebResponse response;
            try
            {
                //获得响应流
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                response = ex.Response as HttpWebResponse;
            }
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();
            //Logger.Log(retString);
            return retString;//返回Json数据
        }


    }

    public class AccessTokenModel
    {
        public string access_token;

        public string expires_in;
    }

    public class CreateUser
    {
        public string userid;

        public string name;

        public List<int> department;

        public string mobile;

        public string email;

        public string gender;

        public bool to_invite;
    }

    public class CreateDept
    {
        public int? id;

        public int parentid;

        public string name;
    }

    public class Ret
    {
        public int errcode;

        public string errmsg;
    }
    public class UpdateDeptRet:Ret
    {
        public int id;

    }
}