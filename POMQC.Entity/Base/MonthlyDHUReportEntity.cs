namespace POMQC.Entities.Base
{
    public class MonthlyDHUReportEntity
    {
        public string FactoryName { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int OutputQty { get; set; }

        public int InspectionQty { get; set; }

        public int ProductQty { get; set; }

        public int ProductPercent { get; set; }

        public int DefectQty { get; set; }

        public int DefectPercent { get; set; }
    }
}