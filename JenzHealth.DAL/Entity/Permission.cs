using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Claim { get; set; }
        public string Url { get; set; }
    }
}
