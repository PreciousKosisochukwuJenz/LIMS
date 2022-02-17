using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public  class PartPayment
    {
        public int Id { get; set; }
        public string BillInvoiceNumber { get; set; }
        public string InstallmentName { get; set; }
        public decimal PartPaymentAmount { get; set; }
        public bool IsPaidPartPayment { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public int? CreatedByID { get; set; }
        [ForeignKey("CreatedByID")]
        public User CreatedBy { get; set; }
    }
}
