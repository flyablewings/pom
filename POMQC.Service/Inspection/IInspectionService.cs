using System;
using POMQC.Entities.Base;
using POMQC.ViewModels.Base;
using POMQC.ViewModels.Inspection;

namespace POMQC.Services.Inspection
{
    public interface IInspectionService
    {
        InspectionViewModel Select(long custId, string custPO, string aiglPO, DateTime? date = null, int type = 0);

        Result InsertDHU(DHUViewModel dhu);

        Result UpdateDHU(DHUViewModel dhu);

        Result InsertFinal(FinalViewModel final);

        Result UpdateFinal(FinalViewModel final);

        void DeleteDHU(long dhuId, string img);

        void DeleteFinal(long finalId, string img);
    }
}