using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class ApplicationSettingsRecord
    {
        public int Id { get; set; }
        public string AppName { get; set; }
        public byte[] Logo { get; set; }
        public byte[] Watermark { get; set; }
        public bool AllowSetupRouteAccess { get; set; }
        public int CodeGenSeed { get; set; }
        public bool EnablePartPayment { get; set; }
        public bool EnableSpecimentCollectedBy { get; set; }
        public string CustomerNumberPrefix { get; set; }
        public int SalesRecieptCopyCount { get; set; }
        public int BillCount { get; set; }
        public int ShiftCount { get; set; }
        public int PaymentCount { get; set; }
        public int DepositeCount { get; set; }
        public int LabCount { get; set; }
        public int SessionTimeOut { get; set; }
        public bool ExpressWaiver { get; set; }
    }
}
