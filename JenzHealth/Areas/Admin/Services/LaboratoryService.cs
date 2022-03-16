﻿using JenzHealth.Areas.Admin.Interfaces;
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
                    TemplateID = _db.Templates.FirstOrDefault(x=>x.Name == serviceParamter.Template).Id,
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
                    ServiceParameterSetupID = _db.ServiceParameterSetups.FirstOrDefault(x=>x.IsDeleted == false && x.Id == rangesetup.ParameterID).Id,
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
                            ServiceID = _db.Services.FirstOrDefault(x=>x.Description == sample.Service).Id,
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
            foreach(var service in model)
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
                    CustomerName = _db.Billings.FirstOrDefault(x=>x.InvoiceNumber == b.BillInvoiceNumber).CustomerName,
                    CustomerPhoneNumber = _db.Billings.FirstOrDefault(x=>x.InvoiceNumber == b.BillInvoiceNumber).CustomerPhoneNumber,
                    CustomerUniqueID = _db.Billings.FirstOrDefault(x=>x.InvoiceNumber == b.BillInvoiceNumber).CustomerUniqueID,
                }).ToList();
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
                Template  = _db.ServiceParameters.FirstOrDefault(x=>x.ServiceID == b.ServiceID).Template.Name,
                TemplateID  = _db.ServiceParameters.FirstOrDefault(x=>x.ServiceID == b.ServiceID).TemplateID,
                BillNumber = b.InvoiceNumber
            }).ToList();
          
            return model;
        }
        public List<ServiceParameterVM> GetDistinctTemplateForBilledServices(List<ServiceParameterVM> billedServices)
        {
            return billedServices.Distinct(o => o.TemplateID).ToList();
        }

        public List<TemplateServiceCompuationVM> SetupTemplatedServiceForComputation(int TemplateID, string billNumber)
        {
            List<TemplateServiceCompuationVM> model = new List<TemplateServiceCompuationVM>();

            var billedService = this.GetServicesToPrepare(billNumber);
            var billedServiceForTemplate = billedService.Where(x => x.TemplateID == TemplateID);

            foreach(var service in billedServiceForTemplate)
            {
                TemplateServiceCompuationVM billservice = new TemplateServiceCompuationVM();
                billservice.Parameters = new List<ServiceParameterAndRange>();
                var serviceParameterID = _db.ServiceParameters.FirstOrDefault(x=>x.ServiceID == service.ServiceID && x.TemplateID == TemplateID).Id;
                var parameterSetups = _db.ServiceParameterSetups.Where(x => x.ServiceParameterID == serviceParameterID && x.IsDeleted == false).Select(b => new ServiceParameterSetupVM()
                {
                    Id = b.Id,
                    Name = b.Name,
                    Rank = b.Rank
                }).ToList();

                foreach(var parameterSetup in parameterSetups)
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

            return model;
        }
        public bool UpdateLabResults(List<RequestComputedResultVM> results,string labnote)
        {
            foreach(var result in results)
            {
                var exist = _db.TemplatedLabPreparations.Where(x => x.BillInvoiceNumber == result.BillInvoiceNumber && x.IsDeleted == false && x.ServiceParameterSetupID == result.KeyID).FirstOrDefault();

                if(exist != null)
                {
                    exist.Value = result.Value;
                    exist.ServiceRangeID = result.RangeID;
                    _db.Entry(exist).State = System.Data.Entity.EntityState.Modified;
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
                        IsDeleted = false,
                        DateCreated = DateTime.Now
                    };
                    _db.TemplatedLabPreparations.Add(templateLabPreparation);
                }
            }
            _db.SaveChanges();
            return true;
        }
        public NonTemplatedLabPreparationVM GetNonTemplatedLabPreparation(string billnumber)
        {
            var model = _db.NonTemplatedLabPreparations.Where(x => x.IsDeleted == false && x.BillInvoiceNumber == billnumber).Select(b => new NonTemplatedLabPreparationVM()
            {
                Id = b.Id,
                Temperature = b.Temperature,
                SpecificGravity = b.SpecificGravity,
                Acidity = b.Acidity,
                AdultWarm = b.AdultWarm,
                Appearance =  b.Appearance,
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
                Urobilinogen = b.Urobilinogen
            }).FirstOrDefault();
            return model;
        }
        public bool UpdateNonTemplatedLabResults(NonTemplatedLabPreparationVM vmodel, List<NonTemplatedLabPreparationOrganismXAntiBioticsVM> organisms)
        {
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
                IsDeleted = false,
                DateCreated = DateTime.Now
            };
            _db.NonTemplatedLabPreparations.Add(model);
            _db.SaveChanges();

            if(organisms.Count() > 0)
            {
                foreach(var organism in organisms)
                {
                    var organismModel = new NonTemplatedLabResultOrganismXAntibiotics()
                    {
                        AntiBioticID = organism.AntiBioticID,
                        OrganismID = organism.OrganismID,
                        NonTemplateLabResultID = model.Id,
                        IsDeleted = false,
                        DateCreated = DateTime.Now
                    };
                    _db.NonTemplatedLabResultOrganismXAntibiotics.Add(organismModel);
                    _db.SaveChanges();
                }
            }
            return true;
        }
    }
}
