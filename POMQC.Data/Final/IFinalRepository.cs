using System;
using System.Collections.Generic;
using POMQC.Entities.Base;
using POMQC.Entities.Final;

namespace POMQC.Data.Final
{
    public interface IFinalRepository
    {
        IList<FinalEntity> Select(long custId, string custPO, string aiglPO, DateTime? date = null, int type = 0);

        IList<FinalEntity> SelectAll(DateTime from, DateTime to);

        IList<FinalCustPOEntity> Select(string aiglPO, long FinalId);

        Result Insert(FinalEntity entity);

        Result Update(FinalEntity entity);

        Result InsertDetail(FinalCustPOEntity entity);

        Result UpdateDetail(FinalCustPOEntity entity);

        void Delete(long finalId, string img);
    }
}