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
        public string CreateBilling(BillingVM vmodel, List<ServiceListVM> serviceList)
        {
            var billCount = _db.ApplicationSettings.FirstOrDefault().BillCount;
            billCount++;
            var invoiceNumber = string.Format("BILL/{0}",billCount.ToString("D6"));

            foreach (var service in serviceList)
            {
                var model = new Billing()
                {
                    CustomerType = vmodel.CustomerType,
                    CustomerUniqueID = vmodel.CustomerType == CustomerType.REGISTERED_CUSTOMER ? vmodel.CustomerUniqueID : null,
                    CustomerName = vmodel.CustomerName,
                    CustomerGender = vmodel.CustomerGender,
                    CustomerAge = vmodel.CustomerAge,
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
            var updateSettings = _db.ApplicationSettings.FirstOrDefault();
            updateSettings.BillCount = billCount;
            _db.Entry(updateSettings).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();

            return invoiceNumber;
        }
        public string UpdateBilling(BillingVM vmodel, List<ServiceListVM> serviceList)
        {
            var ServiceBills = _db.Billings.Where(x => x.InvoiceNumber == vmodel.InvoiceNumber && x.IsDeleted == false).Select(b => b.ServiceID).ToList();
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
                foreach (var service in serviceList)
                {
                    var model = new Billing()
                    {
                        CustomerType = vmodel.CustomerType,
                        CustomerUniqueID = vmodel.CustomerType == CustomerType.REGISTERED_CUSTOMER ? _db.Billings.FirstOrDefault(x => x.InvoiceNumber == vmodel.InvoiceNumber).CustomerUniqueID : null,
                        CustomerName = vmodel.CustomerName,
                        CustomerGender = vmodel.CustomerGender,
                        CustomerAge = vmodel.CustomerAge,
                        CustomerPhoneNumber = vmodel.CustomerPhoneNumber,
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

            return vmodel.InvoiceNumber;
        }
        public BillingVM GetCustomerForBill(string invoiceNumber)
        {
            var model = _db.Billings.Where(x => x.InvoiceNumber == invoiceNumber && x.IsDeleted == false).Select(b => new BillingVM()
            {
                CustomerName = b.CustomerName == null ? _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).Firstname + " " + _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).Lastname : b.CustomerName,
                CustomerGender = b.CustomerGender == null ? _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).Gender : b.CustomerGender,
                CustomerPhoneNumber = b.CustomerPhoneNumber == null ? _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).PhoneNumber : b.CustomerPhoneNumber,
                CustomerAge = b.CustomerAge == null ? DateTime.Now.Year - _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).DOB.Year : b.CustomerAge,
                InvoiceNumber = b.InvoiceNumber
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
        public bool WaiveAmountForCustomer(WaiverVM vmodel)
        {
            var checkIfWaivedBefore = _db.Waivers.Count(x => x.BillInvoiceNumber == vmodel.BillInvoiceNumber && x.IsDeleted == false);
            if (checkIfWaivedBefore > 0)
            {
                var model = _db.Waivers.FirstOrDefault(x => x.BillInvoiceNumber == vmodel.BillInvoiceNumber && x.IsDeleted == false);
                model.AvailableAmount = vmodel.AvailableAmount;
                model.WaiveAmount = vmodel.WaiveAmount;
                model.NetAmount = vmodel.NetAmount;
                model.WaiveBy = vmodel.WaiveBy;

                _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            }
            else
            {
                var model = new Waiver()
                {
                    BillInvoiceNumber = vmodel.BillInvoiceNumber,
                    AvailableAmount = vmodel.AvailableAmount,
                    NetAmount = vmodel.NetAmount,
                    WaiveAmount = vmodel.WaiveAmount,
                    WaiveBy = vmodel.WaiveBy,
                    IsDeleted = false,
                    DateCreated = DateTime.Now,
                };
                _db.Waivers.Add(model);
            }
            _db.SaveChanges();

            return true;
        }
        public Waiver GetWaivedAmountForBillInvoiceNumber(string billInvoiceNumber)
        {
            return _db.Waivers.FirstOrDefault(x => x.BillInvoiceNumber == billInvoiceNumber && x.IsDeleted == false);
        }
        public List<PartPaymentVM> GetPartPayments(string BillInvoiceNumber)
        {
            var model = _db.PartPayments.Where(x => x.BillInvoiceNumber == BillInvoiceNumber && x.IsDeleted == false).Select(b => new PartPaymentVM()
            {
                Id = b.Id,
                InstallmentName = b.InstallmentName,
                PartPaymentAmount = b.PartPaymentAmount,
                IsPaidPartPayment = b.IsPaidPartPayment
            }).ToList();
            return model;
        }
        public bool MapPartPayment(List<PartPaymentVM> vmodel)
        {
            if (vmodel != null)
            {
                var billInvoiceNumber = vmodel.FirstOrDefault().BillInvoiceNumber;
                // Update installment
                var installments = _db.PartPayments.Where(x => x.BillInvoiceNumber == billInvoiceNumber && x.IsDeleted == false).ToList();
                foreach (var installment in installments)
                {
                    installment.IsDeleted = true;
                    _db.Entry(installment).State = System.Data.Entity.EntityState.Modified;
                }
                _db.SaveChanges();

                foreach (var installment in vmodel)
                {
                    // Add installment
                    var model = new PartPayment()
                    {
                        BillInvoiceNumber = installment.BillInvoiceNumber,
                        InstallmentName = installment.InstallmentName,
                        PartPaymentAmount = installment.PartPaymentAmount,
                        IsPaidPartPayment = false,
                        IsDeleted = false,
                        DateCreated = DateTime.Now
                    };
                    _db.PartPayments.Add(model);
                }
                _db.SaveChanges();
            }
            return true;
        }
        public bool Deposite(DepositeCollectionVM vmodel)
        {
            var model = new DepositeCollection()
            {
                Amount = CustomSerializer.UnMaskString(vmodel.AmountString),
                CustomerUniqueID = vmodel.CustomerUniqueID,
                CustomerID = _db.Customers.FirstOrDefault(x=>x.CustomerUniqueID == vmodel.CustomerUniqueID).Id,
                Description = vmodel.Description,
                PaymentType = vmodel.PaymentType,
                ReferenceNumber = vmodel.ReferenceNumber,
                IsDeleted = false,
                DateCreated = DateTime.Now
            };
            _db.DepositeCollections.Add(model);
            _db.SaveChanges();

            return true;
        }
        public bool CashCollection(CashCollectionVM vmodel, List<ServiceListVM> serviceList)
        {
            switch (vmodel.CollectionType)
            {
                case CollectionType.BILLED:

                    var billCashCollection = new CashCollection()
                    {
                        BillInvoiceNumber = vmodel.BillInvoiceNumber,
                        AmountPaid = vmodel.PartPaymentID == null ? vmodel.NetAmount : _db.PartPayments.FirstOrDefault(x => x.Id == vmodel.PartPaymentID).PartPaymentAmount,
                        NetAmount = vmodel.NetAmount,
                        DatePaid = DateTime.Now,
                        WaivedAmount = vmodel.WaivedAmount,
                        BalanceAmount = vmodel.NetAmount - vmodel.WaivedAmount,
                        IsDeleted = false,
                        TransactionReferenceNumber = vmodel.TransactionReferenceNumber,
                        InstallmentType = vmodel.PartPaymentID == null ? "FULL" : "PART",
                        PartPaymentID = vmodel.PartPaymentID,
                        PaymentType = vmodel.PaymentType
                    };
                    _db.CashCollections.Add(billCashCollection);
                    break;
                case CollectionType.UNBILLED:
                    // Create bill for registered customer
                    var customer = _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == vmodel.CustomerUniqueID && x.IsDeleted == false);
                    var registeredCustomerBill = new BillingVM()
                    {
                        CustomerUniqueID = vmodel.CustomerUniqueID,
                        CustomerName = string.Format("{0} {1}",customer.Firstname, customer.Lastname),
                        CustomerGender = customer.Gender,
                        CustomerAge = (DateTime.Now.Year - customer.DOB.Year),
                        CustomerPhoneNumber = customer.PhoneNumber,
                        CustomerType = CustomerType.REGISTERED_CUSTOMER,
                        CustomerID = customer.Id,
                    };
                    if(serviceList.Count > 0)
                    {
                        var billInvoiceNumber = this.CreateBilling(registeredCustomerBill, serviceList);

                        var unbilledCashCollection = new CashCollection()
                        {
                            BillInvoiceNumber = billInvoiceNumber,
                            TransactionReferenceNumber = vmodel.TransactionReferenceNumber,
                            DatePaid = DateTime.Now,
                            IsDeleted = false,
                            AmountPaid = vmodel.AmountPaid,
                            WaivedAmount = vmodel.WaivedAmount,
                            BalanceAmount = vmodel.NetAmount - vmodel.WaivedAmount,
                            NetAmount = vmodel.NetAmount,
                            PaymentType = vmodel.PaymentType,
                            PartPaymentID = vmodel.PartPaymentID,
                            InstallmentType = "FULL",
                        };

                        _db.CashCollections.Add(unbilledCashCollection);
                    }
                    break;
                case CollectionType.WALK_IN:
                    // Create bill for walk-in customer
                    var walkinBill = new BillingVM()
                    {
                        CustomerName = vmodel.CustomerName,
                        CustomerGender = vmodel.CustomerGender,
                        CustomerAge = vmodel.CustomerAge,
                        CustomerPhoneNumber = vmodel.CustomerPhoneNumber,
                        CustomerType = CustomerType.WALK_IN_CUSTOMER,
                    };
                    if (serviceList.Count > 0)
                    {
                        var billInvoiceNumber = this.CreateBilling(walkinBill, serviceList);

                        var walkinCashCollection = new CashCollection()
                        {
                            BillInvoiceNumber = billInvoiceNumber,
                            TransactionReferenceNumber = vmodel.TransactionReferenceNumber,
                            DatePaid = DateTime.Now,
                            IsDeleted = false,
                            AmountPaid = vmodel.AmountPaid,
                            NetAmount = vmodel.NetAmount,
                            PaymentType = vmodel.PaymentType,
                            BalanceAmount = vmodel.NetAmount - vmodel.WaivedAmount,
                            PartPaymentID = vmodel.PartPaymentID,
                            InstallmentType = "FULL",
                        };

                        _db.CashCollections.Add(walkinCashCollection);
                    }
                    break;
            }
            _db.SaveChanges();
            return true;
        }
    }

}