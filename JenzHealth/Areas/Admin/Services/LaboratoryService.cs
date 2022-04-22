using JenzHealth.Areas.Admin.Interfaces;
using JenzHealth.Areas.Admin.ViewModels;
using JenzHealth.DAL.DataConnection;
using JenzHealth.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PowerfulExtensions.Linq;
namespace JenzHealth.Areas.Admin.Services
{
    public class LaboratoryService : ILaboratoryService
    {
        readonly DatabaseEntities _db;
        readonly ISeedService _seedService;
        readonly IPaymentService _paymentService;
        public LaboratoryService()
        {
            _db = new DatabaseEntities();
            _seedService = new SeedService();
            _paymentService = new PaymentService();
        }
        public LaboratoryService(DatabaseEntities db)
        {
            _db = db;
            _seedService = new SeedService(db);
            _paymentService = new PaymentService(db, new UserService());
        }

        public ServiceParameterVM GetServiceParameter(string ServiceName)
        {
            var model = _db.ServiceParameters.Where(x => x.Service.Description == ServiceName).Select(b => new ServiceParameterVM()
            {
                Id = b.Id,
                ServiceID = b.ServiceID,
                Service = b.Service.Description,
                SpecimenID = b.SpecimenID,
                Specimen = b.Specimen.Name,
                TemplateID = b.TemplateID,
                Template = b.Template.Name,
                RequireApproval = b.RequireApproval,
            }).FirstOrDefault();

            return model;
        }

        public ServiceParameterVM GetServiceParameter(int serviceID)
        {
            var model = _db.ServiceParameters.Where(x => x.ServiceID == serviceID).Select(b => new ServiceParameterVM()
            {
                Id = b.Id,
                ServiceID = b.ServiceID,
                Service = b.Service.Description,
                SpecimenID = b.SpecimenID,
                Specimen = b.Specimen.Name,
                TemplateID = b.TemplateID,
                Template = b.Template.Name,
                RequireApproval = b.RequireApproval,
            }).FirstOrDefault();

            return model;
        }
        public void UpdateParamterSetup(ServiceParameterVM serviceParamter, List<ServiceParameterSetupVM> paramterSetups)
        {
            var ExistingServiceParamterExist = _db.ServiceParameters.Where(x => x.Service.Description == serviceParamter.Service).FirstOrDefault();
            int serviceParameterID = 0;
            if (ExistingServiceParamterExist != null)
            {
                ExistingServiceParamterExist.RequireApproval = serviceParamter.RequireApproval;
                ExistingServiceParamterExist.SpecimenID = _db.Specimens.FirstOrDefault(x => x.Name == serviceParamter.Specimen).Id;
                ExistingServiceParamterExist.TemplateID = _db.Templates.FirstOrDefault(x => x.Name == serviceParamter.Template).Id;
                _db.Entry(ExistingServiceParamterExist).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();

                serviceParameterID = ExistingServiceParamterExist.Id;
            }
            else
            {
                var serviceParameter = new ServiceParameter()
                {
                    ServiceID = _db.Services.FirstOrDefault(x => x.Description == serviceParamter.Service).Id,
                    SpecimenID = _db.Specimens.FirstOrDefault(x => x.Name == serviceParamter.Specimen).Id,
                    TemplateID = _db.Templates.FirstOrDefault(x => x.Name == serviceParamter.Template).Id,
                    RequireApproval = serviceParamter.RequireApproval,
                };
                _db.ServiceParameters.Add(serviceParameter);
                _db.SaveChanges();
                serviceParameterID = serviceParameter.Id;
            }

            if (paramterSetups != null)
            {
                var existingParameterSetups = _db.ServiceParameterSetups.Where(x => x.ServiceParameterID == serviceParameterID);
                if (existingParameterSetups.Any())
                {
                    foreach (var existingparamtersetup in existingParameterSetups)
                    {
                        existingparamtersetup.IsDeleted = true;
                        _db.Entry(existingparamtersetup).State = System.Data.Entity.EntityState.Modified;
                    }
                }

                foreach (var paramter in paramterSetups)
                {
                    var serviceparamtersetup = new ServiceParameterSetup()
                    {
                        ServiceParameterID = serviceParameterID,
                        Name = paramter.Name,
                        Rank = paramter.Rank,
                        DateCreated = DateTime.Now,
                        IsDeleted = false,
                    };
                    _db.ServiceParameterSetups.Add(serviceparamtersetup);
                }
                _db.SaveChanges();

            }
        }
        public List<ServiceParameterSetupVM> GetServiceParamterSetups(string serviceName)
        {
            var serviceparamtersetups = _db.ServiceParameterSetups.Where(x => x.IsDeleted == false && x.ServiceParameter.Service.Description == serviceName).Select(b => new ServiceParameterSetupVM()
            {
                Id = b.Id,
                Name = b.Name,
                Rank = b.Rank,
                ServiceParameterID = b.ServiceParameterID,
            }).ToList();
            return serviceparamtersetups;
        }

        public List<ServiceParameterRangeSetupVM> GetRangeSetups(string serviceName)
        {
            var rangesetups = _db.ServiceParameterRangeSetups.Where(x => x.ServiceParameterSetup.ServiceParameter.Service.Description == serviceName && x.IsDeleted == false).Select(b => new ServiceParameterRangeSetupVM()
            {
                Id = b.Id,
                Range = b.Range,
                Unit = b.Unit,
                ServiceParameterSetupID = b.ServiceParameterSetupID,
                ServiceParameterSetup = b.ServiceParameterSetup.Name
            }).ToList();
            return rangesetups;
        }
        public void UpdateParameterRangeSetup(List<ServiceParameterRangeSetupVM> rangeSetups)
        {
            var uniqueIDs = rangeSetups.Distinct(o => o.ServiceParameterSetupID).ToList();
            if (uniqueIDs.Any())
            {
                foreach (var id in uniqueIDs)
                {
                    var existingRangesSetups = _db.ServiceParameterRangeSetups.Where(x => x.ServiceParameterSetupID == id.ServiceParameterSetupID && x.IsDeleted == false);
                    foreach (var existingrangesetup in existingRangesSetups)
                    {
                        existingrangesetup.IsDeleted = true;
                        _db.Entry(existingrangesetup).State = System.Data.Entity.EntityState.Modified;
                    }
                    _db.SaveChanges();
                }
            }
            foreach (var rangesetup in rangeSetups)
            {
                var rangeSetup = new ServiceParameterRangeSetup()
                {
                    ServiceParameterSetupID = _db.ServiceParameterSetups.FirstOrDefault(x => x.IsDeleted == false && x.Id == rangesetup.ParameterID).Id,
                    Range = rangesetup.Range,
                    Unit = rangesetup.Unit,
                    IsDeleted = false,
                    DateCreated = DateTime.Now
                };
                _db.ServiceParameterRangeSetups.Add(rangeSetup);
                _db.SaveChanges();
            }
        }

        public void UpdateSpecimenSampleCollection(SpecimenCollectionVM specimenCollected, List<SpecimenCollectionCheckListVM> checklist)
        {
            int specimenCollectionID = 0;
            var sampleExist = _db.SpecimenCollections.Where(x => x.BillInvoiceNumber == specimenCollected.BillInvoiceNumber && x.IsDeleted == false).FirstOrDefault();
            if (sampleExist != null)
            {
                sampleExist.RequestingPhysician = specimenCollected.RequestingPhysician;
                sampleExist.ClinicalSummary = specimenCollected.ClinicalSummary;
                sampleExist.ProvitionalDiagnosis = specimenCollected.ProvitionalDiagnosis;
                sampleExist.OtherInformation = specimenCollected.OtherInformation;
                sampleExist.RequestingDate = specimenCollected.RequestingDate;
                sampleExist.CollectedByID = Global.AuthenticatedUserID;
                sampleExist.LabNumber = specimenCollected.LabNumber;
                _db.Entry(sampleExist).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                specimenCollectionID = sampleExist.Id;
            }
            else
            {
                var labCount = _db.ApplicationSettings.FirstOrDefault().LabCount;
                labCount++;
                //var labnumber = string.Format("LAB/{0}", labCount.ToString("D6"));

                var specimenCollection = new SpecimenCollection()
                {
                    BillInvoiceNumber = specimenCollected.BillInvoiceNumber,
                    ClinicalSummary = specimenCollected.ClinicalSummary,
                    OtherInformation = specimenCollected.OtherInformation,
                    ProvitionalDiagnosis = specimenCollected.ProvitionalDiagnosis,
                    RequestingPhysician = specimenCollected.RequestingPhysician,
                    RequestingDate = specimenCollected.RequestingDate,
                    IsDeleted = false,
                    DateTimeCreated = DateTime.Now,
                    LabNumber = specimenCollected.LabNumber,
                    CollectedByID = Global.AuthenticatedUserID
                };

                _db.SpecimenCollections.Add(specimenCollection);
                _db.SaveChanges();
                specimenCollectionID = specimenCollection.Id;
            }

            if (checklist.Count() > 0)
            {
                foreach (var sample in checklist)
                {
                    var checkSample = _db.SpecimenCollectionCheckLists.FirstOrDefault(x => x.Specimen.Name == sample.Specimen && x.SpecimenCollectionID == specimenCollectionID && x.IsDeleted == false);
                    if (checkSample != null)
                    {
                        checkSample.IsCollected = sample.IsCollected;
                        _db.Entry(checkSample).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();
                    }
                    else
                    {
                        var newSample = new SpecimenCollectionCheckList()
                        {
                            SpecimenCollectionID = specimenCollectionID,
                            SpecimenID = _db.Specimens.FirstOrDefault(x => x.Name == sample.Specimen).Id,
                            ServiceID = _db.Services.FirstOrDefault(x => x.Description == sample.Service).Id,
                            IsCollected = sample.IsCollected,
                            IsDeleted = false,
                            DateTimeCreated = DateTime.Now,
                        };

                        _db.SpecimenCollectionCheckLists.Add(newSample);
                        _db.SaveChanges();
                    }
                }
            }
        }
        public List<ServiceParameterVM> GetServiceParameters(string invoiceNumber)
        {
            var model = _db.Billings.Where(x => x.InvoiceNumber == invoiceNumber && x.IsDeleted == false).Select(b => new ServiceParameterVM()
            {
                Id = b.Id,
                ServiceID = b.ServiceID,
                Service = b.Service.Description,
            }).ToList();
            foreach (var service in model)
            {
                service.Specimen = this.GetSpecimen((int)service.ServiceID);
            }
            return model;
        }
        public string GetSpecimen(int ServiceID)
        {
            var specimen = _db.ServiceParameters.FirstOrDefault(x => x.ServiceID == ServiceID);
            if (specimen == null)
                return "-";
            else
                return _db.Specimens.FirstOrDefault(x => x.Id == specimen.SpecimenID).Name;
        }
        public bool CheckSpecimenCollectionWithBillNumber(string billnumber)
        {
            var exist = _db.SpecimenCollections.FirstOrDefault(x => x.IsDeleted == false && x.BillInvoiceNumber == billnumber);
            if (exist != null)
                return true;
            else
                return false;
        }

        public SpecimenCollectionVM GetSpecimenCollected(string billnumber)
        {
            var specimenCollected = _db.SpecimenCollections.Where(x => x.IsDeleted == false && x.BillInvoiceNumber == billnumber).Select(b => new SpecimenCollectionVM()
            {
                Id = b.Id,
                ClinicalSummary = b.ClinicalSummary,
                ProvitionalDiagnosis = b.ProvitionalDiagnosis,
                OtherInformation = b.OtherInformation,
                RequestingPhysician = b.RequestingPhysician,
                RequestingDate = b.RequestingDate,
                LabNumber = b.LabNumber
            }).FirstOrDefault();
            specimenCollected.CheckList = this.GetCheckList(specimenCollected.Id);
            return specimenCollected;
        }
        public List<SpecimenCollectionVM> GetSpecimenCollectedForReport(string billnumber, int templateID)
        {
            var specimenCollected = _db.SpecimenCollections.Where(x => x.IsDeleted == false && x.BillInvoiceNumber == billnumber).Select(b => new SpecimenCollectionVM()
            {
                Id = b.Id,
                ClinicalSummary = b.ClinicalSummary,
                ProvitionalDiagnosis = b.ProvitionalDiagnosis,
                OtherInformation = b.OtherInformation,
                RequestingPhysician = b.RequestingPhysician,
                RequestingDate = b.RequestingDate,
                LabNumber = b.LabNumber,
                CollectedBy = b.CollectedBy.Firstname + " " + b.CollectedBy.Lastname,
                DateTimeCreated = b.DateTimeCreated
            }).ToList();
            foreach (var specimenColl in specimenCollected)
            {
                var specimens = this.GetCheckList(specimenColl.Id);
                foreach (var spec in specimens)
                {
                    var parameterSetup = this.GetServiceParameter(spec.Service);
                    if (parameterSetup.TemplateID == templateID)
                    {
                        specimenColl.Specimen = parameterSetup.Specimen;
                        specimenColl.ServiceDepartment = _seedService.GetService((int)parameterSetup.ServiceID).ServiceDepartment.ToUpper();
                    }
                }
            }
            return specimenCollected;
        }

        public List<SpecimenCollectionCheckListVM> GetCheckList(int SpecimentCollectionID)
        {
            var checklist = _db.SpecimenCollectionCheckLists.Where(x => x.IsDeleted == false && x.SpecimenCollectionID == SpecimentCollectionID)
                .Select(b => new SpecimenCollectionCheckListVM()
                {
                    Id = b.Id,
                    Specimen = b.Specimen.Name,
                    IsCollected = b.IsCollected,
                    Service = b.Service.Description
                }).ToList();
            return checklist;
        }

        public List<SpecimenCollectionVM> GetLabPreparations(SpecimenCollectionVM vmodel)
        {
            var preparations = _db.SpecimenCollections.Where(x => (x.BillInvoiceNumber == vmodel.BillInvoiceNumber || x.LabNumber == x.BillInvoiceNumber) || (x.DateTimeCreated >= vmodel.StartDate && x.DateTimeCreated <= vmodel.EndDate) && x.IsDeleted == false)
                .Select(b => new SpecimenCollectionVM()
                {
                    Id = b.Id,
                    LabNumber = b.LabNumber,
                    BillInvoiceNumber = b.BillInvoiceNumber,
                    CustomerName = _db.Billings.FirstOrDefault(x => x.InvoiceNumber == b.BillInvoiceNumber).CustomerName,
                    CustomerPhoneNumber = _db.Billings.FirstOrDefault(x => x.InvoiceNumber == b.BillInvoiceNumber).CustomerPhoneNumber,
                    CustomerUniqueID = _db.Billings.FirstOrDefault(x => x.InvoiceNumber == b.BillInvoiceNumber).CustomerUniqueID,
                }).ToList();
            List<SpecimenCollectionVM> DataToRemoveFromList = new List<SpecimenCollectionVM>();
            foreach (var record in preparations)
            {
                record.HasPaidAllBill = _paymentService.CheckIfPaymentIsCompleted(record.BillInvoiceNumber);
            }
            preparations.RemoveAll(x => x.HasPaidAllBill == false);
            return preparations;
        }
        public SpecimenCollectionVM GetSpecimensForPreparation(int Id)
        {
            var model = _db.SpecimenCollections.Where(x => x.Id == Id).Select(b => new SpecimenCollectionVM()
            {
                Id = b.Id,
                LabNumber = b.LabNumber,
                BillInvoiceNumber = b.BillInvoiceNumber
            }).FirstOrDefault();
            return model;
        }
        public List<ServiceParameterVM> GetServicesToPrepare(string invoiceNumber)
        {
            var model = _db.Billings.Where(x => x.InvoiceNumber == invoiceNumber && x.IsDeleted == false).Select(b => new ServiceParameterVM()
            {
                Id = b.Id,
                ServiceID = b.ServiceID,
                Service = b.Service.Description,
                Template = _db.ServiceParameters.FirstOrDefault(x => x.ServiceID == b.ServiceID).Template.Name,
                TemplateID = _db.ServiceParameters.FirstOrDefault(x => x.ServiceID == b.ServiceID).TemplateID,
                BillNumber = b.InvoiceNumber,
                Templated = _db.ServiceParameters.FirstOrDefault(x => x.ServiceID == b.ServiceID).Template.UseDefaultParameters,
                RequireApproval = _db.ServiceParameters.FirstOrDefault(x => x.ServiceID == b.ServiceID).RequireApproval
            }).ToList();
            foreach(var item in model)
            {
                var serviceParameter = _db.ServiceParameters.FirstOrDefault(x => x.ServiceID == item.ServiceID);
                var checkApproval = _db.ResultApprovals.FirstOrDefault(x => x.ServiceParameterID == serviceParameter.Id && x.BillNumber == item.BillNumber);
                item.Approved = checkApproval != null ? checkApproval.HasApproved : false;
            }
            return model;
        }
        public bool CheckIfTestHasBeenComputed(int templateID, string billnumber)
        {
            var template = _db.Templates.FirstOrDefault(x => x.Id == templateID);
            if (!template.UseDefaultParameters)
            {
                var exist = _db.TemplatedLabPreparations.Where(x => x.BillInvoiceNumber == billnumber && x.ServiceParameterSetup.ServiceParameter.TemplateID == templateID).FirstOrDefault();
                if (exist != null)
                    return true;
            }
            else
            {
                var exist = _db.NonTemplatedLabPreparations.Where(x => x.BillInvoiceNumber == billnumber).FirstOrDefault();
                if (exist != null)
                    return true;
            }
            return false;
        }
        public List<ServiceParameterVM> GetDistinctTemplateForBilledServices(List<ServiceParameterVM> billedServices)
        {
            var distinctTemplate = billedServices.Distinct(o => o.TemplateID).ToList();
            foreach (var record in distinctTemplate)
            {
                record.HasBeenComputed = CheckIfTestHasBeenComputed((int)record.TemplateID, record.BillNumber);
            }
            return distinctTemplate;
        }

        public List<TemplateServiceCompuationVM> SetupTemplatedServiceForComputation(int TemplateID, string billNumber)
        {
            List<TemplateServiceCompuationVM> model = new List<TemplateServiceCompuationVM>();

            var billedService = this.GetServicesToPrepare(billNumber);
            var billedServiceForTemplate = billedService.Where(x => x.TemplateID == TemplateID);

            foreach (var service in billedServiceForTemplate)
            {
                TemplateServiceCompuationVM billservice = new TemplateServiceCompuationVM();
                billservice.Parameters = new List<ServiceParameterAndRange>();
                var serviceParameterID = _db.ServiceParameters.FirstOrDefault(x => x.ServiceID == service.ServiceID && x.TemplateID == TemplateID).Id;
                var parameterSetups = _db.ServiceParameterSetups.Where(x => x.ServiceParameterID == serviceParameterID && x.IsDeleted == false).Select(b => new ServiceParameterSetupVM()
                {
                    Id = b.Id,
                    Name = b.Name,
                    Rank = b.Rank,
                }).ToList();

                // Check for database record and map
                foreach (var parametersetup in parameterSetups)
                {
                    var record = this.GetParamaterValue(billNumber, parametersetup.Id);
                    var range = _db.ServiceParameterRangeSetups.FirstOrDefault(x => x.Id == record.RangeID);

                    parametersetup.Value = record.Value;
                    parametersetup.Labnote = record.Labnote;
                    parametersetup.ScientificComment = record.ScientificComment;
                    parametersetup.Range = range == null ? " " : range.Range;
                    parametersetup.Unit = range == null ? " " : range.Unit;
                }

                // Get mapped ranges
                foreach (var parameterSetup in parameterSetups)
                {
                    var parameter = new ServiceParameterAndRange();
                    var ranges = _db.ServiceParameterRangeSetups.Where(x => x.ServiceParameterSetupID == parameterSetup.Id && x.IsDeleted == false).Select(b => new ServiceParameterRangeSetupVM()
                    {
                        Id = b.Id,
                        Range = b.Range,
                        Unit = b.Unit,
                    }).ToList();
                    parameter.Parameter = parameterSetup;
                    parameter.Ranges = ranges;

                    billservice.Parameters.Add(parameter);
                }
                billservice.Service = service.Service;
                billservice.ServiceID = (int)service.ServiceID;
                model.Add(billservice);
            }
            model[0].Labnote = model[0].Parameters[0].Parameter.Labnote;
            model[0].ScienticComment = model[0].Parameters[0].Parameter.ScientificComment;

            return model;
        }
        public List<TemplateServiceCompuationVM> GetTemplatedLabResultForReport(int templateID, string billnumber)
        {
            var result = _db.TemplatedLabPreparations.Where(x => x.BillInvoiceNumber == billnumber && x.IsDeleted == false && x.ServiceParameterSetup.ServiceParameter.TemplateID == templateID)
                .Select(b => new TemplateServiceCompuationVM()
                {
                    Parameter = b.ServiceParameterSetup.Name,
                    Value = b.Value,
                    Unit = b.ServiceRange.Unit,
                    Range = b.ServiceRange.Range,
                    Service = b.ServiceParameterSetup.ServiceParameter.Service.Description,
                    ScienticComment = b.ScienticComment,
                    PreparedBy =  b.PreparedBy.Firstname + " "+ b.PreparedBy.Lastname,
                    SpecimenCollectedBy = _db.SpecimenCollections.FirstOrDefault(x=>x.BillInvoiceNumber == b.BillInvoiceNumber).CollectedBy.Firstname + " "+
                     _db.SpecimenCollections.FirstOrDefault(x => x.BillInvoiceNumber == b.BillInvoiceNumber).CollectedBy.Lastname,
                    DateCollected = _db.SpecimenCollections.FirstOrDefault(x => x.BillInvoiceNumber == b.BillInvoiceNumber).DateTimeCreated,
                    DatePrepared = b.DateCreated,
                    DateApproved = _db.ResultApprovals.FirstOrDefault(x=>x.BillNumber == b.BillInvoiceNumber && x.ServiceParameterID == b.ServiceParameterSetup.ServiceParameterID) != null ?
                    _db.ResultApprovals.FirstOrDefault(x => x.BillNumber == b.BillInvoiceNumber && x.ServiceParameterID == b.ServiceParameterSetup.ServiceParameterID).DateApproved : null,
                    ApprovedBy = _db.ResultApprovals.FirstOrDefault(x => x.BillNumber == b.BillInvoiceNumber && x.ServiceParameterID == b.ServiceParameterSetup.ServiceParameterID) != null ?
                    _db.ResultApprovals.FirstOrDefault(x => x.BillNumber == b.BillInvoiceNumber && x.ServiceParameterID == b.ServiceParameterSetup.ServiceParameterID).ApprovedBy.Firstname + " "+
                    _db.ResultApprovals.FirstOrDefault(x => x.BillNumber == b.BillInvoiceNumber && x.ServiceParameterID == b.ServiceParameterSetup.ServiceParameterID).ApprovedBy.Lastname :
                    "NIL",
                    Specimen = b.ServiceParameterSetup.ServiceParameter.Specimen.Name,
                    
                }).ToList();

            foreach (var item in result)
            {
                item.Status = GetStatusForTemplateReport(int.Parse(item.Value), item.Range);
            }
            return result;
        }
        private string GetStatusForTemplateReport(int value, string range)
        {
            int[] rangeValues = Array.ConvertAll(range.Split('-'), rangeValue => int.Parse(rangeValue));
            if (value < rangeValues[0])
                return "LOW";
            else if (value > rangeValues[1])
                return "HIGH";
            else
                return "NORMAL";
        }
        public RequestComputedResultVM GetParamaterValue(string billnumber, int SetupID)
        {
            var record = _db.TemplatedLabPreparations.Where(x => x.BillInvoiceNumber == billnumber && x.ServiceParameterSetupID == SetupID)
                .Select(b => new RequestComputedResultVM()
                {
                    Value = b.Value,
                    RangeID = (int)b.ServiceRangeID,
                    Labnote = b.Labnote,
                    ScientificComment = b.ScienticComment
                }).FirstOrDefault();
            if (record != null)
                return record;
            else
                return new RequestComputedResultVM();
        }
        public bool UpdateLabResults(List<RequestComputedResultVM> results, string labnote, string comment)
        {
            foreach (var result in results)
            {
                string service = result.Service;
                var existData = _db.TemplatedLabPreparations.Where(x => x.BillInvoiceNumber == result.BillInvoiceNumber && x.IsDeleted == false && x.ServiceParameterSetupID == result.KeyID);

                if (existData.FirstOrDefault() != null)
                {
                    var exist = existData.FirstOrDefault();
                    exist.Value = result.Value;
                    exist.Labnote = labnote;
                    exist.ScienticComment = comment;
                    exist.ServiceRangeID = result.RangeID;
                    exist.PreparedByID = Global.AuthenticatedUserID;

                    _db.Entry(exist).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                    var ID = existData.Select(b => b.ServiceParameterSetup.ServiceParameterID).FirstOrDefault();
                    int serviceParameterID = Convert.ToInt32(ID);
                }
                else
                {
                    var templateLabPreparation = new TemplatedLabPreparation()
                    {
                        BillInvoiceNumber = result.BillInvoiceNumber,
                        ServiceParameterSetupID = result.KeyID,
                        Key = result.Key,
                        Value = result.Value,
                        ServiceRangeID = result.RangeID,
                        Labnote = labnote,
                        ScienticComment = comment,
                        IsDeleted = false,
                        DateCreated = DateTime.Now,
                        PreparedByID = Global.AuthenticatedUserID
                    };
                    _db.TemplatedLabPreparations.Add(templateLabPreparation);
                    _db.SaveChanges();
                }
                // Update Result Approval 
                var serviceParameter = GetServiceParameter(service);
                if (serviceParameter.RequireApproval)
                {
                    var checkIfExist = _db.ResultApprovals.Where(x => x.ServiceParameterID == serviceParameter.Id && x.BillNumber == result.BillInvoiceNumber).FirstOrDefault();
                    if(checkIfExist == null)
                    {
                        var approvalModel = new ResultApproval()
                        {
                            ServiceParameterID = serviceParameter.Id,
                            BillNumber = result.BillInvoiceNumber,
                        };
                        _db.ResultApprovals.Add(approvalModel);
                        _db.SaveChanges();
                    }
                }
            }
            return true;
        }
        public NonTemplatedLabPreparationVM GetNonTemplatedLabPreparation(string billnumber, int serviceID)
        {
            var model = _db.NonTemplatedLabPreparations.Where(x => x.IsDeleted == false && x.BillInvoiceNumber == billnumber && x.ServiceID == serviceID).Select(b => new NonTemplatedLabPreparationVM()
            {
                Id = b.Id,
                Temperature = b.Temperature,
                SpecificGravity = b.SpecificGravity,
                Acidity = b.Acidity,
                AdultWarm = b.AdultWarm,
                Appearance = b.Appearance,
                AscorbicAcid = b.AscorbicAcid,
                Atomsphere = b.Atomsphere,
                DipstickBlood = b.DipstickBlood,
                BillInvoiceNumber = b.BillInvoiceNumber,
                Duration = b.Duration,
                Blirubin = b.Blirubin,
                Color = b.Color,
                Glucose = b.Glucose,
                Incubatio = b.Incubatio,
                Ketones = b.Ketones,
                Labnote = b.Labnote,
                LeucocyteEsterase = b.LeucocyteEsterase,
                MacrosopyBlood = b.MacrosopyBlood,
                Mucus = b.Mucus,
                Niterite = b.Niterite,
                Plate = b.Plate,
                Protein = b.Protein,
                Urobilinogen = b.Urobilinogen,
                StainType = b.StainType,
                GiemsaStainParasite = b.GiemsaStainParasite,
                AnySpillage = b.AnySpillage,
                OtherStainResult = b.OtherStainResult,
                RBCS = b.RBCS,
                TotalSpermCount = b.TotalSpermCount,
                WBCS = b.WBCS,
                AcisFastBacilli = b.AcisFastBacilli,
                Casts = b.Casts,
                Cystals = b.Cystals,
                DateCreated = b.DateCreated,
                DateOfProduction = b.DateOfProduction,
                DurationOfAbstinence = b.DurationOfAbstinence,
                EpithelialCells = b.EpithelialCells,
                GiemsaOthers = b.GiemsaOthers,
                GramNegativeCocci = b.GramNegativeCocci,
                IsDeleted = b.IsDeleted,
                GramNegativeRods = b.GramNegativeRods,
                GramPositiveCocci = b.GramPositiveCocci,
                GramPositiveRods = b.GramPositiveRods,
                ImmatureCells = b.ImmatureCells,
                IndiaInkResult = b.IndiaInkResult,
                IodineResult = b.IodineResult,
                KOHPrepareation = b.KOHPrepareation,
                KOHResult = b.KOHResult,
                MethyleneResult = b.MethyleneResult,
                MicroscopyType = b.MicroscopyType,
                ModeOfProduction = b.ModeOfProduction,
                Morphology = b.Morphology,
                Motility = b.Motility,
                OthersResult = b.OthersResult,
                Ova = b.Ova,
                PH = b.PH,
                Protozoa = b.Protozoa,
                PusCells = b.PusCells,
                RedBloodCells = b.RedBloodCells,
                TimeExamined = b.TimeExamined,
                TimeOfProduction = b.TimeOfProduction,
                TimeRecieved = b.TimeRecieved,
                TrichomonasVaginalis = b.TrichomonasVaginalis,
                VincetsOrganisms = b.VincetsOrganisms,
                Viscosity = b.Viscosity,
                WetMountBacteria = b.WetMountBacteria,
                WetMountOthers = b.WetMountOthers,
                WetMountParasite = b.WetMountParasite,
                WetMountYesats = b.WetMountYesats,
                WhiteBloodCells = b.WhiteBloodCells,
                YeastCells = b.YeastCells,
                ZiehlOthers = b.ZiehlOthers,
                Service = b.Service.Description,
                ServiceID = b.ServiceID
            }).FirstOrDefault();
            if (model == null)
            {
                model = new NonTemplatedLabPreparationVM();
                model.ServiceID = serviceID;
            }
            
            return model;
        }
        public List<NonTemplatedLabPreparationVM> GetNonTemplatedLabPreparationForReport(string billnumber)
        {
            var model = _db.NonTemplatedLabPreparations.Where(x => x.IsDeleted == false && x.BillInvoiceNumber == billnumber).Select(b => new NonTemplatedLabPreparationVM()
            {
                Id = b.Id,
                Temperature = b.Temperature,
                SpecificGravity = b.SpecificGravity,
                Acidity = b.Acidity,
                AdultWarm = b.AdultWarm,
                Appearance = b.Appearance,
                AscorbicAcid = b.AscorbicAcid,
                Atomsphere = b.Atomsphere,
                DipstickBlood = b.DipstickBlood,
                BillInvoiceNumber = b.BillInvoiceNumber,
                Duration = b.Duration,
                Blirubin = b.Blirubin,
                Color = b.Color,
                Glucose = b.Glucose,
                Incubatio = b.Incubatio,
                Ketones = b.Ketones,
                Labnote = b.Labnote,
                LeucocyteEsterase = b.LeucocyteEsterase,
                MacrosopyBlood = b.MacrosopyBlood,
                Mucus = b.Mucus,
                Niterite = b.Niterite,
                Plate = b.Plate,
                Protein = b.Protein,
                Urobilinogen = b.Urobilinogen,
                StainType = b.StainType,
                GiemsaStainParasite = b.GiemsaStainParasite,
                AnySpillage = b.AnySpillage,
                OtherStainResult = b.OtherStainResult,
                RBCS = b.RBCS,
                TotalSpermCount = b.TotalSpermCount,
                WBCS = b.WBCS,
                AcisFastBacilli = b.AcisFastBacilli,
                Casts = b.Casts,
                Cystals = b.Cystals,
                DateCreated = b.DateCreated,
                DateOfProduction = b.DateOfProduction,
                DurationOfAbstinence = b.DurationOfAbstinence,
                EpithelialCells = b.EpithelialCells,
                GiemsaOthers = b.GiemsaOthers,
                GramNegativeCocci = b.GramNegativeCocci,
                IsDeleted = b.IsDeleted,
                GramNegativeRods = b.GramNegativeRods,
                GramPositiveCocci = b.GramPositiveCocci,
                GramPositiveRods = b.GramPositiveRods,
                ImmatureCells = b.ImmatureCells,
                IndiaInkResult = b.IndiaInkResult,
                IodineResult = b.IodineResult,
                KOHPrepareation = b.KOHPrepareation,
                KOHResult = b.KOHResult,
                MethyleneResult = b.MethyleneResult,
                MicroscopyType = b.MicroscopyType,
                ModeOfProduction = b.ModeOfProduction,
                Morphology = b.Morphology,
                Motility = b.Motility,
                OthersResult = b.OthersResult,
                Ova = b.Ova,
                PH = b.PH,
                Protozoa = b.Protozoa,
                PusCells = b.PusCells,
                RedBloodCells = b.RedBloodCells,
                TimeExamined = b.TimeExamined,
                TimeOfProduction = b.TimeOfProduction,
                TimeRecieved = b.TimeRecieved,
                TrichomonasVaginalis = b.TrichomonasVaginalis,
                VincetsOrganisms = b.VincetsOrganisms,
                Viscosity = b.Viscosity,
                WetMountBacteria = b.WetMountBacteria,
                WetMountOthers = b.WetMountOthers,
                WetMountParasite = b.WetMountParasite,
                WetMountYesats = b.WetMountYesats,
                WhiteBloodCells = b.WhiteBloodCells,
                YeastCells = b.YeastCells,
                ZiehlOthers = b.ZiehlOthers,
                ScienticComment = b.ScienticComment,
                PreparedBy = b.PreparedBy.Firstname + " " + b.PreparedBy.Lastname
            }).ToList();
            foreach (var item in model)
            {
                item.MicroscopyTypee = item.MicroscopyType.DisplayName();
                item.StainTypee = item.StainType.DisplayName();
            }
            return model;
        }

        public bool UpdateNonTemplatedLabResults(NonTemplatedLabPreparationVM vmodel, List<NonTemplatedLabPreparationOrganismXAntiBioticsVM> organisms)
        {
            var checkResultIfExist = _db.NonTemplatedLabPreparations.Where(x => x.IsDeleted == false && x.BillInvoiceNumber == vmodel.BillInvoiceNumber).FirstOrDefault();
            if (checkResultIfExist != null)
            {
                checkResultIfExist.IsDeleted = true;
                _db.Entry(checkResultIfExist).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
            }
            var model = new NonTemplatedLabPreparation()
            {
                Temperature = vmodel.Temperature,
                SpecificGravity = vmodel.SpecificGravity,
                Acidity = vmodel.Acidity,
                AdultWarm = vmodel.AdultWarm,
                Appearance = vmodel.Appearance,
                AscorbicAcid = vmodel.AscorbicAcid,
                Atomsphere = vmodel.Atomsphere,
                DipstickBlood = vmodel.DipstickBlood,
                BillInvoiceNumber = vmodel.BillInvoiceNumber,
                Duration = vmodel.Duration,
                Blirubin = vmodel.Blirubin,
                Color = vmodel.Color,
                Glucose = vmodel.Glucose,
                Incubatio = vmodel.Incubatio,
                Ketones = vmodel.Ketones,
                Labnote = vmodel.Labnote,
                LeucocyteEsterase = vmodel.LeucocyteEsterase,
                MacrosopyBlood = vmodel.MacrosopyBlood,
                Mucus = vmodel.Mucus,
                Niterite = vmodel.Niterite,
                Plate = vmodel.Plate,
                Protein = vmodel.Protein,
                Urobilinogen = vmodel.Urobilinogen,
                DateOfProduction = vmodel.DateOfProduction,
                DurationOfAbstinence = vmodel.DurationOfAbstinence,
                StainType = vmodel.StainType,
                GiemsaStainParasite = vmodel.GiemsaStainParasite,
                AnySpillage = vmodel.AnySpillage,
                OtherStainResult = vmodel.OtherStainResult,
                RBCS = vmodel.RBCS,
                TotalSpermCount = vmodel.TotalSpermCount,
                WBCS = vmodel.WBCS,
                AcisFastBacilli = vmodel.AcisFastBacilli,
                Casts = vmodel.Casts,
                VincetsOrganisms = vmodel.VincetsOrganisms,
                Cystals = vmodel.Cystals,
                EpithelialCells = vmodel.EpithelialCells,
                GiemsaOthers = vmodel.GiemsaOthers,
                GramNegativeCocci = vmodel.GramNegativeCocci,
                GramNegativeRods = vmodel.GramNegativeRods,
                GramPositiveCocci = vmodel.GramPositiveCocci,
                GramPositiveRods = vmodel.GramPositiveRods,
                ImmatureCells = vmodel.ImmatureCells,
                IndiaInkResult = vmodel.IndiaInkResult,
                IodineResult = vmodel.IodineResult,
                KOHPrepareation = vmodel.KOHPrepareation,
                KOHResult = vmodel.KOHResult,
                MethyleneResult = vmodel.MethyleneResult,
                MicroscopyType = vmodel.MicroscopyType,
                ModeOfProduction = vmodel.ModeOfProduction,
                Morphology = vmodel.Morphology,
                Motility = vmodel.Motility,
                OthersResult = vmodel.OthersResult,
                Ova = vmodel.Ova,
                PH = vmodel.PH,
                Protozoa = vmodel.Protozoa,
                PusCells = vmodel.PusCells,
                RedBloodCells = vmodel.RedBloodCells,
                TimeExamined = vmodel.TimeExamined,
                TimeOfProduction = vmodel.TimeOfProduction,
                TimeRecieved = vmodel.TimeRecieved,
                TrichomonasVaginalis = vmodel.TrichomonasVaginalis,
                Viscosity = vmodel.Viscosity,
                WetMountBacteria = vmodel.WetMountBacteria,
                WetMountOthers = vmodel.WetMountOthers,
                WetMountParasite = vmodel.WetMountParasite,
                WetMountYesats = vmodel.WetMountYesats,
                WhiteBloodCells = vmodel.WhiteBloodCells,
                YeastCells = vmodel.YeastCells,
                ZiehlOthers = vmodel.ZiehlOthers,
                ScienticComment = vmodel.ScienticComment,
                ServiceID = vmodel.ServiceID,
                IsDeleted = false,
                DateCreated = DateTime.Now,
                PreparedByID = Global.AuthenticatedUserID
            };
            _db.NonTemplatedLabPreparations.Add(model);
            _db.SaveChanges();

            if (organisms.Count() > 0)
            {
                foreach (var organism in organisms)
                {
                    if (checkResultIfExist != null)
                    {
                        var checkIfOrganismExist = _db.NonTemplatedLabResultOrganismXAntibiotics.FirstOrDefault(x => x.IsDeleted == false && x.OrganismID == organism.OrganismID && x.NonTemplateLabResultID == checkResultIfExist.Id);
                        if (checkIfOrganismExist != null)
                        {
                            checkIfOrganismExist.IsDeleted = true;
                            _db.Entry(checkIfOrganismExist).State = System.Data.Entity.EntityState.Modified;
                            _db.SaveChanges();
                        }
                    }

                    var organismModel = new NonTemplatedLabResultOrganismXAntibiotics()
                    {
                        AntiBioticID = organism.AntiBioticID,
                        OrganismID = organism.OrganismID,
                        NonTemplateLabResultID = model.Id,
                        ResistanceDegree = organism.ResistanceDegree,
                        SensitiveDegree = organism.SensitiveDegree,
                        IsSensitive = organism.IsSensitive,
                        IsIntermediate = organism.IsIntermediate,
                        IsResistance = organism.IsResistance,
                        IsDeleted = false,
                        DateCreated = DateTime.Now
                    };
                    _db.NonTemplatedLabResultOrganismXAntibiotics.Add(organismModel);
                    _db.SaveChanges();
                }
            }
            var service = _seedService.GetService((int)model.ServiceID).Description;
            var serviceParameter = GetServiceParameter(service);
            if (serviceParameter.RequireApproval)
            {
                var checkIfExist = _db.ResultApprovals.Where(x => x.ServiceParameterID == serviceParameter.Id && x.BillNumber == vmodel.BillInvoiceNumber).FirstOrDefault();
                if (checkIfExist == null)
                {
                    var approvalModel = new ResultApproval()
                    {
                        ServiceParameterID = serviceParameter.Id,
                        BillNumber = vmodel.BillInvoiceNumber,
                    };
                    _db.ResultApprovals.Add(approvalModel);
                    _db.SaveChanges();
                }
            }
            return true;
        }
        public List<NonTemplatedLabPreparationOrganismXAntiBioticsVM> GetComputedOrganismXAntibiotics(int nonTemplatedId)
        {
            var results = _db.NonTemplatedLabResultOrganismXAntibiotics.Where(x => x.IsDeleted == false && x.NonTemplateLabResultID == nonTemplatedId)
                .Select(b => new NonTemplatedLabPreparationOrganismXAntiBioticsVM()
                {
                    Id = b.Id,
                    NonTemplateLabResultID = b.NonTemplateLabResultID,
                    SensitiveDegree = b.SensitiveDegree == null ? "N/A" : b.SensitiveDegree,
                    IsSensitive = b.IsSensitive,
                    IsResistance = b.IsResistance,
                    AntiBioticID = b.AntiBioticID,
                    AntiBiotic = b.AntiBiotic.Name,
                    Organism = b.Organism.Name,
                    OrganismID = b.OrganismID,
                    IsIntermediate = b.IsIntermediate,
                    ResistanceDegree = b.ResistanceDegree == null ? "N/A" : b.ResistanceDegree,
                }).ToList();
            return results;
        }
        public List<ResultApprovalVM> GetAllTestForApprovalByBillNumber(string billnumber)
        {
            var records = _db.ResultApprovals.Where(x => x.BillNumber == billnumber).Select(b => new ResultApprovalVM()
            {
                Id = b.Id,
                ServiceID = b.ServiceParameter.ServiceID,
                Service = b.ServiceParameter.Service.Description,
                Template = b.ServiceParameter.Template.Name,
                TemplateID = b.ServiceParameter.TemplateID,
                HasApproved = b.HasApproved,
                ApprovedBy = b.ApprovedBy.Firstname + " " + b.ApprovedBy.Lastname,
                DateApproved = b.DateApproved,
                ApprovedByID = b.ApprovedByID,
                ServiceParameterID = b.ServiceParameterID,
                BillNumber = b.BillNumber
            }).ToList();
            return records;
        }
        public List<TemplateServiceCompuationVM> GetComputedResultForTemplatedService(string billnumber, int serviceParameterID)
        {
            var record = _db.TemplatedLabPreparations.Where(x => x.BillInvoiceNumber == billnumber && x.ServiceParameterSetup.ServiceParameterID == serviceParameterID).Select(b => new TemplateServiceCompuationVM()
            {
                Parameter = b.ServiceParameterSetup.Name,
                Value = b.Value,
                Unit = b.ServiceRange.Unit,
                Range = b.ServiceRange.Range,
                Service = b.ServiceParameterSetup.ServiceParameter.Service.Description,
                Labnote = b.Labnote,
                ScienticComment = b.ScienticComment,
                PreparedBy = b.PreparedBy.Firstname + " " + b.PreparedBy.Lastname,
                DatePrepared = b.DateCreated,
                Specimen = b.ServiceParameterSetup.ServiceParameter.Specimen.Name,
            }).ToList();
            foreach (var item in record)
            {
                item.Status = GetStatusForTemplateReport(int.Parse(item.Value), item.Range);
            }
            return record;
        }
        public bool ApproveTestResult(int Id)
        {
            var test = _db.ResultApprovals.FirstOrDefault(x => x.Id == Id);
            test.HasApproved = true;
            test.ApprovedByID = Global.AuthenticatedUserID;
            test.DateApproved = DateTime.Now;

            _db.Entry(test).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();

            return true;
        }
    }
}
