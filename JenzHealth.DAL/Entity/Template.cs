using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class Template
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ServiceDepartmentID { get; set; }
        public bool UseDefaultParameters { get; set; }
        public bool UseDocParameter { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }

        [ForeignKey("ServiceDepartmentID")]
        public ServiceDepartment ServiceDepartment { get; set; }
    }
}
