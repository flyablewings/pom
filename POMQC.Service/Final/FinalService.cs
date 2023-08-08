using System;
using System.Collections.Generic;
using System.Linq;
using POMQC.Data.Defect;
using POMQC.Data.Final;
using POMQC.Utilities;
using POMQC.ViewModels.Base;
using POMQC.ViewModels.Inspection;
using POMQC.Entities.Final;

namespace POMQC.Services.Final
{
    public class FinalService : IFinalService
    {
        private readonly IFinalRepository _final = new FinalRepository();
        private readonly IDefectRepository _defect = new DefectRepository();

        #region IFinalService Members

        public IList<FinalViewModel> Select(long custId, string custPO, string aiglPO, DateTime? date = null, int type = 0)
        {
            var result = new List<FinalViewModel>();
            var data = _final.Select(custId, custPO, aiglPO, date, type);
            var defects = _defect.DefectDetails(custId, custPO, aiglPO, type).Where(i => i.DefId != 0 && i.LocId != 0);
            var finalItem = data.FirstOrDefault() ?? new FinalEntity();

            foreach (var item in data)
            {
                result.Add(new FinalViewModel
                {
                    Item = item,
                    Items = data.Where(i => i.Type == item.Type).ToList(),
                    Function = (InspectionFunctionViewModel)item.Type.ConvertTo<int>(),
                    Defects = defects.Where(d => d.DHUType == item.Type && d.InspectionId == item.InspectionId).ToList(),
                    POs = _final.Select(aiglPO, item.FinalId),
                    DateRange = new EditorModel { From = new DatePickerViewModel { Date = item.ActualInspectionDate == "01/01/0001 00:00:00.000" ? DateTime.Now : Convert.ToDateTime(item.ActualInspectionDate) } }
                });
            }

            return result;
        }

        public IList<FinalCustPOEntity> Select(string aiglPO, long FinalId)
        {
            return _final.Select(aiglPO, FinalId);
        }

        public IList<FinalEntity> SelectAll(DateTime from, DateTime to)
        {
            return _final.SelectAll(from, to);
        }

        #endregion
    }
}