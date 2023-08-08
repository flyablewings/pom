using POMQC.ViewModels.Filter;

namespace POMQC.ViewModels.Dashboard
{
    public class POInfoViewModel
    {
        public long CustId { get; set; }

        public string AIGLPO { get; set; }

        public string CustPO { get; set; }

        public string Customer { get; set; }

        public int FactoryId { get; set; }

        public string Factory { get; set; }

        public string Division { get; set; }

        public int Defect { get; set; }

        public FunctionViewModel Function { get; set; }

        public string InlineDate { get; set; }

        public string Status { get; set; }

        public int POQty { get; set; }
    }
}
