using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class TemplateServiceCompuationVM
    {
        public int Id { get; set; }
        public int ServiceID { get; set; }
        public string Service { get; set; }
        public string Parameter { get; set; }
        public string Labnote { get; set; }
        public string ScientificComment { get; set; }
        public string Range { get; set; }
        public string Status { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public int PreparedByID { get; set; }
        public string PreparedBy { get; set; }
        public List<ServiceParameterAndRange> Parameters { get; set; }
    }
    public class ServiceParameterAndRange
    {
        public ServiceParameterSetupVM Parameter { get; set; }
        public List<ServiceParameterRangeSetupVM> Ranges { get; set; }
    }
}