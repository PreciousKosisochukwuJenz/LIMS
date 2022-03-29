using JenzHealth.Areas.Admin.ViewModels;
using JenzHealth.Areas.Admin.ViewModels.Report;
using JenzHealth.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.Areas.Admin.Interfaces
{
    public interface IPaymentService
    {
        string CreateBilling(BillingVM vmodel, List<ServiceListVM> serviceList);
        BillingVM GetCustomerForBill(string invoiceNumber);
        List<BillingVM> GetCustomerForReport(string invoiceNumber);
        List<BillingVM> GetBillServices(string invoiceNumber);
        List<BillDetailsVM> GetBillingDetails(string billnumber);
        string UpdateBilling(BillingVM vmodel, List<ServiceListVM> serviceList);
        bool WaiveAmountForCustomer(WaiverVM vmodel);
        List<PartPaymentVM> GetPartPayments(string BillInvoiceNumber);
        bool MapPartPayment(List<PartPaymentVM> vmodel);
        bool Deposite(DepositeCollectionVM vmodel);
        CashCollectionVM CashCollection(CashCollectionVM vmodel, List<ServiceListVM> serviceList);
        Waiver GetWaivedAmountForBillInvoiceNumber(string billInvoiceNumber);
        TransactionVM GetTransactionReports(TransactionVM vmodel);
        TransactionVM GetShiftTransactionDetails(int shiftID);
        List<CashCollectionVM> GetCashCollectionsForShift(int shiftID);
        string CalcualateTotalAmount(int shiftID, PaymentType paymentType);
        bool Refund(RefundVM vmodel);
        string GetBillNumberWithReceipt(string receipt);
        void CancelReciept(CashCollectionVM vmodel);
        List<CashCollectionVM> GetPaymentDetails(string recieptnumber);
        decimal GetTotalPaidBillAmount(string billnumber);
    }
}
