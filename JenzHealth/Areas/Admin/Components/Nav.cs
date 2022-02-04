using JenzHealth.DAL.DataConnection;
using JenzHealth.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.Components
{
    public static class Nav
    {
        private static DatabaseEntities db = new DatabaseEntities();
        private static string defaultIcon = "";
        public static List<Menu> AllMenus = new List<Menu>();
        public static List<Menu> ApplicationMenu = new List<Menu>()
        {
              // Home
            new Menu(url: "/Admin/Home/Home",stringText:"Home", icon: "home", isMenu: false, claim: "home", childMenus: new List<Menu>(){ }),

              // Customer
            new Menu(url: "#",stringText:"Customer Management", icon: "accessible", isMenu: true,claim: "customer", childMenus: new List<Menu>(){
                new Menu(url: "/Admin/Customer/ManageCustomers",stringText:"Customers", icon: null ?? defaultIcon, isMenu: true,claim: "customer.managecustomers", childMenus: null),
            }),

             // Seed
              new Menu(url: "#",stringText:"Seed Management", icon: "apartment", isMenu: true, claim: "user", childMenus: new List<Menu>(){
                new Menu(url: "/Admin/Seed/ManageRevenueDepartments",stringText:"Revenue Departments", icon: null ?? defaultIcon, isMenu: true,claim: "seed.revenuedepartment", childMenus: null),
                new Menu(url: "/Admin/Seed/ManageServiceDepartments",stringText:"Service Departments", icon: null ?? defaultIcon, isMenu: true, claim: "seed.servicedepartment",childMenus: null),
                new Menu(url: "/Admin/Seed/ManageServices",stringText:"Services", icon: null ?? defaultIcon, isMenu: true, claim: "seed.Services",childMenus: null),
                new Menu(url: "/Admin/Seed/ManagePriviledges",stringText:"Priviledges", icon: null ?? defaultIcon, isMenu: true, claim: "seed.priviledges",childMenus: null),
                new Menu(url: "/Admin/Seed/ManageTemplates",stringText:"Templates", icon: null ?? defaultIcon, isMenu: true, claim: "seed.templates",childMenus: null),
                new Menu(url: "/Admin/Seed/ManageVendors",stringText:"Vendors", icon: null ?? defaultIcon, isMenu: true, claim: "seed.vendors",childMenus: null),
            }),

              // User
            new Menu(url: "#",stringText:"User Management", icon: "&#xE7FD;", isMenu: true, claim: "user", childMenus: new List<Menu>(){
                new Menu(url: "/Admin/User/Manage",stringText:"Users", icon: null ?? defaultIcon, isMenu: true,claim: "user.manageusers", childMenus: null),
                new Menu(url: "/Admin/User/ManageRoles",stringText:"Roles", icon: null ?? defaultIcon, isMenu: true, claim: "user.manageroles",childMenus: null),
            }),

            // Settings
            new Menu(url: "#",stringText:"Settings Management", icon: "settings", isMenu: true,claim: "settings", childMenus: new List<Menu>(){
                new Menu(url: "/Admin/ApplicationSettings/Manage",stringText:"Basic", icon: null ?? defaultIcon, isMenu: true,claim: "settings.manageapplicationsettings", childMenus: null),
            }),
        };
        public static List<Menu> AppMenu = new List<Menu>()
          {
              // Home
            new Menu(url: "/Admin/Home/Home",stringText:"Home", icon: "home", isMenu: false, claim: "home", childMenus: new List<Menu>(){ }),

              // Customer
            new Menu(url: "#",stringText:"Customer Management", icon: "accessible", isMenu: true,claim: "customer", childMenus: new List<Menu>(){
                new Menu(url: "/Admin/Customer/ManageCustomers",stringText:"Customers", icon: null ?? defaultIcon, isMenu: true,claim: "customer.managecustomers", childMenus: null),
            }),

             // Seed
            new Menu(url: "#",stringText:"Seed Management", icon: "apartment", isMenu: true, claim: "user", childMenus: new List<Menu>(){
                new Menu(url: "/Admin/Seed/ManageRevenueDepartments",stringText:"Revenue Departments", icon: null ?? defaultIcon, isMenu: true,claim: "seed.revenuedepartment", childMenus: null),
                new Menu(url: "/Admin/Seed/ManageServiceDepartments",stringText:"Service Departments", icon: null ?? defaultIcon, isMenu: true, claim: "seed.servicedepartment",childMenus: null),
                new Menu(url: "/Admin/Seed/ManageServices",stringText:"Services", icon: null ?? defaultIcon, isMenu: true, claim: "seed.Services",childMenus: null),
                new Menu(url: "/Admin/Seed/ManagePriviledges",stringText:"Priviledges", icon: null ?? defaultIcon, isMenu: true, claim: "seed.priviledges",childMenus: null),
                new Menu(url: "/Admin/Seed/ManageTemplates",stringText:"Templates", icon: null ?? defaultIcon, isMenu: true, claim: "seed.templates",childMenus: null),
                new Menu(url: "/Admin/Seed/ManageVendors",stringText:"Vendors", icon: null ?? defaultIcon, isMenu: true, claim: "seed.vendors",childMenus: null),
            }),

              // User
            new Menu(url: "#",stringText:"User Management", icon: "&#xE7FD;", isMenu: true, claim: "user", childMenus: new List<Menu>(){
                new Menu(url: "/Admin/User/Manage",stringText:"Users", icon: null ?? defaultIcon, isMenu: true,claim: "user.manageusers", childMenus: null),
                new Menu(url: "/Admin/User/ManageRoles",stringText:"Roles", icon: null ?? defaultIcon, isMenu: true, claim: "user.manageroles",childMenus: null),
            }),

            // Settings
            new Menu(url: "#",stringText:"Settings Management", icon: "settings", isMenu: true,claim: "settings", childMenus: new List<Menu>(){
                new Menu(url: "/Admin/ApplicationSettings/Manage",stringText:"Basic", icon: null ?? defaultIcon, isMenu: true,claim: "settings.manageapplicationsettings", childMenus: null),
            }),
        };

        private static void GetAllMenu(List<Menu> menus, List<Menu> applicationMenus)
        {
            foreach (var menu in applicationMenus)
            {
                menus.Add(menu);
                if (menu._childMenus != null)
                {
                    GetAllMenu(AllMenus, menu._childMenus);
                }
            }
        }
        public static void UpdateAllMenus()
        {
            GetAllMenu(AllMenus, AppMenu);
        }
        public static void StorePermissions(List<Menu> menuViewModels)
        {
            List<Permission> model = new List<Permission>();

            foreach (var menu in menuViewModels)
            {
                var permission = new Permission()
                {
                    Description = menu._stringText,
                    Claim = menu._claim,
                    Url = menu._url
                };
                model.Add(permission);
                if (menu._childMenus.Count > 0)
                {
                    foreach (var submenu in menu._childMenus)
                    {
                        var childpermission = new Permission()
                        {
                            Description = submenu._stringText,
                            Claim = menu._claim,
                            Url = submenu._url
                        };
                        model.Add(childpermission);
                    }
                }
            }
            db.Permissions.AddRange(model);
            db.SaveChanges();
        }

        public static void GetRolePermissionMenu(List<Menu> applicationMenus, List<RolePermission> permissionMenus)
        {
            foreach (var appmenu in applicationMenus)
            {
                var permissionRow = permissionMenus.FirstOrDefault(x => x.Permission == appmenu._stringText);

                appmenu.isAssigned = permissionRow == null ? false : permissionRow.IsAssigned;
                if (appmenu._childMenus.Count > 0)
                {
                    foreach (var subappmenu in appmenu._childMenus)
                    {
                        var permission = permissionMenus.FirstOrDefault(x => x.Permission == subappmenu._stringText);
                        if (permission != null)
                            subappmenu.isAssigned = permission.IsAssigned;
                    }
                    appmenu.isAssigned = appmenu._childMenus.Select(x => x.isAssigned).Contains(true);
                }
            }
        }
        public static List<Menu> GetAssignedPermission(int ID)
        {
            var CheckPermissionExists = db.RolePermissions.Where(x => x.RoleID == ID).Count();
            List<Menu> appmenus = new List<Menu>();
            appmenus = Nav.AppMenu;
            foreach (var menu in appmenus)
            {
                menu.isAssigned = db.RolePermissions.FirstOrDefault(x => x.Permission == menu._stringText && x.RoleID == ID).IsAssigned;
                if (menu._childMenus.Count > 0)
                {
                    foreach (var submenu in menu._childMenus)
                    {
                        submenu.isAssigned = db.RolePermissions.FirstOrDefault(x => x.Permission == submenu._stringText && x.RoleID == ID).IsAssigned;
                    }
                }
            }
            return appmenus;
        }

        public static void SavePermissionForRole(int roleID)
        {
            UpdateAllMenus();
            foreach (var menu in AllMenus)
            {
                if (CheckPermissionExist(roleID, menu._stringText) == 0)
                {
                    var rolePermission = new RolePermission();
                    rolePermission.PermissionID = db.Permissions.FirstOrDefault(x => x.Description == menu._stringText).Id;
                    rolePermission.Permission = menu._stringText;
                    rolePermission.RoleID = roleID;
                    rolePermission.IsAssigned = false;
                    rolePermission.IsDeleted = false;
                    db.RolePermissions.Add(rolePermission);
                }
                db.SaveChanges();
            }
        }
        public static int CheckPermissionExist(int RoleID, string Permission)
        {
            int result;
            result = db.RolePermissions.Where(x => x.RoleID == RoleID && x.Permission == Permission).Count();
            return result;
        }

        public static void AssignPermission(string permission, int roleID, bool isAssigned)
        {
            var RolePermission = db.RolePermissions.Where(p => p.RoleID == roleID && p.PermissionID == db.Permissions.FirstOrDefault(x => x.Description == permission).Id).FirstOrDefault();
            RolePermission.IsAssigned = isAssigned;
            db.Entry(RolePermission).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public static bool CheckAuthorization(string resourceUrl)
        {
            var permissionResourceURLs = db.RolePermissions.Where(x => x.IsDeleted == false && x.RoleID == Global.AuthenticatedUserRoleID && x.IsAssigned == true).Select(b=>b.Permissions.Url).ToList();
            if (permissionResourceURLs.Contains(resourceUrl))
                return true;
            else
                return false;
        }
    }
}