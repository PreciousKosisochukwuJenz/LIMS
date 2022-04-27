using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class ServiceParameterSetupVM
    {
        public int Id { get; set; }
        public int? ServiceParameterID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public string Range { get; set; }
        public string Labnote { get; set; }
        public string ScientificComment { get; set; }
        public string FilmingReport { get; set; }
        public int Rank { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public string ServiceParameter { get; set; }
    }
}