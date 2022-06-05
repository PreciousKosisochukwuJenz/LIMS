using JenzHealth.Areas.Admin.ViewModels.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.Areas.Admin.Interfaces
{
    public interface IReportService
    {
        List<RequestTrackerVM> TrackRequest(RequestTrackerVM vmodel);
    }
}
