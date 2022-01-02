using JenzHealth.Areas.Admin.Helpers;
using JenzHealth.Areas.Admin.Interfaces;
using JenzHealth.Areas.Admin.ViewModels;
using JenzHealth.DAL.DataConnection;
using JenzHealth.DAL.Entity;
using System;
using System.Collections.Generic;
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
                ServiceDepartmentID = b.ServiceDepartmentID
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
                ServiceDepartmentID = b.ServiceDepartmentID
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
    }
}