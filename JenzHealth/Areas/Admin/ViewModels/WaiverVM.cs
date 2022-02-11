using JenzHealth.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class WaiverVM
    {
        public int Id { get; set; }
        public string BillInvoiceNumber { get; set; }
        public WaiveBy WaiveBy { get; set; }
        public decimal WaiveAmount { get; set; }
        public decimal AvailableAmount { get; set; }
        public decimal NetAmount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public string CustomerName { get; set; }
        public string Gender { get; set; }
        public string CustomerAge { get; set; }
        public string CustomerPhoneNumber { get; set; }
    }
}