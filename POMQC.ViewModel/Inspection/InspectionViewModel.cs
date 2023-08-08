using System.Collections.Generic;
using POMQC.ViewModels.Base;
using POMQC.ViewModels.Checklist;
using POMQC.ViewModels.Dashboard;
using POMQC.ViewModels.Filter;

namespace POMQC.ViewModels.Inspection
{
    public class InspectionViewModel : ViewModelBase
    {
        public InspectionViewModel()
        {
            POs = new List<POInfoViewModel>();
            Filter = new FilterViewModel() { Function = FunctionViewModel.Inspection };
            Fabric = new ChecklistViewModel() { Function = ChecklistFunctionViewModel.Fabric };
            Cutting = new DHUViewModel() { Function = InspectionFunctionViewModel.Cutting };
            Inline = new DHUViewModel() { Function = InspectionFunctionViewModel.Inline };
            Endline = new DHUViewModel() { Function = InspectionFunctionViewModel.Endline };
            Finishing = new DHUViewModel() { Function = InspectionFunctionViewModel.Finishing };
            Packing = new DHUViewModel() { Function = InspectionFunctionViewModel.Packing };
            Prefinal = new FinalViewModel() { Function = InspectionFunctionViewModel.Prefinal };
            Final = new FinalViewModel() { Function = InspectionFunctionViewModel.Final };
            Styles = new List<string>();
        }

        public IList<POInfoViewModel> POs { get; set; }

        public FilterViewModel Filter { get; set; }

        public ChecklistViewModel Fabric { get; set; }

        public DHUViewModel Cutting { get; set; }

        public DHUViewModel Inline { get; set; }

        public DHUViewModel Endline { get; set; }

        public DHUViewModel Finishing { get; set; }

        public DHUViewModel Packing { get; set; }

        public FinalViewModel Prefinal { get; set; }

        public FinalViewModel Final { get; set; }

        public IList<string> Styles { get; set; }
    }
}