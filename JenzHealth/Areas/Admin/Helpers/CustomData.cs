using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JenzHealth.Areas.Admin.Helpers
{
    public class CustomData
    {
        public static List<SelectListItem> GenderList = new List<SelectListItem>()
        {
          new SelectListItem() { Text = "Male", Value = "Male" },
          new SelectListItem() { Text = "Female", Value = "Female" }
        };
        public static List<SelectListItem> SearchBy = new List<SelectListItem>()
        {
          new SelectListItem() { Text = "New", Value = "New" },
          new SelectListItem() { Text = "Existing", Value = "Existing" }
        };
    }
}