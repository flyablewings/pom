using System.Collections.Generic;
using POMQC.Entities.Base;
using POMQC.Entities.Defect;

namespace POMQC.Services.Defect
{
    public interface IDefectService
    {
        IList<DefectCategory> DefectCategories();

        IList<DefectCode> DefectCodes();

        IList<DefectLocation> DefectionLocations();

        Result InsertCode(DefectCode item);

        Result UpdateCode(DefectCode item);

        Result InsertCategory(DefectCategory item);

        Result UpdateCategory(DefectCategory item);

        Result InsertLocation(DefectLocation item);

        Result UpdateLocation(DefectLocation item);

        DefectCodeMultiColumns DefectMultiColumns();
    }
}