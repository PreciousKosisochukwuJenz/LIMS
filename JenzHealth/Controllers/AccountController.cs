using JenzHealth.Areas.Admin.Components;
using JenzHealth.Areas.Admin.Services;
using JenzHealth.Areas.Admin.ViewModels;
using JenzHealth.DAL.DataConnection;
using JenzHealth.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace JenzHealth.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    { // Instantiation
        #region Instantiation
        private DatabaseEntities db = new DatabaseEntities();
        public readonly UserService _userService;
        public AccountController()
        {
            _userService = new UserService();
        }
        public AccountController(UserService userService)
        {
            _userService = userService;
        }
        #endregion
        // Action Methods
        #region Action Methods
        public ActionResult Login(string returnUrl)
        {
            Global.ReturnUrl = returnUrl;
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserVM userVM)
        {
            Session.Clear();
            var user = _userService.CheckCreditials(userVM);
            if (user.Count > 0)
            {
                LoginSession(user);
                if (Session["UserId"] != null)
                {
                    Global.AuthenticatedUserID = Convert.ToInt32(Session["UserId"].ToString());
                    Global.AuthenticatedUserRoleID = Convert.ToInt32(Session["RoleID"].ToString());
                    var UserID = Convert.ToInt32(Session["UserId"]);
                    var RoleID = db.Users.Where(x => x.Id == UserID).FirstOrDefault().RoleID;
                    var PermissionList = db.RolePermissions.Where(x => x.RoleID == RoleID).ToList();
                    Nav.GetRolePermissionMenu(Nav.ApplicationMenu, PermissionList);
                }
                if (Global.ReturnUrl != null)
                {
                    return Redirect(Global.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Home", "Home", new { area = "Admin" });
                }
            }
            else
            {
                ViewBag.Error = true;
            }
            return View(userVM);
        }
        [AllowAnonymous]
        public ActionResult Logout()
        {
            if (Session["UserId"] != null)
            {
                Session.Clear();
            }
            if (Session["MemberId"] != null)
            {
                Session.Clear();
            }
            return RedirectToAction("Login", "Account", new { area = "" });
        }
        public JsonResult CheckSessionExists()
        {
            bool IsExisting = false;
            if (Session["MemberId"] == null)
                IsExisting = false;
            else
                IsExisting = true;

            return Json(IsExisting, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Register()
        {
            return View();
        }

        #endregion
        // Methods
        #region Methods
        public void LoginSession(List<User> user)
        {
            var Firstname = user.FirstOrDefault().Firstname;
            var Lastname = user.FirstOrDefault().Lastname;
            var DateCreated = user.FirstOrDefault().DateCreated;
            var Email = user.FirstOrDefault().Email;
            var Username = user.FirstOrDefault().Username;
            var ID = user.FirstOrDefault().Id;
            var Role = user.FirstOrDefault().Role.Description;
            var RoleID = user.FirstOrDefault().RoleID;

            var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, Firstname),
                    new Claim(ClaimTypes.Name, Lastname),
                    new Claim(ClaimTypes.DateOfBirth, DateCreated.ToString()),
                    new Claim(ClaimTypes.Email, Email),
                    new Claim(ClaimTypes.PrimarySid, ID.ToString()),
                    new Claim(ClaimTypes.Name, Username)
                },
                "ApplicationCookie");

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignIn(identity);
            Session["UserId"] = ID.ToString();
            Session["Username"] = Username;
            Session["DateCreated"] = Convert.ToDateTime(DateCreated).ToLongDateString();
            Session["Name"] = Firstname + " " + Lastname;
            Session["Role"] = Role;
            Session["RoleID"] = RoleID;
        }

        #endregion
    }
}