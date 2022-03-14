using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class ServiceParameterRangeSetupVM
    {
        public int Id { get; set; }
        public int ParameterID { get; set; }
        public int? ServiceParameterSetupID { get; set; }
        public string Range { get; set; }
        public string Unit { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public string ServiceParameterSetup { get; set; }
    }
}