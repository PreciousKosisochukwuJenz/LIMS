using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class ServiceListVM
    {
        public int ServiceID { get; set; }
        public int Quantity { get; set; }
        public decimal GrossAmount { get; set; }
    }
}