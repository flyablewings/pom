using System.Collections.Generic;
using POMQC.Entities.Defect;
using POMQC.Entities.Final;
using POMQC.ViewModels.Inspection;

namespace POMQC.ViewModels.Base
{
    public class FinalViewModel
    {
        public FinalViewModel()
        {
            Items = new List<FinalEntity>();
            Item = new FinalEntity();
            DefectCodes = new List<DefectCode>();
            DefectLocations = new List<DefectLocation>();
            Defects = new List<DefectDetail>();
            POs = new List<FinalCustPOEntity>();
            DateRange = new EditorModel();
        }

        public IList<FinalEntity> Items { get; set; }

        public FinalEntity Item { get; set; }
        
        public InspectionFunctionViewModel Function { get; set; }

        public string Title { get; set; }

        public bool IsReadOnly { get; set; }

        public IList<DefectCode> DefectCodes { get; set; }

        public IList<DefectLocation> DefectLocations { get; set; }

        public IList<DefectDetail> Defects { get; set; }

        public IList<FinalCustPOEntity> POs { get; set; }

        public EditorModel DateRange { get; set; }
    }
}