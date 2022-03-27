using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels.Report
{
    public class SettingsDataSetVM
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public byte[] Logo { get; set; }
        public DateTime DateGenerated { get; set; }
    }
}