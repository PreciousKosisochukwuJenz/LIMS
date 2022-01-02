using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.Helpers
{
    public class CustomSerializer
    {
        public static byte[] Serialize(HttpPostedFileBase e)
        {
            byte[] data = { };
            MemoryStream target = new MemoryStream();
            if(e != null)
            {
                e.InputStream.CopyTo(target);
                data = target.ToArray();
            }
            return data;
        }
        public static decimal UnMaskString(string maskedStr)
        {
            string str = maskedStr.Remove(0, 1);
            str.Replace(",", "");
            return Convert.ToDecimal(str);
        }
    }
}