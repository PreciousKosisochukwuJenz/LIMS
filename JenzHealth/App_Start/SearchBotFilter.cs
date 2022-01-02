using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JenzHealth.App_Start
{
    public class SearchBotFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Request.Browser.Crawler)
            {
                filterContext.Result = new ViewResult() { ViewName = "Error" };
            }
        }
    }
}