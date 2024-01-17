using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class TemplateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ServiceDepartmentID { get; set; }
        public bool UseDefaultParameters { get; set; }
        public bool UseDocParameter { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ServiceDepartment { get; set; }
    }
}