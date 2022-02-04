using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public int ServiceDepartmentID { get; set; }

        public int RevenueDepartmentID { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("ServiceDepartmentID")]
        public ServiceDepartment ServiceDepartment { get; set; }
        [ForeignKey("RevenueDepartmentID")]
        public RevenueDepartment RevenueDepartment { get; set; }
    }
}
