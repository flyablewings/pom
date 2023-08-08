using System.Collections.Generic;
using System.Linq;
using POMQC.Entities.Base;
using POMQC.Entities.Defect;
using POMQC.ViewModels.Base;
using POMQC.ViewModels.Checklist;
using POMQC.ViewModels.Filter;

namespace POMQC.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public FilterViewModel Filter { get; set; }

        public IList<POInfoViewModel> POs { get; set; }

        public ChecklistViewModel FitSample { get; set; }

        public ChecklistViewModel PPSample { get; set; }

        public ChecklistViewModel TopSample { get; set; }

        public ChecklistViewModel QAChecklist { get; set; }

        public ChecklistViewModel PPMeeting { get; set; }

        public ChecklistViewModel Fabric { get; set; }

        public DHUViewModel Cutting { get; set; }

        public DHUViewModel Inline { get; set; }

        public DHUViewModel Endline { get; set; }

        public DHUViewModel Finishing { get; set; }

        public DHUViewModel Packing { get; set; }

        public FinalViewModel Prefinal { get; set; }

        public FinalViewModel Final { get; set; }

        public ReportViewModelBase Summary { get; set; }

        public IEnumerable<IGrouping<DHUType, DefectDetail>> ReportItems { get; set; }

        public string ReportTitle { get; set; }

        public IList<string> Styles { get; set; }
    }
}