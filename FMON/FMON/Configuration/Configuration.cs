using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FMON.Configuration
{
    public class Configuration
    {
        public static string AdminSite
        {
            get
            {
                return HttpContext.Current.Request.IsLocal ? "http://localhost:5498": "http://admin.fmon.app.fit.ba/";
            }
        }
        public string GetLink (string a)
        {

            return HttpContext.Current.Request.IsLocal ? "http://localhost:5498" + "" + a : "http://admin.fmon.app.fit.ba/" + "" + a;
            
        }

    }
}