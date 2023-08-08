using System.Collections.Generic;
using POMQC.Entities.Defect;
using POMQC.Entities.DHU;
using POMQC.ViewModels.Inspection;
using POMQC.Entities.Base;

namespace POMQC.ViewModels.Base
{
    public class DHUViewModel
    {
        public DHUViewModel()
        {
            Items = new List<DHUEntity>();
            Item = new DHUEntity();
            DefectCodes = new List<DefectCode>();
            DefectLocations = new List<DefectLocation>();
            Defects = new List<DefectDetail>();
            DefectCodeMultiColumns = new DefectCodeMultiColumns();
        }

        public IList<DefectCode> DefectCodes { get; set; }

        public IList<DefectLocation> DefectLocations { get; set; }

        public IList<DHUEntity> Items { get; set; }

        public IList<int> PCSQuantities { get; set; }

        public IList<DefectDetail> Defects { get; set; }

        public DHUEntity Item { get; set; }
        
        public InspectionFunctionViewModel Function { get; set; }

        public DefectCodeMultiColumns DefectCodeMultiColumns { get; set; }

        public string Title { get; set; }

        public bool IsReadOnly { get; set; }
    }
}