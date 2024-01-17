using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class DocServiceComputationVM
    {
        public int Id { get; set; }
        public int ServiceID { get; set; }
        public string Service { get; set; }
        public string Parameter { get; set; }
        public string Labnote { get; set; }
        public string ScienticComment { get; set; }
        [AllowHtml]
        public string Value { get; set; }
        public int PreparedByID { get; set; }
        public string PreparedBy { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? DatePrepared { get; set; }
        public string Specimen { get; set; }
        public string SpecimenCollectedBy { get; set; }
        public DateTime? DateCollected { get; set; }
        public DateTime? DateApproved { get; set; }
        public string Labnumber { get; set; }
        public bool RequireApproval { get; set; }
        public int? ServiceParameterID { get; set; }
        public List<DocServiceParameterSetupVM> Parameters { get; set; }
    }
    public class DocServiceParameterSetupVM
    {
        public int Id { get; set; }
        public int? ServiceParameterID { get; set; }
        public string Name { get; set; }
        public string Labnote { get; set; }
        public string ScientificComment { get; set; }
        public int Rank { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public string ServiceParameter { get; set; }
        [AllowHtml]
        public string Value { get; internal set; }
    }
}