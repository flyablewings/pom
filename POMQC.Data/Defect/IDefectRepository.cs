using System.Collections.Generic;
using POMQC.Entities.Base;
using POMQC.Entities.Defect;

namespace POMQC.Data.Defect
{
    public interface IDefectRepository
    {
        IList<DefectCategory> DefectCategories();

        IList<DefectCode> DefectCodes();

        IList<DefectLocation> DefectionLocations();

        IList<DefectDetail> DefectDetails(long custId, string custPO, string AIGLPO, int Type);

        Result InsertCode(DefectCode item);

        Result UpdateCode(DefectCode item);

        Result InsertDetail(DefectDetail item);

        Result UpdateDetail(DefectDetail item);

        Result InsertCategory(DefectCategory item);

        Result UpdateCategory(DefectCategory item);

        Result InsertLocation(DefectLocation item);

        Result UpdateLocation(DefectLocation item);        
    }
}