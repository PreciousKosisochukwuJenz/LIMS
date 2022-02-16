using JenzHealth.DAL.DataConnection;
using JenzHealth.DAL.Entity;
using JenzHealth.Areas.Admin.Components;
using JenzHealth.Areas.Admin.Helpers;
using JenzHealth.Areas.Admin.Interfaces;
using JenzHealth.Areas.Admin.Services;
using JenzHealth.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace JenzHealth.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            filterContext.ExceptionHandled = true;

            //Log the error!!
            log.Error("Error trying to do something", filterContext.Exception);
            Global.ErrorMessage = filterContext.Exception.ToString();

            var errorType = filterContext.Exception.GetType().Name;

            switch (errorType)
            {
                case "UnauthorizedAccessException":
                    //Redirect or return a view, but not both.
                    filterContext.Result = RedirectToAction("Unauthorized", "Error", new { area = "Admin" });
                    break;
                default:
                    //Redirect or return a view, but not both.
                    filterContext.Result = RedirectToAction("Error", "Error", new { area = "Admin" });
                    break;
            }


        }


        // Initializing dependency services
        #region Instanciation
        DatabaseEntities db = new DatabaseEntities();
        IUserService _userService;
        public UserController()
        {
            _userService = new UserService(new DatabaseEntities());
        }
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        #endregion

        public ActionResult Manage(bool? Added,bool? Editted)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "User added successfully.";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "User updated successfully.";
            }
            ViewBag.Roles = new SelectList(db.Roles.Where(x => x.IsDeleted == false), "Id", "Description");
            ViewBag.Users = _userService.GetUsers();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateUser(UserVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _userService.CreateUser(vmodel);
            }
            return RedirectToAction("Manage", new { Added = hasSaved });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditUser(UserVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _userService.EditUser(vmodel);
            }
            return RedirectToAction("Manage", new { Editted = hasSaved });
        }
        public JsonResult GetUser(int id)
        {
            var model = _userService.GetUser(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteUser(int id)
        {
            var model = _userService.DeleteUser(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeactivateUser(int id)
        {
            var model = _userService.DeactivateUser(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ActivateUser(int id)
        {
            var model = _userService.ActivateUser(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangePassword(bool? Updated)
        {
            if (Updated == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Password changed successfully.";
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(UserVM vmodel)
        {
            bool hasUpdated = false;
            if (ModelState.IsValid)
            {
               hasUpdated = _userService.ChangePassword(vmodel);
            }
            return RedirectToAction("ChangePassword", new { Updated = hasUpdated });
        }

        // Role
        public ActionResult ManageRoles(bool? Added,bool? Editted)
        {
            var allowSetupRouteAccess = db.ApplicationSettings.FirstOrDefault().AllowSetupRouteAccess;
            if (!allowSetupRouteAccess)
            {
                if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
                {
                    throw new UnauthorizedAccessException();
                }
            }
            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Role added successfully.";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Role updated successfully.";
            }
            ViewBag.Roles = _userService.GetRoles();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateRole(RoleVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _userService.CreateRole(vmodel);
            }
            return RedirectToAction("ManageRoles", new { Added = hasSaved });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditRole(RoleVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _userService.EditRole(vmodel);
            }
            return RedirectToAction("ManageRoles", new { Editted = hasSaved });
        }
        public JsonResult GetRole(int id)
        {
            var model = _userService.GetRole(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteRole(int id)
        {
            var model = _userService.DeleteRole(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManagePermissions(int RoleID, bool? Saved)
        {
            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertMessage"] = "Permissions updated successfully";
                TempData["AlertType"] = "alert-success";
            }
            Nav.SavePermissionForRole(RoleID);
            ViewBag.RoleName = db.Roles.FirstOrDefault(x => x.Id == RoleID).Description;
            ViewBag.ApplicationMenus = Nav.GetAssignedPermission(RoleID);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ManagePermissions(string selectedItems, string role)
        {
            var roleID = db.Roles.FirstOrDefault(x => x.Description == role).Id;
            bool saved = false;
            if (ModelState.IsValid)
            {
                List<Menu> selectedPermissions = (new JavaScriptSerializer()).Deserialize<List<Menu>>(selectedItems);
                foreach (var each in selectedPermissions)
                {
                    each._stringText = HtmlConverter.RemoveHTMLTags(each._stringText);
                }
                var menus = Nav.AllMenus;
                foreach (var menu in menus)
                {
                    menu.isAssigned = selectedPermissions.Select(b => b._stringText).Contains(menu._stringText);
                    Nav.AssignPermission(menu._stringText, roleID, menu.isAssigned);
                }
            }
            saved = true;
            return ManagePermissions(roleID, saved);
        }

        public JsonResult CloseShift(int Id)
        {
            _userService.CloseShift(Id);
            return Json("Success",JsonRequestBehavior.AllowGet);
        }

    }
}