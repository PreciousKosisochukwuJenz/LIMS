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
        public PaymentController()
        {
            _paymentService = new PaymentService(new DatabaseEntities());
            _customerService = new CustomerService(new DatabaseEntities());
        }
        public PaymentController(PaymentService paymentService, CustomerService customerService)
        {
            _paymentService = paymentService;
            _customerService = customerService;
        }
        #endregion


        public ActionResult Billings()
        {
            if (!Nav.CheckAuthorization(Request.Url.AbsolutePath))
            {
                throw new UnauthorizedAccessException();
            }
            ViewBag.SearchBy = new SelectList(CustomData.SearchBy, "Value", "Text");
            return View();
        }
        [HttpPost]
        public ActionResult Billings(BillingVM vmodel)
        {
            return View();
        }

        public JsonResult GetCustomerByUsername(string username)
        {
            var model = _customerService.GetCustomer(username);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}