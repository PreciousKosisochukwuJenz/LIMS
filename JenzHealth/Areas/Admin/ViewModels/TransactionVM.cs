using JenzHealth.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class TransactionVM
    {
        public int Id { get; set; }
        public int ShiftID { get; set; }
        public string ShiftUniqeID { get; set; }
        public DateTime ShiftStarts { get; set; }
        public DateTime? ShiftEnds { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public PaymentType PaymentType { get; set; }
        public string PaymentTypeString { get; set; }
        public int TransactionCount { get; set; }
        public string ShiftStatus { get; set; }
        public string ShiftClosedBy { get; set; }
        public decimal TotalAmount { get; set; }
        public string TotalAmountString { get; set; }
        public decimal CashAmount { get; set; }
        public string CashAmountString { get; set; }
        public decimal EFTAmount { get; set; }
        public string EFTAmountString { get; set; }
        public decimal POSAmount { get; set; }
        public string POSAmountString { get; set; }
        public IList<TransactionVM> TableData { get; set; }
    }
}