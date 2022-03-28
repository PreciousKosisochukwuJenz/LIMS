using JenzHealth.Areas.Admin.ViewModels;
using JenzHealth.Areas.Admin.ViewModels.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace JenzHealth.Areas.Admin.Interfaces
{
    interface IApplicationSettingsService
    {
        // Application settings
        ApplicationSettingsVM GetApplicationSettings();
        bool UpdateApplicationSettings(ApplicationSettingsVM Vmodel, HttpPostedFileBase Logo, HttpPostedFileBase Watermark);
        List<SettingsDataSetVM> GetReportHeader();
    }
}
