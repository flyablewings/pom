using System.Collections.Generic;
using POMQC.Entities.Defect;
using POMQC.ViewModels.Filter;

namespace POMQC.ViewModels.Base
{
    public class ReportViewModelBase
    {
        public ReportViewModelBase()
        {
            Items = new List<DefectDetail>();
        }

        public string Title { get; set; }

        public FunctionViewModel Function { get; set; }

        public IList<DefectDetail> Items { get; set; }
    }
}