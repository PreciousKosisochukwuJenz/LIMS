using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class ServiceVM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int ServiceDepartmentID { get; set; }

        public int RevenueDepartmentID { get; set; }
        public decimal CostPrice { get; set; }
        public string CostPriceString { get; set; }
        public string SellingPriceString { get; set; }
        public decimal SellingPrice { get; set; }
        public string ServiceDepartment { get;set; }
        public string RevenueDepartment { get; set; }
    }
}