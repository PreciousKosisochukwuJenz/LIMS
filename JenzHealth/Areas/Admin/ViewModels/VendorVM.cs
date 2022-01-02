using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class VendorVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CompanyRegistrationNumber { get; set; }
        public string OfficeAddress { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string PostalAddress { get; set; }
        public string Website { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
    }
}