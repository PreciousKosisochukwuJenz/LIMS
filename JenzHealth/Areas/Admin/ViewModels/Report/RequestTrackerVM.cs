using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels.Report
{
    public class RequestTrackerVM
    {
        public int Id { get; set; }
        public string BillNumber { get; set; }
        public string PatientName { get; set; }
        public bool HasCompletedPayment { get; set; }
        public bool SampleCollected { get; set; }
        public DateTime? SampleCollectedOn { get; set; }
        public string SampleCollectedBy { get; set; }
        public bool TestComputed { get; set; }
        public bool TestRequiresApproval { get; set; }
        public bool TestApproved { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}