using JenzHealth.Areas.Admin.Components;
using JenzHealth.Areas.Admin.Helpers;
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
using System.Web.Script.Serialization;

namespace JenzHealth.Areas.Admin.Controllers
{
    public class PaymentController : Controller
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
        IPaymentService _paymentService;
        ICustomerService _customerService;
        ISeedService _seedService;
        public PaymentController()
        {
            _paymentService = new PaymentService(new DatabaseEntities(), new UserService());
            _customerService = new CustomerService(new DatabaseEntities());
            _seedService = new SeedService(new DatabaseEntities());
        }
        public PaymentController(PaymentService paymentService, CustomerService customerService, SeedService seedService)
        {
            _paymentService = paymentService;
            _customerService = customerService;
            _seedService = seedService;
        }
        #endregion


        public ActionResult Billings(bool? Saved, bool? Updated, string invoicenumber)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = string.Format("Bill was generated successfully for inoivce number '{0}'.", invoicenumber);
            }
            if (Updated == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = string.Format("Bill was re-generated successfully for inoivce number '{0}'.", invoicenumber); ;
            }
            ViewBag.SearchBy = new SelectList(CustomData.SearchBy, "Value", "Text");
            return View();
        }
        [HttpPost]
        public ActionResult Billings(BillingVM vmodel, List<ServiceListVM> serviceList)
        {
            ViewBag.SearchBy = new SelectList(CustomData.SearchBy, "Value", "Text");
            string invoicenumber = "";
            if ((vmodel.InvoiceNumber != null && vmodel.CustomerUniqueID == null))
            {
                invoicenumber = _paymentService.UpdateBilling(vmodel, serviceList);
            }
            else
            {
                invoicenumber = _paymentService.CreateBilling(vmodel, serviceList);
            }
            var response = new
            {
                Status = true,
                InvoiceNumber = invoicenumber
            };
            return Json(response, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Waivers(bool? Saved)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Amount was waived successfully.";
            }
            return View();
        }
        [HttpPost]
        public ActionResult Waivers(WaiverVM vmodel)
        {
            var status = _paymentService.WaiveAmountForCustomer(vmodel);
            return RedirectToAction("Waivers", new { Saved = status });
        }
        public ActionResult PartPayments(bool? Saved)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Part payment was mapped successfully.";
            }
            return View();
        }

        public ActionResult DepositeCollections(bool? Saved)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Deposited successfully.";
            }
            return View();
        }
        [HttpPost]
        public ActionResult DepositeCollections(DepositeCollectionVM vmodel)
        {
            var status = _paymentService.Deposite(vmodel);
            return RedirectToAction("DepositeCollections", new { Saved = status });
        }

        public ActionResult CashCollections(bool? Saved)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Cash Collected successfully.";
            }
            return View();
        }

        [HttpPost]
        public ActionResult CashCollections(CashCollectionVM vmodel, List<ServiceListVM> serviceList)
        {
            var status = _paymentService.CashCollection(vmodel, serviceList);
            return Json(status, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Transactions()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            var model = new TransactionVM();
            return View(model);
        }
        [HttpPost]
        public ActionResult Transactions(TransactionVM vmodel)
        {
            var model = _paymentService.GetTransactionReports(vmodel);
            return View(model);
        }
        public ActionResult TransactionDetails(int shiftID)
        {
            var model = _paymentService.GetShiftTransactionDetails(shiftID);
            ViewBag.DetailedCashCollection = _paymentService.GetCashCollectionsForShift(shiftID);

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<int> paymentTypeTransactionCountArr = new List<int>();
            var paymentTypes = Enum.GetValues(typeof(PaymentType)).Cast<PaymentType>();
            foreach (var paymentType in paymentTypes.ToList())
            {
                var count = db.CashCollections.Count(x => x.IsDeleted == false && x.ShiftID == shiftID && x.PaymentType == paymentType);
                paymentTypeTransactionCountArr.Add(count);
            }

            //Analytics
            List<decimal> analyticsCountArr = new List<decimal>();
            foreach (var paymentType in paymentTypes.ToList())
            {
                decimal totalamount;
                var cashcollectionamount = db.CashCollections.Where(x => x.IsDeleted == false && x.ShiftID == shiftID && x.PaymentType == paymentType);
                if(cashcollectionamount != null)
                {
                    totalamount =  cashcollectionamount.ToList().Sum(x => x.AmountPaid);
                }
                else
                {
                    totalamount = 0;
                }

                analyticsCountArr.Add(totalamount);
            }

            ViewBag.TransactionByPaymentTypeCountArr = jsSerializer.Serialize(paymentTypeTransactionCountArr);
            ViewBag.PaymentTypes = jsSerializer.Serialize(paymentTypes.Select(x => x.DisplayName()).ToArray());
            ViewBag.AnalysticsCountArr = jsSerializer.Serialize(analyticsCountArr);

            return View(model);
        }
        public ActionResult Refunds(bool? Saved)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Refund was successfully.";
            }
            return View();
        }

        public ActionResult RecieptCancellations(bool? Saved)
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Reciept cancelled successfully.";
            }
            return View();
        }

        #region Json
        [HttpPost]
        public JsonResult Refunds(RefundVM vmodel)
        {
            var model = _paymentService.Refund(vmodel);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CancelReciept(CashCollectionVM vmodel)
        {
            _paymentService.CancelReciept(vmodel);
            return Json("success", JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCustomerByUsername(string username)
        {
            var model = _customerService.SearchCustomerWithIDorPhoneNumber(username);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCustomerByInvoiceNumber(string invoiceNumber)
        {
            var model = _paymentService.GetCustomerForBill(invoiceNumber);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBillInvoiceWithReciept(string recipet)
        {
            var model = _paymentService.GetBillNumberWithReceipt(recipet);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServiceByName(string servicename)
        {
            var model = _seedService.GetService(servicename);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetServiceAutoComplete(string query)
        {
            var model = _seedService.GetServiceAutoComplete(query);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetServicesByInvoiceNumber(string invoiceNumber)
        {
            var model = _paymentService.GetBillServices(invoiceNumber);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetWaivedAmountsForInvoiceNumber(string invoiceNumber)
        {
            var model = _paymentService.GetWaivedAmountForBillInvoiceNumber(invoiceNumber);
            if (model == null)
                model = new Waiver();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetTotalPaidBillAmount(string invoiceNumber)
        {
            var model = _paymentService.GetTotalPaidBillAmount(invoiceNumber);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInstallmentsByInvoiceNumber(string invoiceNumber)
        {
            var model = _paymentService.GetPartPayments(invoiceNumber);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult PartPayments(List<PartPaymentVM> vmodel)
        {
            var status = _paymentService.MapPartPayment(vmodel);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}