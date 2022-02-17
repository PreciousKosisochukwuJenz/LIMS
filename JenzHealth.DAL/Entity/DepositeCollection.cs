using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class DepositeCollection
    {
        public int Id { get; set; }
        public string CustomerUniqueID { get; set; }
        public string DepositeReciept { get; set; }
        public int CustomerID { get; set; }
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }
        public string ReferenceNumber { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }

        public int? DepositedByID { get; set; }
        [ForeignKey("DepositedByID")]
        public User DepositedBy { get; set; }
    }
}
