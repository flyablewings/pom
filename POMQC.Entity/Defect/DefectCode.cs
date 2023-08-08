using System.Collections.Generic;
using POMQC.Entities.Base;

namespace POMQC.Entities.Defect
{
    public class DefectCode : EntityBase
    {
        public int DefId { get; set; }

        public int CatId { get; set; }

        public string DefCode { get; set; }

        public string DefName { get; set; }

        public string DefVN { get; set; }

        public DefectType Type { get; set; }

        public int Defect { get; set; }

        public string CatName { get; set; }

        public bool Active { get; set; }

        public IList<DefectCategory> Categories { get; set; }
    }
}