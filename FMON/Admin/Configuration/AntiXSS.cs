using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Security.Application;

namespace Admin.Configuration
{
    public static class AntiXSS
    {
        public static string GetSafeHtml(this string value)
        {
            return Sanitizer.GetSafeHtmlFragment(value);
        }
    }
}