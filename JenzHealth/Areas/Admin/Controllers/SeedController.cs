using JenzHealth.Areas.Admin.Components;
using JenzHealth.Areas.Admin.Interfaces;
using JenzHealth.Areas.Admin.Services;
using JenzHealth.Areas.Admin.ViewModels;
using JenzHealth.DAL.DataConnection;
using JenzHealth.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JenzHealth.Areas.Admin.Controllers
{
    public class SeedController : Controller
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


        //Initializing dependency services
        #region Instanciation
        DatabaseEntities db = new DatabaseEntities();
        ISeedService _seedService;
        public SeedController()
        {
            _seedService = new SeedService(new DatabaseEntities());
        }
        public SeedController(SeedService seedService)
        {
            _seedService = seedService;
        }
        #endregion


        // Revenue Department
        public ActionResult ManageRevenueDepartments(bool? Added, bool? Editted)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Revenue department added successfully.";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Revenue department updated successfully.";
            }
            ViewBag.RevenueDepartments = _seedService.GetRevenueDepartment();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateRevenueDepartment(RevenueDepartmentVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _seedService.CreateRevenueDepartment(vmodel);
            }
            return RedirectToAction("ManageRevenueDepartments", new { Added = hasSaved });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditRevenueDepartment(RevenueDepartmentVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _seedService.EditRevenueDepartment(vmodel);
            }
            return RedirectToAction("ManageRevenueDepartments", new { Editted = hasSaved });
        }
        public JsonResult GetRevenueDepartment(int id)
        {
            var model = _seedService.GetRevenueDepartment(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteRevenueDepartment(int id)
        {
            var model = _seedService.DeleteRevenueDepartment(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // Service Department
        public ActionResult ManageServiceDepartments(bool? Added, bool? Editted)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Service department added successfully.";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Service department updated successfully.";
            }
            ViewBag.ServiceDepartments = _seedService.GetServiceDepartment();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateServiceDepartment(ServiceDepartmentVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _seedService.CreateServiceDepartment(vmodel);
            }
            return RedirectToAction("ManageServiceDepartments", new { Added = hasSaved });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditServiceDepartment(ServiceDepartmentVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _seedService.EditServiceDepartment(vmodel);
            }
            return RedirectToAction("ManageServiceDepartments", new { Editted = hasSaved });
        }
        public JsonResult GetServiceDepartment(int id)
        {
            var model = _seedService.GetServiceDepartment(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteServiceDepartment(int id)
        {
            var model = _seedService.DeleteServiceDepartment(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // Priviledge
        public ActionResult ManagePriviledges(bool? Added, bool? Editted)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Priviledge added successfully.";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Priviledge updated successfully.";
            }
            ViewBag.Priviledges = _seedService.GetPriviledges();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreatePriviledge(PriviledgeVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _seedService.CreatePriviledge(vmodel);
            }
            return RedirectToAction("ManagePriviledges", new { Added = hasSaved });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditPriviledge(PriviledgeVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _seedService.EditPriviledge(vmodel);
            }
            return RedirectToAction("ManagePriviledges", new { Editted = hasSaved });
        }
        public JsonResult GetPriviledge(int id)
        {
            var model = _seedService.GetPriviledge(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeletePriviledge(int id)
        {
            var model = _seedService.DeletePriviledge(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // Template
        public ActionResult ManageTemplates(bool? Added, bool? Editted)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Template added successfully.";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Template updated successfully.";
            }
            ViewBag.ServiceDepartments = new SelectList(db.ServiceDepartments.Where(x => x.IsDeleted == false), "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult ManageTemplates(TemplateVM vmodel)
        {
            ViewBag.Templates = _seedService.GetTemplates((int)vmodel.ServiceDepartmentID);
            ViewBag.ServiceDepartments = new SelectList(db.ServiceDepartments.Where(x => x.IsDeleted == false), "Id", "Name");
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateTemplate(TemplateVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _seedService.CreateTemplate(vmodel);
            }
            return RedirectToAction("ManageTemplates", new { Added = hasSaved });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditTemplate(TemplateVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _seedService.EditTemplate(vmodel);
            }
            return RedirectToAction("ManageTemplates", new { Editted = hasSaved });
        }
        public JsonResult GetTemplate(int id)
        {
            var model = _seedService.GetTemplate(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteTemplate(int id)
        {
            var model = _seedService.DeleteTemplate(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // Vendor
        public ActionResult ManageVendors(bool? Added, bool? Editted)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }

            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Vendor added successfully.";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Vendor updated successfully.";
            }
            ViewBag.Vendors = _seedService.GetVendors();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateVendor(VendorVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _seedService.CreateVendors(vmodel);
            }
            return RedirectToAction("ManageVendors", new { Added = hasSaved });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Editvendor(VendorVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _seedService.EditVendor(vmodel);
            }
            return RedirectToAction("ManageVendors", new { Editted = hasSaved });
        }
        public JsonResult GetVendor(int id)
        {
            var model = _seedService.GetVendor(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteVendor(int id)
        {
            var model = _seedService.DeleteVendor(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        // Service Department
        public ActionResult ManageServices(bool? Added, bool? Editted)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Service added successfully.";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Service updated successfully.";
            }
            ViewBag.ServiceDepartments = new SelectList(db.ServiceDepartments.Where(x => x.IsDeleted == false), "Id", "Name");
            ViewBag.RevenueDepartments = new SelectList(db.RevenueDepartments.Where(x => x.IsDeleted == false), "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult ManageServices(ServiceVM vmodel)
        {
            ViewBag.Services = _seedService.GetServices(vmodel);
            ViewBag.ServiceDepartments = new SelectList(db.ServiceDepartments.Where(x => x.IsDeleted == false), "Id", "Name");
            ViewBag.RevenueDepartments = new SelectList(db.RevenueDepartments.Where(x => x.IsDeleted == false), "Id", "Name");
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateService(ServiceVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _seedService.CreateService(vmodel);
            }
            return RedirectToAction("ManageServices", new { Added = hasSaved });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditService(ServiceVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _seedService.EditService(vmodel);
            }
            return RedirectToAction("ManageServices", new { Editted = hasSaved });
        }
        public JsonResult GetService(int id)
        {
            var model = _seedService.GetService(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteService(int id)
        {
            var model = _seedService.DeleteService(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}