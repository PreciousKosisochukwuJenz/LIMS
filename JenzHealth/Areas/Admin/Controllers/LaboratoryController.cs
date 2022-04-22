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
        ISeedService _seedService;
        private static int SpecimenCollectionID;
        public LaboratoryController()
        {
            _laboratoryService = new LaboratoryService(db);
            _customerService = new CustomerService(db);
            _paymentService = new PaymentService(db, new UserService());
            _seedService = new SeedService(db);
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
        public ActionResult Prepare(int ID, bool? Saved)
        {
            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-primary";
                TempData["AlertMessage"] = "Result computation saved successfully";
            }
            var record = _laboratoryService.GetSpecimensForPreparation(ID);
            ViewBag.Customer = _paymentService.GetCustomerForBill(record.BillInvoiceNumber);
            var billedServices = _laboratoryService.GetServicesToPrepare(record.BillInvoiceNumber);
            TempData["SpecimenCollectedID"] = ID;
            SpecimenCollectionID = ID;
            TempData["BillNumber"] = record.BillInvoiceNumber;
            ViewBag.Services = billedServices;
            ViewBag.Templates = _laboratoryService.GetDistinctTemplateForBilledServices(billedServices);
            return View();
        }
        public ActionResult Compute(int templateID, string billNumber,string serviceIds)
        {
            TempData["BillNumber"] = billNumber;
            TempData["SpecimenCollectedID"] = SpecimenCollectionID;
            int[] serviceIDs = Array.ConvertAll(serviceIds.Split(','), element => int.Parse(element));
            var template = _seedService.GetTemplate(templateID);
            switch (template.UseDefaultParameters)
            {
                case true:
                    return View("ComputeNonTemplatedServicePreparation", _laboratoryService.GetNonTemplatedLabPreparation(billNumber, serviceIDs[0]));
                case false:
                    return View("ComputeTemplatedServicePreparation", _laboratoryService.SetupTemplatedServiceForComputation(templateID, billNumber));
            }
            return View();
        }

        public ActionResult ResultApproval()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }

            return View();
        }
        [HttpPost]
        public ActionResult ResultApproval(ResultApprovalVM vmodel)
        {
            ViewBag.TableData = _laboratoryService.GetAllTestForApprovalByBillNumber(vmodel.BillNumber);
            return View(vmodel);
        }

        public ActionResult ComputedResult(int serviceParameterID, string billnumber,int templateID, int Id, int serviceID)
        {
            var template = _seedService.GetTemplate(templateID);
            ViewBag.Id = Id;
            switch (template.UseDefaultParameters)
            {
                case false:
                    return View("ComputeTemplatedResult", _laboratoryService.GetComputedResultForTemplatedService(billnumber,serviceParameterID));
                case true:
                    return View("ComputeNonTemplatedResult", _laboratoryService.GetNonTemplatedLabPreparation(billnumber,serviceID));
            }
            return View();
        }

        public JsonResult ApproveResult(int Id)
        {
            var status = _laboratoryService.ApproveTestResult(Id);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateLabResults(List<RequestComputedResultVM> results, string labnote, string comment)
        {
            var status = _laboratoryService.UpdateLabResults(results, labnote, comment);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateNonTemplatedLabResults(NonTemplatedLabPreparationVM vmodel, List<NonTemplatedLabPreparationOrganismXAntiBioticsVM> organisms)
        {
            var status = _laboratoryService.UpdateNonTemplatedLabResults(vmodel, organisms);
            return Json(status, JsonRequestBehavior.AllowGet);
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
        public JsonResult GetComputedAntibioticsAndOrgansm(int id)
        {
            var result = _laboratoryService.GetComputedOrganismXAntibiotics(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}