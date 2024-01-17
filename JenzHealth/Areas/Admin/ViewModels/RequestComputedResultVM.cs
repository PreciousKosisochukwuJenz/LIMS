using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class RequestComputedResultVM
    {
        public int Id { get; set; }
        public string Service { get; set; }
        public int? ServiceID { get; set; }
        public string BillInvoiceNumber { get; set; }
        public int? KeyID { get; set; }
        public string Labnote { get; set; }
        public string ScientificComment { get; set; }
        public string FilmingReport { get; set; }
        public string Key { get; set; }
        [AllowHtml]
        public string Value { get; set; }
        public int? RangeID { get; set; }
    }
}