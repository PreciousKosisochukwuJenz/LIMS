using JenzHealth.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class DepositeCollectionVM
    {
        public int Id { get; set; }
        public string CustomerUniqueID { get; set; }
        public decimal Amount { get; set; }
        public string AmountString { get; set; }
        public PaymentType PaymentType { get; set; }
        public string ReferenceNumber { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAge { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerGender { get; set; }
    }
}