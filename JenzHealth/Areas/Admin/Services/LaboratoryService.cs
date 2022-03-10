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

        public LaboratoryService()
        {
            _db = new DatabaseEntities();
        }
        public LaboratoryService(DatabaseEntities db)
        {
            _db = db;
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
        public void UpdateParamterSetup(ServiceParameterVM serviceParamter, List<ServiceParameterSetupVM> paramterSetups)
        {
            var ExistingServiceParamterExist = _db.ServiceParameters.Where(x => x.Service.Description == serviceParamter.Service).FirstOrDefault();
            int serviceParameterID = 0;
            if (ExistingServiceParamterExist != null)
            {
                ExistingServiceParamterExist.RequireApproval = serviceParamter.RequireApproval;
                ExistingServiceParamterExist.SpecimenID = _db.Specimens.FirstOrDefault(x => x.Name == serviceParamter.Specimen).Id;
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
                    TemplateID = _db.Templates.FirstOrDefault(x=>x.Name == serviceParamter.Template).Id,
                    RequireApproval = serviceParamter.RequireApproval,
                };
                _db.ServiceParameters.Add(serviceParameter);
                _db.SaveChanges();
                serviceParameterID = serviceParameter.Id;
            }

            if (paramterSetups.Any())
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
                ServiceParameterID = b.ServiceParameterID
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
            var uniqueIDs = rangeSetups.Distinct(o=>o.ServiceParameterSetupID).ToList();
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
                    ServiceParameterSetupID = rangesetup.ServiceParameterSetupID,
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
                _db.Entry(sampleExist).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
                specimenCollectionID = sampleExist.Id;
            }
            else
            {
                var labCount = _db.ApplicationSettings.FirstOrDefault().LabCount;
                labCount++;
                var labnumber = string.Format("LAB/{0}", labCount.ToString("D6"));

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
                    LabNumber = labnumber
                };

                _db.SpecimenCollections.Add(specimenCollection);
                _db.SaveChanges();
                specimenCollectionID = specimenCollection.Id;
            }

            if(checklist.Count() > 0)
            {
                foreach (var sample in checklist)
                {
                    var checkSample = _db.SpecimenCollectionCheckLists.FirstOrDefault(x => x.Specimen.Name == sample.Specimen && x.SpecimenCollectionID == specimenCollectionID && x.IsDeleted == false);
                    if(checkSample != null)
                    {
                        checkSample.IsCollected = sample.IsCollected;
                        _db.Entry(checkSample).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();
                    }
                    else{
                        var newSample = new SpecimenCollectionCheckList()
                        {
                            SpecimenCollectionID = specimenCollectionID,
                            SpecimenID = _db.Specimens.FirstOrDefault(x => x.Name == sample.Specimen).Id,
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
    }
}
