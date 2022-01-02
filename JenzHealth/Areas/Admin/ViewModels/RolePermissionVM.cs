using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class RolePermissionVM
    {
        public int Id { get; set; }
        public bool IsAssigned { get; set; }
        public string RoleName { get; set; }
        public string PermissionName { get; set; }
        public string ModuleLinkName { get; set; }
        public IList<dynamic> TableData { get; set; }
        public IList<RolePermissionList> TableDataSource { get; set; }
        public RolePermissionVM()
        {

        }
        public RolePermissionVM(IEnumerable<dynamic> TData)
        {
            TableData = TData.ToArray();
        }
        public RolePermissionVM(IEnumerable<RolePermissionList> TData)
        {
            TableDataSource = TData.ToArray();
        }
    }
    public class RolePermissionList
    {
        public int Id { get; set; }
        public bool IsAssigned { get; set; }
        public int RoleID { get; set; }
        public int PermissionID { get; set; }
        public Nullable<int> PermissionParentID { get; set; }
        public string RoleName { get; set; }
        public string PermissionName { get; set; }
    }

}