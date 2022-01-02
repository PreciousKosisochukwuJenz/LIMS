using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JenzHealth.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthorizeAttribute());
            filters.Add(new SearchBotFilter());
            filters.Add(new CompressFilter());
            filters.Add(new ETagAttribute());
            filters.Add(new NoCacheConfig());
            //filters.Add(new WhitespaceFilterAttribute());
        }
    }
}