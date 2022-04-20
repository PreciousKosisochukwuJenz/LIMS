using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class ResultApproval
    {
        public int Id { get; set; }
        public int? ServiceParameterID { get; set; }
        public string BillNumber { get; set; }
        public bool HasApproved { get; set; }
        public int? ApprovedByID { get; set; }
        public DateTime? DateApproved { get; set; }
        [ForeignKey("ServiceParameterID")]
        public ServiceParameter ServiceParameter { get; set; }
        [ForeignKey("ApprovedByID")]
        public User ApprovedBy { get; set; }
    }
}
