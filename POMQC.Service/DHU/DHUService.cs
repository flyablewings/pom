using System;
using System.Collections.Generic;
using System.Linq;
using POMQC.Data.Defect;
using POMQC.Data.DHU;
using POMQC.ViewModels.Base;
using POMQC.ViewModels.Inspection;
using POMQC.Utilities;

namespace POMQC.Services.DHU
{
    public class DHUService : IDHUService
    {
        private readonly IDHURepository _dhu = new DHURepository();
        private readonly IDefectRepository _defect = new DefectRepository();

        #region IDHUService Members

        public IList<DHUViewModel> Select(long custId, string custPO, string aiglPO, DateTime? date = null, int type = 0)
        {
            var result = new List<DHUViewModel>();
            var data = _dhu.Select(custId, custPO, aiglPO, date, type);
            var defects = _defect.DefectDetails(custId, custPO, aiglPO, type).Where(i => i.DefId != 0 && i.LocId != 0);

            foreach (var item in data)
            {
                result.Add(new DHUViewModel
                {
                    Function = (InspectionFunctionViewModel)item.DHUType.ConvertTo<int>(),
                    Item = item,
                    Items = data.Where(i => i.DHUType == item.DHUType).ToList(),
                    Defects = defects.Where(d => d.DHUType == item.DHUType && d.InspectionId == item.InspectionId).ToList()
                });
            }
            
            return result;
        }

        #endregion
    }
}