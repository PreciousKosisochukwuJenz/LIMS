using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.Helpers
{
    public static class HtmlConverter
    {
        public static string RemoveHTMLTags(string HTMLCode)
        {
            string str = System.Text.RegularExpressions.Regex.Replace(HTMLCode, "<[^>]*>", "");
            return str;
        }
    }
}