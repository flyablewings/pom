using POMQC.Entities.Base;

namespace POMQC.Entities.Defect
{
    public class DefectLocation : EntityBase
    {
        public int LocId { get; set; }

        public string LocName { get; set; }

        public bool Active { get; set; }
    }
}