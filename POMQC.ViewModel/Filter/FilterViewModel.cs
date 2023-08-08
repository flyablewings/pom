using System.Collections.Generic;
using POMQC.ViewModels.Base;

namespace POMQC.ViewModels.Filter
{
    public class FilterViewModel
    {
        public FilterViewModel()
        {
            Factories = new List<FactoryBase>();
            Customers = new List<CustomerViewModel>();
            CustPOs = new List<POViewModel>();
            AIGLPOs = new List<POViewModel>();
            DateRange = new EditorModel { Function = Function };
            ShowStatus = true;
        }

        public bool ShowAllLabel { get; set; }

        public bool ShowViewButton { get; set; }

        public bool ShowNewButton { get; set; }

        public bool ShowDateRange { get; set; }

        public bool ShowStatus { get; set; }

        public bool ShowFactory { get; set; }

        public EditorModel DateRange { get; set; }

        public IList<FactoryBase> Factories { get; set; }

        public IList<CustomerViewModel> Customers { get; set; }

        public IList<POViewModel> CustPOs { get; set; }

        public IList<POViewModel> AIGLPOs { get; set; }

        public FunctionViewModel Function { get; set; }
    }
}