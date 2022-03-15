using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class NonTemplatedLabPreparation
    {
        public int Id { get; set; }
        public string BillInvoiceNumber { get; set; }
        public string Appearance { get; set; }
        public string Color { get; set; }
        public string MacrosopyBlood { get; set; }
        public string AdultWarm { get; set; }
        public string Mucus { get; set; }
        public string SpecificGravity { get; set; }
        public string Acidity { get; set; }
        public string Glucose { get; set; }
        public string Protein { get; set; }
        public string Niterite { get; set; }
        public string Ketones { get; set; }
        public string Blirubin { get; set; }
        public string Urobilinogen { get; set; }
        public string AscorbicAcid { get; set; }
        public string DipstickBlood { get; set; }
        public string LeucocyteEsterase { get; set; }
        public string Temperature { get; set; }
        public string Duration { get; set; }
        public string Atomsphere { get; set; }
        public string Plate { get; set; }
        public string Incubatio { get; set; }
        public string Labnote { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
