using JenzHealth.Areas.Admin.Helpers;
using JenzHealth.Areas.Admin.Interfaces;
using JenzHealth.Areas.Admin.ViewModels;
using JenzHealth.DAL.DataConnection;
using JenzHealth.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public bool CreateBilling(BillingVM vmodel, List<ServiceListVM> serviceList)
        {
            var dateOfBill = DateTime.Now.Date.ToShortDateString().Replace("/", "");
            var invoiceNumber = "BILL" + dateOfBill + Generator.GeneratorCode();

            foreach (var service in serviceList)
            {
                var model = new Billing()
                {
                    CustomerType = vmodel.CustomerType,
                    CustomerUniqueID = vmodel.CustomerType == CustomerType.REGISTERED_CUSTOMER ? vmodel.CustomerUniqueID : null,
                    CustomerName =  vmodel.CustomerName,
                    CustomerGender = vmodel.CustomerGender,
                    CustomerAge =  vmodel.CustomerAge,
                    CustomerPhoneNumber = vmodel.CustomerPhoneNumber,
                    IsDeleted = false,
                    InvoiceNumber = invoiceNumber,
                    DateCreated = DateTime.Now,
                    ServiceID = service.ServiceID,
                    GrossAmount = service.GrossAmount,
                    Quantity = service.Quantity,
                };
                _db.Billings.Add(model);
            }
            _db.SaveChanges();

            return true;
        }
        public bool UpdateBilling(BillingVM vmodel, List<ServiceListVM> serviceList)
        {
            var ServiceBills = _db.Billings.Where(x => x.InvoiceNumber == vmodel.InvoiceNumber &&  x.IsDeleted == false).Select(b=>b.ServiceID).ToList();
            if (ServiceBills.Count > 0)
            {
                foreach (var service in ServiceBills)
                {
                    var removeBillService = _db.Billings.FirstOrDefault(x => x.ServiceID == service && x.IsDeleted == false);
                    removeBillService.IsDeleted = true;
                    _db.Entry(removeBillService).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            if (serviceList.Count > 0)
            {
                foreach(var service in serviceList)
                {
                    var model = new Billing()
                    {
                        CustomerType = vmodel.CustomerType,
                        CustomerUniqueID = vmodel.CustomerType == CustomerType.REGISTERED_CUSTOMER ? _db.Billings.FirstOrDefault(x => x.InvoiceNumber == vmodel.InvoiceNumber).CustomerUniqueID : null,
                        CustomerName = vmodel.CustomerName,
                        CustomerGender = vmodel.CustomerGender,
                        CustomerAge = vmodel.CustomerAge,
                        CustomerPhoneNumber =  vmodel.CustomerPhoneNumber,
                        IsDeleted = false,
                        InvoiceNumber = vmodel.InvoiceNumber,
                        DateCreated = DateTime.Now,
                        ServiceID = service.ServiceID,
                        GrossAmount = service.GrossAmount,
                        Quantity = service.Quantity,
                    };
                    _db.Billings.Add(model);
                }
            }
            _db.SaveChanges();

            return true;
        }
        public BillingVM GetCustomerForBill(string invoiceNumber)
        {
            var model = _db.Billings.Where(x => x.InvoiceNumber == invoiceNumber && x.IsDeleted == false).Select(b => new BillingVM()
            {
                CustomerName = b.CustomerName == null ?_db.Customers.FirstOrDefault(x=>x.CustomerUniqueID == b.CustomerUniqueID).Firstname + " "+ _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).Lastname : b.CustomerName,
                CustomerGender = b.CustomerGender == null ? _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).Gender : b.CustomerGender,
                CustomerPhoneNumber = b.CustomerPhoneNumber == null ? _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).PhoneNumber : b.CustomerPhoneNumber,
                CustomerAge = b.CustomerAge == null ? DateTime.Now.Year - _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).DOB.Year : b.CustomerAge,
            }).FirstOrDefault();
            return model;
        }
        public List<BillingVM> GetBillServices(string invoiceNumber)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var model = _db.Billings.Where(x => x.InvoiceNumber == invoiceNumber && x.IsDeleted == false).Select(b => new BillingVM()
            {
                Id = b.ServiceID,
                ServiceName = b.Service.Description,
                GrossAmount = b.GrossAmount,
                Quantity = b.Quantity,
                SellingPrice = b.Service.SellingPrice
            }).ToList();
            foreach (var each in model)
            {
                each.SellingPriceString = "₦" + each.SellingPrice.ToString("N", nfi);
            }
            return model;
        }
    }
}