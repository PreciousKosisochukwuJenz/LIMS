using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels.Report
{
    public class BillDetailsVM
    {
        public int Id { get; set; }
        public string BillInvoiceNumber { get; set; }
        public string BilledBy { get; set; }
        public decimal NetAmount { get; set; }
        public decimal WaivedAmount { get; set; }
        public decimal BalanceAmount { get; set; }
    }
}