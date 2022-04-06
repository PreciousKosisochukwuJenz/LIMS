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
        List<ServiceParameterVM> GetServiceParameters(string invoiceNumber);
        bool CheckSpecimenCollectionWithBillNumber(string billnumber);
        SpecimenCollectionVM GetSpecimenCollected(string billnumber);
        List<SpecimenCollectionVM> GetLabPreparations(SpecimenCollectionVM vmodel);
        SpecimenCollectionVM GetSpecimensForPreparation(int Id);
        List<ServiceParameterVM> GetServicesToPrepare(string invoiceNumber);
        List<ServiceParameterVM> GetDistinctTemplateForBilledServices(List<ServiceParameterVM> billedServices);
        List<TemplateServiceCompuationVM> SetupTemplatedServiceForComputation(int TemplateID, string billNumber);
        bool UpdateLabResults(List<RequestComputedResultVM> results, string labnote);
        NonTemplatedLabPreparationVM GetNonTemplatedLabPreparation(string billnumber);
        List<NonTemplatedLabPreparationVM> GetNonTemplatedLabPreparationForReport(string billnumber);
        bool UpdateNonTemplatedLabResults(NonTemplatedLabPreparationVM vmodel, List<NonTemplatedLabPreparationOrganismXAntiBioticsVM> organisms);
        List<NonTemplatedLabPreparationOrganismXAntiBioticsVM> GetComputedOrganismXAntibiotics(int nonTemplatedId);
        List<SpecimenCollectionVM> GetSpecimenCollectedForReport(string billnumber, int templateID);
    }
}
