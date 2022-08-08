using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class SpecimenCollectionVM
    {
        public int Id { get; set; }
        public string BillInvoiceNumber { get; set; }
        public DateTime RequestingDate { get; set; }
        public string Referrer { get; set; }
        public string LabNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerUniqueID { get; set; }
        public string ClinicalSummary { get; set; }
        public string ProvitionalDiagnosis { get; set; }
        public string OtherInformation { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public List<SpecimenCollectionCheckListVM> CheckList { get; set; }
        public string CollectedBy { get; set; }
        public int? CollectedByID { get; set; }
        public string Specimen { get; set; }
        public string ServiceDepartment { get; set; }
        public bool HasPaidAllBill { get; set; }
    }
}