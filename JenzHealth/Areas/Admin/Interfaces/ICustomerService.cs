using JenzHealth.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.Areas.Admin.Interfaces
{
    internal interface ICustomerService
    {
        List<CustomerVM> GetCustomers();
        object CreateCustomer(CustomerVM vmodel);
        CustomerVM GetCustomer(int ID);
        bool EditCustomer(CustomerVM vmodel);
        bool DeleteCustomer(int ID);
    }
}
