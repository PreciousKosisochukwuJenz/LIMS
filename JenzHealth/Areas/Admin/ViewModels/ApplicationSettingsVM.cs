using System;
using System.Collections.Generic;
using System.Linq;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class ApplicationSettingsVM
    {
        public int ID { get; set; }
        public string AppName { get; set; }
        public byte[] Logo { get; set; }
        public bool EnablePartPayment { get; set; }
        public bool EnableSpecimentCollectedBy { get; set; }
        public string CustomerNumberPrefix { get; set; }
        public int SalesRecieptCopyCount { get; set; }
        public int CodeGenSeed { get; set; }
        public int BillCount { get; set; }
        public int ShiftCount { get; set; }
        public int PaymentCount { get; set; }
        public int DepositeCount { get; set; }
        public int LabCount { get; set; }
        public int SessionTimeOut { get; set; }

    }
}