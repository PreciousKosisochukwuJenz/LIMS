﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class ServiceParameterVM
    {
        public int Id { get; set; }
        public int? ServiceID { get; set; }
        public int? SpecimenID { get; set; }
        public int? TemplateID { get; set; }
        public bool RequireApproval { get; set; }
        public string Service { get; set; }
        public string Specimen { get; set; }
        public string Template { get; set; }
        public string BillNumber { get; set; }
        public bool Templated { get; set; }
        public bool HasBeenComputed { get; set; }
        public bool Approved { get; set; }
        public string DateComputed { get; set; }
        public string DateApproved { get; set; }
        public string ComputedBy { get; set; }
        public string ApprovedBy { get; set; }
        public List<int> ComputedServiceIds { get; set; }
    }
}