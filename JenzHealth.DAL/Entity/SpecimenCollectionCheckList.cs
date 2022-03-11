using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class SpecimenCollectionCheckList
    {
        public int Id { get; set; }
        public int? SpecimenCollectionID { get; set; }
        public int? ServiceID { get; set; }
        public int? SpecimenID { get; set; }
        public bool IsCollected { get; set; }
        [ForeignKey("SpecimenID")]
        public Specimen Specimen { get; set; }
        [ForeignKey("SpecimenCollectionID")]
        public SpecimenCollection SpecimenCollection { get; set; }
        [ForeignKey("ServiceID")]

        public Service Service { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateTimeCreated { get; set; }
    }
}
