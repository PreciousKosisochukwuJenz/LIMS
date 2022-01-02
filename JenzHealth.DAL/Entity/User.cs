using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int? RoleID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        [ForeignKey("RoleID")]
        public virtual Role Role { get; set; }
    }
}
