using JenzHealth.Areas.Admin.Interfaces;
using JenzHealth.Areas.Admin.Services;
using JenzHealth.DAL.DataConnection;
using JenzHealth.DAL.Entity;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        public ReportController()
        {
            _customerService = new CustomerService(db);
            _laboratoryService = new LaboratoryService(db);
            _paymentService = new PaymentService(db, new UserService());
            _seedService = new SeedService(db);
            _settingService = new ApplicationSettingsService(db);
            _userService = new UserService(db);
        }
        public ReportController(
            CustomerService customerService,
            LaboratoryService laboratoryService,
            PaymentService paymentService,
            SeedService seedService,
            ApplicationSettingsService settingService,
            UserService userService
            )
        {
            _customerService = customerService;
            _laboratoryService = laboratoryService;
            _paymentService = paymentService;
            _seedService = seedService;
            _settingService = settingService;
            _userService = userService;
        }
        #endregion
        // GET: Admin/Report
        public ActionResult BillingInvoice(string billnumber)
        {
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/Areas/Admin/Reports"), "BillInvoice.rdlc");
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
            var billDetails = _paymentService.GetBillingDetails(billnumber);
            ReportDataSource Header = new ReportDataSource("SettingDataSet", header);
            ReportDataSource Customer = new ReportDataSource("CustomerDataSet", customer);
            ReportDataSource BilledService = new ReportDataSource("BillingDataSet", billServices);
            ReportDataSource BillDetails = new ReportDataSource("BillingDetailsDataSet", billDetails);
            if (Header != null && Customer != null && BilledService != null)
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
    }
}