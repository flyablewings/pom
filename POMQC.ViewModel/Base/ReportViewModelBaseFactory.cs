using System.Collections.Generic;
using System.Linq;
using POMQC.Entities.Defect;
using POMQC.ViewModels.Filter;
using POMQC.Entities.Base;

namespace POMQC.ViewModels.Base
{
    public class ReportViewModelBaseFactory
    {
        public ReportViewModelBaseFactory()
        {
            Items = new List<DefectDetail>();
            Factories = new List<IGrouping<string, DefectDetail>>();
            DHUs = new List<IGrouping<DHUType, DefectDetail>>();
        }

        public string Title { get; set; }

        public FunctionViewModel Function { get; set; }

        public IList<DefectDetail> Items { get; set; }

        public IEnumerable<IGrouping<string, DefectDetail>> Factories { get; set; }

        public IEnumerable<IGrouping<DHUType, DefectDetail>> DHUs { get; set; }
    }
}