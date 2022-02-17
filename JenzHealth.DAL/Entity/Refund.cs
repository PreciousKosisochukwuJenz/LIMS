using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class Refund
    {
        public int Id { get; set; }
        public string PaymentReciept { get; set; }
        public string DepositeReciept { get; set; }
        public decimal AmountToRefund { get; set; }
        public string Comment { get; set; }
        public bool IsDeletetd { get; set; }
        public DateTime DateCreated { get; set; }
        public int? RefundedByID { get; set; }
        [ForeignKey("RefundedByID")]
        public User RefundedBy { get; set; }
    }
}
