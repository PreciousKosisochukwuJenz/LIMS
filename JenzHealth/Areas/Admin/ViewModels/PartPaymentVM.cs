using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class PartPaymentVM
    {
        public int Id { get; set; }
        public string InstallmentName { get; set; }
        public string BillInvoiceNumber { get; set; }
        public decimal PartPaymentAmount { get; set; }
        public bool IsPaidPartPayment { get; set; }
        public bool IsDeleted { get; set; }
        public bool HasPaid { get; set; }
        public DateTime DateCreated { get; set; }
    }
}