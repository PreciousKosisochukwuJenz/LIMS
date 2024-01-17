using JenzHealth.Areas.Admin.Components;
using JenzHealth.Areas.Admin.Interfaces;
using JenzHealth.Areas.Admin.Services;
using JenzHealth.Areas.Admin.ViewModels;
using JenzHealth.DAL.DataConnection;
using JenzHealth.DAL.Entity;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using PowerfulExtensions.Linq;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JenzHealth.Areas.Admin.ViewModels.Report;
using System.Globalization;

namespace JenzHealth.Areas.Admin.Controllers
{
    public class ReportController : Controller
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
        ICustomerService _customerService;
        IApplicationSettingsService _settingService;
        IPaymentService _paymentService;
        ISeedService _seedService;
        IUserService _userService;
        ILaboratoryService _laboratoryService;
        IReportService _reportService;
        public ReportController()
        {
            _customerService = new CustomerService(db);
            _laboratoryService = new LaboratoryService(db);
            _paymentService = new PaymentService(db, new UserService());
            _seedService = new SeedService(db);
            _settingService = new ApplicationSettingsService(db);
            _userService = new UserService(db);
            _reportService = new ReportService(db, new PaymentService(), new LaboratoryService(), new SeedService());
        }
        public ReportController(
            CustomerService customerService,
            LaboratoryService laboratoryService,
            PaymentService paymentService,
            SeedService seedService,
            ApplicationSettingsService settingService,
            UserService userService,
            ReportService reportService
            )
        {
            _customerService = customerService;
            _laboratoryService = laboratoryService;
            _paymentService = paymentService;
            _seedService = seedService;
            _settingService = settingService;
            _userService = userService;
            _reportService = reportService;
        }
        #endregion

        public ActionResult RequestTracker()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            return View();
        }
        [HttpPost]
        public ActionResult RequestTracker(RequestTrackerVM vmodel)
        {
            var records = _reportService.TrackRequest(vmodel);

            ViewBag.TableData = records;
            return View(vmodel);
        }

        public ActionResult LabResultCollectors()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            return View();
        }
        [HttpPost]
        public ActionResult LabResultCollectors(LabResultCollectionVM vmodel)
        {
            var records = _laboratoryService.GetLabResultCollections(vmodel);
            var distinctBills = records.Distinct(o => o.BillNumber).ToList();

            ViewBag.TableData = records;
            ViewBag.DistinctBills = distinctBills;
            return View(vmodel);
        }

        // GET: Admin/Report

        #region Payment Report
        public ActionResult BillingInvoice(string billnumber)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Admin/Reports/Payment"), "BillInvoice.rdlc");

            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                throw new Exception($"Report path not found in the specified directory: {path}");
            }

            var header = _settingService.GetReportHeader();
            var customer = _paymentService.GetCustomerForReport(billnumber);
            var billServices = _paymentService.GetBillServices(billnumber);
            var billDetails = _paymentService.GetBillingDetails(billnumber);

            ReportDataSource Header = new ReportDataSource("SettingsDataSet", header);
            ReportDataSource Customer = new ReportDataSource("CustomerDataSet", customer);
            ReportDataSource BilledService = new ReportDataSource("BillingDataSet", billServices);
            ReportDataSource BillDetails = new ReportDataSource("BillingDetailsDataSet", billDetails);

            if (Header != null && Customer != null && BilledService != null && BillDetails != null)
            {
                lr.DataSources.Add(Header);
                lr.DataSources.Add(Customer);
                lr.DataSources.Add(BilledService);
                lr.DataSources.Add(BillDetails);
            }

            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                                "  <PageWidth>80mm</PageWidth>" +
                                "  <PageHeight>200mm</PageHeight>" +
                                "  <MarginTop>0.25in</MarginTop>" +
                                "  <MarginLeft>0.1in</MarginLeft>" +
                                "  <MarginRight>0.1in</MarginRight>" +
                                "  <MarginBottom>0.25in</MarginBottom>" +
                                "  <EmbedFonts>None</EmbedFonts>" +
                                "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, mimeType);
        }

        public ActionResult PaymentReciept(string recieptnumber, string billnumber)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Admin/Reports/Payment"), "PaymentReciept.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                throw new Exception(String.Format("Report path not found in the specified directory: {0", path));
            }
            var header = _settingService.GetReportHeader();
            var customer = _paymentService.GetCustomerForReport(billnumber);
            var billServices = _paymentService.GetBillServices(billnumber);
            var paymentDetail = _paymentService.GetPaymentDetails(recieptnumber);
            ReportDataSource Header = new ReportDataSource("SettingsDataSet", header);
            ReportDataSource Customer = new ReportDataSource("CustomerDataSet", customer);
            ReportDataSource BilledService = new ReportDataSource("BillingDataSet", billServices);
            ReportDataSource PaymentDetails = new ReportDataSource("PaymentDataSet", paymentDetail);
            if (Header != null && Customer != null && BilledService != null && PaymentDetails!= null)
            {
                lr.DataSources.Add(Header);
                lr.DataSources.Add(Customer);
                lr.DataSources.Add(BilledService);
                lr.DataSources.Add(PaymentDetails);
            }
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                                 "  <PageWidth>80mm</PageWidth>" +
                                 "  <PageHeight>300mm</PageHeight>" +
                                 "  <MarginTop>0.25in</MarginTop>" +
                                 "  <MarginLeft>0.1in</MarginLeft>" +
                                 "  <MarginRight>0.1in</MarginRight>" +
                                 "  <MarginBottom>0.25in</MarginBottom>" +
                                 "  <EmbedFonts>None</EmbedFonts>" +
                                 "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }

        public ActionResult FinancialReport()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            var model = new CashCollectionVM();
            return View(model);
        }

        [HttpPost]
        public ActionResult FinancialReport(CashCollectionVM vmodel)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

            var payments = _paymentService.GetFinancialReport(vmodel);
            ViewBag.Payments = payments;
            ViewBag.PaymentTotal = "₦" + payments.Sum(x=>x.AmountPaid).ToString("N", nfi);
            return View(vmodel);
        }

        public ActionResult ServiceReport()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            var model = new BillingVM();
            return View(model);
        }

        [HttpPost]
        public ActionResult ServiceReport(BillingVM vmodel)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

            var billServices = _paymentService.GetBillServicesForReport(vmodel);
            ViewBag.BillServices = billServices;
            ViewBag.GrossTotal = "₦" + billServices.Sum(x => x.GrossAmount).ToString("N", nfi);
            return View(vmodel);
        }
        #endregion

        #region Laboratory report

        public ActionResult LabReport(string billnumber, int templateID, bool templated)
        {
            LocalReport lr = new LocalReport();
            DatabaseEntities db = new DatabaseEntities();
            string path;
            var template = db.Templates.FirstOrDefault(x => x.Id == templateID);
            if (template == null) throw new Exception("Template not found");
            if (!template.UseDocParameter)
            {
                 path = !template.UseDefaultParameters ? Path.Combine(Server.MapPath("~/Areas/Admin/Reports/Laboratory"), "TemplatedLabResult.rdlc") : Path.Combine(Server.MapPath("~/Areas/Admin/Reports/Laboratory"), "NonTemplatedLabResult.rdlc");
            }
            else
            {
                path = Path.Combine(Server.MapPath("~/Areas/Admin/Reports/Laboratory"), "DocLabResult.rdlc");
            }
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                throw new Exception(String.Format("Report path not found in the specified directory: {0}", path));
            }
            var header = _settingService.GetReportHeader();
            var customer = _paymentService.GetCustomerForReport(billnumber);
            var specimenCollection = _laboratoryService.GetSpecimenCollectedForReport(billnumber, templateID);
            ReportDataSource Header = new ReportDataSource("SettingDataSet", header);
            ReportDataSource Customer = new ReportDataSource("CustomerDataSet", customer);
            ReportDataSource SpecimenCollection = new ReportDataSource("SpecimenCollectionDataSet", specimenCollection);
            if (Header != null && Customer != null && SpecimenCollection != null)
            {
                lr.DataSources.Add(Header);
                lr.DataSources.Add(Customer);
                lr.DataSources.Add(SpecimenCollection);
            }

            if (template.UseDocParameter)
            {
                var templatedResult = _laboratoryService.GetDocLabResultForReport(templateID, billnumber);
                ReportDataSource TemplatedLabResult = new ReportDataSource("TemplatedDataSet", templatedResult);
                if (TemplatedLabResult != null)
                {
                    lr.DataSources.Add(TemplatedLabResult);
                }
            }
            else if (!template.UseDefaultParameters)
            {
                var templatedResult = _laboratoryService.GetTemplatedLabResultForReport(templateID, billnumber);
                ReportDataSource TemplatedLabResult = new ReportDataSource("TemplatedDataSet", templatedResult);
                if (TemplatedLabResult != null)
                {
                    lr.DataSources.Add(TemplatedLabResult);
                }
            }
            else
            {
                var nonTemplateResult = _laboratoryService.GetNonTemplatedLabPreparationForReport(billnumber);
                var nonTemplateOrganism = _laboratoryService.GetComputedOrganismXAntibiotics(nonTemplateResult.FirstOrDefault().Id);

                ReportDataSource NonTemplateResult = new ReportDataSource("NonTemplatedLabResultDataSet", nonTemplateResult);
                ReportDataSource NonTemplateOrganism = new ReportDataSource("NonTemplatedOrganismDataSet", nonTemplateOrganism);

                if (NonTemplateResult != null && NonTemplateOrganism != null)
                {
                    lr.DataSources.Add(NonTemplateResult);
                    lr.DataSources.Add(NonTemplateOrganism);
                }
            }
         
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo = "<DeviceInfo>" +
                    "  <PageWidth>8.27in</PageWidth>" +
                    "  <PageHeight>11.69in</PageHeight>" +
                    "  <MarginTop>0.25in</MarginTop>" +
                    "  <MarginLeft>0.4in</MarginLeft>" +
                    "  <MarginRight>0.4in</MarginRight>" +
                    "  <MarginBottom>0.25in</MarginBottom>" +
                    "  <EmbedFonts>None</EmbedFonts>" +
                    "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);
            return File(renderedBytes, mimeType);
        }

        public ActionResult ViewResult(string billnumber)
        {
            var billedServices = _laboratoryService.GetServicesToPrepare(billnumber);
            var distinctServices = _laboratoryService.GetDistinctTemplateForBilledServices(billedServices);

            var response = new { distinctServices, billedServices };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}