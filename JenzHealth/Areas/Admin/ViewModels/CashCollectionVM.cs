using JenzHealth.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class CashCollectionVM
    {
        public int Id { get; set; }
        public string BillInvoiceNumber { get; set; }
        public string PaymentReciept { get; set; }
        public string CustomerName { get; set; }
        public string CustomerUniqueID { get; set; }
        public int CustomerAge { get; set; }
        public string CustomerGender { get; set; }
        public string CustomerType { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public decimal AmountPaid { get; set; }
        public string InstallmentType { get; set; }
        public int? PartPaymentID { get; set; }
        public string PartPayment { get; set; }
        public decimal NetAmount { get; set; }
        public decimal WaivedAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public string TransactionReferenceNumber { get; set; }
        public PaymentType PaymentType { get; set; }
        public CollectionType  CollectionType { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DatePaid { get; set; }
        public string ServiceName { get; set; }
        public string AmountPaidString { get; set; }
        public string Comment { get; set; }
        public string PaymentTypee { get; set; }
        public string CollectedBy { get; set; }
        public string ShiftNumber { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public WaiveBy WaiveBy { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}