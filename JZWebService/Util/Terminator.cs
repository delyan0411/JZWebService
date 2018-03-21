using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JZWebService.Util
{
    public class Terminator
    {
        public virtual string template
        {
            get
            {
                return AppExceptions.Terminator_ExceptionTemplate;
            }
        }

        public virtual void Alert(string s)
        {
            this.Echo("<script language='javascript'>alert('" + s.Replace("'", "\\'") + "');history.back();</script>");
            this.End();
        }

        public virtual void Alert(string s, string backurl)
        {
            this.Echo(string.Concat(new string[]
			{
				"<script language='javascript'>alert('",
				s.Replace("'", "\\'"),
				"');location.href='",
				backurl,
				"';</script>"
			}));
            this.End();
        }

        private void Echo(string s)
        {
            HttpContext.Current.Response.Write(s);
        }

        private void End()
        {
            HttpContext.Current.Response.End();
        }

        public virtual void Throw(string message)
        {
            HttpContext.Current.Response.ContentType = "text/html";
            HttpContext.Current.Response.AddHeader("Content-Type", "text/html");
            this.Throw(message, null, null, null, true);
        }

        public virtual void Throw(string message, string title, string links, string autojump, bool showback)
        {
            this.Echo("系统发生错误，错误信息：”" + message + "“");
            this.End();
        }
    }
}