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
    public class SeedService : ISeedService
    {
        /* Instancation of the database context model
        * and injecting some buisness layer services
         */
        #region Instanciation
        readonly DatabaseEntities _db;
        public SeedService()
        {
            _db = new DatabaseEntities();
        }
        public SeedService(DatabaseEntities db)
        {
            _db = db;
        }
        #endregion

        /* *************************************************************************** */
        //Revenue Department

        // Fetching revenue department
        public List<RevenueDepartmentVM> GetRevenueDepartment()
        {
            var model = _db.RevenueDepartments.Where(x => x.IsDeleted == false).Select(b => new RevenueDepartmentVM()
            {
                Id = b.Id,
                Name = b.Name,
                Code = b.Code
            }).ToList();
            return model;
        }

        // Creating revenue department
        public bool CreateRevenueDepartment(RevenueDepartmentVM vmodel)
        {
            bool HasSaved = false;
            RevenueDepartment model = new RevenueDepartment()
            {
                Name = vmodel.Name,
                Code = Generator.GeneratorCode(),
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };
            _db.RevenueDepartments.Add(model);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Getting revenue department
        public RevenueDepartmentVM GetRevenueDepartment(int ID)
        {
            var model = _db.RevenueDepartments.Where(x => x.Id == ID).Select(b => new RevenueDepartmentVM()
            {
                Id = b.Id,
                Name = b.Name,
                Code = b.Code
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating revenue department
        public bool EditRevenueDepartment(RevenueDepartmentVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.RevenueDepartments.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Name = vmodel.Name;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting revenue department
        public bool DeleteRevenueDepartment(int ID)
        {
            bool HasDeleted = false;
            var model = _db.RevenueDepartments.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }

        /* *************************************************************************** */
        //Service Department

        // Fetching revenue department
        public List<ServiceDepartmentVM> GetServiceDepartment()
        {
            var model = _db.ServiceDepartments.Where(x => x.IsDeleted == false).Select(b => new ServiceDepartmentVM()
            {
                Id = b.Id,
                Name = b.Name,
                Code = b.Code
            }).ToList();
            return model;
        }

        // Creating revenue department
        public bool CreateServiceDepartment(ServiceDepartmentVM vmodel)
        {
            bool HasSaved = false;
            ServiceDepartment model = new ServiceDepartment()
            {
                Name = vmodel.Name,
                Code = Generator.GeneratorCode(),
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };
            _db.ServiceDepartments.Add(model);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Getting Service department
        public ServiceDepartmentVM GetServiceDepartment(int ID)
        {
            var model = _db.ServiceDepartments.Where(x => x.Id == ID).Select(b => new ServiceDepartmentVM()
            {
                Id = b.Id,
                Name = b.Name,
                Code = b.Code
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating Service department
        public bool EditServiceDepartment(ServiceDepartmentVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.ServiceDepartments.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Name = vmodel.Name;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting revenue department
        public bool DeleteServiceDepartment(int ID)
        {
            bool HasDeleted = false;
            var model = _db.ServiceDepartments.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }

        /* *************************************************************************** */
        //Priviledge

        // Fetching Priviledge
        public List<PriviledgeVM> GetPriviledges()
        {
            var model = _db.Priviledges.Where(x => x.IsDeleted == false).Select(b => new PriviledgeVM()
            {
                Id = b.Id,
                Name = b.Name,
            }).ToList();
            return model;
        }

        // Creating Priviledges
        public bool CreatePriviledge(PriviledgeVM vmodel)
        {
            bool HasSaved = false;
            Priviledge model = new Priviledge()
            {
                Name = vmodel.Name,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };
            _db.Priviledges.Add(model);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Getting Priviledges
        public PriviledgeVM GetPriviledge(int ID)
        {
            var model = _db.Priviledges.Where(x => x.Id == ID).Select(b => new PriviledgeVM()
            {
                Id = b.Id,
                Name = b.Name,
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating Priviledges
        public bool EditPriviledge(PriviledgeVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.Priviledges.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Name = vmodel.Name;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting Priviledges
        public bool DeletePriviledge(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Priviledges.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }

        /* *************************************************************************** */
        //Templates

        // Fetching Templates
        public List<TemplateVM> GetTemplates(int serviceDepartmentID)
        {
            var model = _db.Templates.Where(x => x.IsDeleted == false && x.ServiceDepartmentID == serviceDepartmentID).Select(b => new TemplateVM()
            {
                Id = b.Id,
                Name = b.Name,
                ServiceDepartment = b.ServiceDepartment.Name,
                ServiceDepartmentID = b.ServiceDepartmentID,
                UseDefaultParameters = b.UseDefaultParameters
            }).ToList();
            return model;
        }

        // Creating Template
        public bool CreateTemplate(TemplateVM vmodel)
        {
            bool HasSaved = false;
            Template model = new Template()
            {
                Name = vmodel.Name,
                ServiceDepartmentID = vmodel.ServiceDepartmentID,
                UseDefaultParameters = vmodel.UseDefaultParameters,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
            };
            _db.Templates.Add(model);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Getting Template
        public TemplateVM GetTemplate(int ID)
        {
            var model = _db.Templates.Where(x => x.Id == ID).Select(b => new TemplateVM()
            {
                Id = b.Id,
                Name = b.Name,
                ServiceDepartmentID = b.ServiceDepartmentID,
                UseDefaultParameters = b.UseDefaultParameters
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating Priviledges
        public bool EditTemplate(TemplateVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.Templates.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Name = vmodel.Name;
            model.ServiceDepartmentID = vmodel.ServiceDepartmentID;
            model.UseDefaultParameters = vmodel.UseDefaultParameters;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting Template
        public bool DeleteTemplate(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Templates.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }

        /* *************************************************************************** */
        //Vendor

        // Fetching Vendors
        public List<VendorVM> GetVendors()
        {
            var model = _db.Vendors.Where(x => x.IsDeleted == false).Select(b => new VendorVM()
            {
                Id = b.Id,
                Name = b.Name,
                OfficeAddress = b.OfficeAddress,
                CompanyRegistrationNumber = b.CompanyRegistrationNumber,
                PostalAddress = b.PostalAddress,
                Website = b.Website,
                Email = b.Email,
                PhoneNumber = b.PhoneNumber,
            }).ToList();
            return model;
        }

        // Creating Vendor
        public bool CreateVendors(VendorVM vmodel)
        {
            bool HasSaved = false;
            Vendor model = new Vendor()
            {
                Name = vmodel.Name,
                CompanyRegistrationNumber = vmodel.CompanyRegistrationNumber,
                Website = vmodel.Website,
                Email = vmodel.Email,
                PhoneNumber = vmodel.PhoneNumber,
                OfficeAddress = vmodel.OfficeAddress,
                PostalAddress = vmodel.PostalAddress,
                IsDeleted = false,
                DateCreated = DateTime.Now
            };
            _db.Vendors.Add(model);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Getting Vendor
        public VendorVM GetVendor(int ID)
        {
            var model = _db.Vendors.Where(x => x.Id == ID).Select(b => new VendorVM()
            {
                Id =b.Id,
                Name = b.Name,
                OfficeAddress = b.OfficeAddress,
                CompanyRegistrationNumber = b.CompanyRegistrationNumber,
                PostalAddress = b.PostalAddress,
                Website = b.Website,
                Email = b.Email,
                PhoneNumber =b.PhoneNumber,
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating Vendor
        public bool EditVendor(VendorVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.Vendors.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Name = vmodel.Name;
            model.OfficeAddress = vmodel.OfficeAddress;
            model.Email = vmodel.Email;
            model.PhoneNumber = vmodel.PhoneNumber;
            model.CompanyRegistrationNumber = vmodel.CompanyRegistrationNumber;
            model.PostalAddress = vmodel.PostalAddress;
            model.Website = vmodel.Website;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting Vendor
        public bool DeleteVendor(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Vendors.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }

        /******************************************************/

        //Service

        // Fetching service
        public List<ServiceVM> GetServices(ServiceVM vmodel)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var model = _db.Services.Where(x => x.IsDeleted == false && x.RevenueDepartmentID == vmodel.RevenueDepartmentID && x.ServiceDepartmentID == vmodel.ServiceDepartmentID).Select(b => new ServiceVM()
            {
                Id = b.Id,
                Description = b.Description,
                RevenueDepartmentID = b.RevenueDepartmentID,
                ServiceDepartmentID = b.ServiceDepartmentID,
                RevenueDepartment = b.RevenueDepartment.Name,
                ServiceDepartment = b.ServiceDepartment.Name,
                CostPrice = b.CostPrice,
                SellingPrice = b.SellingPrice,
            }).ToList();
            foreach(var each in model)
            {
                each.SellingPriceString = "₦" + each.SellingPrice.ToString("N", nfi);
                each.CostPriceString = "₦" + each.CostPrice.ToString("N", nfi);
            }
            return model;
        }

        // Creating service
        public bool CreateService(ServiceVM vmodel)
        {
            bool HasSaved = false;
            Service model = new Service()
            {
                RevenueDepartmentID = vmodel.RevenueDepartmentID,
                ServiceDepartmentID = vmodel.ServiceDepartmentID,
                Description = vmodel.Description,
                SellingPrice = CustomSerializer.UnMaskString(vmodel.SellingPriceString),
                CostPrice = CustomSerializer.UnMaskString(vmodel.CostPriceString),
                IsDeleted = false,
                DateCreated = DateTime.Now
            };
            _db.Services.Add(model);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Getting Service
        public ServiceVM GetService(int ID)
        {
            var model = _db.Services.Where(x => x.Id == ID).Select(b => new ServiceVM()
            {
                Id = b.Id,
                Description = b.Description,
                RevenueDepartmentID = b.RevenueDepartmentID,
                ServiceDepartmentID = b.ServiceDepartmentID,
                RevenueDepartment = b.RevenueDepartment.Name,
                ServiceDepartment = b.ServiceDepartment.Name,
                CostPrice = b.CostPrice,
                SellingPrice = b.SellingPrice,
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating service
        public bool EditService(ServiceVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.Services.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Description = vmodel.Description;
            model.RevenueDepartmentID = vmodel.RevenueDepartmentID;
            model.ServiceDepartmentID = vmodel.ServiceDepartmentID;
            model.CostPrice = CustomSerializer.UnMaskString(vmodel.CostPriceString);
            model.SellingPrice = CustomSerializer.UnMaskString(vmodel.SellingPriceString);

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting service
        public bool DeleteService(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Services.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }

        public ServiceVM GetService(string servicename)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var model = _db.Services.Where(x => x.Description == servicename).Select(b => new ServiceVM()
            {
                Id = b.Id,
                Description = b.Description,
                RevenueDepartmentID = b.RevenueDepartmentID,
                ServiceDepartmentID = b.ServiceDepartmentID,
                RevenueDepartment = b.RevenueDepartment.Name,
                ServiceDepartment = b.ServiceDepartment.Name,
                CostPrice = b.CostPrice,
                SellingPrice = b.SellingPrice,
            }).FirstOrDefault();

            model.SellingPriceString = "₦" + model.SellingPrice.ToString("N", nfi);
            model.CostPriceString = "₦" + model.CostPrice.ToString("N", nfi);
            return model;
        }
        public List<ServiceVM> GetServiceAutoComplete(string query)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            var model = _db.Services.Where(x => x.Description.StartsWith(query)).Select(b => new ServiceVM()
            {
                Id = b.Id,
                Description = b.Description,
                RevenueDepartmentID = b.RevenueDepartmentID,
                ServiceDepartmentID = b.ServiceDepartmentID,
                RevenueDepartment = b.RevenueDepartment.Name,
                ServiceDepartment = b.ServiceDepartment.Name,
            }).ToList();
            return model;
        }
        public List<string> GetServiceNameAutoComplete(string term)
        {
            List<string> services;
            services = _db.Services.Where(x => x.IsDeleted == false && x.Description.StartsWith(term)).Select(b => b.Description).ToList();
            return services;
        }
        public List<string> GetSpecimenAutoComplete(string term)
        {
            List<string> specimens;
            specimens = _db.Specimens.Where(x => x.IsDeleted == false && x.Name.StartsWith(term)).Select(b => b.Name).ToList();
            return specimens;
        }

        public List<string> GetTemplateAutoComplete(string term)
        {
            List<string> templates;
            templates = _db.Templates.Where(x => x.IsDeleted == false && x.Name.StartsWith(term)).Select(b => b.Name).ToList();
            return templates;
        }

        /* *************************************************************************** */
        //Specimen

        // Fetching Specimen
        public List<SpecimenVM> GetSpecimens()
        {
            var model = _db.Specimens.Where(x => x.IsDeleted == false).Select(b => new SpecimenVM()
            {
                Id = b.Id,
                Name = b.Name,
            }).ToList();
            return model;
        }

        // Creating Specimen
        public bool CreateSpecimen(SpecimenVM vmodel)
        {
            bool HasSaved = false;
            Specimen model = new Specimen()
            {
                Name = vmodel.Name,
                IsDeleted = false,
                DateCreated = DateTime.Now,
            };
            _db.Specimens.Add(model);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Getting Specimen
        public SpecimenVM GetSpecimen(int ID)
        {
            var model = _db.Specimens.Where(x => x.Id == ID).Select(b => new SpecimenVM()
            {
                Id = b.Id,
                Name = b.Name,
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating Specimen
        public bool EditSpecimen(SpecimenVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.Specimens.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Name = vmodel.Name;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting Specimen
        public bool DeleteSpecimen(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Specimens.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }



        /* *************************************************************************** */
        //Organism

        // Fetching Organism
        public List<OrganismVM> GetOrganisms()
        {
            var model = _db.Organisms.Where(x => x.IsDeleted == false).Select(b => new OrganismVM()
            {
                Id = b.Id,
                Name = b.Name,
            }).ToList();
            return model;
        }

        // Creating Organism
        public bool CreateOrganism(OrganismVM vmodel)
        {
            bool HasSaved = false;
            Organism model = new Organism()
            {
                Name = vmodel.Name,
                IsDeleted = false,
                DateCreated = DateTime.Now,
            };
            _db.Organisms.Add(model);
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Getting Organism
        public OrganismVM GetOrganism(int ID)
        {
            var model = _db.Organisms.Where(x => x.Id == ID).Select(b => new OrganismVM()
            {
                Id = b.Id,
                Name = b.Name,
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating Organism
        public bool EditOrganism(OrganismVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.Organisms.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Name = vmodel.Name;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting Organism
        public bool DeleteOrganism(int ID)
        {
            bool HasDeleted = false;
            var model = _db.Organisms.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }

        /* *************************************************************************** */
        //AntiBiotic

        // Fetching AntiBiotics
        public List<AntiBioticVM> GetAntiBiotics(int OrganismID)
        {
            var model = _db.AntiBiotics.Where(x => x.IsDeleted == false && x.OrganismID == OrganismID).Select(b => new AntiBioticVM()
            {
                Id = b.Id,
                Name = b.Name,
                OrganismID = b.OrganismID,
                OrganismName = b.Organism.Name
            }).ToList();
            return model;
        }

        // Creating AntiBiotic
        public AntiBioticVM CreateAnitBiotic(AntiBioticVM vmodel)
        {
            AntiBiotic model = new AntiBiotic()
            {
                Name = vmodel.Name,
                OrganismID = vmodel.OrganismID,
                IsDeleted = false,
                DateCreated = DateTime.Now,
            };
            _db.AntiBiotics.Add(model);
            _db.SaveChanges();
            return vmodel;
        }

        // Getting AntiBioticVM
        public AntiBioticVM GetAntiBiotic(int ID)
        {
            var model = _db.AntiBiotics.Where(x => x.Id == ID).Select(b => new AntiBioticVM()
            {
                Id = b.Id,
                Name = b.Name,
                OrganismID = b.OrganismID,
                OrganismName = b.Organism.Name
            }).FirstOrDefault();
            return model;
        }
        public AntiBioticVM GetAntiBioticByOrganismName(string organismName)
        {
            var model = _db.AntiBiotics.Where(x => x.Organism.Name == organismName).Select(b => new AntiBioticVM()
            {
                Id = b.Id,
                Name = b.Name,
                OrganismID = b.OrganismID,
                OrganismName = b.Organism.Name
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating AntiBiotic
        public bool EditAntiBiotic(AntiBioticVM vmodel)
        {
            bool HasSaved = false;
            var model = _db.AntiBiotics.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Name = vmodel.Name;
            model.OrganismID = vmodel.OrganismID;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Deleting AntiBiotic
        public bool DeleteAntiBiotic(int ID)
        {
            bool HasDeleted = false;
            var model = _db.AntiBiotics.Where(x => x.Id == ID).FirstOrDefault();
            model.IsDeleted = true;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            HasDeleted = true;
            return HasDeleted;
        }
        public List<string> GetOrganismAutoComplete(string term)
        {
            List<string> organisms;
            organisms = _db.Organisms.Where(x => x.IsDeleted == false && x.Name.StartsWith(term)).Select(b => b.Name).ToList();
            return organisms;
        }

    }
}