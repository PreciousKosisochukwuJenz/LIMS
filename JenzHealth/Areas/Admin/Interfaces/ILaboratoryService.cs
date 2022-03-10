using JenzHealth.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.Areas.Admin.Interfaces
{
    public interface ILaboratoryService
    {
        ServiceParameterVM GetServiceParameter(string ServiceName);
        void UpdateParamterSetup(ServiceParameterVM serviceParamter, List<ServiceParameterSetupVM> paramterSetups);
        List<ServiceParameterSetupVM> GetServiceParamterSetups(string serviceName);
        List<ServiceParameterRangeSetupVM> GetRangeSetups(string serviceName);
        void UpdateParameterRangeSetup(List<ServiceParameterRangeSetupVM> rangeSetups);
        void UpdateSpecimenSampleCollection(SpecimenCollectionVM specimenCollected, List<SpecimenCollectionCheckListVM> checklist);
    }
}
