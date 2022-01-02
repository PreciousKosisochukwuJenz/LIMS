using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class UserVM
    {
        public void Initilize()
        {
            MyProperty = new List<SelectListItem>() { new SelectListItem() { }, new SelectListItem() { } };
        }
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
        public DateTime DateCreated { get; set; }
        public int? RoleID { get; set; }
        public string Role { get; set; }
        public List<SelectListItem> MyProperty { get; set; }

    }
}