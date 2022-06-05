using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class LabResultCollectionVM
    {
        public int Id { get; set; }
        public string CollectorName { get; set; }
        public string BillNumber { get; set; }
        public string Template { get; set; }
        public int? TemplateID { get; set; }
        public DateTime? DateCollected { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PatientName { get; set; }
    }
}