using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class Shift
    {
        [Key]
        public int Id { get; set; }
        public string ShiftUniqueID { get; set; }
        public int UserID { get; set; }
        public bool HasExpired { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime? ExpiresDateTime { get; set; }
        public string ClosedBy { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; }
    }
}
