using JenzHealth.DAL.DataConnection;
using JenzHealth.DAL.Entity;
using JenzHealth.Areas.Admin.Helpers;
using JenzHealth.Areas.Admin.Interfaces;
using JenzHealth.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JenzHealth.Areas.Admin.ViewModels.Report;

namespace JenzHealth.Areas.Admin.Services
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        /* Instancation of the database context model
         * and injecting some buisness layer services
         */
        #region Instanciation
        readonly DatabaseEntities _db;
        public ApplicationSettingsService()
        {
            _db = new DatabaseEntities();
        }
        public ApplicationSettingsService(DatabaseEntities db)
        {
            _db = db;
        }
        #endregion

        /* ********************************************************************************************************** */
        // Application settings

        // Getting the basic system application setting data
        public ApplicationSettingsVM GetApplicationSettings()
        {
            byte[] emptyArr = { 4, 3 };
            var model = _db.ApplicationSettings.FirstOrDefault();
            var Vmodel = new ApplicationSettingsVM()
            {
                ID = model.Id,
                AppName = model.AppName,
                Logo = model.Logo == null ? emptyArr : model.Logo,
                Watermark = model.Watermark == null ? emptyArr : model.Watermark,
                EnablePartPayment = model.EnablePartPayment,
                EnableSpecimentCollectedBy = model.EnableSpecimentCollectedBy,
                SalesRecieptCopyCount = model.SalesRecieptCopyCount,
                CustomerNumberPrefix = model.CustomerNumberPrefix,
                CodeGenSeed = model.CodeGenSeed,
                DepositeCount = model.DepositeCount,
                BillCount = model.BillCount,
                LabCount = model.LabCount,
                SessionTimeOut = model.SessionTimeOut,
                PaymentCount = model.PaymentCount,
                ShiftCount = model.ShiftCount,
                ExpressWaiver = model.ExpressWaiver
            };
            return Vmodel;
        }

        // Editting and updating the system application setting data
        public bool UpdateApplicationSettings(ApplicationSettingsVM Vmodel, HttpPostedFileBase Logo, HttpPostedFileBase Watermark)
        {
            bool hasSucceed = false;
            var model = _db.ApplicationSettings.FirstOrDefault(x => x.Id == Vmodel.ID);
            model.AppName = Vmodel.AppName;
            model.EnableSpecimentCollectedBy = Vmodel.EnableSpecimentCollectedBy;
            model.EnablePartPayment = Vmodel.EnablePartPayment;
            model.CustomerNumberPrefix = Vmodel.CustomerNumberPrefix;
            model.SalesRecieptCopyCount = Vmodel.SalesRecieptCopyCount;
            model.CodeGenSeed = Vmodel.CodeGenSeed;
            model.SessionTimeOut = Vmodel.SessionTimeOut;
            model.ExpressWaiver = Vmodel.ExpressWaiver;
            if (Logo != null)
                model.Logo = CustomSerializer.Serialize(Logo);
            if (Watermark != null)
                model.Watermark = CustomSerializer.Serialize(Watermark);
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            var state = _db.SaveChanges();
            if (state > 0)
            {
                hasSucceed = true;
            }
            return hasSucceed;
        }

    public List<SettingsDataSetVM> GetReportHeader()
        {
            byte[] empty = { 4,3 };
            var response = _db.ApplicationSettings.Where(x => x.Id != 0).Select(b => new SettingsDataSetVM()
            {
               Id = b.Id,
               BrandName = b.AppName,
               Logo = b.Logo == null ? empty : b.Logo,
               Watermark = b.Watermark == null ? empty : b.Watermark,
               DateGenerated = DateTime.Now
            }).ToList();
            return response;
        }
    }
}