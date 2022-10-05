using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class ServiceParameter
    {
        public int Id { get; set; }
        public int? ServiceID { get; set; }
        public int? SpecimenID { get; set; }
        public int? TemplateID { get; set; }
        public bool RequireApproval { get; set; }
        public bool UseFilmReport { get; set; }
        [ForeignKey("ServiceID")]
        public Service Service { get; set; }
        [ForeignKey("SpecimenID")]
        public Specimen Specimen { get; set; }
        [ForeignKey("TemplateID")]
        public Template Template { get; set; }
    }
}
