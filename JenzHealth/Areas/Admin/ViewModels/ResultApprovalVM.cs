using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class ResultApprovalVM
    {
        public int Id { get; set; }
        public int? ServiceParameterID { get; set; }
        public int? TemplateID { get; set; }
        public string Template { get; set; }
        public string Service { get; set; }
        public string BillNumber { get; set; }
        public bool HasApproved { get; set; }
        public int? ApprovedByID { get; set; }
        public DateTime? DateApproved { get; set; }
        public string ServiceParameter { get; set; }
        public string ApprovedBy { get; set; }
    }
}