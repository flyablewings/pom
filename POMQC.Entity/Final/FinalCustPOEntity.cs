namespace POMQC.Entities.Final
{
    public class FinalCustPOEntity
    {
        public string CustPO { get; set; }

        public string Style { get; set; }

        public string Color { get; set; }

        public int OrderQuantity { get; set; }

        public string AIGLPO { get; set; }

        public int InspectedQty { get; set; }

        public int ActualProductQty { get; set; }

        public long FinalDetailId { get; set; }

        public long FinalId { get; set; }
    }
}