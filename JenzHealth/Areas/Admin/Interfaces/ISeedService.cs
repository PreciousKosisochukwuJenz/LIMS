using JenzHealth.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.Areas.Admin.Interfaces
{
    public interface ISeedService
    {
        List<RevenueDepartmentVM> GetRevenueDepartment();
        bool CreateRevenueDepartment(RevenueDepartmentVM vmodel);
        RevenueDepartmentVM GetRevenueDepartment(int ID);
        bool EditRevenueDepartment(RevenueDepartmentVM vmodel);
        bool DeleteRevenueDepartment(int ID);

        List<ServiceDepartmentVM> GetServiceDepartment();
        bool CreateServiceDepartment(ServiceDepartmentVM vmodel);
        ServiceDepartmentVM GetServiceDepartment(int ID);
        bool EditServiceDepartment(ServiceDepartmentVM vmodel);
        bool DeleteServiceDepartment(int ID);


        List<PriviledgeVM> GetPriviledges();
        bool CreatePriviledge(PriviledgeVM vmodel);
        PriviledgeVM GetPriviledge(int ID);
        bool EditPriviledge(PriviledgeVM vmodel);
        bool DeletePriviledge(int ID);
        List<TemplateVM> GetTemplates(int serviceDepartmentID);
        bool CreateTemplate(TemplateVM vmodel);
        TemplateVM GetTemplate(int ID);
        bool EditTemplate(TemplateVM vmodel);
        bool DeleteTemplate(int ID);


        List<VendorVM> GetVendors();
        bool CreateVendors(VendorVM vmodel);
        VendorVM GetVendor(int ID);
        bool EditVendor(VendorVM vmodel);
        bool DeleteVendor(int ID);


        List<ServiceVM> GetServices(ServiceVM vmodel);
        bool CreateService(ServiceVM vmodel);
        ServiceVM GetService(int ID);
        ServiceVM GetService(string servicename);
        bool EditService(ServiceVM vmodel);
        bool DeleteService(int ID);
        List<ServiceVM> GetServiceAutoComplete(string query);
        List<string> GetServiceNameAutoComplete(string term);
        List<string> GetSpecimenAutoComplete(string term);
        List<string> GetTemplateAutoComplete(string term);
        List<SpecimenVM> GetSpecimens();
        bool CreateSpecimen(SpecimenVM vmodel);
        SpecimenVM GetSpecimen(int ID);
        bool EditSpecimen(SpecimenVM vmodel);
        bool DeleteSpecimen(int ID);


        List<OrganismVM> GetOrganisms();
        bool CreateOrganism(OrganismVM vmodel);
        OrganismVM GetOrganism(int ID);
        bool EditOrganism(OrganismVM vmodel);
        bool DeleteOrganism(int ID);


        List<AntiBioticVM> GetAntiBiotics(int OrganismID);
        bool CreateAnitBiotic(AntiBioticVM vmodel);
        AntiBioticVM GetAntiBiotic(int ID);
        AntiBioticVM GetAntiBioticByOrganismName(string organismName);
        bool EditAntiBiotic(AntiBioticVM vmodel);
        bool DeleteAntiBiotic(int ID);
        List<string> GetOrganismAutoComplete(string term);
    }
}
