using JenzHealth.DAL.DataConnection;
using JenzHealth.DAL.Helpers;
using JenzHealth.DAL.Entity;
using JenzHealth.Areas.Admin.Interfaces;
using JenzHealth.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace JenzHealth.Areas.Admin.Services
{
    public class UserService : IUserService
    {
        /* Instancation of the database context model
         * and injecting some buisness layer services
          */
        #region Instanciation
        readonly DatabaseEntities _db;
        public UserService()
        {
            _db = new DatabaseEntities();
        }
        public UserService(DatabaseEntities db)
        {
            _db = db;
        }
        #endregion

        /* **************************************************************** */
        //User

        // Fetching users
        public List<UserVM> GetUsers()
        {
            var model = _db.Users.Where(x => x.IsDeleted == false).Select(b => new UserVM()
            {
                Id = b.Id,
                Firstname = b.Firstname,
                Lastname = b.Lastname,
                Email = b.Email,
                Address = b.Address,
                PhoneNumber = b.PhoneNumber,
                Role = b.Role.Description,
                Username = b.Username,
                DateCreated = b.DateCreated,
                IsActive = b.IsActive
            }).ToList();
            return model;
        }

        // Creating users
        public bool CreateUser(UserVM vmodel)
        {
            bool HasSaved = false;
            User model = new User()
            {
                Username = vmodel.Username,
                Firstname = vmodel.Firstname,
                Lastname = vmodel.Lastname,
                Email = vmodel.Email,
                PhoneNumber = vmodel.PhoneNumber,
                RoleID = vmodel.RoleID,
                Address = vmodel.Address,
                IsActive = true,
                IsDeleted = false,
                Password = CustomEnrypt.Encrypt(vmodel.Password),
                PasswordSalt = CustomEnrypt.Encrypt(vmodel.PasswordSalt),
                DateCreated = DateTime.Now,
            };
            _db.Users.Add(model);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Get user
        public UserVM GetUser(int ID)
        {
            var model = _db.Users.Where(x => x.Id == ID).Select(b => new UserVM()
            {
                Id = b.Id,
                Firstname = b.Firstname,
                Lastname = b.Lastname,
                Email = b.Email,
                Username = b.Username,
                PhoneNumber = b.PhoneNumber,
                Address = b.Address,
                RoleID = b.RoleID,
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating users
        public bool EditUser(UserVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.Users.FirstOrDefault(x => x.Id == vmodel.Id);
            model.Firstname = vmodel.Firstname;
            model.Lastname = vmodel.Lastname;
            model.Email = vmodel.Email;
            model.PhoneNumber = vmodel.PhoneNumber;
            model.Address = vmodel.Address;
            model.RoleID = vmodel.RoleID;
            model.DateModified = DateTime.Now;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting users
        public bool DeleteUser(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Users.FirstOrDefault(x => x.Id == ID);
            model.IsDeleted = true;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }

        // Deactivating users
        public bool DeactivateUser(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Users.FirstOrDefault(x => x.Id == ID);
            model.IsActive = false;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }

        // Activating users
        public bool ActivateUser(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Users.FirstOrDefault(x => x.Id == ID);
            model.IsActive = true;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }

        public bool ChangePassword(UserVM vmodel)
        {
            var model = _db.Users.FirstOrDefault(x => x.Id == Global.AuthenticatedUserID);
            model.Password = CustomEnrypt.Encrypt(vmodel.Password);
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        /* *************************************************************************** */
        //Role

        // Fetching system roles
        public List<RoleVM> GetRoles()
        {
            var model = _db.Roles.Where(x => x.IsDeleted == false).Select(b => new RoleVM()
            {
                Id = b.Id,
                Description = b.Description
            }).ToList();
            return model;
        }

        // Creating system roles
        public bool CreateRole(RoleVM vmodel)
        {
            bool HasSaved = false;
            Role role = new Role()
            {
                Description = vmodel.Description,
                EnableShift = vmodel.EnableShift,
                ShiftExpiration = vmodel.ShiftExpiration,
                IsDeleted = false,
                DateCreated = DateTime.Now,
            };
            _db.Roles.Add(role);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Getting system roles
        public RoleVM GetRole(int ID)
        {
            var model = _db.Roles.Where(x => x.Id == ID).Select(b => new RoleVM()
            {
                Id = b.Id,
                Description = b.Description,
                ShiftExpiration = b.ShiftExpiration,
                EnableShift = b.EnableShift,
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating system roles
        public bool EditRole(RoleVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.Roles.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Description = vmodel.Description;
            model.EnableShift = vmodel.EnableShift;
            model.ShiftExpiration = vmodel.ShiftExpiration;
            model.DateModified = DateTime.Now;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting system roles
        public bool DeleteRole(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Roles.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }

        /* ************************************************************************ */
        //Account

        // Checking to verify login creditials for system users
        public List<User> CheckCreditials(UserVM userVM)
        {
            var ecryptedPassword = CustomEnrypt.Encrypt(userVM.Password);
            var user = _db.Users.Where(x => x.Username == userVM.Username && x.Password == ecryptedPassword && x.IsDeleted == false && x.IsActive == true).ToList();
            return user;
        }


        public Shift GetShift()
        {
            var userID = Global.AuthenticatedUserID;
            var userRoleID = Global.AuthenticatedUserRoleID;
            var existingShift = _db.Shifts.Where(x => x.UserID == userID && x.HasExpired == false).OrderByDescending(x=>x.Id).FirstOrDefault();
            var shiftCount = _db.ApplicationSettings.FirstOrDefault().ShiftCount;
            shiftCount++;
            if (existingShift != null)
            {
                if (existingShift.ExpiresDateTime.Value.TimeOfDay >= DateTime.Now.TimeOfDay && existingShift.ExpiresDateTime.Value.Date >= DateTime.Now.Date)
                {
                    return existingShift;
                }
                else
                {
                    existingShift.HasExpired = true;
                    existingShift.ClosedBy = "System";
                    _db.Entry(existingShift).State = System.Data.Entity.EntityState.Modified;
                }
            }
            var shift = new Shift()
            {
                UserID = userID,
                DateTimeCreated = DateTime.Now,
                ExpiresDateTime = DateTime.Now.Date.AddSeconds(86400 - 1),
                ShiftUniqueID = String.Format("SHT/{0}", shiftCount.ToString("D6")),
                HasExpired = false,
            };
            _db.Shifts.Add(shift);

            var updatesettings = _db.ApplicationSettings.FirstOrDefault();
            updatesettings.ShiftCount = shiftCount;

            _db.Entry(updatesettings).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();

            return shift;
        }
        public void CloseShift(int Id)
        {
            var model = _db.Shifts.FirstOrDefault(x => x.Id == Id);
            model.HasExpired = true;
            model.ClosedBy = _db.Users.FirstOrDefault(x => x.Id == Global.AuthenticatedUserID).Username;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
        }
    }
}