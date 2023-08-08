using System;
using System.Collections.Generic;
using POMQC.Entities.Base;

namespace POMQC.Entities.Checklist
{
    public class ChecklistEntity : EntityBase
    {
        public string Doc { get; set; }

        public string Comment { get; set; }

        public ChecklistType Type { get; set; }

        public IList<DroppedImage> DroppedFiles
        {
            get
            {
                var result = new List<DroppedImage>();
                var image = Doc ?? string.Empty;
                var imgs = image.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var img in imgs)
                {
                    result.Add(new DroppedImage
                    {
                        name = img
                    });
                }

                return result;
            }
        }
    }
}