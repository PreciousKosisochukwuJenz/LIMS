using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class NonTemplatedLabPreparationOrganismXAntiBioticsVM
    {
        public int Id { get; set; }
        public int? NonTemplateLabResultID { get; set; }
        public int? AntiBioticID { get; set; }
        public int? OrganismID { get; set; }
        public bool IsSensitive { get; set; }
        public string SensitiveDegree { get; set; }
        public bool IsIntermediate { get; set; }
        public bool IsResistance { get; set; }
        public string ResistanceDegree { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public string NonTemplatedLabPreparation { get; set; }
        public string AntiBiotic { get; set; }
        public string Organism { get; set; }
    }
}