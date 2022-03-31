using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class NonTemplatedLabResultOrganismXAntibiotics
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
        [ForeignKey("NonTemplateLabResultID")]
        public NonTemplatedLabPreparation NonTemplatedLabPreparation { get; set; }
        [ForeignKey("AntiBioticID")]
        public AntiBiotic AntiBiotic { get; set; }
        [ForeignKey("OrganismID")]
        public Organism Organism { get; set; }
    }
}
