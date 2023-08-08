using System;
using System.Collections.Generic;
using System.Linq;
using POMQC.Entities.Base;
using POMQC.Utilities;

namespace POMQC.Entities.Final
{
    public class FinalEntity : EntityBase
    {
        public FinalEntity()
        {
            Styles = new List<FinalCustPOEntity>();
        }

        public string EntitiesString { get; set; }
        
        public long FinalId { get; set; }

        public string FactoryManager { get; set; }

        public AQL WorkmanshipAQL { get; set; }

        public AQL MeasurementAQL { get; set; }

        public StatusType FinalStatus { get; set; }

        public string FinalComment { get; set; }

        public string MainLabel { get; set; }

        public int QtyInspected { get; set; }

        public string CartonComment { get; set; }

        public StatusType WorkmanshipStatus { get; set; }

        public StatusType MeasurementStatus { get; set; }

        public StatusType PackingStatus { get; set; }

        public string WorkmanshipComment { get; set; }

        public string MeasurementComment { get; set; }

        public string PackingComment { get; set; }

        public string Comment { get; set; }

        public string MeasurementPackingAudit { get; set; }

        public string QAComment { get; set; }

        public string FactoryCaption { get; set; }

        public string QAAuditor { get; set; }

        public string QAManager { get; set; }

        public string FactoryRep { get; set; }

        public DHUType Type { get; set; }

        public string AuditedBy { get; set; }

        public string DefectCodes { get; set; }

        public string DefectLocs { get; set; }

        public string DefectIds { get; set; }

        public string TotalDefects { get; set; }

        public string ActualInspectionDate { get; set; }

        public int ActualProductQuantity { get; set; }

        public IList<FinalCustPOEntity> Styles { get; set; }

        public IList<DroppedImage> Images
        {
            get
            {
                var result = new List<DroppedImage>();
                var image = MeasurementPackingAudit ?? string.Empty;
                var imgs = image.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var img in imgs)
                {
                    if (Utils.ImageFileTypes.Split(',').Any(s => s.IndexOf(img.Split('.').Last(), StringComparison.OrdinalIgnoreCase) != -1))
                    {
                        result.Add(new DroppedImage
                        {
                            name = img
                        });
                    }
                }

                return result;
            }
        }
    }
}