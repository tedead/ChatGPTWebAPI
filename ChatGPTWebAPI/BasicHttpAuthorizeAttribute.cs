using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace ChatGPTWebAPI
{
    public class BasicHttpAuthorizeAttribute : AuthorizeAttribute
    {
        bool _requireSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["RequireSsl"]);

        public bool RequireSsl
        {
            get { return _requireSsl; }
            set { _requireSsl = value; }
        }

        bool _requireAuthentication = true;

        public bool RequireAuthentication
        {
            get { return _requireAuthentication; }
            set { _requireAuthentication = value; }
        }
    }
}