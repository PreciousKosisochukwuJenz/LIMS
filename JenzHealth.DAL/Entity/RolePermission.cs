using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class RolePermission
    {
        [Key]
        public int Id { get; set; }
        public int RoleID { get; set; }
        public int PermissionID { get; set; }
        public string Permission { get; set; }
        public bool IsAssigned { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("PermissionID")]
        public Permission Permissions { get; set; }

    }
}
