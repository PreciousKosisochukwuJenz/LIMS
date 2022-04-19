using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class TemplatedLabPreparation
    {
        public int Id { get; set; }
        public string BillInvoiceNumber { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public int? ServiceRangeID { get; set; }
        public int? ServiceParameterID { get; set; }
        public int? ServiceParameterSetupID { get; set; }
        public string Labnote { get; set; }
        public string ScienticComment { get; set; }
        public int? PreparedByID { get; set; }

        public DateTime DateCreated { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("ServiceParameterID")]
        public ServiceParameter ServiceParameter { get; set; }
        [ForeignKey("ServiceParameterSetupID")]
        public ServiceParameterSetup ServiceParameterSetup { get; set; }
        [ForeignKey("ServiceRangeID")]
        public ServiceParameterRangeSetup ServiceRange { get; set; }
        [ForeignKey("PreparedByID")]
        public User PreparedBy { get; set; }
    }
}
