using POMQC.Entities.Base;

namespace POMQC.Entities.Defect
{
    public class DefectCategory : EntityBase
    {
        public int CatId { get; set; }

        public string CatName { get; set; }

        public bool Active { get; set; }
    }
}