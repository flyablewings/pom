using System;
using System.Collections.Generic;
using System.Linq;
using POMQC.Entities.Base;
using POMQC.Utilities;

namespace POMQC.Entities.DHU
{
    public class DHUEntity : EntityBase
    {
        public int AuditSampleSize { get; set; }

        public string Comment { get; set; }

        public string Country { get; set; }

        public string DefectImg { get; set; }

        public DHUItem Type { get; set; }

        public DHUStyle Type2 { get; set; }

        public DHUType DHUType { get; set; }

        public string Inspector { get; set; }

        public string LineNumber { get; set; }

        public int OutputQty { get; set; }

        public int POQty { get; set; }

        public long DHUId { get; set; }

        public string DefectCodes { get; set; }

        public string DefectLocs { get; set; }

        public string PCSQuantities { get; set; }

        public string TotalDefects { get; set; }

        public string DefectIds { get; set; }
        
        public IList<DroppedImage> Images
        {
            get
            {
                var result = new List<DroppedImage>();
                var image = DefectImg ?? string.Empty;
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