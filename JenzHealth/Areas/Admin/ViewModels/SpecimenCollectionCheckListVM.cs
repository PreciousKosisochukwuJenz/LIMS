using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class SpecimenCollectionCheckListVM
    {
        public int Id { get; set; }
        public int? SpecimenCollectionID { get; set; }
        public int? SpecimenID { get; set; }
        public int? ServiceID { get; set; }
        public bool IsCollected { get; set; }
        public string Specimen { get; set; }
        public string Service { get; set; }
        public string SpecimenCollection { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }
}