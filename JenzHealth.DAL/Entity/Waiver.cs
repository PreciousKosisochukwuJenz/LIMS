using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class Waiver
    {
        public int Id { get; set; }
        public string BillInvoiceNumber { get; set; }
        public WaiveBy WaiveBy { get; set; }
        public decimal WaiveAmount { get; set; }
        public decimal AvailableAmount { get; set; }
        public decimal NetAmount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
      
    }
}
