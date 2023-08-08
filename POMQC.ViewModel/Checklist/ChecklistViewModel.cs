using System.Collections.Generic;
using POMQC.Entities.Base;
using POMQC.ViewModels.Base;
using POMQC.ViewModels.Dashboard;
using POMQC.ViewModels.Filter;

namespace POMQC.ViewModels.Checklist
{
    public class ChecklistViewModel : ViewModelBase
    {
        public ChecklistViewModel()
        {
            Images = new List<string>();
            Documents = new List<string>();
            DroppedFiles = new List<DroppedImage>();
            POs = new List<POInfoViewModel>();
            Items = new List<ChecklistViewModel>();
            Filter = new FilterViewModel();
        }

        public string Doc { get; set; }

        public string Comment { get; set; }

        public bool IsActiveTab { get; set; }

        public string Title { get; set; }

        public ChecklistFunctionViewModel Function { get; set; }

        public FilterViewModel Filter { get; set; }

        public IList<string> Images { get; set; }

        public IList<string> Documents { get; set; }

        public IList<DroppedImage> DroppedFiles { get; set; }

        public IList<POInfoViewModel> POs { get; set; }

        public IList<ChecklistViewModel> Items { get; set; }

        public bool IsUpdate { get; set; }
    }
}