using System.Collections.Generic;
using POMQC.Entities.Final;
using POMQC.ViewModels.Filter;

namespace POMQC.ViewModels.Base
{
    public class ReportFinalViewModel
    {
        public FunctionViewModel Function { get; set; }

        public string Title { get; set; }

        public IList<FinalEntity> Items { get; set; }
    }
}