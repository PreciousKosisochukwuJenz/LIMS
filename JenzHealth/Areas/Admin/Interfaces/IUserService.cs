using JenzHealth.DAL.Entity;
using JenzHealth.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.Areas.Admin.Interfaces
{
    interface IUserService
    {
        // Users
        List<UserVM> GetUsers();
        bool CreateUser(UserVM vmodel);
        UserVM GetUser(int ID);
        bool EditUser(UserVM vmodel);
        bool DeleteUser(int ID);
        bool DeactivateUser(int ID);
        bool ActivateUser(int ID);
        bool ChangePassword(UserVM vmodel);

        //Roles
        List<RoleVM> GetRoles();
        bool CreateRole(RoleVM vmodel);
        RoleVM GetRole(int ID);
        bool EditRole(RoleVM vmodel);
        bool DeleteRole(int ID);

        List<User> CheckCreditials(UserVM userVM);
        Shift GetShift();
        void CloseShift(int Id);
        User GetCurrentUser();

    }
}
