using JenzHealth.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JenzHealth.Areas.Admin.ViewModels
{
    public class NonTemplatedLabPreparationVM
    {
        public int Id { get; set; }
        public string BillInvoiceNumber { get; set; }
        public string Appearance { get; set; }
        public string Color { get; set; }
        public string MacrosopyBlood { get; set; }
        public string AdultWarm { get; set; }
        public string Mucus { get; set; }
        public string SpecificGravity { get; set; }
        public string Acidity { get; set; }
        public string Glucose { get; set; }
        public string Protein { get; set; }
        public string Niterite { get; set; }
        public string Ketones { get; set; }
        public string Blirubin { get; set; }
        public string Urobilinogen { get; set; }
        public string AscorbicAcid { get; set; }
        public string DipstickBlood { get; set; }
        public string LeucocyteEsterase { get; set; }
        public string Temperature { get; set; }
        public string Duration { get; set; }
        public string Atomsphere { get; set; }
        public string Plate { get; set; }
        public string Incubatio { get; set; }
        public string Labnote { get; set; }

        public MicroscopyType MicroscopyType { get; set; }
        public StainType StainType { get; set; }

        public string PusCells { get; set; }
        public string WhiteBloodCells { get; set; }
        public string RedBloodCells { get; set; }
        public string WetMountBacteria { get; set; }
        public string WetMountYesats { get; set; }
        public string Casts { get; set; }
        public string Cystals { get; set; }
        public string Ova { get; set; }
        public string WetMountParasite { get; set; }
        public string EpithelialCells { get; set; }
        public string Protozoa { get; set; }
        public string WetMountOthers { get; set; }
        public string GramPositiveCocci { get; set; }
        public string GramPositiveRods { get; set; }
        public string TrichomonasVaginalis { get; set; }
        public string YeastCells { get; set; }
        public string GramNegativeCocci { get; set; }
        public string GramNegativeRods { get; set; }
        public string VincetsOrganisms { get; set; }
        public string GiemsaStainParasite { get; set; }
        public string GiemsaOthers { get; set; }
        public string AcisFastBacilli { get; set; }
        public string ZiehlOthers { get; set; }
        public string IndiaInkResult { get; set; }
        public string IodineResult { get; set; }
        public string MethyleneResult { get; set; }
        public string OthersResult { get; set; }
        public string DurationOfAbstinence { get; set; }
        public DateTime? DateOfProduction { get; set; }
        public DateTime? TimeRecieved { get; set; }
        public DateTime? TimeOfProduction { get; set; }
        public string ModeOfProduction { get; set; }
        public string Viscosity { get; set; }
        public DateTime? TimeExamined { get; set; }
        public string Morphology { get; set; }
        public string Motility { get; set; }
        public int? TotalSpermCount { get; set; }
        public string ImmatureCells { get; set; }
        public string WBCS { get; set; }
        public string AnySpillage { get; set; }
        public string RBCS { get; set; }
        public string PH { get; set; }
        public string KOHPrepareation { get; set; }
        public string KOHResult { get; set; }
        public string OtherStainResult { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
