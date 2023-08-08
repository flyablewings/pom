using System.Collections.Generic;
using System.Linq;
using POMQC.Entities.Defect;
using POMQC.Entities.Final;
using POMQC.Services.Final;
using POMQC.ViewModels.Base;
using POMQC.ViewModels.Checklist;
using POMQC.ViewModels.Inspection;

namespace POMQC.Services.Base
{
    public class ServiceBase
    {
        private IFinalService _final = new FinalService();

        protected ChecklistViewModel GetChecklistViewModel(IList<ChecklistViewModel> items, ChecklistFunctionViewModel function, string title, bool isActiveTab = false)
        {
            var result = items.FirstOrDefault(i => i.Function == function);
            result = result ?? new ChecklistViewModel();
            result.Function = function;
            result.Title = title;
            result.IsActiveTab = isActiveTab;

            return result;
        }

        protected DHUViewModel GetDHUViewModel(IList<DHUViewModel> items, InspectionFunctionViewModel function, string title, IList<DefectCode> codes = null, IList<DefectLocation> locs = null, bool isReadOnly = false)
        {
            var result = items.FirstOrDefault(i => i.Function == function);
            result = result ?? new DHUViewModel();
            result.Function = function;
            result.Title = title;
            result.IsReadOnly = isReadOnly;
            result.DefectCodes = codes;
            result.DefectLocations = locs;

            return result;
        }

        protected FinalViewModel GetFinalViewModel(IList<FinalViewModel> items, InspectionFunctionViewModel function, IList<DefectCode> codes = null, IList<DefectLocation> locs = null, bool isReadOnly = false, IList<FinalCustPOEntity> pos = null)
        {
            var result = items.FirstOrDefault(i => i.Function == function);
            result = result ?? new FinalViewModel();
            var item =  items.FirstOrDefault() ?? new FinalViewModel();
            var aiglPO = item.Item.AIGLPO == null ? (pos == null ? string.Empty : pos.FirstOrDefault() == null ? string.Empty : pos.FirstOrDefault().AIGLPO) : item.Item.AIGLPO;
            var styles = _final.Select(aiglPO ?? string.Empty, result.Item.FinalId);
            result.Function = function;
            result.IsReadOnly = isReadOnly;
            result.DefectCodes = codes;
            result.DefectLocations = locs;
            result.POs = styles;

            return result;
        }
    }
}