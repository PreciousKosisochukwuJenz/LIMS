using JenzHealth.Areas.Admin.Helpers;
using JenzHealth.Areas.Admin.Interfaces;
using JenzHealth.Areas.Admin.ViewModels;
using JenzHealth.DAL.DataConnection;
using JenzHealth.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.Services
{
    public class PaymentService : IPaymentService
    {
        readonly DatabaseEntities _db;
        public PaymentService()
        {
            _db = new DatabaseEntities();
        }
        public PaymentService(DatabaseEntities db)
        {
            _db = db;
        }

        public bool CreateBilling(BillingVM vmodel, List<int> ServiceIDs)
        {
            var dateOfBill = DateTime.Now.Date.ToShortDateString().Replace("/", "");
            foreach (int serviceID in ServiceIDs)
            {
                var model = new Billing()
                {
                    CustomerType = vmodel.CustomerType,
                    CustomerUniqueID = vmodel.CustomerType == CustomerType.REGISTERED_CUSTOMER ? vmodel.CustomerUniqueID : null,
                    CustomerID = vmodel.CustomerType == CustomerType.REGISTERED_CUSTOMER ? vmodel.CustomerID : 0,
                    CustomerName = vmodel.CustomerType == CustomerType.REGISTERED_CUSTOMER ? vmodel.CustomerName : null,
                    CustomerGender = vmodel.CustomerType == CustomerType.REGISTERED_CUSTOMER ? vmodel.CustomerGender : null,
                    CustomerAge = vmodel.CustomerType == CustomerType.REGISTERED_CUSTOMER ? vmodel.CustomerAge : 0,
                    CustomerPhoneNumber = vmodel.CustomerType == CustomerType.REGISTERED_CUSTOMER ? vmodel.CustomerPhoneNumber : null,
                    InvoiceNumber = "BILL"+ dateOfBill + Generator.GeneratorCode(),
                    IsDeleted = false,
                    DateCreated = DateTime.Now,
                    ServiceID = serviceID,
                };
                _db.Billings.Add(model);
            }
            _db.SaveChanges();

            return true;
        }
    }
}