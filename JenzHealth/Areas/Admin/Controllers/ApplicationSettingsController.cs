using JenzHealth.DAL.DataConnection;
using JenzHealth.DAL.Entity;
using JenzHealth.Areas.Admin.Interfaces;
using JenzHealth.Areas.Admin.Services;
using JenzHealth.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JenzHealth.Areas.Admin.Components;

namespace JenzHealth.Areas.Admin.Controllers
{
    public class ApplicationSettingsController : Controller
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
        IApplicationSettingsService _settingService;
        public ApplicationSettingsController()
        {
            _settingService = new ApplicationSettingsService(new DatabaseEntities());
        }
        public ApplicationSettingsController(ApplicationSettingsService settingsService)
        {
            _settingService = settingsService;
        }
        DatabaseEntities db = new DatabaseEntities();
        #endregion

        public ActionResult Manage()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath)){
                throw new UnauthorizedAccessException();
            }
            return View(_settingService.GetApplicationSettings());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(ApplicationSettingsVM Vmodel, HttpPostedFileBase LogoData, HttpPostedFileBase WatermarkData)
        {
            if (ModelState.IsValid)
            {
                bool saveState = _settingService.UpdateApplicationSettings(Vmodel, LogoData, WatermarkData);
                if(saveState == true)
                {
                    ViewBag.ShowAlert = true;
                    TempData["AlertMessage"] = "Application settings updated successfully.";
                    TempData["AlertType"] = "alert-primary";
                }
            }
            return View(_settingService.GetApplicationSettings());
        }

        // Categories
     
    }
}