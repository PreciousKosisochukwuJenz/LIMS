using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class RequestComputedResultVM
    {
        public int Id { get; set; }
        public int ServiceID { get; set; }
        public string BillInvoiceNumber { get; set; }
        public int KeyID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int RangeID { get; set; }
    }
}