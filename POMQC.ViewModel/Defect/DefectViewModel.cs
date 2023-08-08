using System.Collections.Generic;
using POMQC.Entities.Defect;

namespace POMQC.ViewModels.Defect
{
    public class DefectViewModel
    {
        public IList<DefectCategory> Categories { get; set; }

        public IList<DefectCode> Codes { get; set; }

        public IList<DefectLocation> Locations { get; set; }
    }
}