using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class ServiceParameterSetup
    {
        public int Id { get; set; }
        public int? ServiceParameterID { get; set; }
        public string Name { get; set; }
        public int Rank { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }

        [ForeignKey("ServiceParameterID")]
        public ServiceParameter ServiceParameter { get; set; }
    }
}
