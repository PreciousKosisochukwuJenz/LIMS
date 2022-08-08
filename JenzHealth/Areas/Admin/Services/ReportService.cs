using JenzHealth.Areas.Admin.Interfaces;
using JenzHealth.Areas.Admin.ViewModels.Report;
using JenzHealth.DAL.DataConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using PowerfulExtensions.Linq;
using System.Web;
using JenzHealth.DAL.Entity;

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

        public ReportService(DatabaseEntities db, PaymentService paymentService, LaboratoryService laboratoryService, SeedService seedService)
        {
            _db = db;
            _seedService = seedService;
            _paymentService = paymentService;
            _laboratoryService = laboratoryService;
        }

        public List<RequestTrackerVM> TrackRequest(RequestTrackerVM vmodel)
        {
            List<RequestTrackerVM> trackedRequest = new List<RequestTrackerVM>();
            List<Billing> bills = new List<Billing>();
            if (vmodel != null)
            {
                 bills = _db.Billings.Where(x => x.InvoiceNumber == vmodel.BillNumber && x.IsDeleted == false || (x.DateCreated >= vmodel.StartDate && x.DateCreated <= vmodel.EndDate)).Take(50).OrderByDescending(x => x.DateCreated).ToList();
            }
            else
            {
                bills = _db.Billings.Where(x =>x.DateCreated <= DateTime.Now).Take(50).OrderByDescending(x=>x.DateCreated).ToList();
            }

            foreach (var bill in bills.Distinct(x => x.InvoiceNumber))
            {
                var specimenCollected = _laboratoryService.GetSpecimenCollected(bill.InvoiceNumber);
                var request = new RequestTrackerVM()
                {
                    BillNumber = bill.InvoiceNumber,
                    PatientName = bill.CustomerName,
                    SampleCollected = specimenCollected != null ? true : false,
                    HasCompletedPayment = _paymentService.CheckIfPaymentIsCompleted(bill.InvoiceNumber),
                    SampleCollectedBy = specimenCollected != null ? specimenCollected.CollectedBy : "",
                    SampleCollectedOn = specimenCollected != null ? specimenCollected.DateTimeCreated : new DateTime(),
                };
                trackedRequest.Add(request);
            }
            return trackedRequest;
        }
    }
}