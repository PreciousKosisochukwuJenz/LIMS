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
    public class LaboratoryController : Controller
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
        ILaboratoryService _laboratoryService;
        public LaboratoryController()
        {
            _laboratoryService = new LaboratoryService(new DatabaseEntities());
        }
        public LaboratoryController(LaboratoryService laboratoryService)
        {
            _laboratoryService = laboratoryService;
        }
        #endregion

        public ActionResult ParameterSetups()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }

            return View();
        }

        public ActionResult RangeSetups()
        {
            return View();
        }

        public ActionResult UpdateServiceParameters(ServiceParameterVM serviceParameter, List<ServiceParameterSetupVM> serviceParameterSetups)
        {
            _laboratoryService.UpdateParamterSetup(serviceParameter, serviceParameterSetups);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateServiceParameterRanges(List<ServiceParameterRangeSetupVM> serviceParameterSetups)
        {
            _laboratoryService.UpdateParameterRangeSetup(serviceParameterSetups);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetServiceParameter(string service)
        {
            var serviceparameter = _laboratoryService.GetServiceParameter(service);
            return Json(serviceparameter, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetServiceParameterRanges(string service)
        {
            var serviceparameterrange = _laboratoryService.GetRangeSetups(service);
            return Json(serviceparameterrange, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetServiceParameterSetups(string service)
        {
            var serviceparameter = _laboratoryService.GetServiceParamterSetups(service);
            return Json(serviceparameter, JsonRequestBehavior.AllowGet);
        }
    }
}