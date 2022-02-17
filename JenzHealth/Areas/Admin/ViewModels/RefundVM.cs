using JenzHealth.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class RefundVM
    {
        public int Id { get; set; }
        public string PaymentReciept { get; set; }
        public string DepositeReciept { get; set; }
        public RecieptTypes RecieptType { get; set; }
        public decimal AmountToRefund { get; set; }
        public string AmountToRefundString { get; set; }
        public string Comment { get; set; }
        public bool IsDeletetd { get; set; }
        public DateTime DateCreated { get; set; }
    }
}