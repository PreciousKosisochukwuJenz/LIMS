using JenzHealth.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.Areas.Admin.Interfaces
{
    public interface IPaymentService
    {
        bool CreateBilling(BillingVM vmodel, List<ServiceListVM> serviceList);
        BillingVM GetCustomerForBill(string invoiceNumber);
        List<BillingVM> GetBillServices(string invoiceNumber);
        bool UpdateBilling(BillingVM vmodel, List<ServiceListVM> serviceList);
    }
}
