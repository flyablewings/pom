using POMQC.Entities.Base;

namespace POMQC.Entities.Defect
{
    public class DefectDetail : DefectCode
    {
        public DHUType DHUType { get; set; }
        
        public int LocId { get; set; }

        public int PCSQty { get; set; }

        public int Total { get; set; }

        public long DdefId { get; set; }

        public string LocName { get; set; }
    }
}