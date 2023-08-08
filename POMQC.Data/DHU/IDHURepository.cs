using System;
using System.Collections.Generic;
using POMQC.Entities.Base;
using POMQC.Entities.DHU;

namespace POMQC.Data.DHU
{
    public interface IDHURepository
    {
        IList<DHUEntity> Select(long custId, string custPO, string aiglPO, DateTime? date = null, int type = 0);

        Result Insert(DHUEntity entity);

        Result Update(DHUEntity entity);

        void Delete(long dhuId, string img);
    }
}