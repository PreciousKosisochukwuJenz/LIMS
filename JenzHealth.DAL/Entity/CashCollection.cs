using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public  class CashCollection
    {
        [Key]
        public int Id { get; set; }
        public string BillInvoiceNumber { get; set; }
        public decimal AmountPaid { get; set; }
        public string InstallmentType { get; set; }
        public int? PartPaymentID { get; set; }
        public decimal NetAmount { get; set; }
        public decimal WaivedAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public string TransactionReferenceNumber { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DatePaid { get; set; }
        [ForeignKey("PartPaymentID")]
        public PartPayment PartPayment { get; set; }
    }
}
