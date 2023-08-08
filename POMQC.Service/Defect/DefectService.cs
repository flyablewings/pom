using System.Collections.Generic;
using POMQC.Data.Defect;
using POMQC.Entities.Base;
using POMQC.Entities.Defect;

namespace POMQC.Services.Defect
{
    public class DefectService : IDefectService
    {
        private readonly IDefectRepository _defect = new DefectRepository();

        #region IDefectService Members

        public IList<DefectCode> DefectCodes()
        {
            return _defect.DefectCodes();
        }

        public IList<DefectLocation> DefectionLocations()
        {
            return _defect.DefectionLocations();
        }

        public IList<DefectCategory> DefectCategories()
        {
            return _defect.DefectCategories();
        }

        public Result InsertCode(DefectCode item)
        {
            return _defect.InsertCode(item);
        }

        public Result UpdateCode(DefectCode item)
        {
            return _defect.UpdateCode(item);
        }

        public Result InsertCategory(DefectCategory item)
        {
            return _defect.InsertCategory(item);
        }

        public Result UpdateCategory(DefectCategory item)
        {
            return _defect.UpdateCategory(item);
        }

        public Result InsertLocation(DefectLocation item)
        {
            return _defect.InsertLocation(item);
        }

        public Result UpdateLocation(DefectLocation item)
        {
            return _defect.UpdateLocation(item);
        }

        public DefectCodeMultiColumns DefectMultiColumns()
        {
            var result = new DefectCodeMultiColumns();
            var defects = _defect.DefectCodes();
            foreach (var item in defects)
            {
                result.data.Add(new DefectData
                {
                    category = item.CatName,
                    description = item.DefName,
                    text = item.DefCode,
                    value = item.DefId.ToString()
                });
            }

            return result;
        }

        #endregion
    }
}