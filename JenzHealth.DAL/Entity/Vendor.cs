using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class Vendor
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
