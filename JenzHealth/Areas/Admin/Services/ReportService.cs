using JenzHealth.Areas.Admin.Interfaces;
using JenzHealth.Areas.Admin.ViewModels.Report;
using JenzHealth.DAL.DataConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.Services
{
    public class ReportService : IReportService
    {
        private readonly DatabaseEntities _db;
        readonly ISeedService _seedService;
        readonly IPaymentService _paymentService;
        readonly ILaboratoryService _laboratoryService;
        public ReportService()
        {
            _db = new DatabaseEntities();
            _seedService = new SeedService();
            _paymentService = new PaymentService();
            _laboratoryService = new LaboratoryService();
        }

        public ReportService(DatabaseEntities db)
        {
            _db = db;
        }

        List<RequestTrackerVM> TrackRequest(RequestTrackerVM vmodel)
        {
            List<RequestTrackerVM> trackedRequest = new List<RequestTrackerVM>();
            var bills = _db.Billings.Where(x => x.InvoiceNumber == vmodel.BillNumber || (x.DateCreated >= vmodel.StartDate && x.DateCreated <= vmodel.EndDate) && x.IsDeleted == false).ToList();
            foreach(var bill in bills)
            {
                var request = new RequestTrackerVM()
                {
                    BillNumber = bill.InvoiceNumber,
                    SampleCollected = _laboratoryService.GetSpecimenCollected(bill.InvoiceNumber) != null ? true : false,
                    HasCompletedPayment = _paymentService.CheckIfPaymentIsCompleted(bill.InvoiceNumber),
                };
            }
            return trackedRequest;
        }
    }
}