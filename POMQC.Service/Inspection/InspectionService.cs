using System;
using System.Collections.Generic;
using System.Linq;
using POMQC.Data.Defect;
using POMQC.Data.DHU;
using POMQC.Data.Final;
using POMQC.Entities.Base;
using POMQC.Entities.Defect;
using POMQC.Services.Base;
using POMQC.Services.Checklist;
using POMQC.Services.Filter;
using POMQC.Utilities;
using POMQC.ViewModels.Base;
using POMQC.ViewModels.Checklist;
using POMQC.ViewModels.Filter;
using POMQC.ViewModels.Inspection;
using POMQC.Services.Defect;
using POMQC.Entities.Final;

namespace POMQC.Services.Inspection
{
    public class InspectionService : ServiceBase, IInspectionService
    {
        private readonly IChecklistService _checklist = new ChecklistService();
        private readonly IDHURepository _dhu = new DHURepository();
        private readonly IFinalRepository _final = new FinalRepository();
        private readonly IFilterService _filter = new FilterService();
        private readonly IDefectRepository _defect = new DefectRepository();
        private readonly IDefectService _defService = new DefectService();

        #region IInspectionService Members

        public InspectionViewModel Select(long custId, string custPO, string aiglPO, DateTime? date = null, int type = 0)
        {
            var result = new InspectionViewModel();
            var filter = _filter.Filters(true, true, true, FunctionViewModel.Inspection, true);
            var dhu = _dhu.Select(custId, custPO, aiglPO);
            var final = _final.Select(custId, custPO, aiglPO);
            var dhus = new List<DHUViewModel>();
            var finals = new List<FinalViewModel>();
            var defectCodes = _defect.DefectCodes();
            var defectLocations = _defect.DefectionLocations();
            var defectDetail = _defect.DefectDetails(custId, custPO, aiglPO, 0).Where(i => i.DefId != 0 && i.LocId != 0);
            var finalItem = final.FirstOrDefault() ?? new FinalEntity();
            var pos = _final.Select(aiglPO, finalItem.FinalId);
            var checklist = _checklist.Select(custId, custPO, aiglPO, (int)ChecklistType.Fabric);

            defectCodes.Insert(0, new DefectCode { DefId = 0, DefName = "None", Active = true });
            defectLocations.Insert(0, new DefectLocation { LocId = 0, LocName = "None", Active = true });

            foreach (var item in dhu)
            {
                dhus.Add(new DHUViewModel
                {
                    DefectCodeMultiColumns = _defService.DefectMultiColumns(),
                    Function = (InspectionFunctionViewModel)item.DHUType.ConvertTo<int>(),
                    Item = item,
                    Items = dhu.Where(i => i.DHUType == item.DHUType).ToList(),
                    Defects = defectDetail
                        .Where(d => d.DHUType == item.DHUType && d.InspectionId == item.InspectionId).ToList()
                });
            }

            foreach (var item in final)
            {
                finals.Add(new FinalViewModel
                {
                    Function = (InspectionFunctionViewModel)item.Type.ConvertTo<int>(),
                    Item = item,
                    POs = _final.Select(aiglPO, item.FinalId),
                    Items = final.Where(i => i.Type == item.Type).ToList(),
                    Defects = defectDetail
                        .Where(d => d.DHUType == item.Type && d.InspectionId == item.InspectionId).ToList()
                });
            }

            result.Filter = filter;
            result.Fabric = GetChecklistViewModel(checklist, ChecklistFunctionViewModel.Fabric, "FABRIC INSPECTION", true);
            result.Cutting = GetDHUViewModel(dhus, InspectionFunctionViewModel.Cutting, "CUTTING", defectCodes, defectLocations, false);
            result.Inline = GetDHUViewModel(dhus, InspectionFunctionViewModel.Inline, "INLINE", defectCodes, defectLocations, false);
            result.Endline = GetDHUViewModel(dhus, InspectionFunctionViewModel.Endline, "ENDLINE", defectCodes, defectLocations, false);
            result.Finishing = GetDHUViewModel(dhus, InspectionFunctionViewModel.Finishing, "FINISHING", defectCodes, defectLocations, false);
            result.Packing = GetDHUViewModel(dhus, InspectionFunctionViewModel.Packing, "PACKING", defectCodes, defectLocations, false);
            result.Prefinal = GetFinalViewModel(finals, InspectionFunctionViewModel.Prefinal, defectCodes, defectLocations, false, pos);
            result.Final = GetFinalViewModel(finals, InspectionFunctionViewModel.Final, defectCodes, defectLocations, false, pos);

            var styles = pos.Select(i => string.Join(" - ", i.Style, i.Color, i.OrderQuantity)).ToList();
            result.Styles = styles;

            return result;
        }

        public Result InsertDHU(DHUViewModel dhu)
        {
            var result = _dhu.Insert(dhu.Item);
            if (result.IsSuccess && dhu.Item.DefectCodes.Trim().Length > 0)
            {
                var codes = dhu.Item.DefectCodes.Split(',');
                var locations = dhu.Item.DefectLocs.Split(',');
                var pcsQuantities = dhu.Item.PCSQuantities.Split(',');
                var totalDefects = dhu.Item.TotalDefects.Split(',');
                for (int i = 0; i < codes.Length; i++)
                {
                    if (codes[i].ConvertTo<int>() == 0)
                    {
                        continue;
                    }

                    var defectDetail = new DefectDetail
                    {
                        AgentId = dhu.Item.AgentId,
                        AIGLPO = dhu.Item.AIGLPO,
                        CustId = dhu.Item.CustId,
                        CustPO = dhu.Item.CustPO,
                        DefId = codes[i].ConvertTo<int>(),
                        LocId = locations[i].ConvertTo<int>(),
                        PCSQty = pcsQuantities[i].ConvertTo<int>(),
                        Total = totalDefects[i].ConvertTo<int>(),
                        FactoryId = dhu.Item.FactoryId,
                        DHUType = dhu.Item.DHUType,
                        InspectionId = result.Id
                    };

                    _defect.InsertDetail(defectDetail);
                }
            }

            return result;
        }

        public Result UpdateDHU(DHUViewModel dhu)
        {
            var result = _dhu.Update(dhu.Item);
            if (result.IsSuccess && dhu.Item.DefectCodes.Trim().Length > 0)
            {
                var codes = dhu.Item.DefectCodes.Split(',');
                var locations = dhu.Item.DefectLocs.Split(',');
                var pcsQuantities = dhu.Item.PCSQuantities.Split(',');
                var totalDefects = dhu.Item.TotalDefects.Split(',');
                var ids = dhu.Item.DefectIds.Split(',');
                for (int i = 0; i < totalDefects.Length; i++)
                {
                    var ddefid = 0L;
                    var locid = 0;
                    var defid = 0;
                    if (i < ids.Length)
                    {
                        ddefid = ids[i].ConvertTo<long>();
                    }

                    locid = locations[i].ConvertTo<int>();
                    defid = codes[i].ConvertTo<int>();

                    var defectDetail = new DefectDetail
                    {
                        AgentId = dhu.Item.AgentId,
                        AIGLPO = dhu.Item.AIGLPO,
                        CustId = dhu.Item.CustId,
                        CustPO = dhu.Item.CustPO,
                        DefId = codes[i].ConvertTo<int>(),
                        LocId = locations[i].ConvertTo<int>(),
                        PCSQty = pcsQuantities[i].ConvertTo<int>(),
                        Total = totalDefects[i].ConvertTo<int>(),
                        FactoryId = dhu.Item.FactoryId,
                        DHUType = dhu.Item.DHUType,
                        InspectionId = dhu.Item.DHUId,
                        CreatedDate = dhu.Item.CreatedDate,
                        DdefId = ddefid
                    };

                    if (ddefid != 0)
                    {
                        _defect.UpdateDetail(defectDetail);                        
                    }
                    else if (defid != 0 && locid != 0)
                    {
                        _defect.InsertDetail(defectDetail);
                    }
                }
            }

            return result;
        }

        public Result InsertFinal(FinalViewModel final)
        {
            final.Item.ActualInspectionDate =
                final.Item.ActualInspectionDate == "01/01/0001 00:00:00.000" ?
                DateTime.Now.ToShortDateString() : final.Item.ActualInspectionDate;

            var result = _final.Insert(final.Item);
            if (result.IsSuccess && final.Item.DefectCodes.Trim().Length > 0)
            {
                var codes = final.Item.DefectCodes.Split(',');
                var locations = final.Item.DefectLocs.Split(',');
                var totalDefects = final.Item.TotalDefects.Split(',');
                var ids = final.Item.DefectIds.Split(',');
                for (int i = 0; i < codes.Length; i++)
                {
                    if (codes[i].ConvertTo<int>() == 0)
                    {
                        continue;
                    }

                    var defectDetail = new DefectDetail
                    {
                        AgentId = final.Item.AgentId,
                        AIGLPO = final.Item.AIGLPO,
                        CustId = final.Item.CustId,
                        CustPO = final.Item.CustPO,
                        DefId = codes[i].ConvertTo<int>(),
                        LocId = locations[i].ConvertTo<int>(),
                        Total = totalDefects[i].ConvertTo<int>(),
                        FactoryId = final.Item.FactoryId,
                        DHUType = final.Item.Type,
                        InspectionId = result.Id
                    };

                    _defect.InsertDetail(defectDetail);
                }

                foreach (var po in final.POs)
                {
                    po.FinalId = result.Id;
                    _final.InsertDetail(po);
                }

            }

            return result;
        }

        public Result UpdateFinal(FinalViewModel final)
        {
            final.Item.ActualInspectionDate =
                final.Item.ActualInspectionDate == "01/01/0001 00:00:00.000" ?
                DateTime.Now.ToShortDateString() : final.Item.ActualInspectionDate;

            var result = _final.Update(final.Item);
            if (result.IsSuccess && final.Item.DefectCodes.Trim().Length > 0)
            {
                var codes = final.Item.DefectCodes.Split(',');
                var locations = final.Item.DefectLocs.Split(',');
                var totalDefects = final.Item.TotalDefects.Split(',');
                var ids = final.Item.DefectIds.Split(',');
                for (int i = 0; i < totalDefects.Length; i++)
                {
                    var ddefid = 0L;
                    var locid = 0;
                    var defid = 0;
                    if (i < ids.Length)
                    {
                        ddefid = ids[i].ConvertTo<long>();
                    }

                    locid = locations[i].ConvertTo<int>();
                    defid = codes[i].ConvertTo<int>();

                    var defectDetail = new DefectDetail
                    {
                        AgentId = final.Item.AgentId,
                        AIGLPO = final.Item.AIGLPO,
                        CustId = final.Item.CustId,
                        CustPO = final.Item.CustPO,
                        DefId = codes[i].ConvertTo<int>(),
                        LocId = locations[i].ConvertTo<int>(),
                        Total = totalDefects[i].ConvertTo<int>(),
                        FactoryId = final.Item.FactoryId,
                        DHUType = final.Item.Type,
                        InspectionId = final.Item.FinalId,
                        DdefId = ddefid
                    };

                    if (ddefid != 0)
                    {
                        _defect.UpdateDetail(defectDetail);
                    }
                    else if (defid != 0 && locid != 0)
                    {
                        _defect.InsertDetail(defectDetail);
                    }
                }

                foreach (var po in final.POs)
                {
                    if (po.FinalDetailId == 0)
                    {
                        _final.InsertDetail(po);
                    }
                    else
                    {
                        _final.UpdateDetail(po);
                    }
                }
            }

            return result;
        }

        public void DeleteDHU(long dhuId, string img)
        {
            _dhu.Delete(dhuId, img);
        }

        public void DeleteFinal(long finalId, string img)
        {
            _final.Delete(finalId, img);
        }

        #endregion
    }
}