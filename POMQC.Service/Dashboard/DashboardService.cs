using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POMQC.Data.Dashboard;
using POMQC.ViewModels.Dashboard;
using POMQC.ViewModels.Filter;

namespace POMQC.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository = new DashboardRepository();

        #region IDashboardService Members

        public IList<POInfoViewModel> POs(DateTime from, DateTime to, long custId = 0, string custPO = "", string aiglPO = "", string status = "")
        {
            var allPOStatus = string.IsNullOrWhiteSpace(status) || status == "All" ? true : false;
            var result = new List<POInfoViewModel>();
            var POs = _repository.POs(from, to, custId, custPO, aiglPO);

            POs = allPOStatus ? POs : POs.Where(p => p.Status.Trim().Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (var po in POs)
            {
                result.Add(new POInfoViewModel  
                {
                    CustId = po.CustId,
                    AIGLPO = po.AIGLPO,
                    Customer = po.Customer,
                    CustPO = po.CustPO,
                    Defect = po.Defect,
                    Division = po.Division,
                    Factory = po.Factory,
                    InlineDate = po.InlineDate,
                    Status = po.Status,
                    POQty = po.POQty,
                    FactoryId = po.FactoryId
                });
            }

            return result;
        }

        #endregion
    }
}
