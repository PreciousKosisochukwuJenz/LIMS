using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public  class ServiceParameterRangeSetup
    {
        public int Id { get; set; }
        public int? ServiceParameterSetupID { get; set; }
        public string Range { get; set; }
        public string Unit { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        [ForeignKey("ServiceParameterSetupID")]
        public ServiceParameterSetup ServiceParameterSetup { get; set; }
    }
}
