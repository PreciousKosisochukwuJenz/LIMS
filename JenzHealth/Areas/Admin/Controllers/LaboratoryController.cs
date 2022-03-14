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
        ICustomerService _customerService;
        IPaymentService _paymentService;
        public LaboratoryController()
        {
            _laboratoryService = new LaboratoryService(new DatabaseEntities());
            _customerService = new CustomerService(new DatabaseEntities());
            _paymentService = new PaymentService(new DatabaseEntities(),new UserService());
        }
        public LaboratoryController(LaboratoryService laboratoryService, CustomerService customerService, PaymentService paymentService)
        {
            _laboratoryService = laboratoryService;
            _customerService = customerService;
            _paymentService = paymentService;
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

        public ActionResult SpecimenCollections()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            return View();
        }
        public ActionResult Preparations()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Preparations(SpecimenCollectionVM vmodel)
        {
            ViewBag.TableData = _laboratoryService.GetLabPreparations(vmodel);
            return View(vmodel);
        }
        public ActionResult Prepare(int ID)
        {

            var record = _laboratoryService.GetSpecimensForPreparation(ID);
            ViewBag.Customer = _paymentService.GetCustomerForBill(record.BillInvoiceNumber);
            var billedServices = _laboratoryService.GetServicesToPrepare(record.BillInvoiceNumber);
            ViewBag.Services = billedServices;
            ViewBag.Templates = _laboratoryService.GetDistinctTemplateForBilledServices(billedServices);
            return View();
        }
        public ActionResult ComputeTemplatedServicePreparation(int templateID, string billNumber)
        {
            return View(_laboratoryService.SetupTemplatedServiceForComputation(templateID, billNumber));
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
        public ActionResult UpdateSpecimenCollection(SpecimenCollectionVM specimenCollection, List<SpecimenCollectionCheckListVM> checklist)
        {
            _laboratoryService.UpdateSpecimenSampleCollection(specimenCollection, checklist);
            return Json("Success", JsonRequestBehavior.AllowGet);
        }
        public ActionResult CheckSpecimenCollection(string invoicenumber)
        {
            var exist = _laboratoryService.CheckSpecimenCollectionWithBillNumber(invoicenumber);
            return Json(exist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSpecimenCollected(string invoicenumber)
        {
            var specimenCollected = _laboratoryService.GetSpecimenCollected(invoicenumber);
            return Json(specimenCollected, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetServicesAndSpecimenByInvoiceNumber(string invoiceNumber)
        {
            var model = _laboratoryService.GetServiceParameters(invoiceNumber);
            return Json(model, JsonRequestBehavior.AllowGet);
        }


    }
}