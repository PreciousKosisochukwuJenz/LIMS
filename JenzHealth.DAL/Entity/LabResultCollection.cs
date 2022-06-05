using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class LabResultCollection
    {
        public int Id { get; set; }
        public string Collector { get;set; }
        public string BillNumber { get; set; }
        public int? TemplateID { get; set; }
        public int? IssuerID { get; set; }
        [ForeignKey("IssuerID")]
        public User Issuer { get; set; }
        [ForeignKey("TemplateID")]
        public Template Template { get; set; }
        public DateTime DateCollected { get; set; }
    }
}
