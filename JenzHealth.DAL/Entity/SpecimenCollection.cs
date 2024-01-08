using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public  class SpecimenCollection
    {
        [Key]
        public int Id { get; set; }
        public string BillInvoiceNumber { get; set; }
        public DateTime? RequestingDate { get; set; }
        public string LabNumber { get; set; }
        public string ClinicalSummary { get; set; }
        public string ProvitionalDiagnosis { get; set; }
        public string OtherInformation { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public int? CollectedByID { get; set; }
        [ForeignKey("CollectedByID")]
        public User CollectedBy { get; set; }
        public string RequestingPhysician { get; set; }

    }
}
