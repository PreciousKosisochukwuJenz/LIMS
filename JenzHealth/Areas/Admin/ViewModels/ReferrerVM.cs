using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class ReferrerVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Hospital { get; set; }
        public string Phonenumber { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
    }
}