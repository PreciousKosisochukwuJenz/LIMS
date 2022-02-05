using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public  class Billing
    {
        [Key]
        public int Id { get; set; }
        public int CustomerID { get; set; }
        public string CustomerUniqueID { get; set; }
        public CustomerType CustomerType { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGender { get; set; }
        public uint CustomerAge { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public int ServiceID { get; set; }
        public string InvoiceNumber { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("CustomerID")]
        public Customer Customer { get; set; }

        [ForeignKey("ServiceID")]
        public Service Service { get; set; }
    }
}
