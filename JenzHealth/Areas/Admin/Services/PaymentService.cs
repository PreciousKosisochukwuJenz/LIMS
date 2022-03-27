using JenzHealth.Areas.Admin.Helpers;
using JenzHealth.Areas.Admin.Interfaces;
using JenzHealth.Areas.Admin.ViewModels;
using JenzHealth.Areas.Admin.ViewModels.Report;
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
        IUserService _userService;
        public PaymentService()
        {
            _db = new DatabaseEntities();
            _userService = new UserService(new DatabaseEntities());
        }
        public PaymentService(DatabaseEntities db, UserService userService)
        {
            _db = db;
            _userService = userService;
        }
        public string CreateBilling(BillingVM vmodel, List<ServiceListVM> serviceList)
        {
            var billCount = _db.ApplicationSettings.FirstOrDefault().BillCount;
            billCount++;
            var invoiceNumber = string.Format("BILL/{0}", billCount.ToString("D6"));

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
                    BilledByID = Global.AuthenticatedUserID
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
            var ServiceBills = _db.Billings.Where(x => x.InvoiceNumber == vmodel.InvoiceNumber && x.IsDeleted == false).ToList();
            if (ServiceBills.Count > 0)
            {
                foreach (var service in ServiceBills)
                {
                    service.IsDeleted = true;
                    _db.Entry(service).State = System.Data.Entity.EntityState.Modified;
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
                        BilledByID = Global.AuthenticatedUserID,
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
                CustomerAge = b.CustomerAge == 0 ? DateTime.Now.Year - _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).DOB.Year : b.CustomerAge,
                InvoiceNumber = b.InvoiceNumber
            }).FirstOrDefault();
            return model;
        }
        public List<BillingVM> GetCustomerForReport(string invoiceNumber)
        {
            var model = _db.Billings.Where(x => x.InvoiceNumber == invoiceNumber && x.IsDeleted == false).Select(b => new BillingVM()
            {
                CustomerName = b.CustomerName == null ? _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).Firstname + " " + _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).Lastname : b.CustomerName,
                CustomerGender = b.CustomerGender == null ? _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).Gender : b.CustomerGender,
                CustomerPhoneNumber = b.CustomerPhoneNumber == null ? _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).PhoneNumber : b.CustomerPhoneNumber,
                CustomerAge = b.CustomerAge == 0 ? DateTime.Now.Year - _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == b.CustomerUniqueID).DOB.Year : b.CustomerAge,
                CustomerUniqueID = b.CustomerUniqueID,
                InvoiceNumber = b.InvoiceNumber.ToUpper()
            }).ToList();
            return model;
        }
        public List<BillingVM> GetBillServices(string invoiceNumber)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var model = _db.Billings.Where(x => x.InvoiceNumber == invoiceNumber && x.IsDeleted == false).Select(b => new BillingVM()
            {
                Id = b.ServiceID,
                ServiceName = b.Service.Description,
                Quantity = b.Quantity,
                SellingPrice = b.Service.SellingPrice,
            }).ToList();
            foreach (var each in model)
            {
                each.GrossAmount = (each.Quantity * each.SellingPrice);
                each.SellingPriceString = "₦" + each.SellingPrice.ToString("N", nfi);
            }
            return model;
        }

        public List<BillDetailsVM> GetBillingDetails(string billnumber)
        {
            var billList = new List<BillDetailsVM>();

            var bill = _db.Billings.Where(x => x.IsDeleted == false && x.InvoiceNumber == billnumber).Select(b => new BillDetailsVM()
            {
                BilledBy = b.BilledBy.Firstname + " "+ b.BilledBy.Lastname,
                BillInvoiceNumber = b.InvoiceNumber.ToUpper(),
            }).FirstOrDefault();
            var waived = GetWaivedAmountForBillInvoiceNumber(billnumber);
            decimal waivedAmount = 0;
            if (waived != null)
                waivedAmount = waived.WaiveAmount;
        
            bill.WaivedAmount = waivedAmount;
            bill.NetAmount = GetBillServices(billnumber).Sum(x => x.GrossAmount);
            bill.BalanceAmount = (bill.NetAmount - bill.WaivedAmount);

            billList.Add(bill);

            return billList;
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
                model.WaivedByID = Global.AuthenticatedUserID;

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
                    WaivedByID = Global.AuthenticatedUserID
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
                IsPaidPartPayment = b.IsPaidPartPayment,
            }).ToList();
            foreach(var partpayment in model)
            {
                partpayment.HasPaid = this.HasPaidInstallment(partpayment.Id);
            }
            return model;
        }
        public bool HasPaidInstallment(int installmentID)
        {
            var paidinstallment = _db.CashCollections.Count(x => x.PartPaymentID == installmentID);
            if (paidinstallment > 0)
                return true;
            else
                return false;
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
                        DateCreated = DateTime.Now,
                        CreatedByID = Global.AuthenticatedUserID
                    };
                    _db.PartPayments.Add(model);
                }
                _db.SaveChanges();
            }
            return true;
        }
        public bool Deposite(DepositeCollectionVM vmodel)
        {
            var depositeCount = _db.ApplicationSettings.FirstOrDefault().DepositeCount;
            depositeCount++;
            var model = new DepositeCollection()
            {
                Amount = CustomSerializer.UnMaskString(vmodel.AmountString),
                CustomerUniqueID = vmodel.CustomerUniqueID,
                CustomerID = _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == vmodel.CustomerUniqueID).Id,
                Description = vmodel.Description,
                PaymentType = vmodel.PaymentType,
                ReferenceNumber = vmodel.ReferenceNumber,
                DepositeReciept = String.Format("TR/{0}", depositeCount.ToString("D6")),
                IsDeleted = false,
                DateCreated = DateTime.Now,
                DepositedByID = Global.AuthenticatedUserID
            };
            _db.DepositeCollections.Add(model);

            var updatesettings = _db.ApplicationSettings.FirstOrDefault();
            updatesettings.DepositeCount = depositeCount;

            _db.Entry(updatesettings).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();

            return true;
        }
        public bool CashCollection(CashCollectionVM vmodel, List<ServiceListVM> serviceList)
        {
            var shift = _userService.GetShift();
            var paymentCount = _db.ApplicationSettings.FirstOrDefault().PaymentCount;
            paymentCount++;
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
                        PaymentType = vmodel.PaymentType,
                        ShiftID = shift.Id,
                        PaymentReciept = String.Format("TR/{0}", paymentCount.ToString("D6")),
                        CollectedByID = Global.AuthenticatedUserID
                    };
                    _db.CashCollections.Add(billCashCollection);
                    break;
                case CollectionType.UNBILLED:
                    // Create bill for registered customer
                    var customer = _db.Customers.FirstOrDefault(x => x.CustomerUniqueID == vmodel.CustomerUniqueID && x.IsDeleted == false);
                    var registeredCustomerBill = new BillingVM()
                    {
                        CustomerUniqueID = vmodel.CustomerUniqueID,
                        CustomerName = string.Format("{0} {1}", customer.Firstname, customer.Lastname),
                        CustomerGender = customer.Gender,
                        CustomerAge = (DateTime.Now.Year - customer.DOB.Year),
                        CustomerPhoneNumber = customer.PhoneNumber,
                        CustomerType = CustomerType.REGISTERED_CUSTOMER,
                        CustomerID = customer.Id,
                    };
                    if (serviceList.Count > 0)
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
                            ShiftID = shift.Id,
                            PaymentReciept = String.Format("TR/{0}", paymentCount.ToString("D6")),
                            CollectedByID = Global.AuthenticatedUserID
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
                            ShiftID = shift.Id,
                            PaymentReciept = String.Format("TR/{0}", paymentCount.ToString("D6")),
                            CollectedByID = Global.AuthenticatedUserID
                        };

                        _db.CashCollections.Add(walkinCashCollection);
                    }
                    break;
            }
            var updatesettings = _db.ApplicationSettings.FirstOrDefault();
            updatesettings.PaymentCount = paymentCount;

            _db.Entry(updatesettings).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            return true;
        }
        public TransactionVM GetTransactionReports(TransactionVM vmodel)
        {
            var model = new TransactionVM();
            var summaries = _db.Shifts.Where(x => x.User.Username == vmodel.ShiftUniqeID || x.ShiftUniqueID == vmodel.ShiftUniqeID || (x.DateTimeCreated >= vmodel.ShiftStarts && x.ExpiresDateTime <= vmodel.ShiftEnds)).Select(b => new TransactionVM()
            {
                Id = b.Id,
                ShiftUniqeID = b.ShiftUniqueID,
                Username = b.User.Username,
                Name = b.User.Firstname + " " + b.User.Lastname,
                PhoneNumber = b.User.PhoneNumber,
                Email = b.User.Email,
                ShiftStarts = b.DateTimeCreated,
                ShiftEnds = b.ExpiresDateTime,
                ShiftStatus = b.HasExpired ? "Closed" : "Open",
                ShiftClosedBy = b.HasExpired ? b.ClosedBy : " - ",
                TransactionCount = _db.CashCollections.Count(x => x.Shift.Id == b.Id),
                TotalAmount = _db.CashCollections.Where(x => x.ShiftID == b.Id).ToList().Sum(x => x.AmountPaid),
            }).OrderByDescending(x => x.Id).ToList();

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

            foreach (var summary in summaries)
            {
                summary.TotalAmountString = "₦" + summary.TotalAmount.ToString("N", nfi);
            }

            model.TableData = summaries;

            return model;
        }
        public TransactionVM GetShiftTransactionDetails(int shiftID)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var model = new TransactionVM();
            var shift = _db.Shifts.Where(x => x.Id == shiftID).Select(b => new TransactionVM()
            {
                Id = b.Id,
                ShiftUniqeID = b.ShiftUniqueID,
                Username = b.User.Username,
                Name = b.User.Firstname + " " + b.User.Lastname,
                PhoneNumber = b.User.PhoneNumber,
                Email = b.User.Email,
                ShiftStarts = b.DateTimeCreated,
                ShiftEnds = b.ExpiresDateTime,
                ShiftStatus = b.HasExpired ? "Closed" : "Open",
                ShiftClosedBy = b.HasExpired ? b.ClosedBy : " - ",
                TransactionCount = _db.CashCollections.Count(x => x.Shift.Id == b.Id),
                TotalAmount = _db.CashCollections.Where(x => x.ShiftID == b.Id).ToList().Sum(x => x.AmountPaid),
            }).FirstOrDefault();
            shift.CashAmountString = this.CalcualateTotalAmount(shiftID, PaymentType.CASH);
            shift.POSAmountString = this.CalcualateTotalAmount(shiftID, PaymentType.POS);
            shift.EFTAmountString = this.CalcualateTotalAmount(shiftID, PaymentType.EFT);
            shift.TotalAmountString = "₦" + shift.TotalAmount.ToString("N", nfi);

            return shift;
        }
        public List<CashCollectionVM> GetCashCollectionsForShift(int shiftID)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var cashcollections = _db.CashCollections.Where(x => x.ShiftID == shiftID).Select(b => new CashCollectionVM()
            {
                Id = b.Id,
                AmountPaid = b.AmountPaid,
                PaymentReciept = b.PaymentReciept,
                InstallmentType = b.InstallmentType,
                TransactionReferenceNumber = b.TransactionReferenceNumber == null ? " - " : b.TransactionReferenceNumber,
                PaymentType = (PaymentType)b.PaymentType,
                BillInvoiceNumber = b.BillInvoiceNumber,
                CustomerName = _db.Billings.FirstOrDefault(x => x.InvoiceNumber == b.BillInvoiceNumber).CustomerName,
                CustomerGender = _db.Billings.FirstOrDefault(x => x.InvoiceNumber == b.BillInvoiceNumber).CustomerGender,
                CustomerUniqueID = _db.Billings.FirstOrDefault(x => x.InvoiceNumber == b.BillInvoiceNumber).CustomerUniqueID == null ? " - " : _db.Billings.FirstOrDefault(x => x.InvoiceNumber == b.BillInvoiceNumber).CustomerUniqueID,
                CustomerPhoneNumber = _db.Billings.FirstOrDefault(x => x.InvoiceNumber == b.BillInvoiceNumber).CustomerPhoneNumber,
                CustomerType = ((CustomerType)_db.Billings.FirstOrDefault(x => x.InvoiceNumber == b.BillInvoiceNumber).CustomerType).ToString(),
            }).ToList();
            foreach (var cashcollection in cashcollections)
            {
                cashcollection.AmountPaidString = "₦" + cashcollection.AmountPaid.ToString("N", nfi);
            }
            return cashcollections;
        }
        public string CalcualateTotalAmount(int shiftID, PaymentType paymentType)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            decimal total = 0;
            var cashcollections = _db.CashCollections.Where(x => x.ShiftID == shiftID && x.PaymentType == paymentType).ToList();
            if (cashcollections.Count > 0)
            {
                total = cashcollections.Sum(x => x.AmountPaid);
            }
            else
            {
                total = 0;
            }
            return "₦" + total.ToString("N", nfi);
        }

        public string GetBillNumberWithReceipt(string receipt)
        {
            var model = _db.CashCollections.Where(x => x.PaymentReciept == receipt).Select(b => b.BillInvoiceNumber).FirstOrDefault();
            return model;
        }
        public bool Refund(RefundVM vmodel)
        {
            var model = new Refund()
            {
                PaymentReciept = vmodel.PaymentReciept,
                DepositeReciept = vmodel.DepositeReciept,
                AmountToRefund = vmodel.AmountToRefund,
                Comment = vmodel.Comment,
                DateCreated = DateTime.Now,
                IsDeletetd = false
            };
            _db.Refunds.Add(model);
            _db.SaveChanges();

            return true;
        }
        public void CancelReciept(CashCollectionVM vmodel)
        {
            var cashcollection = _db.CashCollections.FirstOrDefault(x => x.PaymentReciept == vmodel.PaymentReciept);
            cashcollection.IsCancelled = true;
            cashcollection.Comment = vmodel.Comment;
            cashcollection.CanceledByID = Global.AuthenticatedUserID;

            _db.Entry(cashcollection).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
        }
        public decimal GetTotalPaidBillAmount(string billnumber)
        {
            decimal totalbillamount = 0;
            var billamount = _db.CashCollections.Where(x => x.BillInvoiceNumber == billnumber);
            if (billamount.Count() > 0)
            {
                totalbillamount = billamount.Sum(b => b.AmountPaid);
            }
            else
            {
                totalbillamount = 0;
            }

            return totalbillamount;
        }
    }

}